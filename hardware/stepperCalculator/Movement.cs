using System.Collections.Generic;

namespace stepperCalculator
{
    public class Movement
    {
        public List<StepData> Steps { get; set; }
        public float  StartPoint { get; set; }
        public float  EndPoint { get; set; }
    }
}