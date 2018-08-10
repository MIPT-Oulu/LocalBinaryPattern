using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;
using Xunit;

namespace LBP.UnitTests
{
    public class PaddingTests
    {
        string method;
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Xunit.Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1)]
        [InlineData(0)]
        public void Padding_ZeroPadArray_EqualsToZeropadded(int padLength)
        {
            string method = "Zero";
            testImg.New("Ones");
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

        [Fact]
        public void Padding_ReflectOverArraySize_ThrowsCorrectException()
        {   /// Test whether Medianfilter class throws correct exceptions
            testImg.New("Quarters", new int[] { 6, 6 });
            int padding = 7;

            // Filter
            Exception ex = NUnit.Framework.Assert.Throws<Exception>(
                delegate { Functions.ArrayPadding(testImg.Image, padding, "Reflect"); });

            NUnit.Framework.Assert.AreEqual(ex.Message, "Cannot reflect over array size!");
        }
    }
}
