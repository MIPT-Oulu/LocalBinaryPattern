using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using System.IO;
using Xunit;

namespace LBP.UnitTests
{
    public class CSVWriterTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        string filename = Directory.GetCurrentDirectory() + @"\Test.csv";

        [Fact]
        public void SaveAndRead_IntegerArray_EqualsInputArray()
        {
            testImg.New();

            Functions.WriteCSV(testImg.Image, filename);
            float[,] readedArray = Functions.ReadCSV(filename);

            Assert.Equal(testImg.Image, readedArray);
        }

        [Fact]
        public void SaveAndRead_FloatArray_EqualsInputArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.WriteCSV(testImg.Image, filename);
            float[,] readedArray = Functions.ReadCSV(filename);

            Assert.Equal(testImg.Image, readedArray);
        }
    }
}
