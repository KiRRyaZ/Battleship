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
        [TestMethod()]
        public void StateTest()
        {
            FirstHitState state = new FirstHitState();
            Assert.IsNull(state.Bot);
            state.Bot = Bot.GetBot(10);
            Assert.IsNotNull(state.Bot);
            Assert.AreEqual(10, state.Bot.BoardSize);            
        }
        [TestMethod()]
        public void ChooseCellTest()
        {
            FirstHitState state = new FirstHitState();
            Assert.ThrowsException<NullReferenceException>(() => state.ChooseCell());
            state.Bot = Bot.GetBot(10);
            state.Bot.State = state;
            state.Bot.LastHitted.Clear();

            state.Bot.LastPoint = new Point(0, 0);
            state.Bot.LastHitted.Add(state.Bot.LastPoint);
            state.Bot.Board[0, 1] = CellState.Missed;
            Assert.AreEqual(new Point(1, 0), state.ChooseCell());
            state.Bot.LastHitted.Clear();

            state.Bot.LastPoint = new Point(5, 5);
            state.Bot.LastHitted.Add(state.Bot.LastPoint);
            state.Bot.Board[4, 5] = CellState.Missed;
            state.Bot.Board[5, 4] = CellState.Missed;
            state.Bot.Board[6, 5] = CellState.Missed;
            Assert.AreEqual(new Point(5, 6), state.ChooseCell());

            state.Bot.Board[5, 6] = CellState.Missed;
            Assert.ThrowsException<Exception>(() => state.ChooseCell());

        }

        [TestMethod()]
        public void AfterShootTest()
        {
            FirstHitState state = new FirstHitState();
            state.Bot = Bot.GetBot(10);
            state.Bot.LastPoint = new Point(0, 0);
            state.Bot.LastHitted.Add(state.Bot.LastPoint);
            state.Bot.State = state;
            state.Bot.LastHitted.Clear();

            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(state.Bot.State, new FirstHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHitted.Count);

            state.AfterShoot(HitType.Kill);
            Assert.IsInstanceOfType(state.Bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHitted.Count);

            state.Bot.State = state;

            state.AfterShoot(HitType.Hit);
            Assert.IsInstanceOfType(state.Bot.State, new NextHitState().GetType());
            Assert.AreEqual(1, state.Bot.LastHitted.Count);
        }
    }
}