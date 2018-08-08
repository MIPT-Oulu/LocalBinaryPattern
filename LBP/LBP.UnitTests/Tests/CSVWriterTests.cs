using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBPTesting.Tests
{
    [TestFixture]
    class CSVWriterTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        string filename = @"C:\Users\sarytky\Desktop\trials\Test.csv";

        [Test]
        public void SaveAndRead_IntegerArray_EqualsInputArray()
        {
            testImg.New();

            Functions.WriteCSV(testImg.Image, filename);
            float[,] readedArray = Functions.ReadCSV(filename);

            Functions.DisplayArray(testImg.Image);
            Functions.DisplayArray(readedArray);
            CollectionAssert.AreEqual(testImg.Image, readedArray);
        }

        [Test]
        public void SaveAndRead_FloatArray_EqualsInputArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.WriteCSV(testImg.Image, filename);
            float[,] readedArray = Functions.ReadCSV(filename);

            Functions.DisplayArray(testImg.Image);
            Functions.DisplayArray(readedArray);
            CollectionAssert.AreEqual(testImg.Image, readedArray);
        }
    }
}
