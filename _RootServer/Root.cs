using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;
namespace _RootServer
{
    public partial class Root : Form
    {
        private TCPModel tcp;
        private List<SocketModel> socketList;       
        private int currentClient;  
        private object thisLock = new object();
        private string myIP;
        //giả sử mỗi server chỉ phục vụ 1 client
        public Root()
        {           
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();           
        }

        public string GetIPsever()
        {
            string ipS = string.Empty;        
            IPAddress[] ipadd = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress x in ipadd)
            {
                if (IPAddress.Parse(x.ToString()).AddressFamily == AddressFamily.InterNetwork)
                {
                    ipS = x.ToString();
                }
            }           
            lbIP.Text = "Server IP address is: " + ipS;
            return ipS;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
        }

        public void StartServer()
        {
            myIP = GetIPsever();
            int port = int.Parse(tbPort.Text);
            tcp = new TCPModel(myIP, port);
            tcp.Listen();
            btnStart.Enabled = false;
            currentClient = 0;
        }
        public void ServeClients()
        {
            socketList = new List<SocketModel>();
            while(true)
            {
                ServeAClient();
            }
        }
        public void Accept()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);           
            socketList.Add(new SocketModel(s));
            string str = socketList[currentClient].GetRemoteEndpoint();
            string str1 = "New connection from: " + str + "\n";
            rtbConnect.AppendText(str1);
        }
        public void ServeAClient()
        {
            int num = -1;
            lock (thisLock)
            {
                Accept();
                currentClient++;
                num = currentClient - 1;
            }
            Commmunication(num);
        }

        private int balance = 1;
        public void Commmunication(int num)
        {
            string str = "";
            if (balance == 1)
                str = "12000";
            if (balance == 2)
                str = "13000";
            balance++;
            //Console.WriteLine("Balance la: " + balance);
            if (balance == 3)
                balance = 1;
            socketList[num].SendData(str);
        }

        private void Root_Load(object sender, EventArgs e)
        {
            Text = "ROOT SERVER";
        }
    }
}
