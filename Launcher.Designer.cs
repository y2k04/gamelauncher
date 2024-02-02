namespace GameLauncher
{
    partial class Launcher
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gameList = new System.Windows.Forms.TreeView();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.launchGame = new System.Windows.Forms.Button();
            this.selectedGameName = new System.Windows.Forms.Label();
            this.selectedGameArt = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedGameArt)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gameList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.launchGame);
            this.splitContainer1.Panel2.Controls.Add(this.selectedGameName);
            this.splitContainer1.Panel2.Controls.Add(this.selectedGameArt);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // gameList
            // 
            this.gameList.ContextMenuStrip = this.menu;
            this.gameList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameList.FullRowSelect = true;
            this.gameList.HideSelection = false;
            this.gameList.Location = new System.Drawing.Point(0, 0);
            this.gameList.Name = "gameList";
            this.gameList.ShowLines = false;
            this.gameList.Size = new System.Drawing.Size(266, 450);
            this.gameList.TabIndex = 0;
            this.gameList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.gameList_AfterSelect);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGameButton,
            this.editGameButton,
            this.deleteGameButton});
            this.menu.Name = "contextMenuStrip1";
            this.menu.ShowImageMargin = false;
            this.menu.Size = new System.Drawing.Size(117, 70);
            // 
            // addGameButton
            // 
            this.addGameButton.Name = "addGameButton";
            this.addGameButton.Size = new System.Drawing.Size(116, 22);
            this.addGameButton.Text = "Add Game";
            this.addGameButton.Click += new System.EventHandler(this.addGameButton_Click);
            // 
            // editGameButton
            // 
            this.editGameButton.Enabled = false;
            this.editGameButton.Name = "editGameButton";
            this.editGameButton.Size = new System.Drawing.Size(116, 22);
            this.editGameButton.Text = "Edit Game";
            this.editGameButton.Click += new System.EventHandler(this.editGameButton_Click);
            // 
            // deleteGameButton
            // 
            this.deleteGameButton.Enabled = false;
            this.deleteGameButton.Name = "deleteGameButton";
            this.deleteGameButton.Size = new System.Drawing.Size(116, 22);
            this.deleteGameButton.Text = "Delete Game";
            this.deleteGameButton.Click += new System.EventHandler(this.deleteGameButton_Click);
            // 
            // launchGame
            // 
            this.launchGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.launchGame.Location = new System.Drawing.Point(37, 321);
            this.launchGame.Name = "launchGame";
            this.launchGame.Size = new System.Drawing.Size(131, 36);
            this.launchGame.TabIndex = 2;
            this.launchGame.Text = "Play";
            this.launchGame.UseVisualStyleBackColor = true;
            this.launchGame.Click += new System.EventHandler(this.launchGame_Click);
            // 
            // selectedGameName
            // 
            this.selectedGameName.AutoSize = true;
            this.selectedGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedGameName.Location = new System.Drawing.Point(32, 279);
            this.selectedGameName.Name = "selectedGameName";
            this.selectedGameName.Size = new System.Drawing.Size(81, 25);
            this.selectedGameName.TabIndex = 1;
            this.selectedGameName.Text = "[Game]";
            // 
            // selectedGameArt
            // 
            this.selectedGameArt.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.selectedGameArt.Dock = System.Windows.Forms.DockStyle.Top;
            this.selectedGameArt.Location = new System.Drawing.Point(0, 0);
            this.selectedGameArt.Name = "selectedGameArt";
            this.selectedGameArt.Size = new System.Drawing.Size(530, 256);
            this.selectedGameArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.selectedGameArt.TabIndex = 0;
            this.selectedGameArt.TabStop = false;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Launcher_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectedGameArt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView gameList;
        private System.Windows.Forms.ToolStripMenuItem addGameButton;
        private System.Windows.Forms.ToolStripMenuItem editGameButton;
        private System.Windows.Forms.ToolStripMenuItem deleteGameButton;
        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.PictureBox selectedGameArt;
        private System.Windows.Forms.Label selectedGameName;
        private System.Windows.Forms.Button launchGame;
    }
}

