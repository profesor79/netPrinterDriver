﻿﻿using System;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.Linq;

namespace stepperCalculator
{
    public class MovementCalculator
    {
        private readonly AxisConfiguration _axisConf;
        private int _stepsNumber;
        private double _traveledDistance;
        private double _timeBeforeStep;
        private bool _acceleratedToMaxSpeed;

        private Movement _move = new Movement();
        private double _speed;
        private double _distance;
        private bool _onlyOneStep;


        public MovementCalculator(AxisConfiguration axisConf)
        {
            _axisConf = axisConf;
        }

        public Movement CalculateSteps(double startPosition, double stop, double maxSpeedMmPerSec)
        {
            ResetValues();
            Console.WriteLine($"calculating for: startPosition:{startPosition}, stop:{stop}, maxSpeedMmPerSec:{maxSpeedMmPerSec}");


            // calculate distance
            _distance = stop - startPosition;
            Console.WriteLine(_distance);

            // set minimum step value as a tolerance
            var tolerance = 1 / _axisConf.StepsPerMM;
            if (Math.Abs(_distance) < tolerance)
            {
                Console.WriteLine("no move");
                return _move;
            }

            _move.Direction = _distance > 0; // true forward

            // given speed
            // max printer speed
            _speed = maxSpeedMmPerSec > _axisConf.MaxSpeedPerMM
                ? _axisConf.MaxSpeedPerMM
                : maxSpeedMmPerSec;

            // get the time for a full speed (acceleration)
            /*
             * t=(v-v0)/a --> t => speed / axis max accel
             */
            var timeToFullSpeed = _speed / _axisConf.MaxAcceleration;

            /*
             * s = at^2/2 -->
             *Speed after a given distance of travel with constant acceleration:/
             */


            /*
             * now we could calculate time after a given distance
             * distance = 1/2 * accel * time^2 --> time^2 = (distance * 2) / accel ==>
             * time = sqrt((2*distance)/accel)
             *
             * v^2 = 2*a(distance) --> v = sqrt(2*a(distance))
             */


            // as we know distance traveled to get full speed, then we can calculate step than we need to go


            var accelerating = true;
            while (accelerating)
            {
                accelerating = Accelerating(startPosition, _distance, _speed, _move.Direction, _move);
            }

// as we could have a edge case where only one step need to be added,
// accelarating method need to give a flag to stop processing


            Debug.Assert(0 < _move.HeadSteps.Count, $"0 < _move.HeadSteps.Count {_move.HeadSteps.Count}",$"calculating for: startPosition:{startPosition}, stop:{stop}, maxSpeedMmPerSec:{maxSpeedMmPerSec}, steps/mm {_axisConf.StepsPerMM}, min step: {1/_axisConf.StepsPerMM}, distance to travel: {stop-startPosition}" );

            // now we can do deceleration
            if (!_onlyOneStep) // prevent creating deceleration steps when only one need to be added to the move
            {
                var decelerationSteps = DecelarationStepData(stop);
                CalculateBodySteps(startPosition, decelerationSteps[0].SpeedAfterMove);
                _move.TailSteps.AddRange(decelerationSteps);
            }

            return _move;
        }

        private void ResetValues()
        {
            _move = new Movement();
            _stepsNumber = 0;
            _traveledDistance = 0;
            _timeBeforeStep = 0;
            _acceleratedToMaxSpeed = true;

        _speed = 0;
         _distance = 0;
        }

        private void CalculateBodySteps(double startPosition, double decelerationStepsSpeedAfterMove)
        {
            var distanceToMoveWithMaxSpeed = _distance - 2 * _traveledDistance;
            var bodyMovementSpeed = _acceleratedToMaxSpeed ? _speed : decelerationStepsSpeedAfterMove;
            var timeWithFullSpeed =  distanceToMoveWithMaxSpeed / _speed;
            var stepsCountWithMaxSpeed =(int) (distanceToMoveWithMaxSpeed * _axisConf.StepsPerMM);
            var maxSpeedCycleTime = timeWithFullSpeed / stepsCountWithMaxSpeed;


            for (int i = 1; i <= stepsCountWithMaxSpeed; i++)
            {
                var stepData = new StepData
                {
                    DistanceAfterStep = _stepsNumber / _axisConf.StepsPerMM,
                    HeadPositionAfterStep =
                        (_move.Direction)
                            ? _stepsNumber / _axisConf.StepsPerMM + startPosition
                            : startPosition - _stepsNumber / _axisConf.StepsPerMM,
                    StepTime = maxSpeedCycleTime,
                    SpeedAfterMove = bodyMovementSpeed,
                    StepNumber = _stepsNumber
                };

                _move.BodySteps.Add(stepData);
                _stepsNumber++;
            }

            _move.TotalTime +=	 maxSpeedCycleTime*stepsCountWithMaxSpeed;
        }

        private List<StepData> DecelarationStepData(double stop)
        {
            var deceleration = new List<StepData>();
            var decelerationStep = 0;
            foreach (var step in _move.HeadSteps)
            {
                var decStep = new StepData
                {
                    DistanceAfterStep = _traveledDistance - decelerationStep / _axisConf.StepsPerMM,
                    StepTime = step.StepTime,
                    StepNumber = -step.StepNumber,
                    SpeedAfterMove = step.SpeedAfterMove,
                    HeadPositionAfterStep = (stop - decelerationStep / _axisConf.StepsPerMM),
                    TimeStamp = step.TimeStamp
                };

                deceleration.Insert(0, decStep);
                _move.TotalTime += step.StepTime;
                decelerationStep++;
            }

            return deceleration;
        }

        private bool Accelerating(double startPosition,
            double distance,
            double speed,
            bool direction,
            Movement move
            )
        {
            bool accelerating;
            _stepsNumber++;

            var distanceAfterStep = _stepsNumber / _axisConf.StepsPerMM;


            var speedAfterDistance = Math.Sqrt(2 * _axisConf.MaxAcceleration * distanceAfterStep);
            accelerating = speed >= speedAfterDistance;

            if (!accelerating) return accelerating;

            var timeCaculatedFromStart = Math.Sqrt(2 * distanceAfterStep / _axisConf.MaxAcceleration);
            var cycleTime = timeCaculatedFromStart - _timeBeforeStep;
            var stepData = new StepData
            {
                DistanceAfterStep = distanceAfterStep,
                HeadPositionAfterStep =
                    (direction) ? distanceAfterStep + startPosition : startPosition - distanceAfterStep,
                StepTime = cycleTime,
                SpeedAfterMove = speedAfterDistance,
                StepNumber = _stepsNumber
            };

            // check if we need to brake
            var fullDistance = 2 * distanceAfterStep;
            if (fullDistance > Math.Abs(distance))
            {
                accelerating = false;
                _acceleratedToMaxSpeed = false;

                // if we do first step then add it to the steps
                if (_stepsNumber > 1)
                {
                    return accelerating;
                }
                else
                {
                    _onlyOneStep = true;
                }
            }


            _move.TotalTime += stepData.StepTime;
            move.HeadSteps.Add(stepData);
            _traveledDistance = distanceAfterStep;
            _timeBeforeStep = timeCaculatedFromStart;
            return accelerating;
        }
    }
}
