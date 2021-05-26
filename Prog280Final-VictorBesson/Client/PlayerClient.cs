using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace Prog280Final_VictorBesson.Client
{
    public class PlayerClient
    {
        private TcpClient client;
        private NetworkStream nStream;
        private BackgroundWorker bgw = new BackgroundWorker();
        public event ReceivedMessageEventHandler ReceivedMessage;
        public delegate void ReceivedMessageEventHandler(string message);
        public PlayerClient(TcpClient tcpc)
        {
            client = tcpc;
            nStream = client.GetStream();
            bgw.WorkerSupportsCancellation = true;
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += Bgw_DoWork;
            bgw.ProgressChanged += Bgw_ProgressChanged;
            bgw.RunWorkerAsync();
        }

        public void SendMessage(String message)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (BinaryWriter writer = new BinaryWriter(nStream, ASCIIEncoding.UTF8, true))
                {
                    formatter.Serialize(writer.BaseStream, message);
                }
            }
            catch
            {

            }
        }

        public void EndConnection()
        {
            nStream.Close();
            nStream = null;
            client.Close();
            client = null;
            bgw.CancelAsync();
        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ReceivedMessage((string)e.UserState);
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            bool running = true;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                while (running)
                {
                    
                    using (BinaryReader reader = new BinaryReader(nStream, ASCIIEncoding.UTF8, true))
                    {
                        object obj = (object)formatter.Deserialize(reader.BaseStream);
                        if (obj is String)
                        {
                            bgw.ReportProgress(0, (String)obj);
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch
            {

            }
        }
    }
}
