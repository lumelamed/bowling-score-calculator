namespace BowlingScoreCalculator.Models
{
    public class Player
    {
        public string Name { get; }

        public List<Frame> Frames { get; }

        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player name cannot be empty", nameof(name));

            this.Name = name;
            this.Frames = new List<Frame>();

            for (int i = 1; i <= 10; i++)
            {
                this.Frames.Add(new Frame(i));
            }
        }

        public void AddRoll(Roll roll)
        {
            ArgumentNullException.ThrowIfNull(roll);

            var currentFrame = this.Frames.FirstOrDefault(f => !f.IsComplete) ?? throw new InvalidOperationException("All frames are complete. Cannot add more rolls.");

            currentFrame.AddRoll(roll);
        }

        public bool IsGameComplete => this.Frames.All(f => f.IsComplete);

        public int TotalScore => this.Frames.Last().CumulativeScore;
    }
}
