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
    public class FirstHitStateTests
    {
        FirstHitState state = new FirstHitState();
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
            bot.LastPoint = new Point(5, 5);
            bot.Board[4, 5] = CellState.Missed;
            bot.Board[5, 4] = CellState.Missed;
            bot.Board[6, 5] = CellState.Missed;

            Assert.AreEqual(new Point(5, 6), state.ChooseCell());
        }

        [TestMethod()]
        public void ChooseCellFromAllKnownTest()
        {
            bot.LastPoint = new Point(5, 5);
            bot.Board[4, 5] = CellState.Missed;
            bot.Board[5, 4] = CellState.Missed;
            bot.Board[6, 5] = CellState.Missed;
            bot.Board[5, 6] = CellState.Missed;

            Assert.ThrowsException<Exception>(() => state.ChooseCell());
        }

        [TestMethod()]
        public void AfterMissShootTest()
        {
            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(bot.State, new FirstHitState().GetType());
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