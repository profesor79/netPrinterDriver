namespace stepperCalculator
{
    public class PrinterConfiguration
    {
        public float XStepsPerMM { get; set; } = 200;
        public float XMaxSpeedPerMM { get; set; } = 30;

        public float YStepsPerMM { get; set; }= 200;
        public float ZStepsPerMM { get; set; }= 200;
        public int XMaxAcceleration { get; set; } = 3;
        public int XJerk { get; set; } = 20;
        public int YMaxAcceleration { get; set; } = 60;
        public int ZMaxAcceleration { get; set; } = 60;

    }
}
