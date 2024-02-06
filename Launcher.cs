using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GameLauncher
{
    public partial class Launcher : Form
    {
        private List<Game> games = new List<Game>();
        private Game selectedGame = new Game("", "", "", "");
        private Timer processCheck = new Timer();
        private Dictionary<Game, Timer> gameTimers = new Dictionary<Game, Timer>();
        private string configJSON = $@"{Environment.CurrentDirectory}\config.json";
        private FileStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        public Launcher()
        {
            InitializeComponent();

            SetupLauncher();
            processCheck.Tick += (object sender, EventArgs e) =>
            {
                try
                {
                    var friendlyName = selectedGame.Location.Substring(selectedGame.Location.LastIndexOf(@"\") + 1).Replace(".exe", "");

                    if (Process.GetProcessesByName(friendlyName).Length == 0)
                    {
                        if (gameTimers.ContainsKey(selectedGame))
                        {
                            gameTimers[selectedGame].Enabled = false;
                            games[games.LastIndexOf(selectedGame)].PlayTime = Convert.ToInt16(gameTimers[selectedGame].Tag);
                            UpdateData();
                            gameTimers.Remove(selectedGame);
                        }
                        launchGame.Text = "Play";
                        launchGame.Enabled = true;
                    }
                    else
                    {
                        launchGame.Text = "Started";
                        launchGame.Enabled = false;

                        if (gameTimers.ContainsKey(selectedGame))
                        {
                            var time = Convert.ToInt16(gameTimers[selectedGame].Tag) / 3600f + "";
                            var playTimeCounter = time.Substring(0, time.LastIndexOf('.') + 2).Replace(".0", "") + (time.StartsWith("1.0") ? " hour" : " hours");
                            if (playTimeLabel.Text != playTimeCounter)
                                playTimeLabel.Text = playTimeCounter;
                        } else
                        {
                            Timer gameTimer = new Timer() { Interval = 1000, Tag = selectedGame.PlayTime };
                            gameTimer.Tick += (object s, EventArgs ee) => (s as Timer).Tag = Convert.ToInt16((s as Timer).Tag) + 1;
                            gameTimer.Enabled = true;
                            gameTimers.Add(selectedGame, gameTimer);
                        }
                    }
                }
                catch { }
            };
        }

        private async void SetupLauncher()
        {
            stream = File.Open(configJSON, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            var data = await reader.ReadToEndAsync();

            if (data != "[]" && data != string.Empty)
            {
                games = JsonConvert.DeserializeObject<List<Game>>(data).OrderBy(x => x.Name).ToList();
                foreach (var game in games)
                {
                    gameList.Nodes.Add(game.Name);
                }
                gameList.SelectedNode = gameList.Nodes[0];
                editGameButton.Enabled =
                    deleteGameButton.Enabled = true;
            }
            else
                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                        playTimeContainer.Visible = false;
        }

        private void UpdateData()
        {
            stream.SetLength(0);
            writer.Write(JsonConvert.SerializeObject(games));
        }

        private void addGameButton_Click(object sender, EventArgs e)
        {
            ItemEditor editor = new ItemEditor();
            if (editor.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(editor.gameArtwork.Text) || editor.gameArtwork.Text == string.Empty)
                    editor.gameArtwork.Text = "USE_APP_ICON";

                games.Add(new Game(
                    editor.gameName.Text,
                    editor.gameLocation.Text,
                    editor.gameArguments.Text,
                    editor.gameArtwork.Text
                ));

                UpdateData();

                gameList.SelectedNode = gameList.Nodes.Add(editor.gameName.Text);

                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible =
                        playTimeContainer.Visible = true;
            }
        }

        private void gameList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (games[e.Node.Index].Location != selectedGame.Location)
            {
                if (!processCheck.Enabled)
                    processCheck.Enabled = true;

                selectedGameName.Visible =
                        launchGame.Visible =
                        selectedGameArt.Visible =
                        playTimeContainer.Visible = true;

                selectedGame = games[e.Node.Index];
                selectedGameName.Text = selectedGame.Name;
                var time = selectedGame.PlayTime / 3600f + "";
                playTimeLabel.Text = time.Substring(0, time.LastIndexOf('.') + 2).Replace(".0", "") + (time.StartsWith("1.0") ? " hour" : " hours");

                if (selectedGame.ArtworkPath == string.Empty || selectedGame.ArtworkPath == "USE_APP_ICON")
                {
                    games[e.Node.Index].ArtworkPath = "USE_APP_ICON";
                    UpdateData();

                    IntPtr hIcon = Shell32.GetJumboIcon(Shell32.GetIconIndex(selectedGame.Location));
                    Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();
                    selectedGameArt.Image = icon.ToBitmap();
                    Shell32.DestroyIcon(hIcon);
                }
                else
                {
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;
                }
            }
        }

        private void launchGame_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(selectedGame.Location, selectedGame.Arguments);
                Timer gameTimer = new Timer() { Interval = 1000, Tag = selectedGame.PlayTime };
                gameTimer.Tick += (object s, EventArgs ee) => (s as Timer).Tag = Convert.ToInt16((s as Timer).Tag) + 1;
                gameTimer.Enabled = true;
                gameTimers.Add(selectedGame, gameTimer);
                launchGame.Text = "Started";
                launchGame.Enabled = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var timer in gameTimers)
            {
                timer.Value.Enabled = false;
                games[games.LastIndexOf(timer.Key)].PlayTime = Convert.ToInt16(timer.Value.Tag);
                UpdateData();
                gameTimers.Remove(timer.Key);
            }
            stream.Close();
        }

        private void editGameButton_Click(object sender, EventArgs e)
        {
            ItemEditor editor = new ItemEditor(selectedGame);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(editor.gameArtwork.Text) || editor.gameArtwork.Text == string.Empty)
                    editor.gameArtwork.Text = "USE_APP_ICON";

                games[gameList.SelectedNode.Index] = new Game(
                    editor.gameName.Text,
                    editor.gameLocation.Text,
                    editor.gameArguments.Text,
                    editor.gameArtwork.Text,
                    selectedGame.PlayTime
                );

                gameList.SelectedNode.Text = editor.gameName.Text;
                selectedGame = games[gameList.SelectedNode.Index];

                selectedGameArt.ImageLocation = string.Empty;
                selectedGameArt.Image = null;

                if (selectedGame.ArtworkPath == "USE_APP_ICON")
                {
                    IntPtr hIcon = Shell32.GetJumboIcon(Shell32.GetIconIndex(selectedGame.Location));
                    Icon icon = (Icon)Icon.FromHandle(hIcon).Clone();
                    selectedGameArt.Image = icon.ToBitmap();
                    Shell32.DestroyIcon(hIcon);
                }
                else
                {
                    selectedGameArt.ImageLocation = selectedGame.ArtworkPath;
                }

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
                    gameList.SelectedNode = gameList.Nodes[0];
                else
                    selectedGameName.Visible =
                        launchGame.Visible =
                        selectedGameArt.Visible =
                        playTimeContainer.Visible = false;
                UpdateData();
            }
        }
    }
}
