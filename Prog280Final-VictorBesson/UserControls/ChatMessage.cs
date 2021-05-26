using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prog280Final_VictorBesson.UserControls
{
    public partial class ChatMessage : UserControl
    {
        public ChatMessage(string message, Panel panelpntr)
        {
            InitializeComponent();
            lblMessage.Text = $"{message}";
            lblMessage.MaximumSize = new Size(panelpntr.Width - 20, 0);
            this.Width = panelpntr.Width - 20;
            this.Height = lblMessage.Height + 10;
            lblMessage.Left = 0;
            lblMessage.Top = this.Height / 2 - lblMessage.Height / 2;
            panelpntr.Controls.Add(this);
        }
    }
}
