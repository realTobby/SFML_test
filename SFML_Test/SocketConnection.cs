using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFML_Test
{
    public class SocketConnection
    {
        public static string SERVER_IP_ADDRESS = "185.223.29.179"; // 185.223.29.179
        public static int SERVER_PORT = 6969;

        public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static List<string> lobbieNames = new List<string>();

        public SocketConnection()
        {
            LoopConnect();
        }

        public void SendLoop()
        {
            byte[] bufferData = Encoding.ASCII.GetBytes("getlobbies");
            clientSocket.Send(bufferData);
            byte[] recievedBuffer = new byte[3072];
            int recieved = clientSocket.Receive(recievedBuffer);

            byte[] data = new byte[recieved];
            Array.Copy(recievedBuffer, data, recieved);

            string recivedLobbyString = Encoding.ASCII.GetString(data);
            recivedLobbyString = recivedLobbyString.Replace("lobbies:", string.Empty);
            lobbieNames = recivedLobbyString.Split(',').ToList();

            Console.WriteLine("Result: " + Encoding.ASCII.GetString(data));

        }

        internal void CloseConnection()
        {
            if(clientSocket.Connected == true)
            {
                byte[] bufferData = Encoding.ASCII.GetBytes("closeclient");
                clientSocket.Send(bufferData);

                RemoveSocket();
            }
            
        }

        private void RemoveSocket()
        {
            clientSocket.Disconnect(true);
        }

        public void LoopConnect()
        {
            try
            {
                clientSocket.Connect(IPAddress.Parse(SERVER_IP_ADDRESS), SERVER_PORT);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("More info: " + ex.Message);
                MyGame.CurrentScene = Scenes.Menu;
            }
        }

        internal List<string> GetLobbyList()
        {
            if (clientSocket.Connected == true)
            {
                SendLoop();
                MyGame.gotLobbyList = true;
                return lobbieNames;
            }
            return new List<string>();
        }
    }
  
}
