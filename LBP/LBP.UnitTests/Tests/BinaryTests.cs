using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using System.IO;
using Xunit;

namespace LBP.UnitTests
{
    public class BinaryTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        BinaryWriterApp lbpreader = new BinaryWriterApp(Directory.GetCurrentDirectory() + @"\Test.dat");

        [Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void SaveAndRead_IntegerArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            lbpreader.SaveLBPFeatures(testImg.Image.ToInt32());
            lbpreader.ReadLBPFeatures("Integer");

            Assert.Equal(testImg.Image.ToInt32(), lbpreader.features);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        [InlineData("Add residual")]
        public void SaveAndRead_FloatArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            lbpreader.SaveLBPFeatures(testImg.Image);
            lbpreader.ReadLBPFeatures("float");

            Assert.Equal(testImg.Image, lbpreader.image);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        [InlineData("Add residual")]
        public void SaveAndRead_DoubleArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            double[,] img = testImg.Image.ToDouble();
            lbpreader.SaveLBPFeatures(img);
            lbpreader.ReadLBPFeatures("double");

            Assert.Equal(img, lbpreader.image_double);
        }
    }
}
