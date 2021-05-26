using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog280Final_VictorBesson.Game
{
    public class Connect4PlayerBase : IConnect4Player
    {
        protected int[,] board = new int[7, 6];
        protected string playerName = "";
        protected int symbol = 0;
        protected List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();

        public Connect4PlayerBase(int symbol)
        {
            this.symbol = symbol;
        }
        public void GameChanged(int[,] b, Tuple<int,int>[] moves)
        {
            this.board = b;
            availableMoves = moves.ToList();
        }

        public string Name()
        {
            return this.playerName;
        }

        public int Symbol()
        {
            return this.symbol;
        }
    }
}
