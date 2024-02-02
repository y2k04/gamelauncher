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
        public List<Game> games = new List<Game>();
        public Game selectedGame = new Game("","","","");
        private Timer processCheck = new Timer();
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
                        launchGame.Text = "Play";
                        launchGame.Enabled = true;
                    }
                    else
                    {
                        launchGame.Text = "Started";
                        launchGame.Enabled = false;
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
                    selectedGameArt.Visible = false;
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

                var data = JsonConvert.SerializeObject(games);
                stream.SetLength(0);
                writer.Write(data);

                gameList.SelectedNode = gameList.Nodes.Add(editor.gameName.Text);

                selectedGameName.Visible =
                    launchGame.Visible =
                    selectedGameArt.Visible = true;
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
                        selectedGameArt.Visible = true;

                selectedGame = games[e.Node.Index];

                selectedGameName.Text = selectedGame.Name;

                if (selectedGame.ArtworkPath == string.Empty)
                {
                    games[e.Node.Index].ArtworkPath = "USE_APP_ICON";
                    var data = JsonConvert.SerializeObject(games);
                    stream.SetLength(0);
                    writer.Write(data);
                }

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
            }
        }

        private void launchGame_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(selectedGame.Location, selectedGame.Arguments);
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
                    editor.gameArtwork.Text
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

                stream.SetLength(0);
                writer.Write(JsonConvert.SerializeObject(games));
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
                        selectedGameArt.Visible = false;
                stream.SetLength(0);
                writer.Write(JsonConvert.SerializeObject(games));
            }
        }
    }
}
