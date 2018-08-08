using System;
using LBP.Components;
//using Microsoft.VisualStudio.TestTools.UnitTesting; // MSTest
using NUnit.Framework;
using Accord.Math;

namespace LBP.UnitTests
{
    [TestFixture]
    public class ConvolutionTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        double[,] kernel;

        [TestCase("Ones", "Nearest")] // Test different kernel widths, image patterns and paddings
        [TestCase("Ones", "Reflect")]
        [TestCase("", "Nearest")]
        [TestCase("", "Reflect")]
        [TestCase("Quarters", "Nearest")]
        [TestCase("Quarters", "Reflect")]
        [TestCase("Running numbers", "Nearest")]
        [TestCase("Running numbers", "Reflect")]
        public void Convolute_TestArrayOneKernel_EqualsInputArray(string pattern, string paddingMethod)
        {
            // Image
            testImg.New(pattern, new int[] { 30, 30 });

            for (int w = 1; w < 21; w+=4)
            {
                // Kernel
                kernel = new double[w, w];
                double d = (w - 1) / 2;
                kernel[(int)Math.Floor(d), (int)Math.Floor(d)] = 1;

                double[,] convResult = Functions.Convolution2D(kernel, testImg.Image.ToDouble(), paddingMethod);

                CollectionAssert.AreEqual(testImg.Image, convResult);
            }
        }

        [TestCase("Ones", "Nearest")] // Test different kernel widths, image patterns and paddings
        [TestCase("Ones", "Reflect")]
        [TestCase("", "Nearest")]
        [TestCase("", "Reflect")]
        [TestCase("Quarters", "Nearest")]
        [TestCase("Quarters", "Reflect")]
        [TestCase("Running numbers", "Nearest")]
        [TestCase("Running numbers", "Reflect")]
        public void Convolute_TestArrayWithModifiedKernel_EqualsInputArray(string pattern, string paddingMethod)
        {
            // Arrange
            testImg.New(pattern);
            for (int k = 0; k < 100; k+=10)
            {
                float e = (float)(0 + k * 0.000000001);

                // Random kernel
                int w = 5;
                kernel = new double[w, w];
                Random r = new Random();
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        kernel[i, j] = r.Next(0, 1000);
                    }
                }
                double[,] kernel2 = kernel.Add(e); // Second kernel with residual
                
                // Act
                double[,] convResult = Functions.Convolution2D(kernel, testImg.Image.ToDouble(), paddingMethod);
                double[,] convResult2 = Functions.Convolution2D(kernel2, testImg.Image.ToDouble(), paddingMethod);

                // Assert
                for (int i = 0; i < convResult.GetLength(0); i++)
                {
                    for (int j = 0; j < convResult.GetLength(1); j++)
                    {
                        Assert.AreEqual(convResult[i, j], convResult2[i, j], 0.0001);
                    }
                }
            }
        }

        [TestCase("Quarters", "Reflect")]
        public void Convolute_SmallQuarter_EqualsPythonArray(string pattern, string paddingMethod)
        {   /// Test whether convolution2D function equals to scipy.ndimage.convolve (default)
            testImg.New(pattern, new int[] { 6, 6 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            Console.WriteLine("Input array:"); Functions.DisplayArray(testImg.Image);
            
            // Convolute
            double[,] kernel = new double[9, 9].Add(1); // Ones kernel
            double[,] convolution = Functions.Convolution2D(kernel, testImg.Image.ToDouble(), paddingMethod);
            Console.WriteLine("Convoluted array:"); Functions.DisplayArray(convolution);

            float[,] refArray = new float[6, 6] // Here, actually columns are written out
                {{ 162, 162, 180, 198, 216, 216},
                { 162, 162, 180, 198, 216, 216},
                { 171, 171, 189, 207, 225, 225},
                { 180, 180, 198, 216, 234, 234},
                { 189, 189, 207, 225, 243, 243},
                { 189, 189, 207, 225, 243, 243} };
            Console.WriteLine("Reference:"); Functions.DisplayArray(refArray);
            CollectionAssert.AreEqual(refArray, convolution);
        }

        [Test]
        public void Convolute_LargeOrOddkernel_ThrowsCorrectException()
        {   /// Test whether Medianfilter class throws correct exceptions
            testImg.New("Quarters", new int[] { 6, 6 });

            // Filter
            double[,] kernelLarge = new double[15, 15].Add(1); // Ones kernel
            double[,] kernelEven = new double[2, 2].Add(1); // Ones kernel

            Exception ex = Assert.Throws<Exception>(
                delegate { Functions.Convolution2D(kernelLarge, testImg.Image.ToDouble(), "Nearest"); });
            Assert.AreEqual(ex.Message, "Kernel radius is larger than input array!");
            Exception ex2 = Assert.Throws<Exception>(
                delegate { Functions.Convolution2D(kernelEven, testImg.Image.ToDouble(), "Nearest"); });
            Assert.AreEqual(ex2.Message, "Kernel width is not odd!");
        }
    }
}
