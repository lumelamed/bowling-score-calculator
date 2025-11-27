namespace BowlingScoreCalculator.Services
{
    using BowlingScoreCalculator.Exceptions;
    using BowlingScoreCalculator.Models;

    public static class GameParser
    {
        public static List<Player> ParseGame(string[] lines)
        {
            if (lines == null || lines.Length == 0)
                throw new InvalidInputException("Input file is empty");

            var playerDict = new Dictionary<string, Player>();
            int lineNumber = 0;

            foreach (var line in lines)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var parts = line.Split('\t');

                    if (parts.Length != 2)
                        throw new InvalidInputException(
                            $"Line {lineNumber}: Invalid format. Expected 'PlayerName\\tPins'");

                    string playerName = parts[0].Trim();
                    string rollValue = parts[1].Trim();

                    if (string.IsNullOrWhiteSpace(playerName))
                        throw new InvalidInputException($"Line {lineNumber}: Player name is empty");

                    var player = GetPlayer(playerDict, playerName);

                    var (pinsKnocked, isFoul) = TryParsePins(rollValue);
                    var roll = new Roll(pinsKnocked, isFoul);

                    player.AddRoll(roll);
                }
                catch (InvalidInputException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new InvalidInputException($"Line {lineNumber}: {ex.Message}", ex);
                }
            }

            var players = playerDict.Values.ToList();

            foreach (var player in players)
            {
                if (!player.IsGameComplete)
                {
                    throw new InvalidInputException(
                        $"Player '{player.Name}' has incomplete game. " +
                        $"Expected 10 complete frames.");
                }
            }

            return players;
        }

        private static Player GetPlayer(Dictionary<string, Player> playerDict, string playerName)
        {
            if (!playerDict.TryGetValue(playerName, out Player? player))
            {
                player = new Player(playerName);
                playerDict[playerName] = player;
            }

            return player;
        }

        private static (int, bool) TryParsePins(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Roll input cannot be empty", nameof(input));

            input = input.Trim().ToUpper();

            if (input == "F")
                return (0, true);

            if (int.TryParse(input, out int pins))
            {
                if (pins < 0 || pins > 10)
                    throw new ArgumentException("Pins knocked must be between 0 and 10", nameof(input));
                return (pins, false);
            }

            throw new ArgumentException($"Invalid roll value: {input}", nameof(input));
        }
    }
}
