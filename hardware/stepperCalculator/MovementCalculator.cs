using System;

namespace stepperCalculator
{
    public class MovementCalculator
    {
        private PrinterConfiguration _printerConfiguration;
        public MovementCalculator(PrinterConfiguration printerConfiguration)
        {
            _printerConfiguration = printerConfiguration;
        }

        public Movement CalculateX(double startPosition, double stop, double maxSpeedMmPerSec)
        {
            // calculate distance
            var distance =  stop - startPosition;

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

            var distanceToFullSpeed = _printerConfiguration.XMaxAcceleration * Math.Sqrt(timeToFullSpeed) / 2;

/*
 * now we could calculate time after a given distance
 * distance = 1/2 * accel * time^2 --> time^2 = (distance * 2) / accel ==>
 * time = sqrt((2*distance)/accel)
 *
 * v^2 = 2*a(distance) --> v = sqrt(2*a(distance))
 */


            var move = new Movement();

            // as we know distance traveled to get full speed, then we can calculate step than we need to go

            var stepsNumber = 0;
            double traveledDistance = 0;
            double timeBeforeStep = 0;
            var accelerating= true;
            while (accelerating)
            {
                stepsNumber++;

                var distanceAfterStep = stepsNumber / _printerConfiguration.XStepsPerMM;
                var speedAfterDistance = Math.Sqrt(2 * _printerConfiguration.XMaxAcceleration * distanceAfterStep);
                accelerating = speed >= speedAfterDistance;

                if (!accelerating) continue; //return from loop

                var timeCaculatedFromStart = Math.Sqrt(2 * distanceAfterStep / _printerConfiguration.XMaxAcceleration);
                var cycleTime = timeCaculatedFromStart - timeBeforeStep;
                var stepData = new StepData
                {
                    DistanceAfterStep = distanceAfterStep,
                    HeadPositionAfterStep =
                        (direction) ? distanceAfterStep + startPosition : startPosition - distanceAfterStep,
                    StepTime = cycleTime,
                    SpeedAfterMove = speedAfterDistance,
                    StepNumber = stepsNumber
                };

                move.Steps.Add(stepData);
                //Console.WriteLine($"HeadPositionAfterStep: {stepData.HeadPositionAfterStep} SpeedAfterMove: {stepData.SpeedAfterMove} StepTime:{stepData.StepTime}");
                // step is finished
                traveledDistance = distanceAfterStep;
                timeBeforeStep = timeCaculatedFromStart;

            }



            return move;
        }
    }
}
