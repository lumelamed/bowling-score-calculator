namespace BowlingScoreCalculator.Tests.Integrations
{
    using BowlingScoreCalculator.Services;
    using NUnit.Framework;
    using System.Linq;
    using System.Collections.Generic;

    [TestFixture]
    public class EndToEndTests
    {
        [Test]
        public void EndToEnd_SampleGame_CalculatesCorrectScores()
        {
            // Arrange - Sample game from specs
            var input = new[]
            {
                "Jeff\t10",
                "John\t3",
                "John\t7",
                "Jeff\t7",
                "Jeff\t3",
                "John\t6",
                "John\t3",
                "Jeff\t9",
                "Jeff\t0",
                "John\t10",
                "Jeff\t10",
                "John\t8",
                "John\t1",
                "Jeff\t0",
                "Jeff\t8",
                "John\t10",
                "Jeff\t8",
                "Jeff\t2",
                "John\t10",
                "Jeff\tF",
                "Jeff\t6",
                "John\t9",
                "John\t0",
                "Jeff\t10",
                "John\t7",
                "John\t3",
                "Jeff\t10",
                "John\t4",
                "John\t4",
                "Jeff\t10",
                "Jeff\t8",
                "Jeff\t1",
                "John\t10",
                "John\t9",
                "John\t0"
            };

            // Act
            var players = GameParser.ParseGame(input);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(2));

            var jeff = players.First(p => p.Name == "Jeff");
            var john = players.First(p => p.Name == "John");

            Assert.Multiple(() =>
            {
                Assert.That(jeff.TotalScore, Is.EqualTo(167));
                Assert.That(john.TotalScore, Is.EqualTo(151));
            });
        }

        [Test]
        public void EndToEnd_PerfectGame_Returns300()
        {
            // Arrange - Perfect game (12 strikes)
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

            // Act
            var players = GameParser.ParseGame(input);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(300));
        }

        [Test]
        public void EndToEnd_ZeroGame_Returns0()
        {
            // Arrange - All zeros
            var input = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                input.Add("Peter\t0");
            }

            // Act
            var players = GameParser.ParseGame([.. input]);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(0));
        }

        [Test]
        public void EndToEnd_AllFoulsGame_Returns0()
        {
            // Arrange - All fouls
            var input = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                input.Add("Mary\tF");
            }

            // Act
            var players = GameParser.ParseGame([.. input]);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(0));
        }

        [Test]
        public void EndToEnd_AllSpares_CalculatesCorrectly()
        {
            // Arrange - All spares (5, 5 pattern) + final 5
            var input = new List<string>();
            for (int i = 0; i < 21; i++)
            {
                input.Add("Lucy\t5");
            }

            // Act
            var players = GameParser.ParseGame([.. input]);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(150)); // Each spare scores 15 (10 + 5 bonus)
        }

        [Test]
        public void EndToEnd_Frame10Strike_AllowsBonusRolls()
        {
            // Arrange - Zeros except last frame with strikes
            var input = new List<string>();
            for (int i = 0; i < 18; i++)
            {
                input.Add("Kate\t0");
            }
            input.Add("Kate\t10");
            input.Add("Kate\t10");
            input.Add("Kate\t10");

            // Act
            var players = GameParser.ParseGame([.. input]);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players, Has.Count.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(30));
        }

        [Test]
        public void EndToEnd_Frame10Spare_AllowsBonusRoll()
        {
            // Arrange - Zeros except last frame with spare
            var input = new List<string>();
            for (int i = 0; i < 18; i++)
            {
                input.Add("Sam\t0");
            }
            input.Add("Sam\t7");
            input.Add("Sam\t3");
            input.Add("Sam\t5");

            // Act
            var players = GameParser.ParseGame([.. input]);
            ScoreCalculator.CalculateScores(players);

            // Assert
            Assert.That(players.Count, Is.EqualTo(1));
            Assert.That(players[0].TotalScore, Is.EqualTo(15));
        }
    }
}
