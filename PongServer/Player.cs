using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer
{
    public class Player
    {
        public string PLAYER_NAME { get; set; }
        public int COLOR_RED { get; set; }
        public int COLOR_GREEN { get; set; }
        public int COLOR_BLUE { get; set; }
        public bool LOBBY_OWNER { get; set; }
    }
}
