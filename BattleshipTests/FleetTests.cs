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
        [TestMethod()]
        public void FleetTest()
        {
            Fleet fleet = new Fleet();
            Assert.IsNotNull(fleet.Ships);
        }

        [TestMethod()]
        public void GetBattleshipTest()
        {
            Fleet fleet = new Fleet();
            Assert.AreEqual(0, fleet.GetBattleships().Length);
            fleet.AddBattleship(new Point(0, 0), new Point(0, 3));
            fleet.AddBattleship(new Point(5, 5), new Point(2, 5));
            Assert.AreEqual(2, fleet.GetBattleships().Length);
            Assert.IsTrue(fleet.GetBattleships()[0].Position.Keys.Contains(new Point(0, 0)));
            Assert.IsTrue(fleet.GetBattleships()[1].Position.Keys.Contains(new Point(2, 5)));
        }

        [TestMethod()]
        public void GetCruisersTest()
        {
            Fleet fleet = new Fleet();
            Assert.AreEqual(0, fleet.GetCruisers().Length);
            fleet.AddCruiser(new Point(0, 0), new Point(0, 2));
            fleet.AddCruiser(new Point(5, 5), new Point(3, 5));
            Assert.AreEqual(2, fleet.GetCruisers().Length);
            Assert.IsTrue(fleet.GetCruisers()[0].Position.Keys.Contains(new Point(0, 0)));
            Assert.IsTrue(fleet.GetCruisers()[1].Position.Keys.Contains(new Point(3, 5)));
        }

        [TestMethod()]
        public void GetDestroyersTest()
        {
            Fleet fleet = new Fleet();
            Assert.AreEqual(0, fleet.GetDestroyers().Length);
            fleet.AddDestroyer(new Point(0, 0), new Point(0, 1));
            fleet.AddDestroyer(new Point(5, 5), new Point(4, 5));
            Assert.AreEqual(2, fleet.GetDestroyers().Length);
            Assert.IsTrue(fleet.GetDestroyers()[0].Position.Keys.Contains(new Point(0, 0)));
            Assert.IsTrue(fleet.GetDestroyers()[1].Position.Keys.Contains(new Point(4, 5)));
        }

        [TestMethod()]
        public void GetSubmarinesTest()
        {
            Fleet fleet = new Fleet();
            Assert.AreEqual(0, fleet.GetSubmarines().Length);
            fleet.AddSubmarine(new Point(0, 2));
            fleet.AddSubmarine(new Point(3, 5));
            Assert.AreEqual(2, fleet.GetSubmarines().Length);
            Assert.IsTrue(fleet.GetSubmarines()[0].Position.Keys.Contains(new Point(0, 2)));
            Assert.IsTrue(fleet.GetSubmarines()[1].Position.Keys.Contains(new Point(3, 5)));
        }

        [TestMethod()]
        public void AddBattleshipTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(-45, 0), new Point(2, 0)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(0, 0), new Point(2, 0)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(0, 0), new Point(0, 3)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddBattleship(new Point(1, 0), new Point(1, 3)));
            fleet.AddBattleship(new Point(1, 6), new Point(4, 6));
        }

        [TestMethod()]
        public void AddCruiserTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddCruiser(new Point(0, 0), new Point(2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(0, 0), new Point(0, -1)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(0, 0), new Point(1, 0)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(0, 0), new Point(0, 1)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddCruiser(new Point(1, 0), new Point(1, 2)));
            fleet.AddCruiser(new Point(1, 6), new Point(3, 6));
        }

        [TestMethod()]
        public void AddDestroyerTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddDestroyer(new Point(0, 0), new Point(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(0, 0), new Point(7, 0)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(0, 0), new Point(-345, 0)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(0, 0), new Point(0, 9)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddDestroyer(new Point(1, 0), new Point(1, 1)));
            fleet.AddDestroyer(new Point(1, 6), new Point(2, 6));
        }

        [TestMethod()]
        public void AddSubmarineTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddSubmarine(new Point(0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddSubmarine(new Point(0, -1)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => fleet.AddSubmarine(new Point(0, 1)));
            fleet.AddSubmarine(new Point(1, 6));
        }

        [TestMethod()]
        public void GetShootInTest()
        {
            Fleet fleet = new Fleet();
            fleet.AddBattleship(new Point(0, 0), new Point(3, 0));
            fleet.AddCruiser(new Point(3, 3), new Point(3, 5));
            fleet.AddDestroyer(new Point(5, 9), new Point(5, 8));

            Assert.AreEqual(HitType.Hit, fleet.GetShootIn(new Point(3, 4)));
            Assert.AreEqual(HitType.Hit, fleet.GetShootIn(new Point(3, 3)));
            Assert.AreEqual(HitType.Kill, fleet.GetShootIn(new Point(3, 5)));
            Assert.AreEqual(HitType.Miss, fleet.GetShootIn(new Point(6, 6)));

            Assert.AreEqual(HitType.Hit, fleet.GetShootIn(new Point(0, 0)));
            Assert.ThrowsException<ArgumentException>(() => fleet.GetShootIn(new Point(0, 0)));
            Assert.ThrowsException<ArgumentException>(() => fleet.GetShootIn(new Point(3, 5)));
        }

        [TestMethod()]
        public void IsAliveTest()
        {
            Fleet fleet = new Fleet();
            Assert.IsFalse(fleet.IsAlive());
            fleet.AddBattleship(new Point(1, 1), new Point(4, 1));
            Assert.IsTrue(fleet.IsAlive());
            fleet.Ships[0].State = ShipState.Sunk;
            Assert.IsFalse(fleet.IsAlive());
        }
    }
}