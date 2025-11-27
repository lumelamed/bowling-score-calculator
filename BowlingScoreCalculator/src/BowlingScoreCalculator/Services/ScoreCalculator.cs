namespace BowlingScoreCalculator.Services
{
    using BowlingScoreCalculator.Extensions;
    using BowlingScoreCalculator.Models;

    public static class ScoreCalculator
    {
        public static void CalculateScores(List<Player> players)
        {
            foreach (var player in players)
            {
                CalculatePlayerScore(player);
            }
        }

        private static void CalculatePlayerScore(Player player)
        {
            var rolls = player.AllRolls;
            int rollIndex = 0;
            int cumulativeScore = 0;

            for (int frameNumber = 0; frameNumber < 10; frameNumber++)
            {
                var frame = player.Frames[frameNumber];

                if (frameNumber < 9)
                {
                    if (rolls.HasStrikeAt(rollIndex))
                    {
                        frame.Score = rolls.ScoreStrike(rollIndex);
                        rollIndex++;
                    }
                    else if (rolls.HasSpareAt(rollIndex))
                    {
                        frame.Score = rolls.ScoreSpare(rollIndex);
                        rollIndex += 2;
                    }
                    else
                    {
                        frame.Score = rolls.ScoreOpen(rollIndex);
                        rollIndex += 2;
                    }
                }
                else
                {
                    frame.Score = rolls.GetPinsAt(rollIndex)
                                + rolls.GetPinsAt(rollIndex + 1)
                                + rolls.GetPinsAt(rollIndex + 2);
                }

                cumulativeScore += frame.Score;
                frame.CumulativeScore = cumulativeScore;
            }
        }
    }
}
