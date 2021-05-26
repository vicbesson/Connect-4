namespace Prog280Final_VictorBesson.UserControls
{
    partial class GameControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUser = new System.Windows.Forms.Label();
            this.lblOpponent = new System.Windows.Forms.Label();
            this.pnBoard = new System.Windows.Forms.Panel();
            this.btnForfeit = new System.Windows.Forms.Button();
            this.pnMessages = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.btnLeave = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.Color.Black;
            this.lblUser.Location = new System.Drawing.Point(92, 543);
            this.lblUser.MaximumSize = new System.Drawing.Size(300, 0);
            this.lblUser.MinimumSize = new System.Drawing.Size(300, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(300, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Your Name";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOpponent
            // 
            this.lblOpponent.AutoSize = true;
            this.lblOpponent.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpponent.ForeColor = System.Drawing.Color.Black;
            this.lblOpponent.Location = new System.Drawing.Point(426, 543);
            this.lblOpponent.MaximumSize = new System.Drawing.Size(300, 0);
            this.lblOpponent.MinimumSize = new System.Drawing.Size(300, 0);
            this.lblOpponent.Name = "lblOpponent";
            this.lblOpponent.Size = new System.Drawing.Size(300, 23);
            this.lblOpponent.TabIndex = 1;
            this.lblOpponent.Text = "Opponent Name";
            this.lblOpponent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnBoard
            // 
            this.pnBoard.BackColor = System.Drawing.Color.Silver;
            this.pnBoard.Enabled = false;
            this.pnBoard.Location = new System.Drawing.Point(96, 0);
            this.pnBoard.Name = "pnBoard";
            this.pnBoard.Size = new System.Drawing.Size(630, 540);
            this.pnBoard.TabIndex = 2;
            // 
            // btnForfeit
            // 
            this.btnForfeit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            this.btnForfeit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnForfeit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForfeit.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForfeit.Location = new System.Drawing.Point(196, 579);
            this.btnForfeit.Name = "btnForfeit";
            this.btnForfeit.Size = new System.Drawing.Size(172, 46);
            this.btnForfeit.TabIndex = 4;
            this.btnForfeit.Text = "Forfeit";
            this.btnForfeit.UseVisualStyleBackColor = false;
            this.btnForfeit.Click += new System.EventHandler(this.btnForfeit_Click);
            // 
            // pnMessages
            // 
            this.pnMessages.AutoScroll = true;
            this.pnMessages.BackColor = System.Drawing.Color.White;
            this.pnMessages.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnMessages.Location = new System.Drawing.Point(752, 3);
            this.pnMessages.Name = "pnMessages";
            this.pnMessages.Size = new System.Drawing.Size(300, 495);
            this.pnMessages.TabIndex = 6;
            this.pnMessages.WrapContents = false;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(752, 504);
            this.txtMessage.MaxLength = 500;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(300, 76);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.Text = "";
            // 
            // btnLeave
            // 
            this.btnLeave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(231)))), ((int)(((byte)(141)))));
            this.btnLeave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLeave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeave.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeave.ForeColor = System.Drawing.Color.Black;
            this.btnLeave.Location = new System.Drawing.Point(456, 579);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(172, 46);
            this.btnLeave.TabIndex = 5;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = false;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSendMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMessage.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendMessage.ForeColor = System.Drawing.Color.White;
            this.btnSendMessage.Location = new System.Drawing.Point(941, 586);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(111, 42);
            this.btnSendMessage.TabIndex = 8;
            this.btnSendMessage.Text = "Send";
            this.btnSendMessage.UseVisualStyleBackColor = false;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.ForeColor = System.Drawing.Color.Yellow;
            this.lblTimer.Location = new System.Drawing.Point(385, 594);
            this.lblTimer.MaximumSize = new System.Drawing.Size(50, 0);
            this.lblTimer.MinimumSize = new System.Drawing.Size(50, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(50, 21);
            this.lblTimer.TabIndex = 9;
            this.lblTimer.Text = "30";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pnMessages);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnForfeit);
            this.Controls.Add(this.pnBoard);
            this.Controls.Add(this.lblOpponent);
            this.Controls.Add(this.lblUser);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameControl";
            this.Size = new System.Drawing.Size(1165, 678);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblOpponent;
        private System.Windows.Forms.Panel pnBoard;
        private System.Windows.Forms.Button btnForfeit;
        private System.Windows.Forms.FlowLayoutPanel pnMessages;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Label lblTimer;
    }
}
