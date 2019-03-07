using System;
using System.Linq;

namespace stepperCalculator
{
    class Program
    {
        static void Main(string[] args)
        {

            var reader = new FileReader();
            var lines = reader.Readfile();

            var posX = 0.0;
            var posY = 0.0;
            var posZ = 0.0;
            var posE = 0.0;

            var noHomeSkip = true;
            var gdata = new GCodeData();

            foreach (var line in lines)
            {
                Console.WriteLine("-------------");
                var commands = line.Split(";")[0].Split(" ");

                if (commands[0].StartsWith("M")){
                    ProcesMachineRelatedCommand(commands);
                }

                if (commands[0] == "G28")
                {
                    Console.WriteLine("executing homing....");
                    gdata.Coordinates["X"] = 0.0;
                    gdata.Coordinates["Y"] = 0.0;
                    gdata.Coordinates["Z"] = 0.0;

                    noHomeSkip = false;
                }

                if (noHomeSkip)
                {
                    // cannot move printer before home executed;
                    Console.WriteLine("no home exectued");
                    continue;
                }

                var prev = gdata.Clone();

                if (commands[0] == "G1")
                {
                    Console.WriteLine(line);
                    foreach (var c in commands)
                    {

                        if (c.Trim().StartsWith("X"))
                        {
                            gdata.Coordinates["X"] = double.Parse(c.Replace("X", string.Empty));
                        }

                        if (c.Trim().StartsWith("Y"))
                        {
                            gdata.Coordinates["Y"] = double.Parse(c.Replace("Y", string.Empty));
                        }

                        if (c.Trim().StartsWith("Z"))
                        {
                            gdata.Coordinates["Z"] = double.Parse(c.Replace("Z", string.Empty));
                        }


                        if (c.Trim().StartsWith("E"))
                        {
                            gdata.Coordinates["E0"] = double.Parse(c.Replace("E", string.Empty));
                        }




                    }

                    foreach (var cor in gdata.Coordinates)
                    {
                        Console.WriteLine($"{cor.Key}: {cor.Value}");
                    }

                    // now we can calculate distance to go;
                    var x = gdata.Coordinates["X"] - prev.Coordinates["X"];
                    var y = gdata.Coordinates["Y"] - prev.Coordinates["Y"];
                    var z = gdata.Coordinates["Z"] - prev.Coordinates["Z"];
                    var e1 = gdata.Coordinates["E0"] - prev.Coordinates["E0"];

                    Console.WriteLine($"x:{x}, y:{y}, z:{z}, e1:{e1}");
                }
            }

        }

        private static void ProcesMachineRelatedCommand(string[] commands)
        {
            Console.WriteLine("Machine command");
            foreach (var c in commands)
            {
                Console.WriteLine(c);
            }
        }

        static void aaa()
        {

            Console.WriteLine("Hello World!");
            var xAxisCalculator = new MovementCalculator(new AxisConfiguration());
            var stepsX = xAxisCalculator.CalculateSteps(0, 100, 15);

            var yAxisCalculator = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 500,
                MaxSpeedPerMM = 300
            });

            var stepsY = yAxisCalculator.CalculateSteps(34, 300, 300);


            var eAxisCalculator = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 15,
                MaxSpeedPerMM = 20,
                StepsPerMM = 1000
            });

            var stepsE = eAxisCalculator.CalculateSteps(3.023, 5.2130, 12);

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
