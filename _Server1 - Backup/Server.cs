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
using System.Net.Sockets;
using System.Threading;
namespace _Server1
{
    public partial class Server : Form
    {
        private TCPModel tcp;
        //private List<SocketModel> MainskList;
        private SocketModel skCln;
        private List<SocketModel> SubskList;
        //private int currentClient;
        private object thisLock = new object();
        private string myIP;
        
        public Server()
        {
            
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
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

        public void StartServer()
        {
            myIP = GetIPsever();
            int port = int.Parse(tbPort.Text);
            tcp = new TCPModel(myIP, port);
            tcp.Listen();
            btnStart.Enabled = false;          
        }
        public void ServeClients()
        {                    
            while(true)
            {
                ServeAClient();
            }                 
        }
        public void Accept()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            skCln= new SocketModel(s);          
            string str = skCln.GetRemoteEndpoint();
            string str1 = "New connection from: " + str + "\n";
            rtbConnect.AppendText(str1);
        }       

        public void ServeAClient()
        {         
            lock (thisLock)
            {
                Accept();                                           
            }
            Thread t = new Thread(Commmunication);
            t.Start();
        }

        public void Commmunication()
        {

            while (true)
            {
                string input = skCln.ReceiveData();                
                Calculate(input);
                
            }   
        }
        
        public void Calculate(string input)
        {
            int k = int.Parse(input);
            long fac = k * k;
            skCln.SendData(fac.ToString());
        }

        private void Server_Load(object sender, EventArgs e)
        {
            Text = "Server Backup";
        }
    }
}
