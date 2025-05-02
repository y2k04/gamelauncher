using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace GameLauncher
{
    public partial class ItemEditor : Form
    {
        private Game _game;
        public ItemEditor(Game game = null)
        {
            InitializeComponent();
            _game = game;

            if (game != null)
            {
                gameName.Text = game.Name;
                gameLocation.Text = game.Location;
                gameArguments.Text = game.Arguments;
                gameArtwork.Text = game.ArtworkPath;
                favoriteCheckBox.Checked = game.IsFavorite;
            }
        }

        private void selectLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new()
            {
                CheckFileExists = true,
                Filter = "EXE files|*.exe",
                Multiselect = false,
                Title = "Select game executable"
            };
            if (file.ShowDialog() == DialogResult.OK)
                gameLocation.Text = file.FileName;
        }
        public bool FavoriteChecked => favoriteCheckBox.Checked;

        private void saveGame_Click(object sender, EventArgs e)
        {
            if (gameName.Text == string.Empty || gameLocation.Text == string.Empty)
                MessageBox.Show("Name and location cannot be blank");
            else
            {
                gameLocation.Text = gameLocation.Text.Replace("\"", "");
                gameArtwork.Text = gameArtwork.Text.Replace("\"", "");
                if (_game != null)
                {
                    _game.Name = gameName.Text;
                    _game.Location = gameLocation.Text;
                    _game.Arguments = gameArguments.Text;
                    _game.ArtworkPath = gameArtwork.Text;
                    _game.IsFavorite = favoriteCheckBox.Checked;
                }
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
            OpenFileDialog file = new()
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void favoriteCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
