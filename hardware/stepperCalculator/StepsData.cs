namespace stepperCalculator
{
    public class StepsData
    {
        public decimal StartPosition { get; }
        public decimal StopPosition { get; }
        public decimal Speed { get; }

        public StepsData(decimal startPosition, decimal stopPosition, decimal speed)
        {
            StartPosition = startPosition;
            StopPosition = stopPosition;
            Speed = speed;
        }
    }
}
