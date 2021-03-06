using System;
using stepperCalculator;
using Xunit;


namespace stepperCalculatorTests
{
    public class AxisMovementCalculatorTests
    {

        [Fact]
        public void WhenCalculatingStepsNumberWithLowMaxSpeed()
        {
            // arange
            var stepsMM = 100;
            var distance = 20;
            var sut = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 2,
                StepsPerMM = stepsMM,
                MaxSpeedPerMM = 1
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateSteps(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
        }




        [Fact]
        public void WhenCalculatingStepsNumberWithHighMaxSpeed()
        {
            // arange
            var stepsMM = 100;
            var distance = 20;
            var sut = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 2,
                StepsPerMM = stepsMM,
                MaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateSteps(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
            Assert.Equal(0, steps.BodySteps.Count);
        }




        [Fact]
        public void WhenCalculatingStepsNumberWithHighMaxSpeedAndHighResultion()
        {
            // arange
            var stepsMM = 2000;
            var distance = 20;
            var sut = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 2,
                StepsPerMM = stepsMM,
                MaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateSteps(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
            Assert.Equal(0, steps.BodySteps.Count);
        }




        [Fact]
        public void WhenCalculatingStepsNumberWithHighMaxSpeedAndLowResultion()
        {
            // arange
            var stepsMM = 1;
            var distance = 23;
            var sut = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 2,
                StepsPerMM = stepsMM,
                MaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateSteps(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
            Assert.Equal(distance % 2 , steps.BodySteps.Count);
            Assert.NotEqual(50, steps.BodySteps[0].SpeedAfterMove);
        }









        [Fact]
        public void WhenGettingFullSpeed()
        {
            // arange
            var stepsMM = 1;
            var distance = 1000;
            var givenSpeed = 50;
            var sut = new MovementCalculator(new AxisConfiguration
            {
                MaxAcceleration = 200,
                StepsPerMM = stepsMM,
                MaxSpeedPerMM = givenSpeed
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateSteps(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
            Assert.True(steps.BodySteps.Count > 1);
            Assert.Equal(givenSpeed, steps.BodySteps[0].SpeedAfterMove);
        }








    }
}
