using System;

namespace stepperCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var c = new MovementCalculator(new PrinterConfiguration());
            var steps = c.CalculateX(0, 100, 15);
            foreach (var stepData in steps.Steps)
            {
                Console.WriteLine($" StepNumber:{stepData.StepNumber} DistanceAfterStep: {stepData.DistanceAfterStep:0.000} HeadPositionAfterStep: {stepData.HeadPositionAfterStep} SpeedAfterMove: {stepData.SpeedAfterMove} StepTime:{stepData.StepTime}");
            }

            Console.WriteLine("done");
        }
    }
}
