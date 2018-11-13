using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Battleship
{
    public interface IBuildStrategy
    {
        void PrepareBeforeBuild(Fleet fleet, int boardSize);

        void BuildBattleship();
        void BuildCruisers();
        void BuildDestroyers();
        void BuildSubmarines();
    }

    public class RandomBuildStrategy : IBuildStrategy
    {
        Random r = new Random();
        Point ps = new Point();
        Point pe = new Point();
        private int boardSize;
        public int BoardSize {
            get {
                return boardSize;
            }
            set {
                if (value >= 0)
                {
                    boardSize = value;
                    max_iter = boardSize * boardSize;
                }
                else
                    throw new ArgumentOutOfRangeException("Размер доски должен быть больше 0");
            }
        }

        bool IsEndPointValid() => !(pe.X >= BoardSize || pe.X < 0 || pe.Y >= BoardSize || pe.Y < 0);
        public Fleet Fleet { get; private set; }
        int max_iter;
        List<Point> visited;
        List<Point> cases;

        public RandomBuildStrategy()
        {            
            visited = new List<Point>();
            cases = new List<Point>();
        }

        private void BuildShip(int size, string name, Action<Point, Point> AddShip)
        {
            if (size <= 1)
                throw new ArgumentOutOfRangeException("size", "Корабль должен быть больше 1-палубного");
            size--;
            visited.Clear();            
            do
            {
                ps = new Point(r.Next(0, BoardSize), r.Next(0, BoardSize));
                if (visited.Contains(ps))
                    continue;
                visited.Add(ps);
                cases.Clear();
                cases.Add(new Point(ps.X + size, ps.Y));
                cases.Add(new Point(ps.X - size, ps.Y));
                cases.Add(new Point(ps.X, ps.Y + size));
                cases.Add(new Point(ps.X, ps.Y - size));
                do
                {
                    pe = cases[r.Next(0, cases.Count)];
                    cases.Remove(pe);
                    if (IsEndPointValid())
                        try
                        {
                            AddShip(ps, pe);
                            return;
                        }
                        catch (ArgumentOutOfRangeException) { }
                } while (cases.Count > 0);
            } while (visited.Count < max_iter);
            throw new Exception($"Не могу разместить {name}");
        }

        public void BuildBattleship()
        {
            BuildShip(4, "линкор", Fleet.AddBattleship);
        }

        public void BuildCruisers()
        {
            BuildShip(3, "крейсер", Fleet.AddCruiser);
        }

        public void BuildDestroyers()
        {
            BuildShip(2, "эсминец", Fleet.AddDestroyer);
        }

        public void BuildSubmarines()
        {
            visited.Clear();
            do
            {
                ps = new Point(r.Next(0, BoardSize), r.Next(0, BoardSize));
                if (visited.Contains(ps))
                    continue;
                visited.Add(ps);
                try
                {
                    Fleet.AddSubmarine(ps);
                    return;
                }
                catch (ArgumentOutOfRangeException) { }
            } while (visited.Count < max_iter);
            throw new Exception($"Не могу разместить субмарину");
        }

        public void PrepareBeforeBuild(Fleet fleet, int boardSize)
        {
            BoardSize = boardSize;
            Fleet = fleet;
        }
    }

    public class GUIBuildStrategy : IBuildStrategy
    {
        Point start, end;
        public Fleet Fleet { get; private set; }

        public void PrepareBeforeBuild(Fleet fleet, int boardSize)
        {
            GUI.LEFT = 0; GUI.TOP = 0;
            GUI.left = GUI.LEFT; GUI.top = GUI.TOP;

            Fleet = fleet;
            GUI.PrintFleet(fleet);
            Console.WriteLine("Здесь и дальше нужно ставить только начальную и конечную точки");
        }

        public void BuildBattleship()
        {
            Console.WriteLine("Постройте линкор (4 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            Fleet.AddBattleship(start, end);
            GUI.PrintFleet(Fleet);
        }

        public void BuildCruisers()
        {
            Console.WriteLine($"Постройте крейсер (3 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            Fleet.AddCruiser(start, end);
            GUI.PrintFleet(Fleet);
        }

        public void BuildDestroyers()
        {
            Console.WriteLine($"Постройте уничтожитель (2 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            Fleet.AddDestroyer(start, end);
            GUI.PrintFleet(Fleet);
        }

        public void BuildSubmarines()
        {
            Console.WriteLine($"Постройте субмарину (1 клетка)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            Fleet.AddSubmarine(start);
            GUI.PrintFleet(Fleet);
        }
    }

    public class FleetBuilder
    {
        Fleet fleet = new Fleet();
        public IBuildStrategy BuildStrategy { get; set; }

        public FleetBuilder(IBuildStrategy strategy) => BuildStrategy = strategy;

        public void PrepareBeforeBuild(int boardSize) => BuildStrategy.PrepareBeforeBuild(fleet, boardSize);

        public void BuildBattleship() => BuildStrategy.BuildBattleship();

        public void BuildCruisers() => BuildStrategy.BuildCruisers();

        public void BuildDestroyers() => BuildStrategy.BuildDestroyers();

        public void BuildSubmarines() => BuildStrategy.BuildSubmarines();

        public Fleet GetFleet(int boardSize)
        {
            if (boardSize > 10 || boardSize < 5)
                throw new ArgumentOutOfRangeException("Размер доски должен быть от 5 до 10");
            try
            {
                PrepareBeforeBuild(boardSize);
                if(boardSize == 10)
                    BuildBattleship();
                for (int i = 0; i < (boardSize > 8 ? 2 : boardSize - 7); i++)
                    BuildCruisers();
                for (int i = 0; i < (boardSize > 6 ? 3 : boardSize - 4); i++)
                    BuildDestroyers();
                for (int i = 0; i <  (boardSize == 5 ? 3 : 4); i++)
                    BuildSubmarines();

                return fleet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                fleet = new Fleet();
            }
        }

        public Fleet GetFleet() => fleet;
    }

}
