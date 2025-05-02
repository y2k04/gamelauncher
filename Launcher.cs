using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace GameLauncher
{
    public partial class Launcher : Form
    {
        readonly string configJSON = $@"{Environment.CurrentDirectory}\config.json";
        List<Game> games = [];
        Game selectedGame = new();
        readonly Timer processCheck = new() { Enabled = true };
        readonly Dictionary<Game, Timer> gameTimers = [];

        FileStream stream;
        StreamReader reader;
        StreamWriter writer;

        public Launcher()
        {
            InitializeComponent();
            SetupLauncher();
            processCheck.Tick += ProcessCheck_Tick;
        }

        private List<Game> visibleGames = new List<Game>();

        private async void SetupLauncher()
        {
            stream = File.Open(configJSON, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            var data = await reader.ReadToEndAsync();

            if (data != "[]" && data != string.Empty)
            {
                games = JsonConvert.DeserializeObject<List<Game>>(data).OrderBy(x => x.Name).ToList();
                if (showFavoritesOnly)
                {
                    games = games.Where(game => game.IsFavorite).ToList();
                }
                gameList.Nodes.Clear();
                games.ForEach(game => gameList.Nodes.Add(game.Name));
                gameList.SelectedNode = gameList.Nodes[0];
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

        private void UpdateData()
        {
            stream.SetLength(0);
            writer.Write(JsonConvert.SerializeObject(games));
        }

        private void gameList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (visibleGames.Count == 0 || e.Node.Index >= visibleGames.Count)
                return;

            if (visibleGames[e.Node.Index].Name != selectedGame.Name)
            {
                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                    playTimeContainer.Visible = true;

                selectedGame = visibleGames[e.Node.Index];
                selectedGameName.Text = selectedGame.Name;
                playTimeLabel.Text = Helpers.FormatPlayTime(selectedGame.PlayTime);

                if (string.IsNullOrWhiteSpace(selectedGame.ArtworkPath))
                    selectedGameArt.Image = Helpers.GetIcon(selectedGame.Location);
                else
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;

                if (!File.Exists(selectedGame.Location))
                    TryFixMissingGame(games.IndexOf(selectedGame));
            }
        }


        private void TryFixMissingGame(int index)
        {
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
                var msg = MessageBox.Show($"The game was found on your {newPath.Substring(0, 1)} drive.{Environment.NewLine}" +
                    $"Did you want to save these changes?", "Scan success", MessageBoxButtons.YesNo);

                switch (msg)
                {
                    case DialogResult.Yes:
                        selectedGame.Location = games[index].Location = newPath;
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
            var filename = Path.GetFileName(games[index].Location);
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
                    selectedGame.Location = games[index].Location = file.FileName;
                    UpdateData();
                    launchGame.Text = "Play";
                    launchGame.Enabled = true;
                }
            }
        }

        private void launchGame_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(selectedGame.Location, selectedGame.Arguments)
                {
                    WorkingDirectory = Path.GetDirectoryName(selectedGame.Location)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        games[games.LastIndexOf(selectedGame)].PlayTime = Convert.ToDouble(gameTimers[selectedGame].Tag);
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

                games.Add(new Game(
                    editor.gameName.Text,
                    editor.gameLocation.Text,
                    editor.gameArguments.Text,
                    editor.gameArtwork.Text,
                    0,
                    editor.FavoriteChecked
                                ));

                UpdateData();

                gameList.SelectedNode = gameList.Nodes.Add(editor.gameName.Text);

                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                        playTimeContainer.Visible = true;
                emptyLibraryNote.Visible = false;
            }
        }

        private void editGameButton_Click(object sender, EventArgs e)
        {
            ItemEditor editor = new(selectedGame);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                games[gameList.SelectedNode.Index] = new Game(
                editor.gameName.Text,
                editor.gameLocation.Text,
                editor.gameArguments.Text,
                editor.gameArtwork.Text,
                selectedGame.PlayTime,
                editor.FavoriteChecked
                );

                gameList.SelectedNode.Text = editor.gameName.Text;
                selectedGame = games[gameList.SelectedNode.Index];

                selectedGameArt.ImageLocation = string.Empty;
                selectedGameArt.Image = null;

                if (selectedGame.ArtworkPath == "")
                    selectedGameArt.Image = Helpers.GetIcon(selectedGame.Location);
                else
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;

                UpdateData();
            }
        }

        private void deleteGameButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete '{selectedGame.Name}'?", "Game Launcher", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                games.RemoveRange(gameList.SelectedNode.Index, 1);
                gameList.Nodes.Remove(gameList.SelectedNode);
                if (gameList.Nodes.Count != 0)
                    gameList.SelectedNode = gameList.TopNode;
                else
                {
                    selectedGameName.Visible =
                        launchGame.Visible =
                        selectedGameArt.Visible =
                        playTimeContainer.Visible = false;
                    emptyLibraryNote.Visible = true;
                }
                UpdateData();
            }
        }

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            processCheck.Tick -= ProcessCheck_Tick;
            processCheck.Enabled = false;

            var keysToRemove = new List<Game>();
            foreach (var timer in gameTimers)
            {
                timer.Value.Enabled = false;
                games[games.LastIndexOf(timer.Key)].PlayTime = Convert.ToDouble(timer.Value.Tag);
                keysToRemove.Add(timer.Key);
            }

            foreach (var key in keysToRemove)
            {
                gameTimers.Remove(key);
            }

            UpdateData();
            stream.Close();
        }

        private bool showFavoritesOnly = false;

        private void emptyLibraryNote_Click(object sender, EventArgs e)
        {

        }

        private void Launcher_Load(object sender, EventArgs e)
        {

        }

        private bool showOnlyFavorites = false;

        private void favoritestoggle_Click(object sender, EventArgs e)
        {
            showOnlyFavorites = !showOnlyFavorites;
            UpdateGameList();
        }

        private void UpdateGameList()
        {
            gameList.Nodes.Clear();

            visibleGames = showOnlyFavorites
                ? games.Where(g => g.IsFavorite).ToList()
                : games;

            foreach (var game in visibleGames)
            {
                gameList.Nodes.Add(new TreeNode(game.Name));
            }

            if (visibleGames.Count > 0)
            {
                gameList.SelectedNode = gameList.Nodes[0];
            }
        }
    }
}