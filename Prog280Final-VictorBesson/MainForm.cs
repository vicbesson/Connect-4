using System;
using System.Windows.Forms;
using Prog280Final_VictorBesson.UserControls;
namespace Prog280Final_VictorBesson
{
    public partial class MainForm : Form
    {
        public UserControl currentControl;
        public MainForm()
        {
            InitializeComponent();
            currentControl = new StartControl(this);
            this.Controls.Add(currentControl);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            currentControl.Left = this.Width / 2 - currentControl.Width / 2;
            currentControl.Top = this.Height / 2 - currentControl.Height / 2;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(currentControl is HostControl || currentControl is GameControl)
            {
                if (currentControl is HostControl)
                    ((HostControl)currentControl).Closing();
                else
                    ((GameControl)currentControl).Closing();
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
