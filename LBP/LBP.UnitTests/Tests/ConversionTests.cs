using System;
using LBPLibrary;
using NUnit.Framework;
using Accord.Math;
using System.Drawing;
using Xunit;

namespace LBPTesting.Tests
{
    public class ConversionTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        // Byte and Float

        [Xunit.Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void FloatArrayToByte_ExpectedInput_ResultEqualsReferenceMethod(string pattern)
        {   /// It's actually better to use default conversion functions from System namespace.
            testImg.New(pattern);

            byte[,] byteImg = Functions.FloatToByteMatrix(testImg.Image);
            byte[,] byteRef = testImg.Image.ToByte();

            CollectionAssert.AreEqual(byteImg, byteRef);
        }

        [Xunit.Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void FloatArrayToByte_ExpectedInput_ResultEqualsInput(string pattern)
        {   /// It's actually better to use default conversion functions from System namespace.
            testImg.New(pattern);

            byte[,] byteImg = Functions.FloatToByteMatrix(testImg.Image);

            CollectionAssert.AreEqual(byteImg, testImg.Image);
        }

        [Xunit.Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void ByteArrayToFloat_ExpectedInput_EqualsRefArray(string pattern)
        {   /// It's actually better to use default conversion functions from System namespace.
            testImg.New(pattern);

            byte[,] byteImg = testImg.Image.ToByte();
            float[,] floatImg = Functions.ByteToFloatMatrix(byteImg);
            float[,] floatRef = byteImg.ToSingle();

            CollectionAssert.AreEqual(floatImg, floatRef);
        }

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

            CollectionAssert.AreEqual(testImg.Image, floatRef);
        }

        [Fact]
        public void BitmapToFloat_ConvertSecondTime_EqualsInputArray()
        {   /// Check if first conversion deletes original bmp image
            testImg.New("Quarters");

            byte[,] byteImg = testImg.Image.ToByte();
            Bitmap bmp = Functions.ByteMatrixToBitmap(byteImg);
            float[,] floatRef = Functions.BitmapToFloatMatrix(bmp);
            float[,] floatRef2 = Functions.BitmapToFloatMatrix(bmp);

            CollectionAssert.AreEqual(testImg.Image, floatRef);
            CollectionAssert.AreEqual(floatRef2, floatRef);
        }
    }
}
