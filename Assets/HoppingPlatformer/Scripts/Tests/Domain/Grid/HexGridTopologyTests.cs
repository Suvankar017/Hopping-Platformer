using NUnit.Framework;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Grid;

namespace HoppingPlatformer.Tests.Domain.Grid
{
    public sealed class HexGridTopologyTests
    {
        [Test]
        public void Constructor_ShouldThrow_WhenWidthIsOne()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new HexGridTopology(1));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenWidthIsZero()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new HexGridTopology(0));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenWidthIsNegative()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new HexGridTopology(-1));
        }

        [Test]
        public void GetRowWidth_OddWidth_ShouldAlternateCorrectly()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            Assert.AreEqual(
                5,
                topology.GetRowWidth(0));

            Assert.AreEqual(
                4,
                topology.GetRowWidth(1));

            Assert.AreEqual(
                5,
                topology.GetRowWidth(2));

            Assert.AreEqual(
                4,
                topology.GetRowWidth(3));
        }

        [Test]
        public void GetRowWidth_EvenWidth_ShouldAlternateCorrectly()
        {
            HexGridTopology topology =
                new HexGridTopology(4);

            Assert.AreEqual(
                3,
                topology.GetRowWidth(0));

            Assert.AreEqual(
                4,
                topology.GetRowWidth(1));

            Assert.AreEqual(
                3,
                topology.GetRowWidth(2));

            Assert.AreEqual(
                4,
                topology.GetRowWidth(3));
        }

        [Test]
        public void IsValid_ShouldReturnTrue_ForValidCell()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid = topology.IsValid(new HexPosition(0, 4));

            Assert.IsTrue(valid);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForNegativeRow()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid = topology.IsValid(new HexPosition(-1, 0));

            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_ForOutOfBoundsColumn()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid = topology.IsValid(new HexPosition(0, 5));

            Assert.IsFalse(valid);
        }

        [Test]
        public void GetUpNeighbours_ShouldReturnExpectedNeighbours()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            var neighbours = topology.GetUpNeighbours(new HexPosition(0, 2));

            Assert.AreEqual(
                2,
                neighbours.Count);

            Assert.Contains(
                new HexPosition(1, 2),
                (System.Collections.ICollection)neighbours);

            Assert.Contains(
                new HexPosition(1, 1),
                (System.Collections.ICollection)neighbours);
        }

        [Test]
        public void GetDownNeighbours_ShouldReturnExpectedNeighbours()
        {
            HexGridTopology topology =
                new HexGridTopology(4);

            var neighbours = topology.GetDownNeighbours(new HexPosition(1, 2));

            Assert.AreEqual(
                2,
                neighbours.Count);

            Assert.Contains(
                new HexPosition(0, 1),
                (System.Collections.ICollection)neighbours);

            Assert.Contains(
                new HexPosition(0, 2),
                (System.Collections.ICollection)neighbours);
        }

        [Test]
        public void JumpLeft_ShouldReachExpectedTarget()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid =
                topology.TryGetJumpTarget(
                    new HexPosition(0, 2),
                    Direction.Left,
                    1,
                    out HexPosition target);

            Assert.IsTrue(valid);

            Assert.AreEqual(
                new HexPosition(1, 1),
                target);
        }

        [Test]
        public void JumpRight_ShouldReachExpectedTarget()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid =
                topology.TryGetJumpTarget(
                    new HexPosition(0, 2),
                    Direction.Right,
                    1,
                    out HexPosition target);

            Assert.IsTrue(valid);

            Assert.AreEqual(
                new HexPosition(1, 2),
                target);
        }

        [Test]
        public void DoubleJump_ShouldReachExpectedTarget()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid =
                topology.TryGetJumpTarget(
                    new HexPosition(0, 2),
                    Direction.Right,
                    2,
                    out HexPosition target);

            Assert.IsTrue(valid);

            Assert.AreEqual(
                new HexPosition(2, 3),
                target);
        }

        [Test]
        public void Jump_ShouldFail_WhenTargetLeavesGrid()
        {
            HexGridTopology topology =
                new HexGridTopology(5);

            bool valid =
                topology.TryGetJumpTarget(
                    new HexPosition(0, 0),
                    Direction.Left,
                    2,
                    out _);

            Assert.IsFalse(valid);
        }
    }
}