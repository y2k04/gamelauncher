namespace GameLauncher
{
    partial class ItemEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.gameName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gameLocation = new System.Windows.Forms.TextBox();
            this.selectLocation = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gameArguments = new System.Windows.Forms.TextBox();
            this.saveGame = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gameArtwork = new System.Windows.Forms.TextBox();
            this.selectArt = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.favoriteCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // gameName
            // 
            this.gameName.Location = new System.Drawing.Point(100, 7);
            this.gameName.Margin = new System.Windows.Forms.Padding(4);
            this.gameName.Name = "gameName";
            this.gameName.Size = new System.Drawing.Size(275, 22);
            this.gameName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location";
            // 
            // gameLocation
            // 
            this.gameLocation.Location = new System.Drawing.Point(100, 37);
            this.gameLocation.Margin = new System.Windows.Forms.Padding(4);
            this.gameLocation.Name = "gameLocation";
            this.gameLocation.Size = new System.Drawing.Size(244, 22);
            this.gameLocation.TabIndex = 3;
            // 
            // selectLocation
            // 
            this.selectLocation.Location = new System.Drawing.Point(344, 34);
            this.selectLocation.Margin = new System.Windows.Forms.Padding(4);
            this.selectLocation.Name = "selectLocation";
            this.selectLocation.Size = new System.Drawing.Size(32, 28);
            this.selectLocation.TabIndex = 4;
            this.selectLocation.Text = "...";
            this.selectLocation.UseVisualStyleBackColor = true;
            this.selectLocation.Click += new System.EventHandler(this.selectLocation_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Arguments";
            // 
            // gameArguments
            // 
            this.gameArguments.Location = new System.Drawing.Point(100, 67);
            this.gameArguments.Margin = new System.Windows.Forms.Padding(4);
            this.gameArguments.Name = "gameArguments";
            this.gameArguments.Size = new System.Drawing.Size(275, 22);
            this.gameArguments.TabIndex = 6;
            // 
            // saveGame
            // 
            this.saveGame.Location = new System.Drawing.Point(119, 174);
            this.saveGame.Margin = new System.Windows.Forms.Padding(4);
            this.saveGame.Name = "saveGame";
            this.saveGame.Size = new System.Drawing.Size(81, 30);
            this.saveGame.TabIndex = 7;
            this.saveGame.Text = "Save";
            this.saveGame.UseVisualStyleBackColor = true;
            this.saveGame.Click += new System.EventHandler(this.saveGame_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(208, 175);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(81, 28);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 32);
            this.label4.TabIndex = 9;
            this.label4.Text = "Artwork\r\n(optional)";
            // 
            // gameArtwork
            // 
            this.gameArtwork.Location = new System.Drawing.Point(100, 97);
            this.gameArtwork.Margin = new System.Windows.Forms.Padding(4);
            this.gameArtwork.Name = "gameArtwork";
            this.gameArtwork.Size = new System.Drawing.Size(244, 22);
            this.gameArtwork.TabIndex = 10;
            // 
            // selectArt
            // 
            this.selectArt.Location = new System.Drawing.Point(344, 94);
            this.selectArt.Margin = new System.Windows.Forms.Padding(4);
            this.selectArt.Name = "selectArt";
            this.selectArt.Size = new System.Drawing.Size(32, 28);
            this.selectArt.TabIndex = 11;
            this.selectArt.Text = "...";
            this.selectArt.UseVisualStyleBackColor = true;
            this.selectArt.Click += new System.EventHandler(this.selectArt_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 137);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Favorite";
            // 
            // favoriteCheckBox
            // 
            this.favoriteCheckBox.AutoSize = true;
            this.favoriteCheckBox.Location = new System.Drawing.Point(100, 138);
            this.favoriteCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.favoriteCheckBox.Name = "favoriteCheckBox";
            this.favoriteCheckBox.Size = new System.Drawing.Size(18, 17);
            this.favoriteCheckBox.TabIndex = 13;
            this.favoriteCheckBox.UseVisualStyleBackColor = true;
            // 
            // ItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 249);
            this.ControlBox = false;
            this.Controls.Add(this.favoriteCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.selectArt);
            this.Controls.Add(this.gameArtwork);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveGame);
            this.Controls.Add(this.gameArguments);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selectLocation);
            this.Controls.Add(this.gameLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gameName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add or Edit Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox gameName;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox gameLocation;
        private System.Windows.Forms.Button selectLocation;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox gameArguments;
        private System.Windows.Forms.Button saveGame;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox gameArtwork;
        private System.Windows.Forms.Button selectArt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox favoriteCheckBox;
    }
}