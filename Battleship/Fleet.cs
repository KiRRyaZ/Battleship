using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Battleship
{
    public enum ShipState { Alive, Sunk }

    public class Fleet
    {
        public List<Ship> Ships { get; private set; }

        public Fleet() { Ships = new List<Ship>(); }

        public Ship GetBattleship() => Ships.Where(s => s.Position.Count == 4).FirstOrDefault();
        public Ship[] GetCruisers() => Ships.Where(s => s.Position.Count == 3).ToArray();
        public Ship[] GetDestroyers() => Ships.Where(s => s.Position.Count == 2).ToArray();
        public Ship[] GetSubmarines() => Ships.Where(s => s.Position.Count == 1).ToArray();

        private double Len(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private bool IsPositionNotCorrect(Ship ship)
        {
            foreach (var s in Ships)
            {
                foreach (var p in s.Position.Keys)
                {
                    foreach (var p1 in ship.Position.Keys)
                    {
                        if (Len(p, p1) < 2) return true;
                    }
                }
            }
            return false;
        }

        public void AddBattleship(Point ps, Point pe)
        {
            if (Ships.Where(s => s.Position.Count == 4).Count() == 1)
                throw new Exception("Линкор уже был добавлен");

            Ship battleship = new Ship();
            battleship.Add(ps);
            if (ps.X == pe.X)
            {
                if (Math.Abs(pe.Y - ps.Y) != 3)
                    throw new ArgumentOutOfRangeException("pe", "Длина корабля не равна 4");
                battleship.Add(new Point(ps.X, ps.Y + (ps.Y < pe.Y ? 1 : -1)));
                battleship.Add(new Point(ps.X, ps.Y + (ps.Y < pe.Y ? 2 : -2)));
            }
            else if (ps.Y == pe.Y)
            {
                if (Math.Abs(pe.X - ps.X) != 3)
                    throw new ArgumentOutOfRangeException("pe", "Длина корабля не равна 4");
                battleship.Add(new Point(ps.X + (ps.X < pe.X ? 1 : -1), ps.Y));
                battleship.Add(new Point(ps.X + (ps.X < pe.X ? 2 : -2), ps.Y));
            }
            else
                throw new ArgumentOutOfRangeException("pe", "Точки не лежат на одной прямой");
            battleship.Add(pe);
            Ships.Add(battleship);
        }

        public void AddCruiser(Point ps, Point pe)
        {
            if (Ships.Where(s => s.Position.Count == 3).Count() == 2)
                throw new Exception("Уже было добавлено максимальное кол-во крейсеров");

            Ship cruiser = new Ship();
            cruiser.Add(ps);
            if (ps.X == pe.X)
            {
                if (Math.Abs(pe.Y - ps.Y) != 2)
                    throw new ArgumentOutOfRangeException("pe", "Длина корабля не равна 3");
                cruiser.Add(new Point(ps.X, ps.Y + (ps.Y < pe.Y ? 1 : -1)));
            }
            else if (ps.Y == pe.Y)
            {
                if (Math.Abs(pe.X - ps.X) != 2)
                    throw new ArgumentOutOfRangeException("pe", "Длина корабля не равна 3");
                cruiser.Add(new Point(ps.X + (ps.X < pe.X ? 1 : -1), ps.Y));
            }
            else
                throw new ArgumentOutOfRangeException("pe", "Точки не лежат на одной прямой");
            cruiser.Add(pe);

            if (IsPositionNotCorrect(cruiser))
                throw new ArgumentOutOfRangeException("pe", "Крейсер находится слишком близко с уже поставленными кораблями");
            Ships.Add(cruiser);
        }

        public void AddDestroyer(Point ps, Point pe)
        {
            if (Ships.Where(s => s.Position.Count == 2).Count() == 3)
                throw new Exception("Уже было добавлено максимальное кол-во эсминцев");

            if (Len(ps, pe) != 1)
                throw new ArgumentOutOfRangeException("pe", "Длина корабля не равна 2");

            Ship destroyer = new Ship();
            destroyer.Add(ps);
            destroyer.Add(pe);

            if (IsPositionNotCorrect(destroyer))
                throw new ArgumentOutOfRangeException("pe", "Эсминец находится слишком близко с уже поставленными кораблями");
            Ships.Add(destroyer);
        }

        public void AddSubmarine(Point p)
        {
            if (Ships.Where(s => s.Position.Count == 1).Count() == 4)
                throw new Exception("Уже было добавлено максимальное кол-во субмарин");

            Ship submarine = new Ship();
            submarine.Add(p);

            if (IsPositionNotCorrect(submarine))
                throw new ArgumentOutOfRangeException("p", "Субмарина находится слишком близко с уже поставленными кораблями");
            Ships.Add(submarine);
        }

        public HitType GetShootIn(Point p)
        {
            Ship ship = Ships.Where(s => s.Position.Keys.Contains(p)).FirstOrDefault();            
            if (ship == null)
                return HitType.Miss;

            if (ship.State != ShipState.Sunk)
                switch (ship.Position[p])
                {
                    case ShipCellState.Alive:
                        ship.Position[p] = ShipCellState.Hitted;
                        if (ship.Position.Values.All(s => s.Equals(ShipCellState.Hitted)))
                        {
                            ship.State = ShipState.Sunk;
                            return HitType.Kill;
                        }
                        return HitType.Hit;
                    case ShipCellState.Hitted:
                        throw new ArgumentException("Вы уже стреляли в эту точку");
                }
            else
                throw new ArgumentException("Вы уже стреляли в эту точку");

            return HitType.Miss;
        }

        public bool IsAlive()
        {
            return Ships.Any(s => s.State.Equals(ShipState.Alive));
        }
    }

    public enum ShipCellState { Alive, Hitted }

    public class Ship
    {
        public ShipState State { get; set; }

        public Dictionary<Point, ShipCellState> Position { get; private set; }

        public Ship()
        {
            State = ShipState.Alive;
            Position = new Dictionary<Point, ShipCellState>();
        }

        public void Add(Point p)
        {
            if (Position.Count == 4)
                throw new Exception("Корабль не может быть больше 4 клеток");
            Position[p] = ShipCellState.Alive;
        }
    }
}
