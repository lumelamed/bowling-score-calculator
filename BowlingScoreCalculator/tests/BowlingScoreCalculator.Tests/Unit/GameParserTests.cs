namespace BowlingScoreCalculator.Tests.Unit
{
    using NUnit.Framework;
    using BowlingScoreCalculator.Services;
    using BowlingScoreCalculator.Models;
    using BowlingScoreCalculator.Exceptions;
    using System.Collections.Generic;

    [TestFixture]
    public class GameParserTests
    {
        [Test]
        public void ParseGame_ValidInput_ReturnsPlayers()
        {
            var input = new[]
            {
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10",
                "Carl\t10"
            };

            var players = GameParser.ParseGame(input);

            Assert.That(players.Count, Is.EqualTo(1));
            Assert.That(players[0].Name, Is.EqualTo("Carl"));
            Assert.That(players[0].Frames.Count, Is.EqualTo(10));
            Assert.That(players[0].IsGameComplete);
        }

        [Test]
        public void ParseGame_MultiplePlayers_ReturnsAllPlayers()
        {
            // Arrange
            var input = new[]
            {
                "Jeff\t10",      // Frame 1: Strike
                "John\t3",       // Frame 1: 3
                "John\t7",       // Frame 1: 7 (spare)
                "Jeff\t7",       // Frame 2: 7
                "Jeff\t3",       // Frame 2: 3 (spare)
                "John\t6",       // Frame 2: 6
                "John\t3",       // Frame 2: 3
                "Jeff\t9",       // Frame 3: 9
                "Jeff\t0",       // Frame 3: 0
                "John\t10",      // Frame 3: Strike
                "Jeff\t10",      // Frame 4: Strike
                "John\t8",       // Frame 4: 8
                "John\t1",       // Frame 4: 1
                "Jeff\t0",       // Frame 5: 0
                "Jeff\t8",       // Frame 5: 8
                "John\t10",      // Frame 5: Strike
                "Jeff\t8",       // Frame 6: 8
                "Jeff\t2",       // Frame 6: 2 (spare)
                "John\t10",      // Frame 6: Strike
                "Jeff\tF",       // Frame 7: Foul
                "Jeff\t6",       // Frame 7: 6
                "John\t9",       // Frame 7: 9
                "John\t0",       // Frame 7: 0
                "Jeff\t10",      // Frame 8: Strike
                "John\t7",       // Frame 8: 7
                "John\t3",       // Frame 8: 3 (spare)
                "Jeff\t10",      // Frame 9: Strike
                "John\t4",       // Frame 9: 4
                "John\t4",       // Frame 9: 4
                "Jeff\t10",      // Frame 10: Strike
                "Jeff\t8",       // Frame 10: 8 (bonus)
                "Jeff\t1",       // Frame 10: 1 (bonus)
                "John\t10",      // Frame 10: Strike
                "John\t9",       // Frame 10: 9 (bonus)
                "John\t0",       // Frame 10: 0 (bonus)
            };

            // Act
            var players = GameParser.ParseGame(input);

            // Assert
            Assert.That(players.Count, Is.EqualTo(2));

            var jeff = players.FirstOrDefault(p => p.Name == "Jeff");
            var john = players.FirstOrDefault(p => p.Name == "John");

            Assert.That(jeff, Is.Not.Null);
            Assert.That(john, Is.Not.Null);

            Assert.That(jeff.IsGameComplete, Is.True, "Jeff's game should be complete");
            Assert.That(john.IsGameComplete, Is.True, "John's game should be complete");

            Assert.That(jeff.AllRolls.Count, Is.EqualTo(17), "Jeff should have 17 rolls");

            Assert.That(john.AllRolls.Count, Is.EqualTo(18), "John should have 18 rolls");
        }

        [Test]
        public void ParseGame_EmptyInput_ThrowsException()
        {
            var input = new string[0];

            Assert.Throws<InvalidInputException>(() => GameParser.ParseGame(input));
        }

        [Test]
        public void ParseGame_InvalidFormat_ThrowsException()
        {
            var input = new[]
            {
                "Carl-10"
            };

            var ex = Assert.Throws<InvalidInputException>(() => GameParser.ParseGame(input));
            StringAssert.Contains("Invalid format", ex.Message);
        }

        [Test]
        public void ParseGame_InvalidPins_ThrowsException()
        {
            var input = new[]
            {
                "Carl\t11"
            };

            var ex = Assert.Throws<InvalidInputException>(() => GameParser.ParseGame(input));
            StringAssert.Contains("Pins knocked must be between 0 and 10", ex.Message);
        }

        [Test]
        public void ParseGame_FoulInput_ParsesAsFoul()
        {
            var input = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                input.Add("Mary\tF");
            }

            var players = GameParser.ParseGame([.. input]);

            Assert.That(players.Count, Is.EqualTo(1));

            var mary = players[0];
            Assert.That(mary.Name, Is.EqualTo("Mary"));
            Assert.That(mary.AllRolls.Count, Is.EqualTo(20));
            Assert.That(mary.AllRolls.All(r => r.IsFoul), Is.True);
            Assert.That(mary.IsGameComplete, Is.True);
            Assert.That(mary.Frames.All(f => f.IsComplete), Is.True);
        }

        [Test]
        public void ParseGame_AllZeros_ParsesCorrectly()
        {
            // Arrange - 20 zeros (no fouls, solo 0 pines)
            var input = new List<string>();

            for (int i = 0; i < 20; i++)
            {
                input.Add("Peter\t0");
            }

            // Act
            var players = GameParser.ParseGame(input.ToArray());

            // Assert
            Assert.That(players.Count, Is.EqualTo(1));
            var peter = players[0];
            Assert.That(peter.AllRolls.Count, Is.EqualTo(20));
            Assert.That(peter.AllRolls.All(r => r.PinsKnocked == 0), Is.True);
            Assert.That(peter.IsGameComplete, Is.True);
        }

        [Test]
        public void ParseGame_IncompleteGame_ThrowsException()
        {
            var input = new[]
            {
                "Carl\t10",
                "Carl\t10"
            };

            var ex = Assert.Throws<InvalidInputException>(() => GameParser.ParseGame(input));
            StringAssert.Contains("incomplete game", ex.Message);
        }
    }
}
