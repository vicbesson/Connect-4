using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Prog280Final_VictorBesson.Server;
namespace Prog280Final_VictorBesson.UserControls
{
    public partial class HostControl : UserControl
    {
        private static string servername = Environment.MachineName;
        private PlayerServer myServer;
        private const int port = 5000;
        private string url = "";
        private string clientName;
        public HostControl(Form formpntr)
        {
            InitializeComponent();
            this.Left = formpntr.Width / 2 - this.Width / 2;
            this.Top = formpntr.Height / 2 - this.Height / 2;
            try
            {
                url = Dns.GetHostEntry(servername).AddressList.Where(
                x => x.AddressFamily == AddressFamily.InterNetwork).ToList()[0].ToString() + ":" + "5000";
                txtUrl.Text = url;
                string[] delimiter = url.Split(':');
                IPAddress tempIP;
                IPAddress.TryParse(delimiter[0], out tempIP);
                if (tempIP != null)
                {
                    TcpListener listener = new TcpListener(tempIP, Convert.ToInt32(delimiter[1]));
                    myServer = new PlayerServer(listener);
                    myServer.PlayerMessage += MyServer_PlayerMessage;
                }
                else
                    throw new Exception("Unexpected Error");
            }
            catch(Exception ex)
            {
                if(ex.Message != "Unexpected Error")
                    MessageBox.Show("Can Only Host Once Per-Machine");
                else
                    MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void MyServer_PlayerMessage(string message)
        {
            // && temp[0] == "GameStartClient"
            string[] temp = message.Split(',');
            string command = "";
            if (temp.Length >= 2)
            {
                if(temp[0] == "Command" && temp[1].StartsWith("GameStartClient"))
                {
                    clientName = message.Substring(24, message.Length - 24);
                    myServer.EnqueueMessage("Command,BeginGame");
                }
                for(int i = 1; i < temp.Length; i++)
                {
                    if (i == temp.Length - 1)
                        command += temp[i];
                    else
                        command += temp[i] + ",";
                }
            }
            if(temp[0] == "Command")
            {
                switch (command)
                {
                    case "BeginGame":
                        myServer.PlayerMessage -= MyServer_PlayerMessage;
                        UserControl uc = new GameControl((Form)this.Parent, myServer, txtName.Text, clientName);
                        ((MainForm)this.Parent).currentControl = uc;
                        this.Parent.Controls.Add(uc);
                        this.Parent.BackColor = Color.DimGray;
                        this.Parent.Controls.Remove(this);
                        this.Dispose();
                        break;
                    case "PlayerJoined":
                        lblWaiting.Text = "Opponent Has Joined!";
                        btnReady.BackColor = Color.FromArgb(255, 126, 126);
                        if (btnReady.Text == "Unready")
                            myServer.EnqueueMessage($"GameStartHost,{txtName.Text}");
                        break;
                    case "OpponentLeft":
                        lblWaiting.Text = "Waiting For Opponent";
                        btnReady.BackColor = Color.FromArgb(116, 231, 141);
                        break;
                }
            }
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "")
                    throw new Exception("Must Enter Username");
                lblNameError.Text = "";
                if(btnReady.Text == "Ready")
                {
                    btnReady.Text = "Unready";
                    myServer.EnqueueMessage($"Command,GameStartHost,{txtName.Text}");
                }
                else
                    btnReady.Text = "Ready";
                
            }
            catch(Exception ex)
            {
                lblNameError.Text = ex.Message;
            }
           
        }
        public void Closing()
        {
            myServer.EnqueueMessage("Command,HostLeft");
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            PlayerServer.Listening = false;
            PlayerServer.connected = false;
            myServer = null;
            UserControl uc = new StartControl((Form)this.Parent);
            ((MainForm)this.Parent).currentControl = uc;
            this.Parent.Controls.Add(uc);
            this.Parent.BackColor = Color.FromArgb(231, 76, 60);
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }
    }
}
