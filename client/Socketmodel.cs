using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace client
{
    class Socketmodel
    {
        private Socket cln;
        private byte[] byteSend;
        private byte[] byteReceive;
        private string IPofServer;
        private int port;
        public Socketmodel(string ip,int _port)
        {
            IPofServer = ip;
            port = _port;
            cln = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byteReceive = new byte[100];
            byteSend = new byte[100];            
        }
        public string UpdateInformation()
        {
            string str = "";
            try
            {
                str = str + cln.LocalEndPoint.ToString();                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            return str;
        }
        //Set up a connection to server
        public int ConnectToServer()
        {
            try
            {
                IPEndPoint Iep = new IPEndPoint(IPAddress.Parse(IPofServer), port);
                cln.Connect(Iep);
            }
            catch (Exception e)
            {
                //Console.WriteLine("Error..... " + e.StackTrace);
                return -1;
            }
            return 1;
        }

        //Send data to server after connection is set up
        public void SendData(string str)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                cln.Send(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        //Read data from server after connection is set up
        public string ReadData()
        {
            string data;
            try
            {
                //count the length of data received
                byte[] buffer = new byte[100];
                int k = cln.Receive(buffer);
                //conver the byte recevied into string
                #region c1
                //char[] c = new char[k];
                //for (int i = 0; i < k; i++)
                //{
                //    Console.Write(Convert.ToChar(byteReceive[i]));
                //    c[i] = Convert.ToChar(byteReceive[i]);
                //}
                //str = new string(c);
                #endregion
                data = Encoding.ASCII.GetString(buffer);
            }
            catch (Exception e)
            {
                data = "Error..... " + e.StackTrace;
                Console.WriteLine(data);
            }
            return data;
        }

        public void CloseConnection()
        {
            cln.Close();
        }
    }
}
