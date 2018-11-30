using System;
using stepperCalculator;
using Xunit;


namespace stepperCalculatorTests
{
    public class UnitTest1
    {

        [Fact]
        public void WhenCalculatingStepsNumberWithLowMaxSpeed()
        {
            // arange
            var stepsMM = 100;
            var distance = 20;
            var sut = new MovementCalculator(new PrinterConfiguration
            {
                XMaxAcceleration = 2,
                XStepsPerMM = stepsMM,
                XMaxSpeedPerMM = 1
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateX(0, distance, 50);
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
            var sut = new MovementCalculator(new PrinterConfiguration
            {
                XMaxAcceleration = 2,
                XStepsPerMM = stepsMM,
                XMaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateX(0, distance, 50);
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
            var sut = new MovementCalculator(new PrinterConfiguration
            {
                XMaxAcceleration = 2,
                XStepsPerMM = stepsMM,
                XMaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateX(0, distance, 50);
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
            var sut = new MovementCalculator(new PrinterConfiguration
            {
                XMaxAcceleration = 2,
                XStepsPerMM = stepsMM,
                XMaxSpeedPerMM = 50
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateX(0, distance, 50);
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
            var sut = new MovementCalculator(new PrinterConfiguration
            {
                XMaxAcceleration = 200,
                XStepsPerMM = stepsMM,
                XMaxSpeedPerMM = givenSpeed
            });

            var expectedStepsCount = distance*stepsMM;


            // act
            var steps = sut.CalculateX(0, distance, 50);
            var actualStepsContu = steps.TailSteps.Count + steps.BodySteps.Count + steps.HeadSteps.Count;

            // assert
            Assert.Equal(expectedStepsCount, actualStepsContu);
            Assert.True(steps.BodySteps.Count > 1);
            Assert.Equal(givenSpeed, steps.BodySteps[0].SpeedAfterMove);
        }








    }
}
