using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;
using Xunit;

namespace LBPTesting.Tests
{

    public class InterpolationTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Fact]
        public void BilinearInterpolation_QuarterArray_EqualsRefArray()
        {
            testImg.New("Quarters", new int[] { 8, 8 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            double[,] interpolated = new double[w - 2, l - 2];
            double e = 1E-12;

            for (int i = 0; i < w - 2; i++)
            {
                for (int j = 0; j < l - 2; j++)
                {
                    interpolated[i, j] = Functions.Bilinear(testImg.Image.ToDouble(), 0.5, 0.5, i, j, e);
                }
            }

            //Functions.DisplayArray(testImg.Image);
            //Functions.DisplayArray(interpolated.ToSingle());
            double[,] refArray = new double[6, 6] // Here, actually columns are written out
                {{ 1, 1, 1, 2, 3, 3},
                { 1, 1, 1, 2, 3, 3},
                { 1, 1, 1, 2, 3, 3},
                { 1.5, 1.5, 1.5, 2.5, 3.5, 3.5},
                { 2, 2, 2, 3, 4, 4},
                { 2, 2, 2, 3, 4, 4} };
            for (int i = 0; i < w - 2; i++)
            {
                for (int j = 0; j < l - 2; j++)
                {
                    NUnit.Framework.Assert.AreEqual(refArray[i, j], interpolated[i, j], 1E-11);
                }
            }
        }

        [Fact]
        public void BilinearInterpolation_QuarterArray_smalldifference_EqualsRefArray()
        {
            testImg.New("Quarters", new int[] { 8, 8 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            double[,] interpolated = new double[w - 2, l - 2];
            double e = 1E-12;

            for (int i = 0; i < w - 2; i++)
            {
                for (int j = 0; j < l - 2; j++)
                {
                    interpolated[i, j] = Functions.Bilinear(testImg.Image.ToDouble(), 0.25, 0.25, i, j, e);
                }
            }

            //Functions.DisplayArray(testImg.Image);
            //Functions.DisplayArray(interpolated);
            double[,] refArray = new double[6, 6] // Here, actually columns are written out
                {{ 1, 1, 1, 1.5, 3, 3},
                { 1, 1, 1, 1.5, 3, 3},
                { 1, 1, 1, 1.5, 3, 3},
                { 1.25, 1.25, 1.25, 1.75, 3.25, 3.25},
                { 2, 2, 2, 2.5, 4, 4},
                { 2, 2, 2, 2.5, 4, 4} };
            for (int i = 0; i < w - 2; i++)
            {
                for (int j = 0; j < l - 2; j++)
                {
                    NUnit.Framework.Assert.AreEqual(refArray[i, j], interpolated[i, j], 1E-11);
                }
            }
        }

        [Fact]
        public void BilinearInterpolation_QuarterArray_LargeResidual_GivesDifferentArray()
        {
            testImg.New("Quarters", new int[] { 8, 8 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            double[,] interpolated = new double[w - 2, l - 2];
            double e = 1E-6;

            for (int i = 0; i < w - 2; i++)
            {
                for (int j = 0; j < l - 2; j++)
                {
                    interpolated[i, j] = Functions.Bilinear(testImg.Image.ToDouble(), 0.25, 0.25, i, j, e);
                }
            }

            //Functions.DisplayArray(testImg.Image);
            //Functions.DisplayArray(interpolated);
            double[,] refArray = new double[6, 6] // Here, actually columns are written out
                {{ 1, 1, 1, 1.5, 3, 3},
                { 1, 1, 1, 1.5, 3, 3},
                { 1, 1, 1, 1.5, 3, 3},
                { 1.25, 1.25, 1.25, 1.75, 3.25, 3.25},
                { 2, 2, 2, 2.5, 4, 4},
                { 2, 2, 2, 2.5, 4, 4} };
            CollectionAssert.AreNotEqual(refArray, interpolated);
        }
    }
}
