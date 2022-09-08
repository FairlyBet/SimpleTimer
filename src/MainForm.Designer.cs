namespace SimpleTimer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TimeLabel = new System.Windows.Forms.Label();
            this.ControlButton = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // TimeLabel
            // 
            this.TimeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TimeLabel.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TimeLabel.Location = new System.Drawing.Point(0, 0);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(210, 90);
            this.TimeLabel.TabIndex = 0;
            this.TimeLabel.Text = "00:00";
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ControlButton
            // 
            this.ControlButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ControlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ControlButton.FlatAppearance.BorderSize = 4;
            this.ControlButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ControlButton.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ControlButton.Location = new System.Drawing.Point(0, 93);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Size = new System.Drawing.Size(210, 58);
            this.ControlButton.TabIndex = 1;
            this.ControlButton.UseVisualStyleBackColor = false;
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "SimpleTimer";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 151);
            this.Controls.Add(this.ControlButton);
            this.Controls.Add(this.TimeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Label TimeLabel;
        private Button ControlButton;
        private NotifyIcon NotifyIcon;
    }
}