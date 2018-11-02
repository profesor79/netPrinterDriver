using System;

namespace stepperCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var c = new MovementCalculator(new PrinterConfiguration());
            var steps = c.CalculateX(432.32, 21.43, 13);
        }
    }

    public class MovementCalculator
    {
        private PrinterConfiguration _printerConfiguration;
        public MovementCalculator(PrinterConfiguration printerConfiguration)
        {
            _printerConfiguration = printerConfiguration;
        }

        public Movement CalculateX(double start, double stop, double maxSpeedMmPerSec)
        {
            // calculate distance
            var distance =  stop - start;
            var direction = distance > 0; // true forward
            // given speed
            // max printer speed
            var speed = maxSpeedMmPerSec > _printerConfiguration.XMaxSpeedPerMM
                ? _printerConfiguration.XMaxSpeedPerMM
                : maxSpeedMmPerSec;

            // get the time for a full speed (acceleration)
/*
 * t=(v-v0)/a --> t => speed / axis max accel
 */
            var timeToFullSpeed = speed / _printerConfiguration.XMaxAcceleration;

/*
 * s = at^2/2 -->
 *Speed after a given distance of travel with constant acceleration:/
 */

            var movementToFullSpeed = _printerConfiguration.XMaxAcceleration * Math.Sqrt(timeToFullSpeed) / 2;

/*
 * now we could calculate time after a given distance
 * distance = 1/2 * accel * time^2 --> time^2 = (distance * 2) / accel ==>
 * time = sqrt((2*distance)/accel)
 */


/*
 * v^2 = 2*a(distance) --> v = sqrt(2*a(distance))
 */

            //  lets calculate distance traveled after 0.1 seconds interval
            var timeInterval = 0.1;
            var numberOfIntervals = (int) (timeToFullSpeed / timeInterval);
            Console.WriteLine($"numberOfIntervals: {numberOfIntervals}");
            var traveledSteps = 0;
            var previousDistance = 0.00;
            for (int i = 1; i <= numberOfIntervals; i++){

                var distanceAfterTime = 0.5 * _printerConfiguration.XMaxAcceleration *(( i*timeInterval)*( i*timeInterval));
                var traveledDistance = distanceAfterTime - previousDistance;
                var steps = (int) (traveledDistance * _printerConfiguration.XStepsPerMM);
                traveledSteps += steps;
                var totalTraveledSteps = distanceAfterTime * _printerConfiguration.XStepsPerMM;

                Console.Write($"distance: {distanceAfterTime} after time: { i*timeInterval}, traveledDistance: {traveledDistance}");
                Console.WriteLine($" totalTraveledSteps: {totalTraveledSteps}, steps: {steps}, traveledSteps: {traveledSteps}");


                previousDistance = distanceAfterTime;
            }

            Console.WriteLine($"distance: {distance} mm");
            Console.WriteLine($"is forward: {direction}");
            Console.WriteLine($"timeToFullSpeed: {timeToFullSpeed} seconds");
            Console.WriteLine($"movementToFullSpeed: {movementToFullSpeed} mm ");


            return null;
        }
    }
}
