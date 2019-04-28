namespace stepperCalculator
{
    public class AxisConfiguration
    {
        public decimal StepsPerMM { get; set; } = 200;
        public decimal MaxSpeedPerMM { get; set; } = 20;
        public int MaxAcceleration { get; set; } = 40;
        public int Jerk { get; set; } = 20;

    }
}
