using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Collections.Concurrent;
namespace Prog280Final_VictorBesson.Server
{
    public class PlayerServer
    {
        private TcpListener listener;
        public static bool Listening;
        private BackgroundWorker bgw = new BackgroundWorker();
        public static bool connected = false;
        private ConcurrentQueue<string> Messages = new ConcurrentQueue<string>();
        public event PlayerMessageEvenHandler PlayerMessage;
        public delegate void PlayerMessageEvenHandler(string message);
        private ServerClient c;
        public PlayerServer(TcpListener listener)
        {
            this.listener = listener;
            Listening = true;
            listener.Start();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += Bgw_DoWork;
            bgw.WorkerReportsProgress = true;
            bgw.ProgressChanged += Bgw_ProgressChanged;
            bgw.RunWorkerAsync();
        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PlayerMessage((string)e.UserState);
        }

        public void EnqueueMessage(string message)
        {
            Messages.Enqueue(message);
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                while(Listening)
                {
                    if (listener.Pending() && connected == false)
                    {
                        c = new ServerClient(this.listener);
                        c.SentMessage += C_SentMessage;
                        connected = true;
                        bgw.ReportProgress(0, "Command,PlayerJoined");
                    }
                    else if(connected == true && c != null)
                    {
                        if(Messages.Count > 0)
                        {
                            string message = "";
                            Messages.TryDequeue(out message);
                            if (message == "Command,OpponentLeft")
                            {
                                connected = false;
                                bgw.ReportProgress(0, message);
                            }
                            else
                            {
                                bgw.ReportProgress(0, message);
                                using (BinaryWriter writer = new BinaryWriter(c.socketStream, ASCIIEncoding.UTF8, true))
                                {
                                    formatter.Serialize(writer.BaseStream, message);
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch
            {
               
            }
            finally
            {
                if(c != null)
                {
                    if (c.socketStream != null)
                    {
                        using (BinaryWriter writer = new BinaryWriter(c.socketStream, ASCIIEncoding.UTF8, true))
                        {
                            formatter.Serialize(writer.BaseStream, "Command,HostLeft");
                        }
                        c = null;
                    }
                }
                listener.Stop();
                connected = false;
                Listening = false;
                bgw.CancelAsync();
            }
        }

        private void C_SentMessage(string message)
        {
            Messages.Enqueue(message);
        }
    }
}
