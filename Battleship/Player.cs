using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public enum HitType { Miss, Hit, Kill}

    public enum CellState { Unknown, Missed, Hit }

    public abstract class Player
    {
        public Player Opponent { get; set; }
        public String Name { get; set; }
        public Fleet Fleet { get; set; }
        public CellState[,] Board { get; protected set; }
        public List<Point> LastHits { get; set; }
        public Point LastPoint { get; set; }
        public int BoardSize => Board.GetLength(0);

        public Player()
        {
            LastHits = new List<Point>();
            LastPoint = new Point(-1, -1);
        }

        public void ChangeBoard(int boardSize)
        {
            if (boardSize > 10 || boardSize < 5)
                throw new ArgumentOutOfRangeException("Размер доски должен быть от 5 до 10");
            Board = new CellState[boardSize, boardSize];
        }

        public void MissedAroundKilled()
        {
            bool isHorizontal = LastHits.Count == 1 ? true : LastHits[0].X == LastHits[1].X;
            int xmin, ymin, xmax, ymax, length;
            if (isHorizontal)
            {
                xmin = LastHits[0].X;
                xmax = xmin;
                ymin = LastHits.Select(s => s.Y).Min();
                ymax = LastHits.Select(s => s.Y).Max();
                length = ymax - ymin;
            }
            else
            {
                ymin = LastHits[0].Y;
                ymax = ymin;
                xmin = LastHits.Select(s => s.X).Min();
                xmax = LastHits.Select(s => s.X).Max();
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
        public HitType GetShootIn(Point p)
        {
            return Fleet.GetShootIn(p);
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
            }
        }

        private Bot():base() { }

        public static Bot GetBot(int boardSize)
        {
            if (bot == null)
                bot = new Bot();
            bot.ChangeBoard(boardSize);
            bot.State = new NotHitState();            
            bot.Name = "Bot";
            bot.LastHits.Clear();
            bot.LastPoint = new Point(-1, -1);
            return bot;
        }

        public HitType Shoot()
        {
            try
            {
                Point p = State.ChooseCell();
                HitType shoot = bot.Opponent.GetShootIn(p);
                bot.Board[p.X, p.Y] = shoot.Equals(HitType.Miss) ? CellState.Missed : CellState.Hit;
                bot.LastPoint = p;
                State.AfterShoot(shoot);
                return shoot;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }

    public class Human : Player
    {
        public Human(string name , int boardSize) : base()
        {
            Name = name;
            ChangeBoard(boardSize);
        }

        public HitType Shoot(Point p)
        {
            if (p.X < 0 || p.X >= BoardSize || p.Y < 0 || p.Y >= BoardSize)
                throw new ArgumentOutOfRangeException($"Точка должна быть в пределах доски {BoardSize}x{BoardSize}");
            if (!Board[p.X, p.Y].Equals(CellState.Unknown))
                throw new ArgumentException("Вы уже стреляли в эту точку");
            HitType shoot = Opponent.GetShootIn(p);
            Board[p.X, p.Y] = shoot.Equals(HitType.Miss) ? CellState.Missed : CellState.Hit;
            if (shoot == HitType.Kill)
            {
                LastHits.Add(p);
                MissedAroundKilled();
                LastHits.Clear();
            }
            else if (shoot == HitType.Hit) LastHits.Add(p);
            LastPoint = p;
            return shoot;
        }
    }
}