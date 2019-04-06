using System.Collections.Generic;

namespace stepperCalculator
{
    public class Movement
    {
        public List<StepData> HeadSteps { get; set; } = new List<StepData>();
        public List<StepData> BodySteps { get; set; } = new List<StepData>();
        public List<StepData> TailSteps { get; set; } = new List<StepData>();
        public double TotalTime { get; set; }
        public double SpeedFactor { get; set; }
        public bool Direction { get; set; }
    }
}
