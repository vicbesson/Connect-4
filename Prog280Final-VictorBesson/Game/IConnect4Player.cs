using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog280Final_VictorBesson.Game
{
    public interface IConnect4Player
    {
        int Symbol();
        string Name();
        void GameChanged(int[,] b, Tuple<int, int>[] moves);
    }
}
