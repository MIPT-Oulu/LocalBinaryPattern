using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBPTesting.Tests
{
    [TestFixture]
    class MedianFilterTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Test]
        public void MedianFilter_SmallQuarter_EqualsPythonArray()
        {   /// Test whether Medianfilter class equals to scipy.signal.medfilt (default)
            testImg.New("Quarters", new int[] { 6, 6 });
            Console.WriteLine("Input array:"); Functions.DisplayArray(testImg.Image);
            
            // Filter
            MedianFilter mc = new MedianFilter(3);
            double[,] imageFiltered = mc.Filtering(testImg.Image.ToDouble());
            Console.WriteLine("Median filtered:"); Functions.DisplayArray(imageFiltered);

            double[,] refArray = new double[6, 6] // Here, actually columns are written out
                {{ 0, 1, 1, 1, 3, 0},
                { 1, 1, 1, 3, 3, 3},
                { 1, 1, 2, 3, 3, 3},
                { 1, 2, 2, 3, 4, 3},
                { 2, 2, 2, 4, 4, 4},
                { 0, 2, 2, 2, 4, 0} };
            Console.WriteLine("Reference:"); Functions.DisplayArray(refArray);
            CollectionAssert.AreEqual(refArray, imageFiltered);
        }

        [Test]
        public void MedianFilter_LargeOrOddkernel_ThrowsCorrectException()
        {   /// Test whether Medianfilter class throws correct exceptions
            testImg.New("Quarters", new int[] { 6, 6 });

            // Filter
            MedianFilter medianLarge = new MedianFilter(15);
            MedianFilter medianEven = new MedianFilter(4);

            Exception ex = Assert.Throws<Exception>(
                delegate { medianLarge.Filtering(testImg.Image.ToDouble()); });
            Assert.AreEqual(ex.Message, "Kernel radius is larger than input array!");
            Exception ex2 = Assert.Throws<Exception>(
                delegate { medianEven.Filtering(testImg.Image.ToDouble()); });
            Assert.AreEqual(ex2.Message, "Kernel width is not odd!");
        }
    }
}
