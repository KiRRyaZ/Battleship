using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab2
{
    public interface IBuildStrategy
    {
        void PrepareBeforeBuild(Fleet fleet);

        void BuildBattleship(Fleet fleet);
        void BuildCruisers(Fleet fleet);
        void BuildDestroyers(Fleet fleet);
        void BuildSubmarines(Fleet fleet);
    }

    public class RandomBuildStrategy : IBuildStrategy
    {
        Random r = new Random();
        Point ps = new Point();
        Point pe = new Point();
        public int BoardSize { get; set; }

        public RandomBuildStrategy(int BoardSize)
        {
            this.BoardSize = BoardSize;
        }

        public void BuildBattleship(Fleet fleet)
        {
            ps = new Point(r.Next(0, BoardSize), r.Next(0, BoardSize));
            do
            {
                pe.X = ps.X;
                pe.Y = ps.Y;
                switch (r.Next(0, 4))
                {
                    case 0:
                        pe.X += 3;
                        break;
                    case 1:
                        pe.X -= 3;
                        break;
                    case 2:
                        pe.Y += 3;
                        break;
                    case 3:
                        pe.Y -= 3;
                        break;
                }
            } while (pe.X >= BoardSize || pe.X < 0 || pe.Y >= BoardSize || pe.Y < 0);
            fleet.AddBattleship(ps, pe);
        }

        public void BuildCruisers(Fleet fleet)
        {
            while (true)
            {
                ps.X = r.Next(0, BoardSize);
                ps.Y = r.Next(0, BoardSize);
                do
                {
                    pe.X = ps.X;
                    pe.Y = ps.Y;
                    switch (r.Next(0, 4))
                    {
                        case 0:
                            pe.X += 2;
                            break;
                        case 1:
                            pe.X -= 2;
                            break;
                        case 2:
                            pe.Y += 2;
                            break;
                        case 3:
                            pe.Y -= 2;
                            break;
                    }
                } while (pe.X >= BoardSize || pe.X < 0 || pe.Y >= BoardSize || pe.Y < 0);
                try
                {
                    fleet.AddCruiser(ps, pe);
                    break;
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        public void BuildDestroyers(Fleet fleet)
        {
            while (true)
            {
                ps.X = r.Next(0, BoardSize);
                ps.Y = r.Next(0, BoardSize);
                do
                {
                    pe.X = ps.X;
                    pe.Y = ps.Y;
                    switch (r.Next(0, 4))
                    {
                        case 0:
                            pe.X++;
                            break;
                        case 1:
                            pe.X--;
                            break;
                        case 2:
                            pe.Y++;
                            break;
                        case 3:
                            pe.Y--;
                            break;
                    }
                } while (pe.X >= BoardSize || pe.X < 0 || pe.Y >= BoardSize || pe.Y < 0);
                try
                {
                    fleet.AddDestroyer(ps, pe);
                    break;
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        public void BuildSubmarines(Fleet fleet)
        {
            while (true)
            {
                ps.X = r.Next(0, BoardSize);
                ps.Y = r.Next(0, BoardSize);
                try
                {
                    fleet.AddSubmarine(ps);
                    break;
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        public void PrepareBeforeBuild(Fleet fleet)
        {
            
        }
    }

    public class GUIBuildStrategy : IBuildStrategy
    {
        Point start, end;

        public void PrepareBeforeBuild(Fleet fleet)
        {
            GUI.LEFT = 0; GUI.TOP = 0;
            GUI.left = GUI.LEFT; GUI.top = GUI.TOP;

            GUI.PrintFleet(fleet);
            Console.WriteLine("Здесь и дальше нужно ставить только начальную и конечную точки");
        }

        public void BuildBattleship(Fleet fleet)
        {
            Console.WriteLine("Постройте линкор (4 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            fleet.AddBattleship(start, end);
            GUI.PrintFleet(fleet);
        }

        public void BuildCruisers(Fleet fleet)
        {
            Console.WriteLine($"Постройте крейсер (3 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            fleet.AddCruiser(start, end);
            GUI.PrintFleet(fleet);
        }

        public void BuildDestroyers(Fleet fleet)
        {
            Console.WriteLine($"Постройте уничтожитель (2 клетки)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            end = GUI.GetCoords();
            fleet.AddDestroyer(start, end);
            GUI.PrintFleet(fleet);
        }

        public void BuildSubmarines(Fleet fleet)
        {
            Console.WriteLine($"Постройте субмарину (1 клетка)");
            Console.SetCursorPosition(GUI.left, GUI.top);
            start = GUI.GetCoords();
            fleet.AddSubmarine(start);
            GUI.PrintFleet(fleet);
        }
    }

    public class FleetBuilder
    {
        Fleet fleet = new Fleet();
        IBuildStrategy buildStrategy;

        public FleetBuilder(IBuildStrategy strategy) => buildStrategy = strategy;

        public void SetStrategy(IBuildStrategy strategy) => buildStrategy = strategy;

        public void PrepareBeforeBuild() => buildStrategy.PrepareBeforeBuild(fleet);

        public void BuildBattleship() => buildStrategy.BuildBattleship(fleet);

        public void BuildCruisers() => buildStrategy.BuildCruisers(fleet);

        public void BuildDestroyers() => buildStrategy.BuildDestroyers(fleet);

        public void BuildSubmarines() => buildStrategy.BuildSubmarines(fleet);

        public Fleet GetFleet(int boardSize)
        {
            try
            {
                PrepareBeforeBuild();
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
    }

}
