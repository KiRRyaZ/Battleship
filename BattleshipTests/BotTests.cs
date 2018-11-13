using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleshipTests
{
    [TestClass()]
    public class BotTests
    {
        Bot bot;
        Human human;
        
        [TestInitialize()]
        public void TestInitialize()
        {
            bot = Bot.GetBot(10);
            human = new Human("", 10) { Opponent = bot };
            bot.Opponent = human;
        }    

        [TestMethod()]
        public void BotsAreEqualTest()
        {
            Assert.AreEqual(bot, Bot.GetBot(10));
        }
        
        [TestMethod()]
        public void NewBotHasNewStateTest()
        {
            bot.State = new FirstHitState();
            bot = Bot.GetBot(8);
            Assert.IsInstanceOfType(bot.State, new NotHitState().GetType());
        }

        [TestMethod()]
        public void ChangeBoardTest()
        {
            bot.ChangeBoard(5);
            Assert.AreEqual(5, bot.BoardSize);
        }

        [TestMethod()]
        public void ShootInNextHitStateTest()
        {
            bot.State = new NextHitState();
            bot.LastHits.Add(new Point(0, 0));
            bot.LastHits.Add(new Point(1, 0));
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetBattleships()[0].Position[new Point(0, 0)] = ShipCellState.Hit;
            fleet.GetBattleships()[0].Position[new Point(1, 0)] = ShipCellState.Hit;
            human.Fleet = fleet;

            Assert.AreEqual(HitType.Hit, bot.Shoot());
            Assert.AreEqual(HitType.Kill, bot.Shoot());
        }

        [TestMethod()]
        public void ShootInNextHitStateThrowsExceptionTest()
        {
            bot.State = new NextHitState();
            bot.LastHits.Add(new Point(0, 0));
            bot.LastHits.Add(new Point(1, 0));
            bot.Board[2, 0] = CellState.Missed;

            Assert.ThrowsException<Exception>(() => bot.Shoot());
        }

        [TestMethod()]
        public void ShootInFirstHitStateTest()
        {
            bot.State = new FirstHitState();
            bot.LastPoint = new Point(5, 5);
            Fleet fleet = new Fleet();
            fleet.AddDestroyer(new Point(5, 5), new Point(5, 6));
            fleet.GetDestroyers()[0].Position[new Point(5, 5)] = ShipCellState.Hit;
            bot.Board[5, 4] = CellState.Missed;
            bot.Board[4, 5] = CellState.Missed;
            bot.Board[6, 5] = CellState.Missed;
            human.Fleet = fleet;

            Assert.AreEqual(HitType.Kill, bot.Shoot());
        }

        [TestMethod()]
        public void ShootInFirstHitStateThrowsExceptionTest()
        {
            bot.State = new FirstHitState();
            bot.LastHits.Add(new Point(5, 5));
            bot.Board[5, 4] = CellState.Missed;
            bot.Board[4, 5] = CellState.Missed;
            bot.Board[6, 5] = CellState.Missed;
            bot.Board[5, 6] = CellState.Missed;

            Assert.ThrowsException<Exception>(() => bot.Shoot());
        }

        [TestMethod()]
        public void ShootInNotHitStateTest()
        {
            bot.ChangeBoard(5);            
            for (int i = 0; i < bot.BoardSize; i++)
                for (int j = 0; j < bot.BoardSize; j++)
                    bot.Board[i, j] = CellState.Missed;
            bot.Board[3, 2] = CellState.Unknown;
            human.Fleet = new Fleet();

            Assert.AreEqual(HitType.Miss, bot.Shoot());
            Assert.ThrowsException<Exception>(() => bot.Shoot());
        }
    }
}