using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBPTesting.Tests
{
    [TestFixture]
    class BinaryTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        BinaryWriterApp lbpreader = new BinaryWriterApp(@"C:\Users\sarytky\Desktop\trials\Test.dat");

        [TestCase("")]
        [TestCase("Quarters")]
        [TestCase("Ones")]
        [TestCase("Running numbers")]
        public void SaveAndRead_IntegerArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            lbpreader.SaveLBPFeatures(testImg.Image.ToInt32());
            lbpreader.ReadLBPFeatures("Integer");

            Functions.DisplayArray(lbpreader.features);
            CollectionAssert.AreEqual(testImg.Image, lbpreader.features);
        }

        [TestCase("")]
        [TestCase("Quarters")]
        [TestCase("Ones")]
        [TestCase("Running numbers")]
        [TestCase("Add residual")]
        public void SaveAndRead_FloatArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            lbpreader.SaveLBPFeatures(testImg.Image);
            lbpreader.ReadLBPFeatures("float");

            Functions.DisplayArray(lbpreader.image);
            CollectionAssert.AreEqual(testImg.Image, lbpreader.image);
        }

        [TestCase("")]
        [TestCase("Quarters")]
        [TestCase("Ones")]
        [TestCase("Running numbers")]
        [TestCase("Add residual")]
        public void SaveAndRead_DoubleArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);

            double[,] img = testImg.Image.ToDouble();
            lbpreader.SaveLBPFeatures(img);
            lbpreader.ReadLBPFeatures("double");

            Functions.DisplayArray(img);
            Functions.DisplayArray(lbpreader.image_double);
            CollectionAssert.AreEqual(img, lbpreader.image_double);
        }
    }
}
