using System;
using System.Numerics;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using System.IO;
using Xunit;

namespace LBP.UnitTests
{

    public class BMPWriterTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        string filename = Directory.GetCurrentDirectory() + @"\Test.png";

        [Fact]
        public void SaveAndRead_IntegerArray_EqualsInputArray()
        {
            testImg.New();

            Functions.Save(filename, testImg.Image.ToDouble(), false);
            float[,] readedArray = Functions.Load(filename);

            Assert.Equal(testImg.Image, readedArray);
        }

        [Fact]
        public void SaveAndRead_FloatArray_EqualsInputArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.Save(filename, testImg.Image.ToDouble(), false);
            float[,] readedArray = Functions.Load(filename);

            testImg.Image = testImg.Image.Round().ToSingle(); // Round
            Assert.Equal(testImg.Image, readedArray);
        }

        [Fact]
        public void SaveAndRead_FloatArrayNormalized_EqualsNormalizedArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.Save(filename, testImg.Image.ToDouble(), true);
            float[,] readedArray = Functions.Load(filename);

            testImg.Image = Functions.Normalize(testImg.Image.ToDouble()).Multiply(255).Round().ToSingle(); // Normalize array
            Assert.Equal(testImg.Image, readedArray);
        }
    }
}
