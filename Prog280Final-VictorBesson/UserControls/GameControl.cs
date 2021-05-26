using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Prog280Final_VictorBesson.UserControls
{
    public partial class GameControl : UserControl //I know the doubling up of stuff could be done using single methods but someone once told me if you do some 3 or times you should do that not 2.
    {//Some of the copies are different, just tried to get everything done.
        Client.PlayerClient c;
        Server.PlayerServer myServer;
        Game.GameState myGame;
        bool IsServer = false;
        bool IsGameLive = false;
        string PlayerName;
        bool IsGameOver = false;
        string OpponentName;
        int HostWinCount = 0;
        int ClientWinCount = 0;
        int GameTime = 30;
        List<Panel> Rows = new List<Panel>();
        Timer myTimer;
        public GameControl(Form formpntr, object client, string hostName, string clientName)
        {
            InitializeComponent();
            if (client is Prog280Final_VictorBesson.Client.PlayerClient)
            {
                c = (Prog280Final_VictorBesson.Client.PlayerClient)client;
                c.ReceivedMessage += C_ReceivedMessage;
                lblOpponent.Text = hostName + " - " + HostWinCount.ToString();
                lblUser.Text = clientName + "(You) - " + ClientWinCount.ToString();
                IsServer = false;
                PlayerName = clientName;
                OpponentName = hostName;
                for (int i = 0; i < 7; i++)
                {
                    Panel p = new Panel();
                    p.Cursor = Cursors.Hand;
                    p.BorderStyle = BorderStyle.FixedSingle;
                    p.Height = pnBoard.Height;
                    p.Width = pnBoard.Width / 7;
                    p.Left = i * pnBoard.Width / 7;
                    p.Top = 0;
                    p.Click += P_Click;
                    p.Name = $"Client{i}";
                    Rows.Add(p);
                    pnBoard.Controls.Add(p);
                }
            }
            else
            {
                myTimer = new Timer();
                myTimer.Enabled = false;
                myTimer.Tick += MyTimer_Tick;
                myTimer.Interval = 1000;
                myServer = (Prog280Final_VictorBesson.Server.PlayerServer)client;
                myServer.PlayerMessage += MyServer_PlayerMessage;
                lblUser.Text = hostName + "(You) - " + HostWinCount.ToString(); 
                lblOpponent.Text = clientName + " - " + ClientWinCount.ToString(); 
                IsServer = true;
                PlayerName = hostName;
                OpponentName = clientName;
                myGame = new Game.GameState(new Game.Player(-1), new Game.Player(1));
                if(myGame.GetNextPlayerName() == "Host")
                    myServer.EnqueueMessage("Command,HostTurn");
                else
                    myServer.EnqueueMessage("Command,ClientTurn");
                for (int i = 0; i < 7; i++)
                {
                    Panel p = new Panel();
                    p.Cursor = Cursors.Hand;
                    p.BorderStyle = BorderStyle.FixedSingle;
                    p.Height = pnBoard.Height;
                    p.Width = pnBoard.Width / 7;
                    p.Left = i * pnBoard.Width / 7;
                    p.Top = 0;
                    p.Click += P_Click;
                    p.Name = $"Host{i}";
                    Rows.Add(p);
                    pnBoard.Controls.Add(p);
                }
                myTimer.Enabled = true;
            }
            formpntr.Width = this.Width;
            formpntr.Height = this.Height + 20;
            this.Left = formpntr.Width / 2 - this.Width / 2;
            this.Top = formpntr.Height / 2 - this.Height / 2;
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            GameTime--;
            if(GameTime == 0)
            {
                Tuple<int,int> move = myGame.getAvailableMoves().FirstOrDefault();
                GameTime = 30;
                myServer.EnqueueMessage($"Move,{move.Item1}");
                myServer.EnqueueMessage($"Timer,{GameTime}");
            }
            else
                myServer.EnqueueMessage($"Timer,{GameTime}");
        }

        private void P_Click(object sender, EventArgs e)
        {
            if(((Panel)sender).Name.StartsWith("Host"))
            {
                string move = ((Panel)sender).Name.Substring(4, ((Panel)sender).Name.Length - 4);
                myServer.EnqueueMessage($"Move,{move}");
            }
            else
            {
                string move = ((Panel)sender).Name.Substring(6, ((Panel)sender).Name.Length - 6);
                c.SendMessage($"Move,{move}");
            }
        }

        #region PlayerClient
        private void C_ReceivedMessage(string message)
        {
            string[] temp = message.Split(',');
            string incomingMessage = "";
            if (temp.Length >= 2)
            {
                for (int i = 1; i < temp.Length; i++)
                {
                    if (i == temp.Length - 1)
                        incomingMessage += temp[i];
                    else
                        incomingMessage += temp[i] + ",";
                }
            }
            if (temp[0] == "Command")
            {
                switch (temp[1])
                {
                    case "HostLeft":
                        lblTimer.Text = "";
                        MessageBox.Show("Host Left");
                        System.Threading.Thread.Sleep(200);
                        if (c != null)
                        {
                            c.EndConnection();
                            c = null;
                        }
                        UserControl uc = new JoinControl((Form)this.Parent);
                        ((MainForm)this.Parent).currentControl = uc;
                        this.Parent.Controls.Add(uc);
                        this.Parent.BackColor = Color.FromArgb(148, 239, 243);
                        this.Parent.Controls.Remove(this);
                        break;
                    case "HostTurn":
                        if (IsGameOver == false)
                        {
                            btnForfeit.Text = "Forfeit";
                            btnForfeit.Enabled = true;
                            pnBoard.Enabled = false;
                            IsGameLive = false;
                            lblUser.ForeColor = Color.Black;
                            lblOpponent.ForeColor = Color.White;
                        }
                        break;
                    case "ClientTurn":
                        if (IsGameOver == false)
                        {
                            btnForfeit.Text = "Forfeit";
                            btnForfeit.Enabled = true;
                            pnBoard.Enabled = true;
                            IsGameLive = true;
                            lblOpponent.ForeColor = Color.Black;
                            lblUser.ForeColor = Color.White;
                        }
                        break;
                    case "HostForfeit":
                        lblTimer.Text = "";
                        ClientWinCount++;
                        lblUser.Text = PlayerName + "(You) - " + ClientWinCount.ToString();
                        c.SendMessage($"Message,{OpponentName} Forfeited!");
                        btnForfeit.Text = "Rematch";
                        break;
                    case "Rematch":
                        if (btnForfeit.Enabled == false)
                        {
                            DialogResult dialogResult = MessageBox.Show("Do You Want To Accept?", "Rematch?", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                c.SendMessage("Command,HostAccepted");
                                IsGameOver = false;
                                lblTimer.Text = "30";
                                Rows.Clear();
                                pnBoard.Controls.Clear();
                                for (int i = 0; i < 7; i++)
                                {
                                    Panel p = new Panel();
                                    p.Cursor = Cursors.Hand;
                                    p.BorderStyle = BorderStyle.FixedSingle;
                                    p.Height = pnBoard.Height;
                                    p.Width = pnBoard.Width / 7;
                                    p.Left = i * pnBoard.Width / 7;
                                    p.Top = 0;
                                    p.Click += P_Click;
                                    p.Name = $"Client{i}";
                                    Rows.Add(p);
                                    pnBoard.Controls.Add(p);
                                }
                            }
                            else if (dialogResult == DialogResult.No)
                                c.SendMessage("Command,HostDeclined");
                            else
                                c.SendMessage("Command,HostDeclined");
                        }
                        else
                            btnForfeit.Enabled = false;
                        break;
                    case "GameOver":
                        lblTimer.Text = "";
                        lblUser.ForeColor = Color.White;
                        lblOpponent.ForeColor = Color.White;
                        switch (temp[2])
                        {
                            case "Host":
                                HostWinCount++;
                                lblOpponent.Text = OpponentName + " - " + HostWinCount.ToString();
                                btnForfeit.Text = "Rematch";
                                btnForfeit.Enabled = false;
                                break;
                            case "Client":
                                ClientWinCount++;
                                lblUser.Text = PlayerName + "(You) - " + ClientWinCount.ToString();
                                btnForfeit.Text = "Rematch";
                                break;
                            case "Tie":
                                btnForfeit.Text = "Rematch";
                                if (IsGameLive == false)
                                    btnForfeit.Enabled = false;
                                break;
                        }
                        IsGameOver = true;
                        pnBoard.Enabled = false;
                        break;
                    case "ClientAccepted":
                        IsGameOver = false;
                        lblTimer.Text = "30";
                        Rows.Clear();
                        pnBoard.Controls.Clear();
                        for (int i = 0; i < 7; i++)
                        {
                            Panel p = new Panel();
                            p.Cursor = Cursors.Hand;
                            p.BorderStyle = BorderStyle.FixedSingle;
                            p.Height = pnBoard.Height;
                            p.Width = pnBoard.Width / 7;
                            p.Left = i * pnBoard.Width / 7;
                            p.Top = 0;
                            p.Click += P_Click;
                            p.Name = $"Client{i}";
                            Rows.Add(p);
                            pnBoard.Controls.Add(p);
                        }
                        break;
                    case "ClientDeclined":
                        if (btnForfeit.Enabled)
                            MessageBox.Show("Opponent Declined Rematch");
                        System.Threading.Thread.Sleep(200);
                        if (c != null)
                        {
                            c.EndConnection();
                            c = null;
                        }
                        UserControl j = new JoinControl((Form)this.Parent);
                        ((MainForm)this.Parent).currentControl = j;
                        this.Parent.Controls.Add(j);
                        this.Parent.BackColor = Color.FromArgb(148, 239, 243);
                        this.Parent.Controls.Remove(this);
                        break;
                }
            }
            else if(temp[0] == "Message")
            {
                ChatMessage cm = new ChatMessage(incomingMessage, pnMessages);
                pnMessages.ScrollControlIntoView(cm);
            }
            else if (temp[0] == "Move")
            {
                lblTimer.Text = "30";
                int tilecount = 0;
                tilecount = Rows[int.Parse(temp[1])].Controls.Count;
                if (tilecount < 6)
                {
                    PictureBox pb = new PictureBox();
                    pb.Enabled = false;
                    pb.Height = Rows[int.Parse(temp[1])].Height / 6;
                    pb.Width = Rows[int.Parse(temp[1])].Width;
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, pb.Width - 3, pb.Height - 3);
                    Region rg = new Region(gp);
                    pb.Region = rg;
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.Left = 0;
                    pb.Top = Rows[int.Parse(temp[1])].Height - (pb.Height * (tilecount + 1));
                    if (pnBoard.Enabled)
                        pb.BackColor = Color.Yellow;
                    else
                        pb.BackColor = Color.Red;
                    Rows[int.Parse(temp[1])].Controls.Add(pb);
                    tilecount++;
                    if (tilecount >= 6)
                    {
                        Rows[int.Parse(temp[1])].Click -= P_Click;
                        Rows[int.Parse(temp[1])].Cursor = Cursors.Default;
                    }
                    pnBoard.Enabled = false;
                    if (IsGameLive)
                        c.SendMessage("Command,HostTurn");
                }
            }
            else if(temp[0] == "Timer")
                lblTimer.Text = temp[1];
        }
        #endregion

        #region Server
        public void Closing()
        {
            if (myServer != null)
                myServer.EnqueueMessage("Command,HostLeft");
        }

        private void MyServer_PlayerMessage(string message)
        {
            string[] temp = message.Split(',');
            string incomingMessage = "";
            if (temp.Length >= 2)
            {
                for (int i = 1; i < temp.Length; i++)
                {
                    if (i == temp.Length - 1)
                        incomingMessage += temp[i];
                    else
                        incomingMessage += temp[i] + ",";
                }
            }
            if (temp[0] == "Command")
            {
                switch (temp[1])
                {
                    case "OpponentLeft":
                        myTimer.Enabled = false;
                        myServer.PlayerMessage -= MyServer_PlayerMessage;
                        Prog280Final_VictorBesson.Server.PlayerServer.connected = false;
                        Prog280Final_VictorBesson.Server.PlayerServer.Listening = false;
                        MessageBox.Show("Opponent Left");
                        System.Threading.Thread.Sleep(200);
                        UserControl uc = new HostControl((Form)this.Parent);
                        ((MainForm)this.Parent).currentControl = uc;
                        this.Parent.Controls.Add(uc);
                        this.Parent.BackColor = Color.FromArgb(150, 248, 172);
                        this.Parent.Controls.Remove(this);
                        break;
                    case "OpponentForfeit":
                        myTimer.Enabled = false;
                        HostWinCount++;
                        lblUser.Text = PlayerName + "(You) - " + HostWinCount.ToString();
                        lblTimer.Text = "";
                        myServer.EnqueueMessage($"Message,{OpponentName} Forfeited!");
                        btnForfeit.Text = "Rematch";
                        break;
                    case "HostTurn":
                        if(IsGameOver == false)
                        {
                            btnForfeit.Text = "Forfeit";
                            btnForfeit.Enabled = true;
                            pnBoard.Enabled = true;
                            IsGameLive = true;
                            lblUser.ForeColor = Color.White; 
                            lblOpponent.ForeColor = Color.Black;
                        }
                        break;
                    case "ClientTurn":
                        if (IsGameOver == false)
                        {
                            btnForfeit.Text = "Forfeit";
                            btnForfeit.Enabled = true;
                            pnBoard.Enabled = false;
                            IsGameLive = false;
                            lblOpponent.ForeColor = Color.White;
                            lblUser.ForeColor = Color.Black;
                        }
                        break;
                    case "Rematch":
                        if (btnForfeit.Enabled == false)
                        {
                            DialogResult dialogResult = MessageBox.Show("Do You Want To Accept?", "Rematch?", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                myServer.EnqueueMessage("Command,ClientAccepted");
                                IsGameOver = false;
                                lblTimer.Text = "30";
                                myTimer.Enabled = true;
                                Rows.Clear();
                                pnBoard.Controls.Clear();
                                for (int i = 0; i < 7; i++)
                                {
                                    Panel p = new Panel();
                                    p.Cursor = Cursors.Hand;
                                    p.BorderStyle = BorderStyle.FixedSingle;
                                    p.Height = pnBoard.Height;
                                    p.Width = pnBoard.Width / 7;
                                    p.Left = i * pnBoard.Width / 7;
                                    p.Top = 0;
                                    p.Click += P_Click;
                                    p.Name = $"Host{i}";
                                    Rows.Add(p);
                                    pnBoard.Controls.Add(p);
                                }
                                myGame = new Game.GameState(new Game.Player(-1), new Game.Player(1));
                                if (myGame.GetNextPlayerName() == "Host")
                                    myServer.EnqueueMessage("Command,HostTurn");
                                else
                                    myServer.EnqueueMessage("Command,ClientTurn");
                            }
                            else if (dialogResult == DialogResult.No)
                                myServer.EnqueueMessage("Command,ClientDeclined");
                            else
                                myServer.EnqueueMessage("Command,ClientDeclined");
                        }
                        else
                        {
                            btnForfeit.Enabled = false;
                        }
                        break;
                    case "GameOver":
                        myTimer.Enabled = false;
                        lblTimer.Text = "";
                        lblUser.ForeColor = Color.White;
                        lblOpponent.ForeColor = Color.White;
                        switch (temp[2])
                        {
                            case "Host":
                                HostWinCount++;
                                lblUser.Text = PlayerName + "(You) - " + HostWinCount.ToString();
                                myServer.EnqueueMessage($"Message,{PlayerName} Wins!");
                                btnForfeit.Text = "Rematch";
                                break;
                            case "Client":
                                ClientWinCount++;
                                lblOpponent.Text = OpponentName + " - " + ClientWinCount.ToString();
                                myServer.EnqueueMessage($"Message,{OpponentName} Wins!");
                                btnForfeit.Text = "Rematch";
                                btnForfeit.Enabled = false;
                                break;
                            case "Tie":
                                myServer.EnqueueMessage($"Message,The Game Was a Tie!");
                                btnForfeit.Text = "Rematch";
                                if (IsGameLive == false)
                                    btnForfeit.Enabled = false;
                                break;
                        }
                        IsGameOver = true;
                        pnBoard.Enabled = false;
                        break;
                    case "HostAccepted":
                        IsGameOver = false;
                        lblTimer.Text = "30";
                        GameTime = 30;
                        myTimer.Enabled = true;
                        Rows.Clear();
                        pnBoard.Controls.Clear();
                        for (int i = 0; i < 7; i++)
                        {
                            Panel p = new Panel();
                            p.Cursor = Cursors.Hand;
                            p.BorderStyle = BorderStyle.FixedSingle;
                            p.Height = pnBoard.Height;
                            p.Width = pnBoard.Width / 7;
                            p.Left = i * pnBoard.Width / 7;
                            p.Top = 0;
                            p.Click += P_Click;
                            p.Name = $"Host{i}";
                            Rows.Add(p);
                            pnBoard.Controls.Add(p);
                        }
                        myGame = new Game.GameState(new Game.Player(-1), new Game.Player(1));
                        if (myGame.GetNextPlayerName() == "Host")
                            myServer.EnqueueMessage("Command,HostTurn");
                        else
                            myServer.EnqueueMessage("Command,ClientTurn");
                        break;
                    case "HostDeclined":
                        myTimer.Enabled = false;
                        if (btnForfeit.Enabled)
                            MessageBox.Show("Opponent Declined Rematch");
                        Server.PlayerServer.Listening = false;
                        Server.PlayerServer.connected = false;
                        myServer = null;
                        System.Threading.Thread.Sleep(200);
                        UserControl j = new HostControl((Form)this.Parent);
                        ((MainForm)this.Parent).currentControl = j;
                        this.Parent.Controls.Add(j);
                        this.Parent.BackColor = Color.FromArgb(150, 248, 172);
                        this.Parent.Controls.Remove(this);
                        break;
                }
            }
            else if (temp[0] == "Message")
            {
                ChatMessage cm = new ChatMessage(incomingMessage, pnMessages);
                pnMessages.ScrollControlIntoView(cm);
            }
            else if(temp[0] == "Move")
            {
                lblTimer.Text = "30";
                List<Tuple<int,int>> moves = myGame.getAvailableMoves().Where(x => x.Item1 == int.Parse(temp[1])).ToList();
                int highest = 0;
                foreach(Tuple<int, int> m in moves)
                    if(m.Item2 > highest)
                        highest = m.Item2;
                myGame.MakeMove(new Tuple<int, int>(int.Parse(temp[1]), highest));
                int tilecount = 0;
                tilecount = Rows[int.Parse(temp[1])].Controls.Count;
                if (tilecount < 6)
                {
                    PictureBox pb = new PictureBox();
                    pb.Enabled = false;
                    pb.Height = Rows[int.Parse(temp[1])].Height / 6;
                    pb.Width = Rows[int.Parse(temp[1])].Width;
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, pb.Width - 3, pb.Height - 3);
                    Region rg = new Region(gp);
                    pb.Region = rg;
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.Left = 0;
                    pb.Top = Rows[int.Parse(temp[1])].Height - (pb.Height * (tilecount + 1));
                    if (pnBoard.Enabled)
                        pb.BackColor = Color.Red;
                    else
                        pb.BackColor = Color.Yellow;
                    Rows[int.Parse(temp[1])].Controls.Add(pb);
                    tilecount++;
                    if (tilecount >= 6)
                    {
                        Rows[int.Parse(temp[1])].Click -= P_Click;
                        Rows[int.Parse(temp[1])].Cursor = Cursors.Default;
                    }
                    pnBoard.Enabled = false;
                    if (myGame.Winner() != null)
                    {
                        myServer.EnqueueMessage($"Command,GameOver,{myGame.Winner().Name()}");
                        return;
                    }
                    else if (myGame.getAvailableMoves().Count == 0)
                    {
                        myServer.EnqueueMessage("Command,GameOver,Tie");
                        return;
                    }
                    if (IsGameLive)
                        myServer.EnqueueMessage("Command,ClientTurn");
                    GameTime = 30;
                }
            }
            else if (temp[0] == "Timer")
                lblTimer.Text = temp[1];
        }
        #endregion

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text.Trim() != "")
            {
                if (IsServer)
                    myServer.EnqueueMessage($"Message,{PlayerName}: {txtMessage.Text}");
                else
                    c.SendMessage($"Message,{PlayerName}: {txtMessage.Text}");
                txtMessage.Clear();
            }
        }

        private void btnForfeit_Click(object sender, EventArgs e)
        {
            if(btnForfeit.Text == "Forfeit")
            {
                if (IsServer)
                {
                    myServer.EnqueueMessage("Command,HostForfeit");
                    ClientWinCount++;
                    lblOpponent.Text = $"{OpponentName} - {ClientWinCount}";
                    btnForfeit.Text = "Rematch";
                    btnForfeit.Enabled = false;
                    myTimer.Enabled = false;
                    lblTimer.Text = "";
                }
                else
                {
                    c.SendMessage("Command,OpponentForfeit");
                    HostWinCount++;
                    lblOpponent.Text = $"{OpponentName} - {HostWinCount}";
                    btnForfeit.Text = "Rematch";
                    btnForfeit.Enabled = false;
                    lblTimer.Text = "";
                }
            }
           else
           {
                if (IsServer)
                    myServer.EnqueueMessage("Command,Rematch");
                else
                    c.SendMessage("Command,Rematch");
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (IsServer)
            {
                Server.PlayerServer.Listening = false;
                Server.PlayerServer.connected = false;
                myServer = null;
                System.Threading.Thread.Sleep(200);
                UserControl uc = new HostControl((Form)this.Parent);
                ((MainForm)this.Parent).currentControl = uc;
                this.Parent.Controls.Add(uc);
                this.Parent.BackColor = Color.FromArgb(150, 248, 172);
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
            else
            {
                if (c != null)
                {
                    c.EndConnection();
                    c = null;
                }
                System.Threading.Thread.Sleep(200);
                UserControl uc = new JoinControl((Form)this.Parent);
                ((MainForm)this.Parent).currentControl = uc;
                this.Parent.Controls.Add(uc);
                this.Parent.BackColor = Color.FromArgb(148, 239, 243);
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
           
        }
    }
}
