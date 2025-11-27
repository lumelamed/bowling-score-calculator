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
            var input = new[]
            {
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t0",
                "Bob\t0",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10",
                "Alice\t5",
                "Bob\t10"
            };

            var players = GameParser.ParseGame(input);

            Assert.That(players.Count, Is.EqualTo(2));
            Assert.That(players.Exists(p => p.Name == "Alice"));
            Assert.That(players.Exists(p => p.Name == "Bob"));
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

            // Add bonus roll for 10th frame
            input.Add("Mary\tF");

            var players = GameParser.ParseGame(input.ToArray());

            Assert.That(players.Count, Is.EqualTo(1));

            var mary = players[0];
            Assert.That(mary.AllRolls.TrueForAll(r => r.IsFoul));
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
