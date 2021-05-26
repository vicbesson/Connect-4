using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Prog280Final_VictorBesson.Client;
namespace Prog280Final_VictorBesson.UserControls
{
    public partial class JoinControl : UserControl
    {
        public PlayerClient c;
        private string hostName;
        public JoinControl(Form formpntr)
        {
            InitializeComponent();
            this.Left = formpntr.Width / 2 - this.Width / 2;
            this.Top = formpntr.Height / 2 - this.Height / 2;
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnJoin.Text == "Join")
                {
                    string[] temp = txtUrl.Text.Split(':');
                    int port = -1;
                    if (temp.Length != 2)
                        throw new Exception("Invalid Host");
                    if (int.TryParse(temp[1], out port) == false)
                        throw new Exception("Invalid Host");
                    if (port != 5000)
                        throw new Exception("Invalid Host");
                    IPAddress tempIP;
                    if (IPAddress.TryParse(temp[0], out tempIP) == false)
                        throw new Exception("Invalid Host");
                    if (txtName.Text.Trim() == "")
                        throw new Exception("Must Enter Username");
                    lblNameError.Text = "";
                    TcpClient client = new TcpClient();
                    var result = client.BeginConnect(temp[0], port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));
                    if (!success)
                        throw new Exception("Failed to connect to host");
                    c = new PlayerClient(client);
                    c.ReceivedMessage += C_ReceivedMessage;
                    lblWaiting.Visible = true;
                    btnJoin.Text = "Leave Lobby";
                }
                else
                {
                    btnJoin.Text = "Join";
                    if (c != null)
                    {
                        c.EndConnection();
                        c = null;
                    }
                    lblWaiting.Visible = false;
                }
            }
            catch(Exception ex)
            {
                if (ex.Message == "Must Enter Username" || ex.Message == "Username can not contain ','")
                    lblNameError.Text = ex.Message;
                else
                    MessageBox.Show(ex.Message);
            }
        }

        private void C_ReceivedMessage(string message)
        {
            string[] temp = message.Split(',');
            string command = "";
            if (temp.Length >= 2)
            {
                if (temp[0] == "Command" && temp[1].StartsWith("GameStartHost"))
                {
                    hostName = message.Substring(22, message.Length - 22);
                    c.SendMessage($"Command,GameStartClient,{txtName.Text}");
                }
                for (int i = 1; i < temp.Length; i++)
                {
                    if (i == temp.Length - 1)
                        command += temp[i];
                    else
                        command += temp[i] + ",";
                }
            }
            if (temp[0] == "Command")
            {
                switch (command)
                {
                    case "BeginGame":
                        c.ReceivedMessage -= C_ReceivedMessage;
                        UserControl uc = new GameControl((Form)this.Parent, c, hostName, txtName.Text);
                        ((MainForm)this.Parent).currentControl = uc;
                        this.Parent.Controls.Add(uc);
                        this.Parent.BackColor = Color.DimGray;
                        this.Parent.Controls.Remove(this);
                        this.Dispose();
                        break;
                    case "HostLeft":
                        MessageBox.Show("Host Left");
                        if (c != null)
                        {
                            c.EndConnection();
                            c = null;
                        }
                        btnJoin.Text = "Join";
                        lblWaiting.Visible = false;
                        break;
                }
                
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if(c != null)
            {
                c.EndConnection();
                c = null;
            }
            UserControl uc = new StartControl((Form)this.Parent);
            ((MainForm)this.Parent).currentControl = uc;
            this.Parent.Controls.Add(uc);
            this.Parent.BackColor = Color.FromArgb(231, 76, 60);
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }
    }
}
