﻿using GameLauncher.Util;
using KPreisser.UI;
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
            SetupLauncher(null);
            gameList.DrawNode += GameList_DrawNode;
            LoggingUtil.Debug("Binding event: \"processCheck.Tick\"");
            processCheck.Tick += ProcessCheck_Tick;
            LoggingUtil.Info("Launcher window has been initialized!");
        }

        public Launcher(string dismissedVersion)
        {
            InitializeComponent();
            SetupLauncher(dismissedVersion);
            gameList.DrawNode += GameList_DrawNode;
            LoggingUtil.Debug("Binding event: \"processCheck.Tick\"");
            processCheck.Tick += ProcessCheck_Tick;
            LoggingUtil.Info("Launcher window has been initialized!");
        }

        private async void SetupLauncher(string dismissedVersion)
        {
            stream = File.Open(configJSON, System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
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

            // Creates UUID for games created without one
            foreach (Game game in config.Games)
                if (string.IsNullOrEmpty(game.Uuid))
                {
                    game.Uuid = Guid.NewGuid().ToString();
                    LoggingUtil.Info($"{game.Name} UUID was empty and now is {game.Uuid}");
                }

            if (dismissedVersion != null) {
                config.DismissedUpdate = dismissedVersion;
                UpdateData();
            }

            UpdateGameList();
            setGameDisplay();
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
            if (game != selectedGame)
            {
                selectedGame = game;
                setGameDisplay();
            }
        }

        private void TryFixMissingGame(int index)
        {
            LoggingUtil.Error("Selected game could not be found.");
            TaskDialog msg = new(new()
            {
                Title = "GameLauncher",
                Instruction = "Game not found",
                Text = "The game you have selected could not be found.\nDid you want us to check if it is on another drive,\nmanually select the new location, or ignore it?",
                CustomButtons = [new TaskDialogCustomButton("Scan"), new TaskDialogCustomButton("Browse"), new TaskDialogCustomButton("Ignore")],
                Icon = TaskDialogStandardIcon.Error
            });

            TaskDialogButton result = msg.Show();

            if (result == msg.Page.CustomButtons[0])
                TryScanMissingGame(index);
            else if (result == msg.Page.CustomButtons[1])
                TryBrowseMissingGame(index);
            else {
                launchGame.Text = "Not found";
                launchGame.Enabled = false;
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
                TaskDialog msg = new(new()
                {
                    Title = "GameLauncher",
                    Instruction = "Scan failed",
                    Text = "The game still can't be found.\nDid you want to select it yourself or ignore it?",
                    CustomButtons = [new TaskDialogCustomButton("Browse"), new TaskDialogCustomButton("Ignore")],
                    Icon = TaskDialogStandardIcon.Error
                });

                TaskDialogButton result = msg.Show();

                if (result == msg.Page.CustomButtons[0])
                    TryBrowseMissingGame(index);
                else
                {
                    launchGame.Text = "Not found";
                    launchGame.Enabled = false;
                }
            }
            else
            {
                LoggingUtil.Info("Selected game was found.");
                TaskDialog msg = new(new()
                {
                    Title = "GameLauncher",
                    Instruction = "Scan success!",
                    Text = $"The game was found on your {newPath.Substring(0, 1)} drive.\nDid you want to save the new location?",
                    StandardButtons = [new TaskDialogStandardButton(TaskDialogResult.Yes), new TaskDialogStandardButton(TaskDialogResult.No)],
                    Icon = TaskDialogStandardIcon.SecuritySuccessGreenBar
                });

                TaskDialogButton result = msg.Show();

                if (result == msg.Page.StandardButtons[TaskDialogResult.Yes])
                {
                    selectedGame.Location = config.Games[index].Location = newPath;
                    UpdateData();
                    launchGame.Text = "Play";
                    launchGame.Enabled = true;
                } else
                {
                    launchGame.Text = "Not found";
                    launchGame.Enabled = false;
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
                TaskDialog msg = new(new()
                {
                    Title = "GameLauncher",
                    Instruction = "Manual search",
                    Text = "Are you sure you want to save these changes?",
                    StandardButtons = [new TaskDialogStandardButton(TaskDialogResult.Yes), new TaskDialogStandardButton(TaskDialogResult.No)],
                    Icon = TaskDialogStandardIcon.Information
                });

                TaskDialogButton result = msg.Show();

                if (result == msg.Page.StandardButtons[TaskDialogResult.Yes])
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

            foreach (TreeNode node in gameList.Nodes)
            {
                var game = node.Tag as Game;
                if (game.Uuid == selectedGame.Uuid)
                    gameList.SelectedNode = node;
            }
            if (gameList.SelectedNode == null && gameList.Nodes.Count != 0)
                gameList.SelectedNode = gameList.TopNode;
        }

        private void setGameDisplay()
        {
            bool active = gameList.Nodes.Count != 0;

            editGameButton.Enabled = deleteGameButton.Enabled = active;
            emptyLibraryNote.Visible = !active;

            selectedGameName.Visible =
            launchGame.Visible =
            selectedGameArt.Visible =
            playTimeContainer.Visible = active;

            if (!active)
                return;

            selectedGameName.Text = selectedGame.Name;
            playTimeLabel.Text = Helpers.FormatPlayTime(selectedGame.PlayTime);

            if (string.IsNullOrWhiteSpace(selectedGame.ArtworkPath))
                selectedGameArt.Image = Helpers.GetIcon(selectedGame.Location);
            else
                selectedGameArt.ImageLocation = selectedGame.ArtworkPath;

            if (!File.Exists(selectedGame.Location))
                TryFixMissingGame(config.Games.IndexOf(selectedGame));
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
                new TaskDialog(new()
                {
                    Title = "GameLauncher",
                    Instruction = "Error launching game",
                    Text = ex.Message,
                    StandardButtons = [new TaskDialogStandardButton(TaskDialogResult.OK)],
                    Icon = TaskDialogStandardIcon.Error
                });
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
                    editor.FavoriteChecked,
                    Guid.NewGuid().ToString()
                );
                config.Games.Add(game);

                selectedGame = game;

                UpdateData();
                UpdateGameList();
                setGameDisplay();

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
                    editor.FavoriteChecked,
                    selectedGame.Uuid
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

                UpdateData();
                setGameDisplay();
            }
        }

        private void favoritesToggle_Click(object sender, EventArgs e)
        {
            config.FavouritesToggled = !config.FavouritesToggled;
            UpdateData();
            UpdateGameList();
            setGameDisplay();

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