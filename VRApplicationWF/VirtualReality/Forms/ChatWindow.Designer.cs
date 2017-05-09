namespace VRApplicationWF.VirtualReality.Forms
{
    partial class ChatWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.ClientApplicationLabel = new System.Windows.Forms.Label();
            this.qTechLogo = new System.Windows.Forms.PictureBox();
            this.BroadcastLabel = new System.Windows.Forms.Label();
            this.PrivateChatLabel = new System.Windows.Forms.Label();
            this.ChatBoxClient = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.BroadCastTextBox = new System.Windows.Forms.ListBox();
            this.PrivateTextBox = new System.Windows.Forms.ListBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qTechLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.Color.Black;
            this.TopPanel.Controls.Add(this.button2);
            this.TopPanel.Controls.Add(this.button1);
            this.TopPanel.Controls.Add(this.minimizeButton);
            this.TopPanel.Controls.Add(this.exitButton);
            this.TopPanel.Controls.Add(this.ClientApplicationLabel);
            this.TopPanel.Controls.Add(this.qTechLogo);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(1325, 57);
            this.TopPanel.TabIndex = 0;
            this.TopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TopPanel_MouseDown);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Image = global::VRApplicationWF.Properties.Resources.stop_32;
            this.button2.Location = new System.Drawing.Point(676, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 40);
            this.button2.TabIndex = 6;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::VRApplicationWF.Properties.Resources.play_32;
            this.button1.Location = new System.Drawing.Point(630, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("minimizeButton.BackgroundImage")));
            this.minimizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minimizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.minimizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(1)))));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Location = new System.Drawing.Point(1243, 12);
            this.minimizeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(27, 27);
            this.minimizeButton.TabIndex = 4;
            this.minimizeButton.UseVisualStyleBackColor = true;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exitButton.BackgroundImage")));
            this.exitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(1)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Location = new System.Drawing.Point(1277, 12);
            this.exitButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(27, 27);
            this.exitButton.TabIndex = 3;
            this.exitButton.UseMnemonic = false;
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // ClientApplicationLabel
            // 
            this.ClientApplicationLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientApplicationLabel.ForeColor = System.Drawing.Color.White;
            this.ClientApplicationLabel.Location = new System.Drawing.Point(61, 0);
            this.ClientApplicationLabel.Name = "ClientApplicationLabel";
            this.ClientApplicationLabel.Size = new System.Drawing.Size(315, 57);
            this.ClientApplicationLabel.TabIndex = 2;
            this.ClientApplicationLabel.Text = "Client  -  Control Panel";
            this.ClientApplicationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // qTechLogo
            // 
            this.qTechLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("qTechLogo.BackgroundImage")));
            this.qTechLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.qTechLogo.Location = new System.Drawing.Point(9, 6);
            this.qTechLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.qTechLogo.Name = "qTechLogo";
            this.qTechLogo.Size = new System.Drawing.Size(52, 48);
            this.qTechLogo.TabIndex = 1;
            this.qTechLogo.TabStop = false;
            this.qTechLogo.Click += new System.EventHandler(this.qTechLogo_Click);
            // 
            // BroadcastLabel
            // 
            this.BroadcastLabel.AutoSize = true;
            this.BroadcastLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BroadcastLabel.Location = new System.Drawing.Point(16, 73);
            this.BroadcastLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BroadcastLabel.Name = "BroadcastLabel";
            this.BroadcastLabel.Size = new System.Drawing.Size(115, 22);
            this.BroadcastLabel.TabIndex = 3;
            this.BroadcastLabel.Text = "Broadcasts";
            // 
            // PrivateChatLabel
            // 
            this.PrivateChatLabel.AutoSize = true;
            this.PrivateChatLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrivateChatLabel.Location = new System.Drawing.Point(16, 320);
            this.PrivateChatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PrivateChatLabel.Name = "PrivateChatLabel";
            this.PrivateChatLabel.Size = new System.Drawing.Size(123, 22);
            this.PrivateChatLabel.TabIndex = 5;
            this.PrivateChatLabel.Text = "Private Chat";
            // 
            // ChatBoxClient
            // 
            this.ChatBoxClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatBoxClient.Location = new System.Drawing.Point(16, 572);
            this.ChatBoxClient.Margin = new System.Windows.Forms.Padding(4);
            this.ChatBoxClient.Multiline = true;
            this.ChatBoxClient.Name = "ChatBoxClient";
            this.ChatBoxClient.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatBoxClient.Size = new System.Drawing.Size(1163, 111);
            this.ChatBoxClient.TabIndex = 6;
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(108)))), ((int)(((byte)(1)))));
            this.SendButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendButton.Location = new System.Drawing.Point(1188, 572);
            this.SendButton.Margin = new System.Windows.Forms.Padding(4);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(121, 112);
            this.SendButton.TabIndex = 7;
            this.SendButton.Text = "SEND";
            this.SendButton.UseVisualStyleBackColor = false;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // BroadCastTextBox
            // 
            this.BroadCastTextBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BroadCastTextBox.FormattingEnabled = true;
            this.BroadCastTextBox.ItemHeight = 16;
            this.BroadCastTextBox.Location = new System.Drawing.Point(12, 109);
            this.BroadCastTextBox.Name = "BroadCastTextBox";
            this.BroadCastTextBox.Size = new System.Drawing.Size(1292, 196);
            this.BroadCastTextBox.TabIndex = 8;
            // 
            // PrivateTextBox
            // 
            this.PrivateTextBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.PrivateTextBox.FormattingEnabled = true;
            this.PrivateTextBox.ItemHeight = 16;
            this.PrivateTextBox.Location = new System.Drawing.Point(12, 354);
            this.PrivateTextBox.Name = "PrivateTextBox";
            this.PrivateTextBox.Size = new System.Drawing.Size(1292, 196);
            this.PrivateTextBox.TabIndex = 9;
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1325, 699);
            this.Controls.Add(this.PrivateTextBox);
            this.Controls.Add(this.BroadCastTextBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ChatBoxClient);
            this.Controls.Add(this.PrivateChatLabel);
            this.Controls.Add(this.BroadcastLabel);
            this.Controls.Add(this.TopPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ChatWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.qTechLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.PictureBox qTechLogo;
        private System.Windows.Forms.Label ClientApplicationLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Label BroadcastLabel;
        private System.Windows.Forms.Label PrivateChatLabel;
        private System.Windows.Forms.TextBox ChatBoxClient;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ListBox BroadCastTextBox;
        private System.Windows.Forms.ListBox PrivateTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer;
    }
}