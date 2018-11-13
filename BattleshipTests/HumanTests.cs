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
    public class HumanTests
    {
        Human human = new Human("test", 5);
        Bot bot = Bot.GetBot(5);

        [TestInitialize()]
        public void TestInitialize()
        {
            human.ChangeBoard(5);
            human.Opponent = bot;
        }

        [TestMethod()]
        public void HumanNameEqualsTest()
        {            
            Assert.AreEqual("test", human.Name);
        }

        [TestMethod()]
        public void HumanBoardSizeEqualsTest()
        {
            Assert.AreEqual(5, human.BoardSize);
        }

        [TestMethod()]
        public void HumanLastHittedIsInitializedTest()
        {
            Assert.IsNotNull(human.LastHits);
        }

        [TestMethod()]
        public void HumanLastPointIsInitializedTest()
        {
            Assert.IsNotNull(human.LastPoint);
        }


        [TestMethod()]
        public void ShootTest()
        {       
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetBattleships()[0].Position[new Point(0, 0)] = ShipCellState.Hit;
            fleet.GetBattleships()[0].Position[new Point(1, 0)] = ShipCellState.Hit;
            bot.Fleet = fleet;

            Assert.AreEqual(HitType.Hit, human.Shoot(new Point(2, 0)));          
            Assert.AreEqual(HitType.Kill, human.Shoot(new Point(3, 0)));
            Assert.AreEqual(HitType.Miss, human.Shoot(new Point(3, 3)));
        }

        [TestMethod()]
        public void ShootInvalidPointTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => human.Shoot(new Point(12, 56)));
        }

        [TestMethod()]
        public void ShootKnownPointTest()
        {
            human.Board[0, 1] = CellState.Missed;
            Assert.ThrowsException<ArgumentException>(() => human.Shoot(new Point(0, 1)));
        }

        [TestMethod()]
        public void LastHittedAfterHitShootTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            bot.Fleet = fleet;

            human.Shoot(new Point(0, 0));
            human.Shoot(new Point(1, 0));
            CollectionAssert.AreEqual(new List<Point> { new Point(0, 0), new Point(1, 0) }, human.LastHits);
        }

        [TestMethod()]
        public void LastHittedAfterKillShootTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddDestroyer(new Point(0, 0), new Point(1, 0));
            bot.Fleet = fleet;

            human.Shoot(new Point(0, 0));
            human.Shoot(new Point(1, 0));
            
            CollectionAssert.AreEqual(new List<Point>(), human.LastHits);
        }

        [TestMethod()]
        public void LastPointAfterShootTest()
        {
            bot.Fleet = new Fleet();
            human.Shoot(new Point(0, 0));

            Assert.AreEqual(new Point(0, 0), human.LastPoint);
        }

        [TestMethod()]
        public void MissedAroundKilledTest()
        {
            human.LastHits.Add(new Point(2, 1));
            human.LastHits.Add(new Point(2, 2));

            human.MissedAroundKilled();

            Assert.IsTrue(new CellState[] { human.Board[1, 0],
                                            human.Board[1, 1],
                                            human.Board[1, 2],
                                            human.Board[1, 3],
                                            human.Board[2, 0],
                                            human.Board[2, 3],
                                            human.Board[3, 0],
                                            human.Board[3, 1],
                                            human.Board[3, 2],
                                            human.Board[3, 3] }.All(v => v == CellState.Missed));
        }
    }
}