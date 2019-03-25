using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer
{
    public class Lobby
    {
        public List<Player> PLAYER_LIST = new List<Player>();

        public string LOBBY_NAME { get; set; }

        public int LOBBY_ID { get; set; }
    }
}
