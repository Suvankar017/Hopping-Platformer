using System.Collections.Generic;
using NUnit.Framework;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Grid;
using HoppingPlatformer.Domain.Level;
using HoppingPlatformer.Domain.Player;
using HoppingPlatformer.Application.Events;
using HoppingPlatformer.Application.Systems.Movement;
using HoppingPlatformer.Application.Systems.Death;

namespace HoppingPlatformer.Tests.Application.Systems.Movement
{
    public class MovementSystemTests
    {
        private static Level CreateLevel()
        {
            Dictionary<HexPosition, Platform>
                platforms = new();

            platforms.Add(
                new HexPosition(0, 2),
                new Platform(
                    new HexPosition(0, 2)));

            platforms.Add(
                new HexPosition(1, 2),
                new Platform(
                    new HexPosition(1, 2)));

            return new Level(
                platforms,
                new HexPosition(0, 2),
                new HexPosition(1, 2));
        }

        [Test]
        public void Move_ShouldMovePlayer()
        {
            EventBus bus =
                new EventBus();

            HexGridTopology topology =
                new HexGridTopology(5);

            Level level =
                CreateLevel();

            Player player =
                new Player(
                    new HexPosition(0, 2));

            DeathSystem deathSystem = new DeathSystem(player, bus);

            MovementSystem movement =
                new MovementSystem(
                    player,
                    level,
                    topology,
                    bus,
                    deathSystem);

            MovementResult result =
                movement.Move(
                    Direction.Right);

            Assert.IsTrue(
                result.Success);

            Assert.AreEqual(
                new HexPosition(1, 2),
                player.Position);
        }

        [Test]
        public void Move_ShouldFail_WhenPlatformMissing()
        {
            EventBus bus =
                new EventBus();

            HexGridTopology topology =
                new HexGridTopology(5);

            Dictionary<HexPosition, Platform>
                platforms = new();

            platforms.Add(
                new HexPosition(0, 2),
                new Platform(
                    new HexPosition(0, 2)));

            Level level =
                new Level(
                    platforms,
                    new HexPosition(0, 2),
                    new HexPosition(0, 2));

            Player player =
                new Player(
                    new HexPosition(0, 2));

            DeathSystem deathSystem = new DeathSystem(player, bus);

            MovementSystem movement =
                new MovementSystem(
                    player,
                    level,
                    topology,
                    bus,
                    deathSystem);

            MovementResult result =
                movement.Move(
                    Direction.Right);

            Assert.IsFalse(
                result.Success);
        }
    }
}
