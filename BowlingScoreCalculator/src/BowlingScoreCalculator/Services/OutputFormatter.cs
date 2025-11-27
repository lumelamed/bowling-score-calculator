namespace BowlingScoreCalculator.Services
{
    using BowlingScoreCalculator.Models;
    using System.Text;

    public static class OutputFormatter
    {
        public static string FormatGame(List<Player> players)
        {
            var output = new StringBuilder();

            output.AppendLine("""
                Frame		1		2		3		4		5		6		7		8		9		10
                """);

            foreach (var player in players)
            {
                output.AppendLine(player.Name);

                output.Append("Pinfalls");
                foreach (var frame in player.Frames)
                {
                    output.Append('\t');
                    if (frame.FrameNumber == 10)
                    {
                        for (int i = 0; i < frame.Rolls.Count; i++)
                        {
                            if (i > 0)
                                output.Append('\t');

                            output.Append(GetDisplayValue(frame, i));
                        }
                    }
                    else
                    {
                        output.Append(GetDisplayValue(frame, 0));
                        output.Append('\t');
                        output.Append(GetDisplayValue(frame, 1));
                    }
                }
                output.AppendLine();

                output.Append("Score");

                foreach (var frame in player.Frames)
                {
                    output.Append("\t\t");
                    output.Append(frame.CumulativeScore);
                }

                output.AppendLine();
            }

            return output.ToString();
        }

        private static string GetDisplayValue(Frame frame, int rollIndex)
        {
            if (rollIndex >= frame.Rolls.Count)
                return "";

            var roll = frame.Rolls[rollIndex];

            if (frame.FrameNumber == 10)
            {
                if (roll.PinsKnocked == 10)
                    return "X";

                if (rollIndex > 0)
                {
                    int prevPins = frame.Rolls[rollIndex - 1].PinsKnocked;

                    if (prevPins + roll.PinsKnocked == 10)
                        return "/";
                }

                return roll.IsFoul ? "F" : roll.PinsKnocked.ToString();
            }

            var isFirstRoll = rollIndex == 0;

            if (isFirstRoll && roll.PinsKnocked == 10)
                return "X";

            if (!isFirstRoll && frame.IsSpare)
                return "/";

            if (roll.IsFoul)
                return "F";

            return roll.PinsKnocked.ToString();
        }
    }
}
