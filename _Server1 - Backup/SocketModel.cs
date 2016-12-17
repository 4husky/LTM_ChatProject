using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace _Server1
{
	public class SocketModel
	{
		private Socket socket;
		private byte[] bufferReceive;
		private string remoteEndPoint;		
		
		public SocketModel(Socket s)
		{
			socket = s;
            bufferReceive = new byte[100];
		}
		public SocketModel(Socket s,int length)
		{
			socket = s;
            bufferReceive = new byte[length];
		}		
		//get the IP and port of connected client
		public string GetRemoteEndpoint(){
			string str = "";
			try{
				str = Convert.ToString(socket.RemoteEndPoint);
				remoteEndPoint = str;			
			}
			catch (Exception e){
				string str1 = "Error..... " + e.StackTrace;
		        //Console.WriteLine(str1);
		        str = "Socket is closed with " + remoteEndPoint;
			}
			return str;			
		}
        //receive data from client
        public string ReceiveData()
        {
            //server just can receive data AFTER a connection is set up between server and client
            string data = string.Empty;
            try
            {
                int k = socket.Receive(bufferReceive);
                data = Encoding.ASCII.GetString(bufferReceive);
            }
            catch (Exception e)
            {
                string str1 = "Error..... " + e.StackTrace;
                //Console.WriteLine(str1);	      
            }
            return data;
        }
        //send data to client
        public void SendData(string str)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                socket.Send(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
        //close sockket
        public void CloseSocket(){
			socket.Close();
		}		

	}
}
