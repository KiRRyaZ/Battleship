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
    public class NotHitStateTests
    {
        NotHitState state = new NotHitState();
        Bot bot = Bot.GetBot(10);

        [TestInitialize()]
        public void TestInitialize()
        {
            bot.State = state;
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
            Assert.IsNotNull(state.ChooseCell());
        }

        [TestMethod()]
        public void ChooseCellFromAllKnownTest()
        {
            bot.ChangeBoard(7);
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                    state.Bot.Board[i, j] = CellState.Missed;
            Assert.ThrowsException<Exception>(() => state.ChooseCell());
        }

        [TestMethod()]
        public void AfterMissShootTest()
        {
            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHits.Count);
        }

        [TestMethod()]
        public void AfterKillShootTest()
        {
            state.AfterShoot(HitType.Kill);
            Assert.IsInstanceOfType(state.Bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHits.Count);
        }

        [TestMethod()]
        public void AfterHitShootTest()
        {
            bot.LastPoint = new Point(0, 0);

            state.AfterShoot(HitType.Hit);
            Assert.IsInstanceOfType(state.Bot.State, new FirstHitState().GetType());
            Assert.AreEqual(bot.LastPoint, state.Bot.LastHits.Last());
        }
    }
}