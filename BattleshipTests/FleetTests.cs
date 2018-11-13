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
    public class FleetTests
    {
        Fleet fleet = new Fleet();
        [TestInitialize()]
        public void TestInitialize()
        {
            fleet.Ships.Clear();
        }

        [TestMethod()]
        public void FleetShipsTest()
        {
            Assert.IsNotNull(fleet.Ships);
        }

        [TestMethod()]
        public void GetBattleshipTest()
        {
            fleet.AddBattleship(new Point(0, 0), new Point(0, 3));
            fleet.AddBattleship(new Point(5, 5), new Point(2, 5));
            Assert.AreEqual(2, fleet.GetBattleships().Length);
        }

        [TestMethod()]
        public void GetCruisersTest()
        {
            fleet.AddCruiser(new Point(0, 0), new Point(0, 2));
            fleet.AddCruiser(new Point(5, 5), new Point(3, 5));
            Assert.AreEqual(2, fleet.GetCruisers().Length);
        }

        [TestMethod()]
        public void GetDestroyersTest()
        {
            fleet.AddDestroyer(new Point(0, 0), new Point(0, 1));
            fleet.AddDestroyer(new Point(5, 5), new Point(4, 5));
            Assert.AreEqual(2, fleet.GetDestroyers().Length);
        }

        [TestMethod()]
        public void GetSubmarinesTest()
        {
            fleet.AddSubmarine(new Point(0, 2));
            fleet.AddSubmarine(new Point(3, 5));
            Assert.AreEqual(2, fleet.GetSubmarines().Length);
        }

        [TestMethod()]
        public void AddBattleshipTest()
        {
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            CollectionAssert.AreEqual(new Point[] { new Point(0, 0),
                                                    new Point(1, 0),
                                                    new Point(2, 0),
                                                    new Point(3, 0) }, 
                fleet.GetBattleships()[0].Position.Keys.ToArray());
        }

        [TestMethod()]
        public void AddBattleshipInvalidPointsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(-45, 0), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddBattleshipInvalidLengthTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(0, 0), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddBattleshipInvalidPointsPositionTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(1, 3), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddBattleshipNearAnotherTest()
        {
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(1, 0), new Point(1, 3)));
        }

        [TestMethod()]
        public void AddCruiserTest()
        {
            fleet.AddCruiser(new Point(0, 0), new Point(2, 0));
            CollectionAssert.AreEqual(new Point[] { new Point(0, 0),
                                                    new Point(1, 0),
                                                    new Point(2, 0)},
                fleet.GetCruisers()[0].Position.Keys.ToArray());
        }

        [TestMethod()]
        public void AddCruiserInvalidPointsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(-45, 0), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddCruiserInvalidLengthTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(0, 0), new Point(6, 0)));
        }

        [TestMethod()]
        public void AddCruiserInvalidPointsPositionTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(1, 3), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddCruiserNearAnotherTest()
        {
            fleet.AddCruiser(new Point(0, 0), new Point(2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(1, 0), new Point(1, 2)));
        }

        [TestMethod()]
        public void AddDestroyerTest()
        {
            fleet.AddDestroyer(new Point(0, 0), new Point(1, 0));
            CollectionAssert.AreEqual(new Point[] { new Point(0, 0),
                                                    new Point(1, 0)},
                fleet.GetDestroyers()[0].Position.Keys.ToArray());
        }

        [TestMethod()]
        public void AddDestroyerInvalidPointsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(-45, 0), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddDestroyerInvalidLengthTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(0, 0), new Point(6, 0)));
        }

        [TestMethod()]
        public void AddDestroyerInvalidPointsPositionTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(1, 3), new Point(2, 0)));
        }

        [TestMethod()]
        public void AddDestroyerNearAnotherTest()
        {
            fleet.AddDestroyer(new Point(0, 0), new Point(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(1, 0), new Point(1, 1)));
        }

        [TestMethod()]
        public void AddSubmarineTest()
        {
            fleet.AddSubmarine(new Point(1, 0));
            CollectionAssert.AreEqual(new Point[] { new Point(1, 0) },
                fleet.GetSubmarines()[0].Position.Keys.ToArray());
        }

        [TestMethod()]
        public void AddSubmarineInvalidPointsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddSubmarine(new Point(-45, 0)));
        }

        [TestMethod()]
        public void AddSubmarineNearAnotherTest()
        {
            fleet.AddSubmarine(new Point(0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddSubmarine(new Point(1, 0)));
        }

        [TestMethod()]
        public void GetShootInHitTest()
        {
            fleet.AddCruiser(new Point(3, 3), new Point(3, 5));

            Assert.AreEqual(HitType.Hit, fleet.GetShootIn(new Point(3, 4)));
        }

        [TestMethod()]
        public void GetShootInKillTest()
        {
            fleet.AddSubmarine(new Point(3, 4));
            Assert.AreEqual(HitType.Kill, fleet.GetShootIn(new Point(3, 4)));
        }

        [TestMethod()]
        public void GetShootInMissTest()
        {
            Assert.AreEqual(HitType.Miss, fleet.GetShootIn(new Point(0, 0)));
        }

        [TestMethod()]
        public void GetShootInKnownCellTest()
        {
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetShootIn(new Point(0, 0));
            Assert.ThrowsException<ArgumentException>(() => fleet.GetShootIn(new Point(0, 0)));
        }

        [TestMethod()]
        public void GetShootInSunkShipTest()
        {
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.GetBattleships()[0].State = ShipState.Sunk;
            Assert.ThrowsException<ArgumentException>(() => fleet.GetShootIn(new Point(0, 0)));
        }

        [TestMethod()]
        public void IsAliveTest()
        {
            fleet.AddBattleship(new Point(1, 1), new Point(4, 1));
            Assert.IsTrue(fleet.IsAlive());
        }

        [TestMethod()]
        public void IsNotAliveTest()
        {
            fleet.AddBattleship(new Point(1, 1), new Point(4, 1));
            fleet.GetBattleships()[0].State = ShipState.Sunk;
            Assert.IsFalse(fleet.IsAlive());
        }
    }
}