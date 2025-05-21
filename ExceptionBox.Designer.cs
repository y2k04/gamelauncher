namespace GameLauncher
{
    partial class ExceptionBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionBox));
            this.stackTraceText = new System.Windows.Forms.TextBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.githubIssuesLink = new System.Windows.Forms.LinkLabel();
            this.btnMoreDetails = new System.Windows.Forms.Button();
            this.stackTraceContainer = new System.Windows.Forms.GroupBox();
            this.btnBreakInDebugger = new System.Windows.Forms.Button();
            this.stackTraceContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // stackTraceText
            // 
            this.stackTraceText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stackTraceText.BackColor = System.Drawing.SystemColors.Control;
            this.stackTraceText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stackTraceText.Location = new System.Drawing.Point(6, 14);
            this.stackTraceText.Multiline = true;
            this.stackTraceText.Name = "stackTraceText";
            this.stackTraceText.ReadOnly = true;
            this.stackTraceText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.stackTraceText.Size = new System.Drawing.Size(717, 358);
            this.stackTraceText.TabIndex = 0;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(616, 458);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(125, 23);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(525, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "An unexpected error occured in the application!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(594, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Well, that was unexpected. We\'re sorry that you\'ve experienced this problem! You " +
    "can report this problem at our\r\n";
            // 
            // githubIssuesLink
            // 
            this.githubIssuesLink.AutoSize = true;
            this.githubIssuesLink.BackColor = System.Drawing.Color.Transparent;
            this.githubIssuesLink.Location = new System.Drawing.Point(600, 45);
            this.githubIssuesLink.Name = "githubIssuesLink";
            this.githubIssuesLink.Size = new System.Drawing.Size(70, 15);
            this.githubIssuesLink.TabIndex = 4;
            this.githubIssuesLink.TabStop = true;
            this.githubIssuesLink.Text = "issues page.";
            this.githubIssuesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.githubIssuesLink_LinkClicked);
            // 
            // btnMoreDetails
            // 
            this.btnMoreDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoreDetails.Location = new System.Drawing.Point(485, 458);
            this.btnMoreDetails.Name = "btnMoreDetails";
            this.btnMoreDetails.Size = new System.Drawing.Size(125, 23);
            this.btnMoreDetails.TabIndex = 6;
            this.btnMoreDetails.Text = ">> More Details";
            this.btnMoreDetails.UseVisualStyleBackColor = true;
            this.btnMoreDetails.Click += new System.EventHandler(this.btnMoreDetails_Click);
            // 
            // stackTraceContainer
            // 
            this.stackTraceContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stackTraceContainer.Controls.Add(this.stackTraceText);
            this.stackTraceContainer.Location = new System.Drawing.Point(12, 74);
            this.stackTraceContainer.Name = "stackTraceContainer";
            this.stackTraceContainer.Size = new System.Drawing.Size(729, 378);
            this.stackTraceContainer.TabIndex = 7;
            this.stackTraceContainer.TabStop = false;
            this.stackTraceContainer.Text = "Error details";
            // 
            // btnBreakInDebugger
            // 
            this.btnBreakInDebugger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBreakInDebugger.Location = new System.Drawing.Point(12, 458);
            this.btnBreakInDebugger.Name = "btnBreakInDebugger";
            this.btnBreakInDebugger.Size = new System.Drawing.Size(137, 23);
            this.btnBreakInDebugger.TabIndex = 8;
            this.btnBreakInDebugger.Text = "Break in debugger";
            this.btnBreakInDebugger.UseVisualStyleBackColor = true;
            this.btnBreakInDebugger.Click += new System.EventHandler(this.btnBreakInDebugger_Click);
            // 
            // ExceptionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 493);
            this.Controls.Add(this.btnBreakInDebugger);
            this.Controls.Add(this.stackTraceContainer);
            this.Controls.Add(this.btnMoreDetails);
            this.Controls.Add(this.githubIssuesLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnQuit);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fatal Error occured!";
            this.Load += new System.EventHandler(this.ExceptionBox_Load);
            this.SizeChanged += new System.EventHandler(this.ExceptionBox_SizeChanged);
            this.stackTraceContainer.ResumeLayout(false);
            this.stackTraceContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stackTraceText;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel githubIssuesLink;
        private System.Windows.Forms.Button btnMoreDetails;
        private System.Windows.Forms.GroupBox stackTraceContainer;
        private System.Windows.Forms.Button btnBreakInDebugger;
    }
}