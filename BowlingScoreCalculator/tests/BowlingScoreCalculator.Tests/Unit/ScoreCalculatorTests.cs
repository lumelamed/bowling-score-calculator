namespace BowlingScoreCalculator.Tests.Unit
{
    using BowlingScoreCalculator.Models;
    using BowlingScoreCalculator.Services;
    using NUnit.Framework;

    [TestFixture]
    public class ScoreCalculatorTests
    {
        [Test]
        public void CalculateScore_AllStrikes_Returns300()
        {
            // Arrange - Perfect game
            var player = new Player("Carl");
            for (int i = 0; i < 12; i++) // 12 strikes
            {
                player.AddRoll(new Roll(10));
            }

            // Act
            ScoreCalculator.CalculateScores([player]);

            // Assert
            Assert.That(player.TotalScore, Is.EqualTo(300));
        }

        [Test]
        public void CalculateScore_AllZeros_Returns0()
        {
            // Arrange - Worst game
            var player = new Player("John");
            for (int i = 0; i < 20; i++)
            {
                player.AddRoll(new Roll(0));
            }

            // Act
            ScoreCalculator.CalculateScores([player]);

            // Assert
            Assert.That(player.TotalScore, Is.EqualTo(0));
        }

        [Test]
        public void CalculateScore_AllFouls_Returns0()
        {
            // Arrange - All fouls
            var player = new Player("Jane");
            for (int i = 0; i < 20; i++)
            {
                player.AddRoll(new Roll(0, true));
            }

            // Act
            ScoreCalculator.CalculateScores([player]);

            // Assert
            Assert.That(player.TotalScore, Is.EqualTo(0));
        }

        [Test]
        public void CalculateScore_SingleStrike_CalculatesCorrectBonus()
        {
            // Arrange - Strike followed by 3 and 4
            var player = new Player("Mike");
            player.AddRoll(new Roll(10)); // Strike
            player.AddRoll(new Roll(3));
            player.AddRoll(new Roll(4));

            // Complete remaining frames
            for (int i = 0; i < 16; i++)
            {
                player.AddRoll(new Roll(0));
            }

            // Act
            ScoreCalculator.CalculateScores([player]);

            // Assert - Strike (10) + next 2 rolls (3+4) = 17
            Assert.That(player.Frames[0].Score, Is.EqualTo(17));
        }

        [Test]
        public void CalculateScore_SingleSpare_CalculatesCorrectBonus()
        {
            // Arrange - Spare followed by 5
            var player = new Player("Sarah");
            player.AddRoll(new Roll(7));
            player.AddRoll(new Roll(3)); // Spare
            player.AddRoll(new Roll(5));

            // Complete remaining frames
            for (int i = 0; i < 17; i++)
            {
                player.AddRoll(new Roll(0));
            }

            // Act
            ScoreCalculator.CalculateScores([player]);

            // Assert - Spare (10) + next 1 roll (5) = 15
            Assert.That(player.Frames[0].Score, Is.EqualTo(15));
        }

        [Test]
        public void CalculateScore_Frame10WithStrike_AllowsExtraRolls()
        {
            // Arrange - All zeros except frame 10 with strikes
            var player = new Player("Tom");

            // 9 frames of zeros
            for (int i = 0; i < 18; i++)
            {
                player.AddRoll(new Roll(0));
            }

            // Frame 10: three strikes
            player.AddRoll(new Roll(10));
            player.AddRoll(new Roll(10));
            player.AddRoll(new Roll(10));

            // Act
            ScoreCalculator.CalculateScores([player]);

            Assert.Multiple(() =>
            {
                // Assert - Frame 10 score should be 30
                Assert.That(player.Frames[9].Score, Is.EqualTo(30));
                Assert.That(player.TotalScore, Is.EqualTo(30));
            });
        }

        [Test]
        public void CalculateScore_MultiplePlayersIndependently()
        {
            // Arrange
            var player1 = new Player("Alice");
            var player2 = new Player("Bob");

            // Alice: all 5s (no spares)
            for (int i = 0; i < 20; i++)
            {
                player1.AddRoll(new Roll(5));
            }

            // Bob: all zeros
            for (int i = 0; i < 20; i++)
            {
                player2.AddRoll(new Roll(0));
            }

            // Act
            ScoreCalculator.CalculateScores([player1, player2]);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(player1.TotalScore, Is.EqualTo(145));
                Assert.That(player2.TotalScore, Is.EqualTo(0));
            });
        }
    }
}
