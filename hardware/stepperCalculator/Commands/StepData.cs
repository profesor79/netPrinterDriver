namespace stepperCalculator
{
    public class StepData
    {
        public decimal TimeStamp { get; set; }
        public decimal PositionAfterStep { get; set; }
        public decimal StepTime { get; set; }
        public decimal SpeedAfterMove { get; set; }
        public decimal DistanceAfterStep { get; set; }
        public int StepNumber { get; set; }
        public string AxisName { get; set; }
    }
}
