using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLauncher.Util;

namespace GameLauncher
{
    public partial class ExceptionBox : Form
    {
        private Exception _ex;

        private Size _fullSize = new(769, 300);
        private Size _normalSize = new(769, 145);
        private bool _moreDetails = false;

        public ExceptionBox(Exception ex)
        {
            InitializeComponent();
            _ex = ex;

            this.btnBreakInDebugger.Visible = Debugger.IsAttached;

            if (_moreDetails)
            {
                this.Size = _fullSize;
                this.stackTraceContainer.Visible = true;

                this.btnMoreDetails.Text = "<< Collapse";
            }
            else
            {
                this.Size = _normalSize;
                this.stackTraceContainer.Visible = false;
                this.btnMoreDetails.Text = ">> More details";
            }
        }

        private void AnimateSize(Size property, Size newProperty)
        {
            AnimateSize(property, newProperty, EasingFunctions.EaseInOut, 300);
        }

        private void AnimateSize(Size startSize, Size targetSize, Func<double, double> easingFunction, int duration = 500)
        {
            Task.Run(async () =>
            {
                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < duration)
                {
                    double progress = (double)stopwatch.ElapsedMilliseconds / duration;
                    progress = Math.Min(progress, 1.0); // Clamp progress to 1.0

                    double easedProgress = easingFunction(progress);

                    int newWidth = (int)(startSize.Width + (targetSize.Width - startSize.Width) * easedProgress);
                    int newHeight = (int)(startSize.Height + (targetSize.Height - startSize.Height) * easedProgress);

                    this.Invoke(() => this.Size = new Size(newWidth, newHeight));
                    this.Invoke(() => this.Refresh());

                    await Task.Delay(10); // Adjust for smoothness
                }

                // Ensure final size is set
                this.Invoke(() => this.Size = targetSize);
            });
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();

            Application.Exit();
        }

        private void ExceptionBox_Load(object sender, EventArgs e)
        {
            this.stackTraceText.Text = _ex.ToString();
        }

        private void githubIssuesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://github.com/y2k04/gamelauncher/issues",
                UseShellExecute = true
            });
        }

        private void btnMoreDetails_Click(object sender, EventArgs e)
        {
            if (!_moreDetails)
            {
                this.stackTraceContainer.Visible = true;
                AnimateSize(this.Size, _fullSize);
                this.btnMoreDetails.Text = "<< Collapse";

                _moreDetails = true;
            }
            else
            {
                AnimateSize(this.Size, _normalSize);
                this.btnMoreDetails.Text = ">> More details";
                this.stackTraceContainer.Visible = false;

                _moreDetails = false;
            }
        }

        private void ExceptionBox_SizeChanged(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        [DebuggerHidden]
        private void btnBreakInDebugger_Click(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                LoggingUtil.Debug("Sending breakpoint to debugger.");
                Debugger.Break();
            }
        }
    }
}
