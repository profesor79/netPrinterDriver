﻿namespace stepperCalculator
{
    public class StepData
    {
        public int TimeStamp { get; set; }
        public double HeadPositionAfterStep { get; set; }
        public double StepTime { get; set; }
        public double SpeedAfterMove { get; set; }
        public double DistanceAfterStep { get; set; }
        public int StepNumber { get; set; }
    }
}
