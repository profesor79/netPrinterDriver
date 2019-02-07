using System;
using System.Linq;

namespace stepperCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var xAxisCalculator = new MovementCalculator(new AxisConfiguration());
            var stepsX = xAxisCalculator.CalculateSteps(0, 100, 15);

 var yAxisCalculator = new MovementCalculator(new AxisConfiguration
 {
     MaxAcceleration   = 500,
     MaxSpeedPerMM = 300
 });

 var stepsY = yAxisCalculator.CalculateSteps(34, 300, 300);



            var eAxisCalculator = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration   = 15,
                MaxSpeedPerMM = 20,
                StepsPerMM = 400
            });

            var stepsE = yAxisCalculator.CalculateSteps(3.023, 5.2130, 12);



            Console.WriteLine(stepsX.TotalTime);
            Console.WriteLine(stepsY.TotalTime);
            Console.WriteLine(stepsE.TotalTime);

            var times = new[] {stepsE.TotalTime, stepsX.TotalTime, stepsY.TotalTime};
            var maxTime = times.Max();
            Console.WriteLine(maxTime);
            stepsE.SpeedFactor = maxTime / stepsE.TotalTime;
            stepsX.SpeedFactor = maxTime / stepsX.TotalTime;
            stepsY.SpeedFactor = maxTime / stepsY.TotalTime;


            Console.WriteLine(stepsX.SpeedFactor);
            Console.WriteLine(stepsY.SpeedFactor);
            Console.WriteLine(stepsE.SpeedFactor);

        }
    }
}
