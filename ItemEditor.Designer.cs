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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // gameName
            // 
            this.gameName.Location = new System.Drawing.Point(75, 6);
            this.gameName.Name = "gameName";
            this.gameName.Size = new System.Drawing.Size(207, 20);
            this.gameName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location";
            // 
            // gameLocation
            // 
            this.gameLocation.Location = new System.Drawing.Point(75, 30);
            this.gameLocation.Name = "gameLocation";
            this.gameLocation.Size = new System.Drawing.Size(184, 20);
            this.gameLocation.TabIndex = 3;
            // 
            // selectLocation
            // 
            this.selectLocation.Location = new System.Drawing.Point(258, 30);
            this.selectLocation.Name = "selectLocation";
            this.selectLocation.Size = new System.Drawing.Size(24, 23);
            this.selectLocation.TabIndex = 4;
            this.selectLocation.Text = "...";
            this.selectLocation.UseVisualStyleBackColor = true;
            this.selectLocation.Click += new System.EventHandler(this.selectLocation_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Arguments";
            // 
            // gameArguments
            // 
            this.gameArguments.Location = new System.Drawing.Point(75, 58);
            this.gameArguments.Name = "gameArguments";
            this.gameArguments.Size = new System.Drawing.Size(207, 20);
            this.gameArguments.TabIndex = 6;
            // 
            // saveGame
            // 
            this.saveGame.Location = new System.Drawing.Point(88, 113);
            this.saveGame.Name = "saveGame";
            this.saveGame.Size = new System.Drawing.Size(61, 24);
            this.saveGame.TabIndex = 7;
            this.saveGame.Text = "Save";
            this.saveGame.UseVisualStyleBackColor = true;
            this.saveGame.Click += new System.EventHandler(this.saveGame_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(155, 114);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(61, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 26);
            this.label4.TabIndex = 9;
            this.label4.Text = "Artwork\r\n(optional)";
            // 
            // gameArtwork
            // 
            this.gameArtwork.Location = new System.Drawing.Point(75, 87);
            this.gameArtwork.Name = "gameArtwork";
            this.gameArtwork.Size = new System.Drawing.Size(184, 20);
            this.gameArtwork.TabIndex = 10;
            // 
            // selectArt
            // 
            this.selectArt.Location = new System.Drawing.Point(258, 87);
            this.selectArt.Name = "selectArt";
            this.selectArt.Size = new System.Drawing.Size(24, 23);
            this.selectArt.TabIndex = 11;
            this.selectArt.Text = "...";
            this.selectArt.UseVisualStyleBackColor = true;
            this.selectArt.Click += new System.EventHandler(this.selectArt_Click);
            // 
            // ItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 144);
            this.ControlBox = false;
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
            this.Name = "ItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Editor";
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
    }
}