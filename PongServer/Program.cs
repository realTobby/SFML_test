using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PongServer
{
    class Program
    {
        public static string IP_ADDRESS = "185.223.29.179"; //185.223.29.179
        public static int PORT = 6969;

        static byte[] buffer = new byte[3072];
        static List<Socket> clientSockets = new List<Socket>();
        static Socket listener;

        static List<Lobby> _lobbies = new List<Lobby>();

        static void Main(string[] args)
        {
            InsertBasicTestLobby();
            SetupServer();
            Console.ReadLine();
        }

        private static void InsertBasicTestLobby()
        {
            Lobby l1 = new Lobby();
            l1.LOBBY_ID = GetNextLobbyID();
            l1.LOBBY_NAME = "TestLobby1";
            _lobbies.Add(l1);
            Lobby l2 = new Lobby();
            l2.LOBBY_ID = GetNextLobbyID();
            l2.LOBBY_NAME = "ServerConnectionWorksLOL";
            _lobbies.Add(l2);
        }

        static void SetupServer()
        {
            Console.Title = "PONG SERVER - Close Server with Enter";
            Console.WriteLine("setting up server...");
            Console.WriteLine("Listening on: " + IP_ADDRESS + ":" + PORT);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse(IP_ADDRESS), PORT));
            listener.Listen(4);
            listener.BeginAccept(new AsyncCallback(acceptCallback), null);
        }
          
        private static void acceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("[INFO] A CLIENT CONNECTED");
            Socket newSocket = listener.EndAccept(ar);
            clientSockets.Add(newSocket);
            newSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recieveCallback), newSocket);
            listener.BeginAccept(new AsyncCallback(acceptCallback), null);
        }

        private static void recieveCallback(IAsyncResult ar)
        {
            Console.WriteLine("[INFO] INCOMING CLIENT REQUEST...");
            Socket socket = (Socket)ar.AsyncState;
            int recieved = socket.EndReceive(ar);
            byte[] dataBuffer = new byte[recieved];
            Array.Copy(buffer, dataBuffer, recieved);
            string text = Encoding.ASCII.GetString(dataBuffer);

            if (text.ToLower().Contains("closeclient"))
            {
                Console.WriteLine("[CRITICAL] A CLIENT CLOSED THE CONNECTION");
                clientSockets.Remove(socket);
                socket.Close();
            }
            if (text.ToLower().Contains("createlobby,"))
            {
                string[] lobbyString = text.Replace("createlobby,", string.Empty).Split(',');

                Lobby newLobby = new Lobby();
                newLobby.LOBBY_ID = GetNextLobbyID();
                newLobby.LOBBY_NAME = lobbyString[0];
                _lobbies.Add(newLobby);

                Console.WriteLine("Creating lobby: " + newLobby.LOBBY_NAME);
            }

            if (text.ToLower() == "getlobbies")
            {
                Console.WriteLine("[INFO] CLIENT REQUESTED LOBBY LIST");
                string lobbies = "lobbies:";
                foreach (var name in _lobbies)
                {
                    lobbies = lobbies + name.LOBBY_NAME + ",";
                }

                byte[] data = Encoding.ASCII.GetBytes(lobbies);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), socket);
            }
            if(socket.Connected)
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recieveCallback), socket);
        }

        private static int GetNextLobbyID()
        {
            if(_lobbies.Count > 0)
            {
                int res = _lobbies.Max(x => x.LOBBY_ID);
                return res;
            }
            return 0;
            
        }

        private static void sendCallback(IAsyncResult ar)
        {
            Console.WriteLine("[INFO] SENDING TO CLIENT");
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

    }
}
