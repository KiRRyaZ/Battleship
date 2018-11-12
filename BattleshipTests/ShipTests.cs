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
    public class ShipTests
    {
        [TestMethod()]
        public void ShipTest()
        {
            Ship ship = new Ship();
            Assert.AreEqual(ShipState.Alive, ship.State);
            Assert.IsNotNull(ship.Position);
        }

        [TestMethod()]
        public void AddTest()
        {
            Ship ship = new Ship();
            ship.Add(new Point(0, 0));
            ship.Add(new Point(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ship.Add(new Point(0, 1)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ship.Add(new Point(3, 0)));
            ship.Add(new Point(2, 0));
            ship.Add(new Point(3, 0));
        }
    }
}