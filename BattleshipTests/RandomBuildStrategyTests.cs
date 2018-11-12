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
    public class RandomBuildStrategyTests
    {
        [TestMethod()]
        public void BuildBattleshipTest()
        {
            RandomBuildStrategy rbs = new RandomBuildStrategy();            
            Fleet fleet = new Fleet();
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.ThrowsException<Exception>(() => rbs.BuildBattleship());

            rbs.PrepareBeforeBuild(fleet, 10);
            rbs.BuildBattleship();
            Assert.AreEqual(1, fleet.GetBattleships().Length);

            rbs.BuildBattleship();
            Assert.AreEqual(2, fleet.GetBattleships().Length);
        }

        [TestMethod()]
        public void BuildCruisersTest()
        {
            RandomBuildStrategy rbs = new RandomBuildStrategy();
            Fleet fleet = new Fleet();
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            Assert.ThrowsException<Exception>(() => rbs.BuildCruisers());

            rbs.PrepareBeforeBuild(fleet, 5);
            rbs.BuildCruisers();
            Assert.AreEqual(1, fleet.GetCruisers().Length);
            rbs.BuildCruisers();
            Assert.AreEqual(2, fleet.GetCruisers().Length);

        }

        [TestMethod()]
        public void BuildDestroyersTest()
        {
            RandomBuildStrategy rbs = new RandomBuildStrategy();
            Fleet fleet = new Fleet();
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.AreEqual(0, fleet.GetDestroyers().Length);
            Assert.ThrowsException<Exception>(() => rbs.BuildDestroyers());

            rbs.PrepareBeforeBuild(fleet, 10);
            rbs.BuildDestroyers();
            Assert.AreEqual(1, fleet.GetDestroyers().Length);
            rbs.BuildDestroyers();
            Assert.AreEqual(2, fleet.GetDestroyers().Length);
        }

        [TestMethod()]
        public void BuildSubmarinesTest()
        {
            RandomBuildStrategy rbs = new RandomBuildStrategy();
            Fleet fleet = new Fleet();
            rbs.PrepareBeforeBuild(fleet, 10);
            Assert.AreEqual(0, fleet.GetSubmarines().Length);            
            rbs.BuildSubmarines();
            Assert.AreEqual(1, fleet.GetSubmarines().Length);
            rbs.BuildSubmarines();
            Assert.AreEqual(2, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void PrepareBeforeBuildTest()
        {
            RandomBuildStrategy rbs = new RandomBuildStrategy();
            Fleet fleet = new Fleet();
            rbs.PrepareBeforeBuild(fleet, 10);
            Assert.AreEqual(10, rbs.BoardSize);
            Assert.AreEqual(fleet, rbs.Fleet);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rbs.PrepareBeforeBuild(new Fleet(), -5));
        }
    }
}