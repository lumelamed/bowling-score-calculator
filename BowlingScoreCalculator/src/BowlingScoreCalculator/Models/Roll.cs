namespace BowlingScoreCalculator.Models
{
    public class Roll
    {
        public int PinsKnocked { get; private set; }

        public Roll(int pinsKnocked)
        {
            if (pinsKnocked < 0 || pinsKnocked > 10)
                throw new ArgumentException("Pins knocked must be between 0 and 10");

            this.PinsKnocked = pinsKnocked;
        }

        public bool IsFoul => this.PinsKnocked == 0;
    }
}
