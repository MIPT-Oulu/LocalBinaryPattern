using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using System.Drawing;
using Xunit;

namespace LBP.UnitTests
{
    public class ConversionTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        // Bitmap conversions

        [Xunit.Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void BitmapToFloat_ConvertBack_EqualsInputArray(string pattern)
        {   /// Bitmap conversions can't be done straight using system namespace
            testImg.New(pattern);

            byte[,] byteImg = testImg.Image.ToByte();
            Bitmap bmp = Functions.ByteMatrixToBitmap(byteImg);
            float[,] floatRef = Functions.BitmapToFloatMatrix(bmp);

            Assert.Equal(testImg.Image, floatRef);
        }

        [Fact]
        public void BitmapToFloat_ConvertSecondTime_EqualsInputArray()
        {   /// Check if first conversion deletes original bmp image
            testImg.New("Quarters");

            byte[,] byteImg = testImg.Image.ToByte();
            Bitmap bmp = Functions.ByteMatrixToBitmap(byteImg);
            float[,] floatRef = Functions.BitmapToFloatMatrix(bmp);
            float[,] floatRef2 = Functions.BitmapToFloatMatrix(bmp);

            Assert.Equal(testImg.Image, floatRef);
            Assert.Equal(floatRef2, floatRef);
        }
    }
}
