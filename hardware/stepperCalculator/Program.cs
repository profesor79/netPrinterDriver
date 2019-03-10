using System;
using System.Collections.Generic;
using System.Linq;

namespace stepperCalculator
{
    class Program
    {
        private static MovementCalculator _yAxisCalculator;
        private static MovementCalculator _eAxisCalculator;
        private static MovementCalculator _xAxisCalculator;

        static void Main(string[] args)
        {
            InitializeAxes();
            var movementDatat = new List<MoveData>();
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
                    gdata.Coordinates["E"] = 0.0;
                    noHomeSkip = false;
                }

                if (noHomeSkip)
                {
                    // cannot move printer before home executed;
                    Console.WriteLine("no home exectued");
                    continue;
                }

                var prev = gdata.Clone();

                if (commands[0] == "G01" ||commands[0] == "G1")
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
                            gdata.Coordinates["E"] = double.Parse(c.Replace("E", string.Empty));
                        }

                        InitializeAxes();
                        var stepsX = _xAxisCalculator.CalculateSteps(prev.Coordinates["X"],gdata.Coordinates["X"], 200);
                        var stepsY = _yAxisCalculator.CalculateSteps(prev.Coordinates["Y"],gdata.Coordinates["Y"], 200);
                        var stepsZ = _yAxisCalculator.CalculateSteps(prev.Coordinates["Z"],gdata.Coordinates["Z"], 200);
                        var stepsE = _eAxisCalculator.CalculateSteps(prev.Coordinates["E"],gdata.Coordinates["E"], 200);

                        var times = new[] {stepsE.TotalTime, stepsX.TotalTime, stepsY.TotalTime, stepsZ.TotalTime};
                        var maxTime = times.Max();

                        stepsE.SpeedFactor = maxTime / stepsE.TotalTime;
                        stepsZ.SpeedFactor = maxTime / stepsZ.TotalTime;
                        stepsX.SpeedFactor = maxTime / stepsX.TotalTime;
                        stepsY.SpeedFactor = maxTime / stepsY.TotalTime;

                        var moveData = new MoveData(stepsE, stepsX, stepsY, stepsZ);
                        movementDatat.Add(moveData);


                        foreach (var cor in gdata.Coordinates)
                        {
                            Console.WriteLine($"{cor.Key}: {cor.Value}");
                        }
                    }




                }
            }

            Console.WriteLine($"move recorded: {movementDatat.Count}");
        }

        private static void ProcesMachineRelatedCommand(string[] commands)
        {
            Console.WriteLine("Machine command");
            foreach (var c in commands)
            {
                Console.WriteLine(c);
            }
        }

        static void Aaa()
        {


            InitializeAxes();


             var stepsX = _xAxisCalculator.CalculateSteps(0,100, 15);
            var stepsY = _yAxisCalculator.CalculateSteps(34, 300, 300);
            var stepsE = _eAxisCalculator.CalculateSteps(3.023, 5.2130, 12);

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

        private static void InitializeAxes()
        {
            _xAxisCalculator = new MovementCalculator(new AxisConfiguration{
                MaxAcceleration = 500,
                MaxSpeedPerMM = 300,
                StepsPerMM = 1000
            });


            _yAxisCalculator = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 500,
                MaxSpeedPerMM = 300,
                StepsPerMM = 1000
            });

            _eAxisCalculator = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 15,
                MaxSpeedPerMM = 20,
                StepsPerMM = 1000
            });
        }
    }

    internal class MoveData
    {
        public Movement StepsE { get; }
        public Movement StepsX { get; }
        public Movement StepsY { get; }
        public Movement StepsZ { get; }

        public MoveData(Movement stepsE, Movement stepsX, Movement stepsY, Movement stepsZ)
        {
            StepsE = stepsE;
            StepsX = stepsX;
            StepsY = stepsY;
            StepsZ = stepsZ;
        }
    }
}
