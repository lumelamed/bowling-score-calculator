namespace BowlingScoreCalculator.Services
{
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

            for (int frameIndex = 0; frameIndex < 10; frameIndex++)
            {
                var frame = player.Frames[frameIndex];

                if (frameIndex < 9)
                {
                    if (IsStrike(rolls, rollIndex))
                    {
                        frame.Score = 10 + GetStrikeBonus(rolls, rollIndex);
                        rollIndex += 1;
                    }
                    else if (IsSpare(rolls, rollIndex))
                    {
                        frame.Score = 10 + GetSpareBonus(rolls, rollIndex);
                        rollIndex += 2;
                    }
                    else
                    {
                        frame.Score = GetPinsInRoll(rolls, rollIndex) +
                                      GetPinsInRoll(rolls, rollIndex + 1);
                        rollIndex += 2;
                    }
                }
                else
                {
                    frame.Score = GetPinsInRoll(rolls, rollIndex) +
                                  GetPinsInRoll(rolls, rollIndex + 1) +
                                  GetPinsInRoll(rolls, rollIndex + 2);
                }

                cumulativeScore += frame.Score;
                frame.CumulativeScore = cumulativeScore;
            }
        }

        private static bool IsStrike(List<Roll> rolls, int rollIndex)
        {
            return rollIndex < rolls.Count && rolls[rollIndex].PinsKnocked == 10;
        }

        private static bool IsSpare(List<Roll> rolls, int rollIndex)
        {
            return rollIndex + 1 < rolls.Count &&
                   rolls[rollIndex].PinsKnocked + rolls[rollIndex + 1].PinsKnocked == 10;
        }

        private static int GetStrikeBonus(List<Roll> rolls, int rollIndex)
        {
            int bonus = 0;

            if (rollIndex + 1 < rolls.Count)
                bonus += rolls[rollIndex + 1].PinsKnocked;

            if (rollIndex + 2 < rolls.Count)
                bonus += rolls[rollIndex + 2].PinsKnocked;

            return bonus;
        }

        private static int GetSpareBonus(List<Roll> rolls, int rollIndex)
        {
            if (rollIndex + 2 < rolls.Count)
                return rolls[rollIndex + 2].PinsKnocked;

            return 0;
        }

        private static int GetPinsInRoll(List<Roll> rolls, int rollIndex)
        {
            return rollIndex < rolls.Count ? rolls[rollIndex].PinsKnocked : 0;
        }
    }
}
