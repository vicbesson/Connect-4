using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog280Final_VictorBesson.Game
{
    public class Player : Connect4PlayerBase
    {
        public Player(int symbol) : base(symbol)
        {
            if (symbol < 0)
                playerName = "Host";
            else
                playerName = "Client";
        }
    }
}
