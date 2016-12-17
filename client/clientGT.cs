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
namespace client
{
    public partial class clientGT : Form
    {
        private Socketmodel tcpForUser;
        private Socketmodel tcpForChoosingServer;
        private int RootPort = 8080; //root port
        private int Sport; //server port
           


        public clientGT()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DetectServer();
            Connect(Sport);

            button2.Enabled = false;
            Thread receiver = new Thread(receiveData);
            receiver.Start();
        }

        public void receiveData()
        {
            while (true)
            {
                try
                {
                    string data = tcpForUser.ReadData();
                    long k = long.Parse(data);
                    tbOutput.Text = k.ToString();
                }
                catch
                {
                    MessageBox.Show("Server bị hư!Vui lòng đợi kết nối server mới");
                    Thread.Sleep(1000);
                    Reconnect();
                }                 
            }
        }

        public void Connect(int port1)
        {
            string ip = GetIPsever();
            tcpForUser = new Socketmodel(ip, port1);
            tcpForUser.ConnectToServer();       
            Text = ip;

        }
        public void DetectServer()
        {
            string ip = GetIPsever();
            tcpForChoosingServer = new Socketmodel(ip, RootPort);
            tcpForChoosingServer.ConnectToServer();
            string str = tcpForChoosingServer.ReadData();
            Sport = int.Parse(str);
            tcpForChoosingServer.CloseConnection();
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
           
            return ipS;
        }

        private void button1_Click(object sender, EventArgs e)
        {          
                tcpForUser.SendData(tbInput.Text);        
        }

        public void Reconnect()
        {
            if(Sport == 12000)
            {
                Sport += 1000;
                Connect(Sport);
                MessageBox.Show("Kết nối thành công");
            }
            else
            {
                Sport -= 1000;
                Connect(Sport);
                MessageBox.Show("Kết nối thành công");
            }
        }
    }
}
