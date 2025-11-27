namespace BowlingScoreCalculator.Extensions
{
    using BowlingScoreCalculator.Models;

    public static class RollExtensions
    {
        public static bool IsStrike(this Roll roll) => roll.PinsKnocked == 10;
    }
}
