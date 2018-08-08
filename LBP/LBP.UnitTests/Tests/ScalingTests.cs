using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;


namespace LBPTesting.Tests
{
    [TestFixture]
    class ScalingTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        [TestCase("Quarters")]
        [TestCase("Running numbers")]
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

            Functions.DisplayArray(app.Image);
            Functions.DisplayArray(refArray);
            // Assert
            for (int i = 0; i < app.Image.GetLength(0); i++)
            {
                for (int j = 0; j < app.Image.GetLength(1); j++)
                {
                    Assert.AreEqual(app.Image[i, j], refArray[i, j], 0.000001);
                }
            }
        }

        [Test]
        public void Scaling_OnesArray_ThrowsException()
        {
            testImg.New("Ones");
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble()
            };

            Exception ex = Assert.Throws<Exception>(
                delegate { app.Scaling(); });
            Assert.AreEqual(ex.Message, "Standard deviation of the image is 0! Cannot divide!");
        }
    }
}
