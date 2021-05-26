using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Prog280Final_VictorBesson.Game
{
    public class GameState
    {
        private static Random rng = new Random();
        private IConnect4Player winner;
        private int[,] board = new int[7,6];
        private List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();
        private Queue<IConnect4Player> players = new Queue<IConnect4Player>();
        public delegate void DeclareWinnerEvent(IConnect4Player p);
        public event DeclareWinnerEvent WinnerIS;
        public GameState(IConnect4Player p1, IConnect4Player p2)
        {
            int turn = 0;
            int[] tmp = { p1.Symbol(), p2.Symbol() };
            if (!tmp.Contains(1) || !tmp.Contains(-1))
            {
                throw new Exception("Symbols must be initialized as 1 and -1");
            }
            while (turn == 0)
               turn = rng.Next(-10, 11);
            if(turn < 0)
            {
                players.Enqueue(p1);
                players.Enqueue(p2);
            }
            else
            {
                players.Enqueue(p2);
                players.Enqueue(p1);
            }
            this.WinnerIS += GameState_WinnerIS;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Tuple<int, int> move = new Tuple<int, int>(i, j);
                    availableMoves.Add(move);
                }
            }
            UpdateBothPlayers();
        }

        public void MakeMove(Tuple<int, int> move)
        {
            IConnect4Player x = players.Dequeue();
            if (availableMoves.Contains(move))
            {
                this.board[move.Item1, move.Item2] = x.Symbol();
                availableMoves.Remove(move);
            }
            else
            {
                move = availableMoves.FirstOrDefault();
                this.board[move.Item1, move.Item2] = x.Symbol();
                availableMoves.Remove(move);
            }
            players.Enqueue(x);
            UpdateBothPlayers();
            CheckForWinner();
        }

        private void GameState_WinnerIS(IConnect4Player p)
        {
            this.winner = p;
        }

        public void UpdateBothPlayers()
        {
            IConnect4Player p1 = players.Dequeue();
            IConnect4Player p2 = players.Dequeue();
            p1.GameChanged(this.board, availableMoves.ToArray());
            p2.GameChanged(this.board, availableMoves.ToArray());
            players.Enqueue(p1);
            players.Enqueue(p2);
        }

        public void CheckForWinner() 
        {
            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Math.Abs(board[j, i] + board[j + 1, i] + board[j + 2, i] + board[j + 3, i]) == 4)
                        WinnerIS(players.Where(x => x.Symbol() == board[j, i]).FirstOrDefault());
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Math.Abs(board[i, j] + board[i, j + 1] + board[i, j + 2] + board[i, j + 3]) == 4)
                        WinnerIS(players.Where(x => x.Symbol() == board[i, j]).FirstOrDefault());
                }
            }
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (Math.Abs(board[i, j] + board[i + 1, j + 1] + board[i + 2, j + 2] + board[i + 3, j + 3]) == 4)
                    {
                        WinnerIS(players.Where(x => x.Symbol() == board[i, j]).FirstOrDefault());
                    }
                    else if(Math.Abs(board[6 - i, j] + board[6 - i - 1, j + 1] + board[6 - i - 2, j + 2] + board[6 - i - 3, j + 3]) == 4)
                    {
                        WinnerIS(players.Where(x => x.Symbol() == board[6 - i, j]).FirstOrDefault());
                    }
                    else if(Math.Abs(board[i, 5 - j] + board[i + 1, 5 - j - 1] + board[i + 2, 5 - j - 2] + board[i + 3, 5- j - 3]) == 4)
                    {
                        WinnerIS(players.Where(x => x.Symbol() == board[i, 5 - j]).FirstOrDefault());
                    }
                    else if(Math.Abs(board[6 - i, 5 - j] + board[6 - i - 1, 5 - j - 1] + board[6 - i - 2, 5 - j - 2] + board[6 - i - 3, 5 - j - 3]) == 4)
                    {
                        WinnerIS(players.Where(x => x.Symbol() == board[6 - i, 5 - j]).FirstOrDefault());
                    }
                }
            }
        }
        public string GetNextPlayerName()
        {
            return players.ElementAt(0).Name();
        }
        public List<Tuple<int,int>> getAvailableMoves()
        {
            return availableMoves;
        }

        public IConnect4Player Winner()
        {
            return this.winner;
        }
    }
}
