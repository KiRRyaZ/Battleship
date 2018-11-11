using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    abstract public class BotState
    {
        public Bot Bot { get; set; }

        protected Random random = new Random();
        public int boardSize;

        public abstract Point ChooseCell();
        public abstract void AfterShoot(HitType shoot);
    }
    

    public class NotHitState : BotState
    {
        public override Point ChooseCell()
        {
            Point p = new Point();
            do
            {
                p.X = random.Next(0, boardSize);
                p.Y = random.Next(0, boardSize);
            } while (!Bot.Board[p.X, p.Y].Equals(CellState.Unknown));

            return p;
        }

        public override void AfterShoot(HitType shoot)
        {
            Bot.LastHitted.Clear();
            if (shoot.Equals(HitType.Hit))
            {
                Bot.State = new FirstHitState();
                Bot.LastHitted.Add(Bot.LastPoint);
            }
            else if (shoot.Equals(HitType.Kill))
            {
                Bot.LastHitted.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
            }
        }
    }

    public class FirstHitState : BotState
    {
        public override Point ChooseCell()
        {
            Point p = new Point();
            do
            {
                p.X = Bot.LastHitted[0].X;
                p.Y = Bot.LastHitted[0].Y;
                switch (random.Next(0, 4))
                {
                    case 0: p.X++;
                        break;
                    case 1: p.X--;
                        break;
                    case 2: p.Y++;
                        break;
                    case 3: p.Y--;
                        break;
                }
            } while (p.X >= boardSize || p.X < 0 || p.Y >= boardSize || p.Y < 0 || !Bot.Board[p.X, p.Y].Equals(CellState.Unknown));

            return p;
        }

        public override void AfterShoot(HitType shoot)
        {
            if (shoot.Equals(HitType.Hit))
            {
                Bot.State = new NextHitState();
                Bot.LastHitted.Add(Bot.LastPoint);
            }
            else if (shoot.Equals(HitType.Kill))
            {
                Bot.State = new NotHitState();
                Bot.LastHitted.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
            }
        }
    }

    public class NextHitState : BotState
    {
        public override Point ChooseCell()
        {
            Point p = new Point();
            bool isHorizontal = Bot.LastHitted[0].X == Bot.LastHitted[1].X;
            if (isHorizontal)
                p.X = Bot.LastHitted[0].X;
            else
                p.Y = Bot.LastHitted[0].Y;
            do
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        if (isHorizontal)
                            p.Y = Bot.LastHitted.Select(s => s.Y).Max() + 1;
                        else
                            p.X = Bot.LastHitted.Select(s => s.X).Max() + 1;
                        break;
                    case 1:
                        if (isHorizontal)
                            p.Y = Bot.LastHitted.Select(s => s.Y).Min() - 1;
                        else
                            p.X = Bot.LastHitted.Select(s => s.X).Min() - 1;
                        break;
                }
            } while (p.X >= boardSize || p.X < 0 || p.Y >= boardSize || p.Y < 0 || !Bot.Board[p.X, p.Y].Equals(CellState.Unknown));

            return p;
        }

        public override void AfterShoot(HitType shoot)
        {
            if (shoot.Equals(HitType.Kill))
            {
                Bot.State = new NotHitState();
                Bot.LastHitted.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
            }
            else if (shoot.Equals(HitType.Hit))
            {
                Bot.LastHitted.Add(Bot.LastPoint);
            }
        }
    }
}
