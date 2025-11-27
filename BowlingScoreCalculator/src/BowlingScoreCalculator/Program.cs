namespace BowlingScoreCalculator
{
    using BowlingScoreCalculator.Exceptions;
    using BowlingScoreCalculator.Services;

    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Error: No input file specified");
                    Console.WriteLine("Usage: BowlingScoreCalculator <input-file>");
                    Console.WriteLine("Example: BowlingScoreCalculator scores.txt");
                }

                string filePath = args[0];

                var lines = FileReader.ReadFile(filePath);
                var players = GameParser.ParseGame(lines);

                ScoreCalculator.CalculateScores(players);

                var output = OutputFormatter.FormatGame(players);

                Console.WriteLine(output);
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Invalid Input: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
