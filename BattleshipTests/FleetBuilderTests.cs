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
        [TestMethod()]
        public void FleetBuilderTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            Assert.IsNotNull(fleetBuilder);
            Assert.IsNotNull(fleetBuilder.BuildStrategy);
        }

        [TestMethod()]
        public void PrepareBeforeBuildTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            fleetBuilder.PrepareBeforeBuild(10);
        }

        [TestMethod()]
        public void BuildBattleshipTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            fleetBuilder.PrepareBeforeBuild(10);
            fleetBuilder.BuildBattleship();
        }

        [TestMethod()]
        public void BuildCruisersTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            fleetBuilder.PrepareBeforeBuild(10);
            fleetBuilder.BuildCruisers();
        }

        [TestMethod()]
        public void BuildDestroyersTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            fleetBuilder.PrepareBeforeBuild(10);
            fleetBuilder.BuildDestroyers();
        }

        [TestMethod()]
        public void BuildSubmarinesTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            fleetBuilder.PrepareBeforeBuild(10);
            fleetBuilder.BuildSubmarines();
        }

        [TestMethod()]
        public void GetFleetTest()
        {
            FleetBuilder fleetBuilder = new FleetBuilder(new RandomBuildStrategy());
            Fleet fleet = fleetBuilder.GetFleet(10);
            Assert.AreEqual(1, fleet.GetBattleships().Length);
            Assert.AreEqual(2, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);
            fleet = fleetBuilder.GetFleet(7);
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            Assert.AreEqual(3, fleet.GetDestroyers().Length);
            Assert.AreEqual(4, fleet.GetSubmarines().Length);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleetBuilder.GetFleet(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleetBuilder.GetFleet(33));
        }
    }
}