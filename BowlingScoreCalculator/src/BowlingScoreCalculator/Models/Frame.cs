namespace BowlingScoreCalculator.Models
{
    public class Frame
    {
        public int FrameNumber { get; init; }

        public List<Roll> Rolls { get; init; } = [];

        public int Score { get; set; }

        public int CumulativeScore { get; set; }

        public Frame(int frameNumber)
        {
            if (frameNumber < 1 || frameNumber > 10)
                throw new ArgumentException("Frame number must be between 1 and 10", nameof(frameNumber));

            this.FrameNumber = frameNumber;
        }

        public bool IsStrike => this.Rolls.Count > 0 && this.Rolls.First().PinsKnocked == 10;

        public bool IsSpare => !this.IsStrike && this.Rolls.Count >= 2 && this.Rolls[0].PinsKnocked + this.Rolls[1].PinsKnocked == 10;

        public int TotalPins => this.Rolls.Sum(r => r.PinsKnocked);

        public bool IsComplete
        {
            get
            {
                if (this.FrameNumber == 10)
                {
                    if (this.Rolls.Count < 2)
                        return false;

                    if (this.Rolls[0].PinsKnocked == 10 || this.Rolls[0].PinsKnocked + this.Rolls[1].PinsKnocked == 10)
                        return Rolls.Count >= 3;

                    return this.Rolls.Count >= 2;
                }

                if (this.Rolls.Count > 0 && this.Rolls[0].PinsKnocked == 10)
                    return true;

                return this.Rolls.Count >= 2;
            }
        }

        public void AddRoll(Roll roll)
        {
            ArgumentNullException.ThrowIfNull(roll);

            if (this.IsComplete)
                throw new InvalidOperationException($"Frame {this.FrameNumber} is already complete. Cannot add more rolls.");

            if (this.FrameNumber < 10 && this.Rolls.Count > 0 && !this.IsStrike)
            {
                int currentTotal = this.TotalPins;

                if (currentTotal + roll.PinsKnocked > 10)
                    throw new InvalidOperationException($"Total pins in frame cannot exceed 10. Current: {currentTotal}, Adding: {roll.PinsKnocked}");
            }

            this.Rolls.Add(roll);
        }
    }
}
