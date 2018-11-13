using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipTests
{
    [TestClass()]
    public class FleetBuilderTests
    {
        FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
        Fleet fleet = new Fleet();

        [TestMethod()]
        public void FleetBuilderStrategyTest()
        {
            Assert.IsNotNull(fleetBuilder.BuildStrategy);
        }

        [TestMethod()]
        public void GetFleetForInvalidBoardSizeTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleetBuilder.GetFleet(3));
        }

        [TestMethod()]
        public void GetFleetFor10BoardTest()
        {
            fleet = fleetBuilder.GetFleet(10);
            Assert.AreEqual(1, fleet.GetBattleships().Length);
            Assert.AreEqual(2, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void GetFleetFor9BoardTest()
        {
            fleet = fleetBuilder.GetFleet(9);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(2, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void GetFleetFor8BoardTest()
        {
            fleet = fleetBuilder.GetFleet(8);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(1, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void GetFleetFor7BoardTest()
        {
            fleet = fleetBuilder.GetFleet(7);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void GetFleetFor6BoardTest()
        {
            fleet = fleetBuilder.GetFleet(6);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            Assert.AreEqual(2, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void GetFleetFor5BoardTest()
        {
            fleet = fleetBuilder.GetFleet(5);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            Assert.AreEqual(1, fleet.GetDestroyers().Length);
            Assert.AreEqual(3, fleet.GetSubmarines().Length);
        }
    }
}