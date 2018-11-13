using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    abstract public class BotState
    {
        public Bot Bot { get; set; }
        protected Point p;
        protected bool IsPointValid() => !(p.X >= Bot.BoardSize || p.X < 0 || p.Y >= Bot.BoardSize || p.Y < 0);
        protected bool IsCellUnknown() => Bot.Board[p.X, p.Y] == CellState.Unknown;
        protected List<Point> cases;

        protected Random random = new Random();

        public abstract Point ChooseCell();
        public abstract void AfterShoot(HitType shoot);

        public BotState()
        {
            p = new Point();            
            cases = new List<Point>();
        }
    }
    

    public class NotHitState : BotState
    {
        public override Point ChooseCell()
        {
            List<Point> visited = new List<Point>();
            int max_iter = Bot.BoardSize * Bot.BoardSize;            
            do
            {
                p.X = random.Next(0, Bot.BoardSize);
                p.Y = random.Next(0, Bot.BoardSize);
                if (visited.Contains(p))
                    continue;
                visited.Add(p);
                if (IsCellUnknown())
                    return p;
            } while (visited.Count < max_iter);
            throw new Exception($"Не могу найти клетку");            
        }

        public override void AfterShoot(HitType shoot)
        {
            Bot.LastHits.Clear();
            if (shoot.Equals(HitType.Hit))
            {
                Bot.State = new FirstHitState();
                Bot.LastHits.Add(Bot.LastPoint);
            }
            else if (shoot.Equals(HitType.Kill))
            {
                Bot.LastHits.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
                Bot.LastHits.Clear();
            }
        }
    }

    public class FirstHitState : BotState
    {
        public override Point ChooseCell()
        {
            cases.Clear();
            p = Bot.LastPoint;
            cases.Add(new Point(p.X + 1, p.Y));
            cases.Add(new Point(p.X - 1, p.Y));
            cases.Add(new Point(p.X, p.Y + 1));
            cases.Add(new Point(p.X, p.Y - 1));
            do
            {
                p = cases[random.Next(0, cases.Count)];
                cases.Remove(p);
                if (IsPointValid() && IsCellUnknown())
                    return p;
            } while (cases.Count > 0);
            throw new Exception("Не могу найти клетку");
        }

        public override void AfterShoot(HitType shoot)
        {
            if (shoot.Equals(HitType.Hit))
            {
                Bot.State = new NextHitState();
                Bot.LastHits.Add(Bot.LastPoint);
            }
            else if (shoot.Equals(HitType.Kill))
            {
                Bot.State = new NotHitState();
                Bot.LastHits.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
                Bot.LastHits.Clear();
            }
        }
    }

    public class NextHitState : BotState
    {
        public override Point ChooseCell()
        {
            cases.Clear();
            if (Bot.LastHits[0].X == Bot.LastHits[1].X)
            {
                p.X = Bot.LastHits[0].X;
                cases.Add(new Point(p.X, Bot.LastHits.Select(s => s.Y).Max() + 1));
                cases.Add(new Point(p.X, Bot.LastHits.Select(s => s.Y).Min() - 1));
            }
            else
            {
                p.Y = Bot.LastHits[0].Y;
                cases.Add(new Point(Bot.LastHits.Select(s => s.X).Max() + 1, p.Y));
                cases.Add(new Point(Bot.LastHits.Select(s => s.X).Min() - 1, p.Y));
            }
            do
            { 
                p = cases[random.Next(0, cases.Count)];
                cases.Remove(p);
                if (IsPointValid() && IsCellUnknown())
                    return p;
            } while (cases.Count > 0);
            throw new Exception("Не могу найти клетку");
        }

        public override void AfterShoot(HitType shoot)
        {
            if (shoot.Equals(HitType.Kill))
            {
                Bot.State = new NotHitState();
                Bot.LastHits.Add(Bot.LastPoint);
                Bot.MissedAroundKilled();
                Bot.LastHits.Clear();
            }
            else if (shoot.Equals(HitType.Hit))
            {
                Bot.LastHits.Add(Bot.LastPoint);
            }
        }
    }
}
