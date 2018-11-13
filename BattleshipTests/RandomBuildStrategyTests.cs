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
        RandomBuildStrategy rbs = new RandomBuildStrategy();
        Fleet fleet = new Fleet();

        [TestInitialize()]
        public void TestInitialize()
        {
            rbs.PrepareBeforeBuild(fleet, 10);
        }

        [TestMethod()]
        public void BuildBattleshipSmallBoardSizeTest()
        {
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.ThrowsException<Exception>(() => rbs.BuildBattleship());
        }

        [TestMethod()]
        public void BuildBattleshipTest()
        {
            rbs.BuildBattleship();
            Assert.AreEqual(1, fleet.GetBattleships().Length);
            rbs.BuildBattleship();
            Assert.AreEqual(2, fleet.GetBattleships().Length);
        }

        [TestMethod()]
        public void BuildCruisersSmallBoardSizeTest()
        {
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.ThrowsException<Exception>(() => rbs.BuildCruisers());
        }

        [TestMethod()]
        public void BuildCruisersTest()
        {
            rbs.BuildCruisers();
            Assert.AreEqual(1, fleet.GetCruisers().Length);
            rbs.BuildCruisers();
            Assert.AreEqual(2, fleet.GetCruisers().Length);
        }

        [TestMethod()]
        public void BuildDestroyersSmallBoardSizeTest()
        {
            rbs.PrepareBeforeBuild(fleet, 1);
            Assert.ThrowsException<Exception>(() => rbs.BuildDestroyers());
        }

        [TestMethod()]
        public void BuildDestroyersTest()
        {
            rbs.BuildDestroyers();
            Assert.AreEqual(1, fleet.GetDestroyers().Length);
            rbs.BuildDestroyers();
            Assert.AreEqual(2, fleet.GetDestroyers().Length);
        }

        [TestMethod()]
        public void BuildSubmarinesTest()
        {       
            rbs.BuildSubmarines();
            Assert.AreEqual(1, fleet.GetSubmarines().Length);
            rbs.BuildSubmarines();
            Assert.AreEqual(2, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void PrepareBeforeBuildBoardSizeTest()
        {
            Assert.AreEqual(10, rbs.BoardSize);
        }

        [TestMethod()]
        public void PrepareBeforeBuildFleetTest()
        {
            Assert.AreEqual(fleet, rbs.Fleet);
        }

        [TestMethod()]
        public void PrepareBeforeBuildInvalidBoardSizeTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rbs.PrepareBeforeBuild(fleet, -5));
        }
    }
}