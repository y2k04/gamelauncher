using System;
using System.Windows.Forms;

namespace GameLauncher
{
    public partial class ItemEditor : Form
    {
        public ItemEditor(Game game = null)
        {
            InitializeComponent();
            if (game != null)
            {
                gameName.Text = game.Name;
                gameLocation.Text = game.Location;
                gameArguments.Text = game.Arguments;
                gameArtwork.Text = game.ArtworkPath == "USE_APP_ICON" ? "" : game.ArtworkPath;
            }
        }

        private void selectLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "EXE files|*.exe",
                Multiselect = false,
                Title = "Select game executable"
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                gameLocation.Text = file.FileName;
            }
        }

        private void saveGame_Click(object sender, EventArgs e)
        {
            if (gameName.Text == string.Empty || gameLocation.Text == string.Empty)
                MessageBox.Show("Name and location cannot be blank");
            else
            {
                gameLocation.Text = gameLocation.Text.Replace("\"", "");
                gameArtwork.Text = gameArtwork.Text.Replace("\"", "");
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void selectArt_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.ico",
                Multiselect = false,
                Title = "Select game artwork"
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                gameArtwork.Text = file.FileName;
            }
        }
    }
}
