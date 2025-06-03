using GameLauncher.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace GameLauncher
{
    public partial class Launcher : Form
    {
        readonly string configJSON = $@"{Environment.CurrentDirectory}\config.json";
        Config config;
        Game selectedGame = new();
        readonly Timer processCheck = new() { Enabled = true };
        readonly Dictionary<Game, Timer> gameTimers = [];

        FileStream stream;
        StreamReader reader;
        StreamWriter writer;

        readonly Font StarFont = new Font(DefaultFont.FontFamily, DefaultFont.Size + 2, FontStyle.Bold);
        readonly Brush StarBrush = Brushes.Gold;
        const string StarChar = "★";

        public Launcher()
        {
            InitializeComponent();
            SetupLauncher();
            gameList.DrawNode += GameList_DrawNode;
            LoggingUtil.Debug("Binding event: \"processCheck.Tick\"");
            processCheck.Tick += ProcessCheck_Tick;
            LoggingUtil.Info("Launcher window has been initialized!");
        }

        private async void SetupLauncher()
        {
            stream = File.Open(configJSON, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            var rawData = await reader.ReadToEndAsync();
            if (rawData.StartsWith("[")) // Compatibility fix with v1.2 and below
            {
                config = new()
                {
                    Games = [.. JsonConvert.DeserializeObject<List<Game>>(rawData).OrderBy(x => x.Name)],
                    FavouritesToggled = false
                };

                UpdateData(); // Update to new schema
            }
            else if (string.IsNullOrEmpty(rawData))
            {
                config = new() { Games = [], FavouritesToggled = false };
                UpdateData();
            }
            else
            {
                config = JsonConvert.DeserializeObject<Config>(rawData);
                config.Games = [.. config.Games.OrderBy(x => x.Name)];
            }

            UpdateGameList();

            if (gameList.Nodes.Count != 0)
            {
                gameList.SelectedNode = gameList.TopNode;
                editGameButton.Enabled = deleteGameButton.Enabled = true;
                emptyLibraryNote.Visible = false;
            }
            else
            {
                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                    playTimeContainer.Visible = false;
            }
        }

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingUtil.Debug("Unregistering event: \"processCheck.Tick\"");
            processCheck.Tick -= ProcessCheck_Tick;
            processCheck.Enabled = false;

            var keysToRemove = new List<Game>();
            foreach (var timer in gameTimers)
            {
                timer.Value.Enabled = false;
                config.Games[config.Games.LastIndexOf(timer.Key)].PlayTime = Convert.ToDouble(timer.Value.Tag);
                keysToRemove.Add(timer.Key);
            }

            foreach (var key in keysToRemove)
            {
                gameTimers.Remove(key);
            }

            UpdateData();
            stream.Close();

            LoggingUtil.Info("Closing main launcher window...");
        }

        private void UpdateData()
        {
            stream.SetLength(0);
            writer.Write(JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private void gameList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is not Game game)
                return;
            //MessageBox.Show("wtf");
            if (game != selectedGame)
            {
                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                    playTimeContainer.Visible = true;

                selectedGame = game;
                selectedGameName.Text = selectedGame.Name;
                playTimeLabel.Text = Helpers.FormatPlayTime(selectedGame.PlayTime);

                if (string.IsNullOrWhiteSpace(selectedGame.ArtworkPath))
                    selectedGameArt.Image = Helpers.GetIcon(selectedGame.Location);
                else
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;

                if (!File.Exists(selectedGame.Location))
                    TryFixMissingGame(config.Games.IndexOf(selectedGame));
            }
        }

        private void TryFixMissingGame(int index)
        {
            LoggingUtil.Error("Selected game could not be found.");
            var errormsg = MessageBox.Show($"The game you have selected could not be found.{Environment.NewLine}" +
                $"Did you want us to check if it is on another drive,{Environment.NewLine}" +
                $"manually select the new location, or ignore it?", "Game not found", MessageBoxButtons.AbortRetryIgnore);

            switch (errormsg)
            {
                /*Scan*/
                case DialogResult.Abort:
                    TryScanMissingGame(index);
                    break;
                /*Browse*/
                case DialogResult.Retry:
                    TryBrowseMissingGame(index);
                    break;
                /*Ignore*/
                default:
                    launchGame.Text = "Not found";
                    launchGame.Enabled = false;
                    break;
            }
        }

        private void TryScanMissingGame(int index)
        {
            ManagementObjectSearcher ms = new("Select * from Win32_Volume");
            var newPath = "";
            foreach (ManagementObject mo in ms.Get().Cast<ManagementObject>())
            {
                var drive = (string)mo["DriveLetter"];
                if (string.IsNullOrEmpty(drive))
                    continue;
                else
                {
                    var testPath = $"{drive.Trim(':')}{selectedGame.Location.Remove(0, 1)}";
                    if (File.Exists(testPath))
                    {
                        newPath = testPath;
                        break;
                    }
                }
            }

            if (newPath == "")
            {
                LoggingUtil.Error("Selected game still couldn't be found.");
                var errormsg = MessageBox.Show($"The game still can't be found.{Environment.NewLine}" +
                    $"Did you want to select it yourself or ignore it?", "Scan failed", MessageBoxButtons.RetryCancel);
                switch (errormsg)
                {
                    /*Browse*/
                    case DialogResult.Retry:
                        TryBrowseMissingGame(index);
                        break;
                    /*Ignore*/
                    default:
                        launchGame.Text = "Not found";
                        launchGame.Enabled = false;
                        break;
                }
            }
            else
            {
                LoggingUtil.Info("Selected game was found.");
                var msg = MessageBox.Show($"The game was found on your {newPath.Substring(0, 1)} drive.{Environment.NewLine}" +
                    $"Did you want to save these changes?", "Scan success", MessageBoxButtons.YesNo);

                switch (msg)
                {
                    case DialogResult.Yes:
                        selectedGame.Location = config.Games[index].Location = newPath;
                        UpdateData();
                        launchGame.Text = "Play";
                        launchGame.Enabled = true;
                        break;
                    default:
                        launchGame.Text = "Not found";
                        launchGame.Enabled = false;
                        break;
                }
            }
        }

        private void TryBrowseMissingGame(int index)
        {
            var filename = Path.GetFileName(config.Games[index].Location);
            OpenFileDialog file = new()
            {
                CheckFileExists = true,
                Filter = $"{filename}|{filename}|EXE files|*.exe",
                Multiselect = false,
                Title = $"Select {filename}"
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                var confirm = MessageBox.Show("Are you sure you want to save these changes?", "Manual search", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    selectedGame.Location = config.Games[index].Location = file.FileName;
                    UpdateData();
                    launchGame.Text = "Play";
                    launchGame.Enabled = true;
                }
            } else
            {
                LoggingUtil.Info("Cancelled manually finding selected game.");
            }
        }

        private void UpdateGameList()
        {
            LoggingUtil.Info("Reloading games list...");
            var CurrentNode = gameList.SelectedNode == null ? 0:gameList.SelectedNode.Index;
            gameList.Nodes.Clear();
            var filtered = config.FavouritesToggled ? [.. config.Games.Where(g => g.IsFavorite)] : config.Games;
            foreach (var game in filtered)
            {
                var node = new TreeNode($"{StarChar} {game.Name}") { Tag = game };
                if (game.IsFavorite)
                {
                    node.ForeColor = Color.Gold; // optional, affects only non-owner-draw
                    node.NodeFont = new Font(gameList.Font.FontFamily, gameList.Font.Size + 2, FontStyle.Bold);
                }
                gameList.Nodes.Add(node);
            }
            favoritestoggle.BackColor = config.FavouritesToggled ? Color.IndianRed : SystemColors.Control;
            favoritestoggle.ForeColor = config.FavouritesToggled ? Color.LightYellow : SystemColors.ControlText;
            gameList.SelectedNode = gameList.Nodes[CurrentNode];
        }

        private void launchGame_Click(object sender, EventArgs e)
        {
            try
            {
                LoggingUtil.Info($"Launching game \"{selectedGame.Name}\" with path \"{selectedGame.Location}\"...");
                Process.Start(new ProcessStartInfo(selectedGame.Location, selectedGame.Arguments)
                {
                    WorkingDirectory = Path.GetDirectoryName(selectedGame.Location)
                });
            }
            catch (Exception ex)
            {
                LoggingUtil.Error($"Error launching game \"{selectedGame.Name}\": \"{ex.Message}\"");
                MessageBox.Show(ex.Message, "Error launching game", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessCheck_Tick(object _, EventArgs __)
        {
            try
            {
                if (!Process.GetProcessesByName(Path.GetFileNameWithoutExtension(selectedGame.Location)).Any())
                {
                    if (File.Exists(selectedGame.Location))
                    {
                        launchGame.Text = "Play";
                        launchGame.Enabled = true;
                    }

                    if (gameTimers.ContainsKey(selectedGame))
                    {
                        gameTimers[selectedGame].Enabled = false;
                        config.Games[config.Games.LastIndexOf(selectedGame)].PlayTime = Convert.ToDouble(gameTimers[selectedGame].Tag);
                        UpdateData();
                        gameTimers.Remove(selectedGame);
                    }
                }
                else
                {
                    launchGame.Text = "Started";
                    launchGame.Enabled = false;

                    if (gameTimers.ContainsKey(selectedGame))
                        Helpers.LazyUpdateLabel(playTimeLabel, Helpers.FormatPlayTime(Convert.ToDouble(gameTimers[selectedGame].Tag)));
                    else
                    {
                        Timer gameTimer = new() { Interval = 1000, Tag = selectedGame.PlayTime };
                        gameTimer.Tick += (object s, EventArgs e) => ((Timer)s).Tag = Convert.ToDouble(((Timer)s).Tag) + 1;
                        gameTimer.Enabled = true;
                        gameTimers.Add(selectedGame, gameTimer);
                    }
                }
            }
            catch { }
        }

        private void addGameButton_Click(object sender, EventArgs e)
        {
            ItemEditor editor = new();
            if (editor.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(editor.gameArtwork.Text) || editor.gameArtwork.Text == string.Empty)
                    editor.gameArtwork.Text = "";

                Game game = new(
                    editor.gameName.Text,
                    editor.gameLocation.Text,
                    editor.gameArguments.Text,
                    editor.gameArtwork.Text,
                    0,
                    editor.FavoriteChecked
                );
                config.Games.Add(game);

                UpdateData();
                UpdateGameList();

                gameList.SelectedNode = gameList.TopNode;
                editGameButton.Enabled = deleteGameButton.Enabled = true;
                emptyLibraryNote.Visible = false;

                LoggingUtil.Info($"Added new game: {editor.gameName.Text}");
            }
        }

        private void editGameButton_Click(object sender, EventArgs e)
        {
            ItemEditor editor = new(selectedGame);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                config.Games[gameList.SelectedNode.Index] = new Game(
                    editor.gameName.Text,
                    editor.gameLocation.Text,
                    editor.gameArguments.Text,
                    editor.gameArtwork.Text,
                    selectedGame.PlayTime,
                    editor.FavoriteChecked
                );

                selectedGame = config.Games[gameList.SelectedNode.Index];
                gameList.SelectedNode.Text = selectedGameName.Text = selectedGame.Name;

                selectedGameArt.ImageLocation = string.Empty;
                selectedGameArt.Image = null;

                if (selectedGame.ArtworkPath == "")
                    selectedGameArt.Image = Helpers.GetIcon(selectedGame.Location);
                else
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;

                UpdateData();
                UpdateGameList();
                LoggingUtil.Info($"Updated game: {selectedGame.Name}");
            }
        }

        private void deleteGameButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete '{selectedGame.Name}'?", "GameLauncher", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoggingUtil.Info($"Deleting {selectedGame.Name}...");
                config.Games.RemoveRange(gameList.SelectedNode.Index, 1);
                gameList.Nodes.Remove(gameList.SelectedNode);
                if (gameList.Nodes.Count != 0)
                    gameList.SelectedNode = gameList.TopNode;
                else
                {
                    selectedGameName.Visible =
                        launchGame.Visible =
                        selectedGameArt.Visible =
                        playTimeContainer.Visible =
                        editGameButton.Enabled =
                        deleteGameButton.Enabled = false;
                    emptyLibraryNote.Visible = true;
                }
                UpdateData();
            }
        }

        private void favoritesToggle_Click(object sender, EventArgs e)
        {
            config.FavouritesToggled = !config.FavouritesToggled;
            UpdateData();
            UpdateGameList();
            selectedGame = new();

            selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                    playTimeContainer.Visible = false;

            favoritestoggle.BackColor = config.FavouritesToggled ? Color.IndianRed : SystemColors.Control;
            favoritestoggle.ForeColor = config.FavouritesToggled ? Color.LightYellow : SystemColors.ControlText;
        }

        private void GameList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node?.Tag is not Game game)
                return;

            var full = game.Name;
            var name = full.StartsWith(StarChar + " ") ? full.Substring(StarChar.Length + 1) : full;
            var bounds = new Rectangle(e.Bounds.X, e.Bounds.Y, gameList.Width, e.Bounds.Height);
            bool isSelected = (e.State & TreeNodeStates.Selected) != 0;

            Brush textBrush = isSelected ? SystemBrushes.HighlightText : SystemBrushes.ControlText;
            e.Graphics.FillRectangle(isSelected ? SystemBrushes.Highlight : SystemBrushes.Window, bounds);

            if (game.IsFavorite)
            {
                float offset = e.Graphics.MeasureString(StarChar, StarFont).Width;
                e.Graphics.DrawString(StarChar, StarFont, StarBrush, new PointF(bounds.Left - offset, bounds.Top + 3));
                e.Graphics.DrawString(name, gameList.Font, textBrush, bounds.Location);
            }
            else
                e.Graphics.DrawString(name, gameList.Font, textBrush, e.Bounds);

            e.DrawDefault = false;
        }
    }
}