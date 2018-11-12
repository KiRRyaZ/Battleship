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

        [TestMethod()]
        public void StateTest()
        {
            NotHitState state = new NotHitState();
            Assert.IsNull(state.Bot);
            state.Bot = Bot.GetBot(10);
            Assert.IsNotNull(state.Bot);
            Assert.AreEqual(10, state.Bot.BoardSize);
        }
        [TestMethod()]
        public void ChooseCellTest()
        {
            NotHitState state = new NotHitState();
            Assert.ThrowsException<NullReferenceException>(() => state.ChooseCell());
            state.Bot = Bot.GetBot(10);
            state.Bot.State = state;
            Assert.IsNotNull(state.ChooseCell());

            int boardSize = 5;
            state.Bot.ChangeBoard(boardSize);
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    state.Bot.Board[i, j] = CellState.Missed;
            Assert.ThrowsException<Exception>(() => state.ChooseCell());

            state.Bot.Board[0, 1] = CellState.Unknown;
            Assert.IsNotNull(state.ChooseCell());
        }

        [TestMethod()]
        public void AfterShootTest()
        {
            NotHitState state = new NotHitState
            {
                Bot = Bot.GetBot(10)
            };
            state.Bot.LastPoint = new Point(0, 0);
            state.Bot.State = state;

            state.AfterShoot(HitType.Miss);
            Assert.IsInstanceOfType(state.Bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHitted.Count);

            state.AfterShoot(HitType.Kill);
            Assert.IsInstanceOfType(state.Bot.State, new NotHitState().GetType());
            Assert.AreEqual(0, state.Bot.LastHitted.Count);

            state.AfterShoot(HitType.Hit);
            Assert.IsInstanceOfType(state.Bot.State, new FirstHitState().GetType());
            Assert.AreEqual(1, state.Bot.LastHitted.Count);



        }
    }
}