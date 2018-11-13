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
    public class NextHitStateTests
    {
        NextHitState state = new NextHitState();
        Bot bot = Bot.GetBot(10);

        [TestInitialize()]
        public void TestInitialize()
        {
            bot.State = state;
            bot.LastHits.Clear();
            bot.ChangeBoard(10);
        }

        [TestMethod()]
        public void ChooseCellWithoutBotTest()
        {
            state.Bot = null;
            Assert.ThrowsException<NullReferenceException>(() => state.ChooseCell());
        }

        [TestMethod()]
        public void ChooseCellTest()
        {
            bot.LastHits.Add(new Point(5, 5));
            bot.LastHits.Add(new Point(5, 6));
            state.Bot.Board[5, 7] = CellState.Missed;
            Assert.AreEqual(new Point(5, 4), state.ChooseCell());
        }

        [TestMethod()]
        public void ChooseCellFromAllKnownTest()
        {
            bot.LastHits.Add(new Point(5, 5));
            bot.LastHits.Add(new Point(5, 6));
            bot.Board[5, 4] = CellState.Missed;
            bot.Board[5, 7] = CellState.Missed;

            Assert.ThrowsException<Exception>(() => state.ChooseCell());
        }

        [TestMethod()]
        public void AfterMissShootTest()
        {
            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(bot.State, new NextHitState().GetType());
        }

        [TestMethod()]
        public void AfterKillShootTest()
        {
            state.AfterShoot(HitType.Kill);
            Assert.IsInstanceOfType(state.Bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, bot.LastHits.Count);
        }

        [TestMethod()]
        public void AfterHitShootTest()
        {
            bot.LastPoint = new Point(0, 0);

            state.AfterShoot(HitType.Hit);
            Assert.IsInstanceOfType(state.Bot.State, new NextHitState().GetType());
            Assert.AreEqual(bot.LastPoint, state.Bot.LastHits.Last());
        }
    }
}