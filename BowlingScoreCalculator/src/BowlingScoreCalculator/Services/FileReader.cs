namespace BowlingScoreCalculator.Services
{
    using BowlingScoreCalculator.Exceptions;

    public static class FileReader
    {
        public static string[] ReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidInputException($"Access denied to file: {filePath}", ex);
            }
            catch (IOException ex)
            {
                throw new InvalidInputException($"Error reading file: {filePath}", ex);
            }
        }
    }
}
