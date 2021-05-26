using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
namespace Prog280Final_VictorBesson.Server
{
    public class ServerClient
    {
        BackgroundWorker clientWorker = new BackgroundWorker();
        TcpClient connection;
        public NetworkStream socketStream;
        public event SentMessageHandler SentMessage;
        public delegate void SentMessageHandler(string message);
        public ServerClient(TcpListener listener)
        {
            connection = listener.AcceptTcpClient();
            socketStream = connection.GetStream();
            clientWorker.WorkerSupportsCancellation = true;
            clientWorker.DoWork += ClientWorker_DoWork;
            clientWorker.WorkerReportsProgress = true;
            clientWorker.ProgressChanged += ClientWorker_ProgressChanged;
            clientWorker.RunWorkerAsync();
        }
        private void EndConnection()
        {
            socketStream.Close();
            socketStream = null;
            connection.Close();
            connection = null;
            clientWorker.CancelAsync();
        }
        private void ClientWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SentMessage((string)e.UserState);
        }

        private void ClientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                while (PlayerServer.Listening)
                {
                    using (BinaryReader reader = new BinaryReader(socketStream, ASCIIEncoding.UTF8, true))
                    {
                        object o = formatter.Deserialize(reader.BaseStream);
                        if (o is string)
                        {
                            clientWorker.ReportProgress(0, (string)o);
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch
            {
                EndConnection();
                SentMessage("Command,OpponentLeft");
            }
        }
    }
}
