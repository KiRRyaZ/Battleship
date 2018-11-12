using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            GamePlayerVSBot Game = new GamePlayerVSBot();
            do {                
                Game.Start();
                Game.Run();
                Console.WriteLine("Нажмите Enter для еще одной игры\nИли любую другую клавишу для выхода...");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);            
        }


    }
}



