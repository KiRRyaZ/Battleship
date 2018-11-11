using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public static class GUI
    {
        public static int LEFT = 32, TOP = 0;
        public static int left = 32, top = 0;
        public static int BoardSize { get; private set; }

        public static Point GetCoords(bool isPrint = true, char toPrint = '#')
        {
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey(true);
                switch (ck.Key)
                {
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        left += left + 2 >= LEFT + BoardSize * 2 ? 0 : 2;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        left -= left - 2 < LEFT ? 0 : 2;
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        top -= top - 1 < TOP ? 0 : 1;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        top += top + 1 >= TOP + BoardSize ? 0 : 1;
                        break;
                    case ConsoleKey.Enter:
                        if (isPrint) Console.Write(toPrint);
                        return new Point(top - TOP, (left - LEFT) / 2);
                }
                Console.SetCursorPosition(left, top);
            }
        }

        public static string StartWithName()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в игру 'Морской бой'");
            Console.Write("Введите Ваше имя -> ");
            return Console.ReadLine();
        }

        public static bool IsGUIStrategy()
        {
            Console.Write("Вы хотели бы сами расставить корабли? (y/n)");
            return Console.ReadKey().Key.Equals(ConsoleKey.Y);
        }

        public static void StartShooting()
        {
            LEFT = BoardSize > 7 ? 32 : 24; TOP = 0;
            left = LEFT; top = TOP;
        }

        public static void PrintException(string message, int position)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(0, position);
            Console.WriteLine(message);
            Console.WriteLine("Попробуйте, пожалуйста, еще раз\nНажмите Enter для продолженя...");
            Console.ReadLine();
            Console.ResetColor();
        }

        public static int GetBoardSize()
        {
            do
            {
                try
                {
                    Console.Write("Введите размер доски (от 5 до 10) -> ");
                    int boardSize = Convert.ToInt32(Console.ReadLine().Trim());
                    if (boardSize > 10 || boardSize < 5)
                        throw new Exception();
                    BoardSize = boardSize;
                    return boardSize;
                }
                catch (Exception)
                {
                    Console.WriteLine("Размер доски неверный. Попытайтесь снова");
                }
            } while (true);
        }

        public static void PrintShoot(Point p, HitType shoot, string who)
        {            
            String s = $"{who} выстрелил в точку ({p.X};{p.Y}) и {shoot}\n";            
            ConsoleColor cc = ConsoleColor.White;
            switch (shoot)
            {
                case HitType.Miss: cc = ConsoleColor.Red; break;
                case HitType.Hit: cc = ConsoleColor.DarkYellow; break;
                case HitType.Kill: cc = ConsoleColor.DarkGreen; break;
            }
            Console.ForegroundColor = cc;
            Console.WriteLine(s);
            Console.ResetColor();
            File.AppendAllText("Battleship.txt", s);
        }

        public static void PrintStats(Player pl1, Player pl2)
        {
            Console.WriteLine($"Корабли\t\t{pl1.Name}\t\t\t{pl2.Name}\n");
            Ship sh1, sh2;
            for (int i = 0; i < pl1.Fleet.Ships.Count; i++)
            {
                sh1 = pl1.Fleet.Ships[i];
                sh2 = pl2.Fleet.Ships[i];
                Console.WriteLine($"{sh1.Position.Count}-палубный: \t{sh1.State}\t\t\t{sh2.State}");
            }
            Console.WriteLine();
        }

        public static void PrintWin(Player player)
        {
            String s = $"Победил {player.Name}\n";
            Console.WriteLine(s);
            File.AppendAllText("Battleship.txt", s);

        }

        public static void PrintYourTurn()
        {
            Console.Write("Ваш ход");
            Console.SetCursorPosition(left, top);
        }

        public static void PrintBoards(Player player, Player opponent, bool isLose = false)
        {
            Console.Clear();
            bool isLastPoint;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    char toPoint = ' ';
                    isLastPoint = i == opponent.LastPoint.X && j == opponent.LastPoint.Y;
                    if (isLastPoint)
                        Console.ForegroundColor = ConsoleColor.Green;
                    switch (opponent.Board[i, j])
                    {
                        case CellState.Hitted:
                            toPoint = 'x';
                            break;
                        case CellState.Missed:
                            toPoint = 'o';
                            break;
                        case CellState.Unknown:
                            toPoint = player.Fleet.Ships.Any(s => s.Position.Keys.Contains(new Point(i, j))) ? '#' : '.';
                            break;
                    }
                    Console.Write(toPoint + " ");
                    if (isLastPoint)
                        Console.ResetColor();
                }
                Console.Write("\t\t");
                for (int j = 0; j < BoardSize; j++)
                {
                    char toPoint = ' ';
                    isLastPoint = i == player.LastPoint.X && j == player.LastPoint.Y;
                    if (isLastPoint)
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    switch (player.Board[i, j])
                    {
                        case CellState.Hitted:
                            toPoint = 'x';
                            break;
                        case CellState.Missed:
                            toPoint = 'o';
                            break;
                        case CellState.Unknown:
                            toPoint = (isLose && opponent.Fleet.Ships.Any(s => s.Position.Keys.Contains(new Point(i, j)))) ? '#' : '.';
                            break;
                    }
                    Console.Write(toPoint + " ");
                    if (isLastPoint)
                        Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public static void PrintFleet(Fleet fleet)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                    Console.Write(fleet.Ships.Any(s => s.Position.Keys.Contains(new Point(i, j))) ? "# " : ". ");
                Console.WriteLine();
            }
        }

        public static void PrintBoard(Player player)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    char toPoint = ' ';
                    switch (player.Board[i, j])
                    {
                        case CellState.Hitted:
                            toPoint = 'x';
                            break;
                        case CellState.Missed:
                            toPoint = 'o';
                            break;
                        case CellState.Unknown:
                            toPoint = '.';
                            break;
                    }
                    Console.Write(toPoint + " ");
                }
                Console.WriteLine();
            }
        }

        public static void PrintShip(Ship ship)
        {
            Console.WriteLine(ship.State);
            foreach (var value in ship.Position)
            {
                Console.WriteLine($"\t{value.Value} - {value.Key}");
            }
        }
    }
}
