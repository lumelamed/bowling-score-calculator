namespace BowlingScoreCalculator.Extensions
{
    using BowlingScoreCalculator.Models;

    public static class RollListExtensions
    {
        public static bool HasStrikeAt(this IReadOnlyList<Roll> rolls, int index)
            => index < rolls.Count && rolls[index].IsStrike();

        public static bool HasSpareAt(this IReadOnlyList<Roll> rolls, int index)
            => index + 1 < rolls.Count &&
               rolls[index].PinsKnocked + rolls[index + 1].PinsKnocked == 10;

        public static int GetPinsAt(this IReadOnlyList<Roll> rolls, int index)
            => index < rolls.Count ? rolls[index].PinsKnocked : 0;

        public static int ScoreStrike(this IReadOnlyList<Roll> rolls, int index)
        => 10 + rolls.GetPinsAt(index + 1) + rolls.GetPinsAt(index + 2);

        public static int ScoreSpare(this IReadOnlyList<Roll> rolls, int index)
            => 10 + rolls.GetPinsAt(index + 2);

        public static int ScoreOpen(this IReadOnlyList<Roll> rolls, int index)
            => rolls.GetPinsAt(index) + rolls.GetPinsAt(index + 1);
    }
}
