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
        Ship ship = new Ship();
        [TestInitialize()] 
        public void TestInitialize()
        {
            ship.Position.Clear();
        }
        [TestMethod()]
        public void ShipIsAliveTest()
        {
            Assert.AreEqual(ShipState.Alive, ship.State);
        }
        [TestMethod()]
        public void ShipHasPositionTest()
        {
            Assert.IsNotNull(ship.Position);
        }
        [TestMethod()]
        public void AddTest()
        {
            ship.Add(new Point(0, 0));
            ship.Add(new Point(1, 0));
            Assert.IsTrue(ship.Position.Values.All(v => v == ShipCellState.Alive));
        }
        [TestMethod()]
        public void AddInvalidPointsTest()
        {
            ship.Add(new Point(0, 0));
            ship.Add(new Point(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ship.Add(new Point(0, 1)));            
        }
        [TestMethod()]
        public void ChangeStateTest()
        {
            Fleet fleet = new Fleet();
            fleet.Ships.Add(ship);
            ship.Add(new Point(0, 0));
            ship.Add(new Point(1, 0));
            ship.Position[new Point(0, 0)] = ShipCellState.Hit;
            fleet.GetShootIn(new Point(1, 0));
            Assert.AreEqual(ShipState.Sunk, ship.State);
        }
    }
}