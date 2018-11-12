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
        [TestMethod()]
        public void GetBotTest()
        {
            Bot bot = Bot.GetBot(5);
            Assert.AreEqual(bot, Bot.GetBot(5));
            bot.State = new FirstHitState();
            bot = Bot.GetBot(10);
            Assert.AreEqual(new NotHitState().GetType(), bot.State.GetType());
        }

        [TestMethod()]
        public void ChangeBoard()
        {
            Bot bot = Bot.GetBot(10);
            bot.ChangeBoard(5);
            Assert.AreEqual(5, bot.BoardSize);
            Assert.AreEqual(5, bot.State.Bot.BoardSize);
        }

        [TestMethod()]
        public void ShootTest()
        {
            Bot bot = Bot.GetBot(10);
            bot.State = new NextHitState();
            bot.LastHitted.Clear();
            bot.LastHitted.Add(new Point(0, 0));
            bot.LastHitted.Add(new Point(1, 0));
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetBattleships()[0].Position[new Point(0, 0)] = ShipCellState.Hitted;
            fleet.GetBattleships()[0].Position[new Point(1, 0)] = ShipCellState.Hitted;
            Human human = new Human("", 10) { Opponent = bot };
            bot.Opponent = human;
            human.Fleet = fleet;

            Assert.AreEqual(HitType.Hit, bot.Shoot());
            Assert.AreEqual(HitType.Kill, bot.Shoot());
            Assert.AreEqual(HitType.Miss, bot.Shoot());
        }
    }
}