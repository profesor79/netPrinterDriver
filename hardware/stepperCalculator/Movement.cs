using System.Collections.Generic;

namespace stepperCalculator
{
    public class Movement
    {
        public List<StepData> Steps { get; set; } = new List<StepData>();
        public float  StartPoint { get; set; }
        public float  EndPoint { get; set; }
        public int StepNumber { get; set; }
    }
}
