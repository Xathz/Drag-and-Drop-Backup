namespace DragAndDropBackup {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.InfoLabel = new System.Windows.Forms.Label();
            this.BackupWorkingProgressBar = new System.Windows.Forms.ProgressBar();
            this.DoBackupBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.InfoLabel.Location = new System.Drawing.Point(0, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(120, 61);
            this.InfoLabel.TabIndex = 0;
            this.InfoLabel.Text = "Drag and drop files here to add to the backup";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.InfoLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(InfoLabel_MouseDown);
            this.InfoLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(InfoLabel_MouseUp);
            this.InfoLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(InfoLabel_MouseMove);
            // 
            // BackupWorkingProgressBar
            // 
            this.BackupWorkingProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackupWorkingProgressBar.Location = new System.Drawing.Point(0, 0);
            this.BackupWorkingProgressBar.MarqueeAnimationSpeed = 20;
            this.BackupWorkingProgressBar.Name = "BackupWorkingProgressBar";
            this.BackupWorkingProgressBar.Size = new System.Drawing.Size(120, 61);
            this.BackupWorkingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.BackupWorkingProgressBar.TabIndex = 1;
            // 
            // DoBackupBackgroundWorker
            // 
            this.DoBackupBackgroundWorker.WorkerReportsProgress = true;
            this.DoBackupBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoBackupBackgroundWorker_DoWork);
            this.DoBackupBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DoBackupBackgroundWorker_ProgressChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 61);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.BackupWorkingProgressBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Backup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(MainForm_DragEnter);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ProgressBar BackupWorkingProgressBar;
        private System.ComponentModel.BackgroundWorker DoBackupBackgroundWorker;
    }
}

