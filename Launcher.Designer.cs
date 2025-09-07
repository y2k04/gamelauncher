﻿using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gameList = new System.Windows.Forms.TreeView();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.editGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyLibraryNote = new System.Windows.Forms.Label();
            this.favoritestoggle = new System.Windows.Forms.Button();
            this.launchGame = new System.Windows.Forms.Button();
            this.selectedGameName = new System.Windows.Forms.Label();
            this.selectedGameArt = new System.Windows.Forms.PictureBox();
            this.playTimeContainer = new System.Windows.Forms.GroupBox();
            this.playTimeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gameList.SuspendLayout();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedGameArt)).BeginInit();
            this.playTimeContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gameList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.favoritestoggle);
            this.splitContainer1.Panel2.Controls.Add(this.launchGame);
            this.splitContainer1.Panel2.Controls.Add(this.selectedGameName);
            this.splitContainer1.Panel2.Controls.Add(this.selectedGameArt);
            this.splitContainer1.Panel2.Controls.Add(this.playTimeContainer);
            this.splitContainer1.Size = new System.Drawing.Size(1064, 544);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 0;
            // 
            // gameList
            // 
            this.gameList.ContextMenuStrip = this.menu;
            this.gameList.Controls.Add(this.emptyLibraryNote);
            this.gameList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.gameList.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.gameList.FullRowSelect = true;
            this.gameList.HideSelection = false;
            this.gameList.Location = new System.Drawing.Point(0, 0);
            this.gameList.Margin = new System.Windows.Forms.Padding(4);
            this.gameList.Name = "gameList";
            this.gameList.ShowLines = false;
            this.gameList.Size = new System.Drawing.Size(352, 544);
            this.gameList.TabIndex = 0;
            this.gameList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.gameList_AfterSelect);
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGameButton,
            this.editGameButton,
            this.deleteGameButton});
            this.menu.Name = "contextMenuStrip1";
            this.menu.ShowImageMargin = false;
            this.menu.Size = new System.Drawing.Size(141, 76);
            // 
            // addGameButton
            // 
            this.addGameButton.Name = "addGameButton";
            this.addGameButton.Size = new System.Drawing.Size(140, 24);
            this.addGameButton.Text = "Add Game";
            this.addGameButton.Click += new System.EventHandler(this.addGameButton_Click);
            // 
            // editGameButton
            // 
            this.editGameButton.Enabled = false;
            this.editGameButton.Name = "editGameButton";
            this.editGameButton.Size = new System.Drawing.Size(140, 24);
            this.editGameButton.Text = "Edit Game";
            this.editGameButton.Click += new System.EventHandler(this.editGameButton_Click);
            // 
            // deleteGameButton
            // 
            this.deleteGameButton.Enabled = false;
            this.deleteGameButton.Name = "deleteGameButton";
            this.deleteGameButton.Size = new System.Drawing.Size(140, 24);
            this.deleteGameButton.Text = "Delete Game";
            this.deleteGameButton.Click += new System.EventHandler(this.deleteGameButton_Click);
            // 
            // emptyLibraryNote
            // 
            this.emptyLibraryNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.emptyLibraryNote.BackColor = System.Drawing.Color.Transparent;
            this.emptyLibraryNote.Location = new System.Drawing.Point(0, 0);
            this.emptyLibraryNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.emptyLibraryNote.Name = "emptyLibraryNote";
            this.emptyLibraryNote.Size = new System.Drawing.Size(349, 537);
            this.emptyLibraryNote.TabIndex = 1;
            this.emptyLibraryNote.Text = "Right-click to access menu";
            this.emptyLibraryNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // favoritestoggle
            // 
            this.favoritestoggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.favoritestoggle.Location = new System.Drawing.Point(575, 512);
            this.favoritestoggle.Margin = new System.Windows.Forms.Padding(4);
            this.favoritestoggle.Name = "favoritestoggle";
            this.favoritestoggle.Size = new System.Drawing.Size(129, 28);
            this.favoritestoggle.TabIndex = 4;
            this.favoritestoggle.Text = "Toggle Favorites";
            this.favoritestoggle.UseVisualStyleBackColor = true;
            this.favoritestoggle.Click += new System.EventHandler(this.favoritesToggle_Click);
            // 
            // launchGame
            // 
            this.launchGame.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.launchGame.Location = new System.Drawing.Point(47, 388);
            this.launchGame.Margin = new System.Windows.Forms.Padding(4);
            this.launchGame.MaximumSize = new System.Drawing.Size(173, 44);
            this.launchGame.Name = "launchGame";
            this.launchGame.Size = new System.Drawing.Size(173, 44);
            this.launchGame.TabIndex = 2;
            this.launchGame.Text = "Play";
            this.launchGame.UseVisualStyleBackColor = true;
            this.launchGame.Click += new System.EventHandler(this.launchGame_Click);
            // 
            // selectedGameName
            // 
            this.selectedGameName.AutoSize = true;
            this.selectedGameName.Dock = System.Windows.Forms.DockStyle.Left;
            this.selectedGameName.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.selectedGameName.Location = new System.Drawing.Point(0, 315);
            this.selectedGameName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectedGameName.Name = "selectedGameName";
            this.selectedGameName.Padding = new System.Windows.Forms.Padding(43, 20, 0, 0);
            this.selectedGameName.Size = new System.Drawing.Size(146, 57);
            this.selectedGameName.TabIndex = 1;
            this.selectedGameName.Text = "[Game]";
            // 
            // selectedGameArt
            // 
            this.selectedGameArt.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.selectedGameArt.Dock = System.Windows.Forms.DockStyle.Top;
            this.selectedGameArt.Location = new System.Drawing.Point(0, 0);
            this.selectedGameArt.Margin = new System.Windows.Forms.Padding(4);
            this.selectedGameArt.Name = "selectedGameArt";
            this.selectedGameArt.Size = new System.Drawing.Size(708, 315);
            this.selectedGameArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.selectedGameArt.TabIndex = 0;
            this.selectedGameArt.TabStop = false;
            // 
            // playTimeContainer
            // 
            this.playTimeContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playTimeContainer.AutoSize = true;
            this.playTimeContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.playTimeContainer.Controls.Add(this.playTimeLabel);
            this.playTimeContainer.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playTimeContainer.Location = new System.Drawing.Point(541, 332);
            this.playTimeContainer.Margin = new System.Windows.Forms.Padding(4);
            this.playTimeContainer.MaximumSize = new System.Drawing.Size(138, 61);
            this.playTimeContainer.MinimumSize = new System.Drawing.Size(138, 61);
            this.playTimeContainer.Name = "playTimeContainer";
            this.playTimeContainer.Padding = new System.Windows.Forms.Padding(4, 20, 27, 4);
            this.playTimeContainer.Size = new System.Drawing.Size(138, 61);
            this.playTimeContainer.TabIndex = 6;
            this.playTimeContainer.TabStop = false;
            this.playTimeContainer.Text = "Play Time";
            // 
            // playTimeLabel
            // 
            this.playTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playTimeLabel.Location = new System.Drawing.Point(8, 24);
            this.playTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.playTimeLabel.MaximumSize = new System.Drawing.Size(119, 33);
            this.playTimeLabel.MinimumSize = new System.Drawing.Size(119, 33);
            this.playTimeLabel.Name = "playTimeLabel";
            this.playTimeLabel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.playTimeLabel.Size = new System.Drawing.Size(119, 33);
            this.playTimeLabel.TabIndex = 0;
            this.playTimeLabel.Text = "Never";
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 544);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1082, 591);
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Launcher_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gameList.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectedGameArt)).EndInit();
            this.playTimeContainer.ResumeLayout(false);
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
        private System.Windows.Forms.Label emptyLibraryNote;
        private System.Windows.Forms.Button favoritestoggle;
        private GroupBox playTimeContainer;
        private Label playTimeLabel;
    }
}

