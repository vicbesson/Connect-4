using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Collections.Concurrent;
namespace Prog280Final_VictorBesson.Server
{
    public class Server
    {
        private TcpListener listener;
        public static bool Listening;
        private BackgroundWorker bgw = new BackgroundWorker();
        private bool connected = false;
        public event UserJoinedEvenHandler UserJoined;
        public delegate void UserJoinedEvenHandler();
        public Server(TcpListener listener)
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
            UserJoined();
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
                        ServerClient c = new ServerClient(this.listener);
                        connected = true;
                        bgw.ReportProgress(0);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }
    }
}
