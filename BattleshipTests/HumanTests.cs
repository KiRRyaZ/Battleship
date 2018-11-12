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
        [TestMethod()]
        public void HumanTest()
        {
            Human human = new Human("test", 5);
            Assert.AreEqual("test", human.Name);
            Assert.AreEqual(5, human.BoardSize);
            Assert.AreEqual(25, human.Board.Length);
            Assert.IsNotNull(human.LastHitted);
            Assert.IsNotNull(human.LastPoint);
            Assert.IsNull(human.Opponent);
        }

        [TestMethod()]
        public void ShootTest()
        {
            Bot bot = Bot.GetBot(10);            
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetBattleships()[0].Position[new Point(0, 0)] = ShipCellState.Hitted;
            fleet.GetBattleships()[0].Position[new Point(1, 0)] = ShipCellState.Hitted;
            Human human = new Human("", 10) { Opponent = bot };
            bot.Opponent = human;
            bot.Fleet = fleet;

            Assert.AreEqual(HitType.Hit, human.Shoot(new Point(2, 0)));
            Assert.AreEqual(new Point(2, 0), human.LastPoint);
            Assert.AreEqual(new Point(2, 0), human.LastHitted.Last());            
            Assert.AreEqual(HitType.Kill, human.Shoot(new Point(3, 0)));
            Assert.AreEqual(new Point(3, 0), human.LastPoint);
            Assert.AreEqual(0, human.LastHitted.Count);
            Assert.AreEqual(HitType.Miss, human.Shoot(new Point(3, 3)));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => human.Shoot(new Point(12, 56)));
            Assert.ThrowsException<ArgumentException>(() => human.Shoot(new Point(2, 0)));
            Assert.ThrowsException<ArgumentException>(() => human.Shoot(new Point(1, 0)));

        }
    }
}