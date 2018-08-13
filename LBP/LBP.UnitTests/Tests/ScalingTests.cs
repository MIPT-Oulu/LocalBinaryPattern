using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using Xunit;

namespace LBP.UnitTests
{
    public class ScalingTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Theory]
        [InlineData("Quarters")]
        [InlineData("Running numbers")]
        public void Scaling_TestArray_EqualsReference(string method)
        {
            testImg.New(method);
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble()
            };

            app.Scaling(); // Scaling from LBPAppilcation
            // Reference
            double mean = testImg.Image
                .Sum() 
                / testImg.Image.Length;
            double std = Math.Sqrt(testImg.Image
                .Subtract(mean)
                .Pow(2)
                .Sum()
                / (testImg.Image.Length - 1));
            float[,] refArray = testImg.Image
                .Subtract(mean)
                .Divide(std)
                .ToSingle();

            // Assert
            for (int i = 0; i < app.Image.GetLength(0); i++)
            {
                for (int j = 0; j < app.Image.GetLength(1); j++)
                {
                   Assert.Equal(app.Image[i, j], refArray[i, j], 5);
                }
            }
        }

        [Fact]
        public void Scaling_OnesArray_ThrowsException()
        {
            testImg.New("Ones");
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble()
            };

            Exception ex = Assert.Throws<Exception>(
                delegate { app.Scaling(); });
            Assert.Equal("Standard deviation of the image is 0! Cannot divide!", ex.Message);
        }
    }
}
