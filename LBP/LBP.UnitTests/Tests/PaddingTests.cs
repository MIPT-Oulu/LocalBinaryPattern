using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBP.UnitTests
{
    [TestFixture]
    public class PaddingTests
    {
        string method;
        TestImage testImg = new TestImage(); // Initialize testimage function

        [SetUp]
        protected void SetUp()
        {
            // Arrange
            method = "Zero";
            testImg.New("Ones");
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(1)]
        [TestCase(0)]
        public void Padding_ZeroPadArray_EqualsToZeropadded(int padLength)
        {
            // Act
            float[,] padImage = Functions.ArrayPadding(testImg.Image, padLength, method); // Call method and pad the test image

            //Assert
            // Reference array
            float[,] refArray = new float[testImg.Size[0] + 2 * padLength, testImg.Size[1] + 2 * padLength];
            for (int i = padLength; i < refArray.GetLength(0) - padLength; i++)
            {
                for (int j = padLength; j < refArray.GetLength(1) - padLength; j++)
                {
                    refArray[i, j] = 1;
                }
            }

            CollectionAssert.AreEqual(refArray, padImage);
        }

        [Test]
        public void Padding_ReflectOverArraySize_ThrowsCorrectException()
        {   /// Test whether Medianfilter class throws correct exceptions
            testImg.New("Quarters", new int[] { 6, 6 });
            int padding = 7;

            // Filter
            Exception ex = Assert.Throws<Exception>(
                delegate { Functions.ArrayPadding(testImg.Image, padding, "Reflect"); });

            Assert.AreEqual(ex.Message, "Cannot reflect over array size!");
        }
    }
}
