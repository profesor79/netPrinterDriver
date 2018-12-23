namespace stepperCalculator
{
    public class StepsData
    {
        public double StartPosition { get; }
        public double StopPosition { get; }
        public double Speed { get; }

        public StepsData(double startPosition, double stopPosition, double speed)
        {
            StartPosition = startPosition;
            StopPosition = stopPosition;
            Speed = speed;
        }
    }
}
