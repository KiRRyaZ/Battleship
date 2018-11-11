using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    public abstract class Game
    {
        static HitType shoot = HitType.Hit;
        static Point toShoot;


        public abstract void Start();
        public abstract void Run();

        public void HumanShoot(Human human, Player bot)
        {
            bool success = false;
            do
            {
                try
                {
                   do
                    {
                        GUI.PrintYourTurn();
                        toShoot = GUI.GetCoords(false);
                        shoot = human.Shoot(toShoot);
                        GUI.PrintBoards(human, bot);
                        GUI.PrintShoot(toShoot, shoot, human.Name);
                        GUI.PrintStats(human, bot);
                    } while (!shoot.Equals(HitType.Miss) && bot.Fleet.IsAlive());
                    success = true;
                    Thread.Sleep(1500);
                }
                catch (Exception ex)
                {
                    GUI.PrintException(ex.Message, 25);
                }
            } while (!success);
        }

        public void BotShoot(Bot bot, Human human)
        {
            do
            {
                shoot = bot.Shoot();
                GUI.PrintBoards(human, bot);
                GUI.PrintShoot(bot.LastPoint, shoot, bot.Name);
                GUI.PrintStats(human, bot);
                Thread.Sleep(1500);
            } while (!shoot.Equals(HitType.Miss) && human.Fleet.IsAlive());
        }
    }

    public class GamePlayerVSBot: Game
    {
        public Human Human { get; private set; }
        public Bot Bot { get; private set; }

        public override void Start()
        {            

            String name = GUI.StartWithName().Trim();
            int boardSize = GUI.GetBoardSize();

            FleetBuilder builder = new FleetBuilder(new RandomBuildStrategy(boardSize));

            Bot = Bot.GetBot(boardSize);
            Bot.Fleet = builder.GetFleet(boardSize);

            Human = new Human(name == "" ? "Игрок" : name, boardSize);
            Bot.Opponent = Human;
            Human.Opponent = Bot;


            if (GUI.IsGUIStrategy())
                builder.SetStrategy(new GUIBuildStrategy());

            bool success = false;
            do
            {
                try
                {
                    Human.Fleet = builder.GetFleet(boardSize);
                    success = true;
                }
                catch (Exception ex)
                {                    
                    GUI.PrintException(ex.Message.Split('\r')[0], 13);
                }
            } while (!success);
        }

        public override void Run()
        {
            File.AppendAllText("Battleship.txt", "\n\nNew Game");

            GUI.PrintBoards(Human, Bot);
            GUI.PrintStats(Human, Bot);
            GUI.StartShooting();
            while (true)
            {
                HumanShoot(Human, Bot);
                if (!Bot.Fleet.IsAlive())
                {
                    GUI.PrintWin(Human);
                    break;
                }
                BotShoot(Bot, Human);
                if (!Human.Fleet.IsAlive())
                {
                    GUI.PrintBoards(Human, Bot, true);
                    GUI.PrintWin(Bot);
                    break;
                }
            }
        }
    }

    public class GamePlayerVSPlayer : Game
    {
        public Human Human1 { get; private set; }
        public Human Human2 { get; private set; }

        public GamePlayerVSPlayer(Human h1, Human h2)
        {
            Human1 = h1;
            Human2 = h2;
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }
    }

}
