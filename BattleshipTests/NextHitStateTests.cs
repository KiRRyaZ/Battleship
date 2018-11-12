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
        [TestMethod()]
        public void StateTest()
        {
            NextHitState state = new NextHitState();
            Assert.IsNull(state.Bot);
            state.Bot = Bot.GetBot(10);
            Assert.IsNotNull(state.Bot);
            Assert.AreEqual(10, state.Bot.BoardSize);            
        }
        [TestMethod()]
        public void ChooseCellTest()
        {
            NextHitState state = new NextHitState();
            Assert.ThrowsException<NullReferenceException>(() => state.ChooseCell());
            state.Bot = Bot.GetBot(10);
            state.Bot.State = state;
            state.Bot.LastHitted.Clear();

            state.Bot.LastHitted.Add(new Point(5, 5));
            state.Bot.LastHitted.Add(new Point(5, 6));
            state.Bot.Board[5, 7] = CellState.Missed;
            Assert.AreEqual(new Point(5, 4), state.ChooseCell());

            state.Bot.LastHitted.Clear();
            state.Bot.LastHitted.Add(new Point(2, 2));
            state.Bot.LastHitted.Add(new Point(3, 2));
            state.Bot.Board[1, 2] = CellState.Missed;
            Assert.AreEqual(new Point(4, 2), state.ChooseCell());

            state.Bot.Board[4, 2] = CellState.Missed;
            Assert.ThrowsException<Exception>(() => state.ChooseCell());
        }

        [TestMethod()]
        public void AfterShootTest()
        {
            NextHitState state = new NextHitState();
            state.Bot = Bot.GetBot(10);
            state.Bot.LastPoint = new Point(0, 0);
            state.Bot.LastHitted.Clear();
            state.Bot.LastHitted.Add(state.Bot.LastPoint);
            state.Bot.State = state;

            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(state.Bot.State, new NextHitState().GetType());
            Assert.AreEqual(1, state.Bot.LastHitted.Count);

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