using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prog280Final_VictorBesson.UserControls
{
    public partial class StartControl : UserControl
    {
        public StartControl(Form formpntr)
        {
            InitializeComponent();
            this.Left = formpntr.Width / 2 - this.Width / 2;
            this.Top = formpntr.Height / 2 - this.Height / 2;
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            UserControl uc = new JoinControl((Form)this.Parent);
            ((MainForm)this.Parent).currentControl = uc;
            this.Parent.Controls.Add(uc);
            this.Parent.BackColor = Color.FromArgb(148, 239, 243);
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        private void btnHost_Click(object sender, EventArgs e)
        {
            UserControl uc = new HostControl((Form)this.Parent);
            ((MainForm)this.Parent).currentControl = uc;
            if (uc != null)
            {
                this.Parent.Controls.Add(uc);
                this.Parent.BackColor = Color.FromArgb(150, 248, 172);
                this.Parent.Controls.Remove(this);
            }
            this.Dispose();
        }
    }
}
