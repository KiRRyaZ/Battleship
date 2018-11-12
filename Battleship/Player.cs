using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public enum HitType { Miss, Hit, Kill}

    public enum CellState { Unknown, Missed, Hitted }

    public abstract class Player
    {
        public Player Opponent { get; set; }
        public String Name { get; set; }
        public Fleet Fleet { get; set; }
        public CellState[,] Board { get; protected set; }
        public List<Point> LastHitted { get; set; }
        public Point LastPoint { get; set; }
        public int BoardSize { get; private set; }

        public Player()
        {
            LastHitted = new List<Point>();
            LastPoint = new Point(-1, -1);
        }

        public void SetBoard(int boardSize)
        {
            Board = new CellState[boardSize, boardSize];
            BoardSize = boardSize;
        }

        public abstract HitType GetShootIn(Point p);

        public void MissedAroundKilled()
        {
            bool isHorizontal = LastHitted.Count == 1 ? true : LastHitted[0].X == LastHitted[1].X;
            int xmin, ymin, xmax, ymax, length;
            if (isHorizontal)
            {
                xmin = LastHitted[0].X;
                xmax = xmin;
                ymin = LastHitted.Select(s => s.Y).Min();
                ymax = LastHitted.Select(s => s.Y).Max();
                length = ymax - ymin;
            }
            else
            {
                ymin = LastHitted[0].Y;
                ymax = ymin;
                xmin = LastHitted.Select(s => s.X).Min();
                xmax = LastHitted.Select(s => s.X).Max();
                length = xmax - xmin;
            }
            length++;
            for (int i = xmin - 1; i < xmin + (isHorizontal ? 2 : length + 1); i++)
                for (int j = ymin - 1; j < ymin + (isHorizontal ? length + 1 : 2); j++)
                {
                    if (i < 0 || i >= BoardSize || j < 0 || j >= BoardSize) continue;
                    if (Board[i, j].Equals(CellState.Unknown))
                        Board[i, j] = CellState.Missed;
                }
        }

    }

    public class Bot : Player
    {
        private static Bot bot;

        private BotState state;
        public BotState State {
            get {
                return state;
            }
            set {
                state = value;
                value.Bot = this;
                value.boardSize = BoardSize;
            }
        }

        private Bot() : base()
        {
            State = new NotHitState();
            Name = "Bot";
        }

        public static Bot GetBot(int boardSize)
        {
            if (bot == null)
                bot = new Bot();
            bot.SetBoard(boardSize);
            return bot;
        }

        public HitType Shoot()
        {
            Point p = State.ChooseCell();
            HitType shoot = bot.Opponent.GetShootIn(p);
            bot.Board[p.X, p.Y] = shoot.Equals(HitType.Miss) ? CellState.Missed : CellState.Hitted;
            bot.LastPoint = p;
            State.AfterShoot(shoot);
            return shoot;
        }

        public override HitType GetShootIn(Point p)
        {
            return Fleet.GetShootIn(p);  
        }
    }

    public class Human : Player
    {
        public Human(string name , int boardSize) : base()
        {
            Name = name;
            SetBoard(boardSize);
        }

        public HitType Shoot(Point p)
        {
            if (p.X < 0 || p.X >= BoardSize || p.Y < 0 || p.Y >= BoardSize)
                throw new ArgumentException($"Точка должна быть в пределах доски {BoardSize}x{BoardSize}");
            if (!Board[p.X, p.Y].Equals(CellState.Unknown))
                throw new ArgumentException("Вы уже стреляли в эту точку");
            HitType shoot = Opponent.GetShootIn(p);
            Board[p.X, p.Y] = shoot.Equals(HitType.Miss) ? CellState.Missed : CellState.Hitted;
            if (shoot == HitType.Kill)
            {
                LastHitted.Add(p);
                MissedAroundKilled();
                LastHitted.Clear();
            }
            else if (shoot == HitType.Hit) LastHitted.Add(p);
            LastPoint = p;
            return shoot;
        }

        public override HitType GetShootIn(Point p)
        {
            return Fleet.GetShootIn(p);
        }
    }
}