using System;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBPTesting.Tests
{
    [TestFixture]
    class CalculateImageTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        // CalculateImage test
        [Test]
        public void CalculateImage_QuarterArray_EqualsReferenceMappedImages()
        {
            testImg.New("Quarters", new int[] { 28, 28});
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            var param = new Parameters();
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble(),
                Param = param,
                d = param.LargeRadius + (param.W_r[0] - 1) / 2,
                MRE = false,
            };
            // Result image size
            app.xSize = w - 2 * app.d;
            app.ySize = l - 2 * app.d;

            app.GetMapping(); // GEt mapping table
            app.CalculateImage();
            double[,] mappedLBP = app.LBPISMapped;
            app.MRE = true;
            app.FilterImage();
            app.CalculateImage();

            Functions.DisplayArray(mappedLBP);
            Functions.DisplayArray(app.LBPISMapped);
            Functions.DisplayArray(app.LBPIRMapped);
            Functions.DisplayArray(app.LBPILMapped);
            double[,] refLBP = new double[6, 6] // Here, actually columns are written out as rows
                {{ 8, 8, 8, 5, 5, 5},
                { 8, 8, 8, 5, 5, 6},
                { 8, 8, 8, 5, 5, 6},
                { 5, 6, 6, 3, 3, 3},
                { 5, 6, 6, 3, 3, 3},
                { 6, 6, 6, 3, 3, 3} };
            double[,] refIS = new double[6, 6]
                {{ 3, 4, 4, 5, 5, 6},
                { 4, 3, 3, 5, 5, 2},
                { 4, 3, 3, 5, 5, 2},
                { 6, 3, 3, 5, 5, 4},
                { 6, 3, 3, 5, 5, 4},
                { 2, 3, 3, 4, 4, 5} };
            double[,] refIR = new double[6, 6]
                {{ 8, 8, 8, 8, 8, 9},
                { 8, 8, 8, 8, 8, 5},
                { 8, 8, 8, 8, 7, 5},
                { 8, 8, 8, 8, 7, 9},
                { 8, 8, 7, 7, 7, 9},
                { 7, 6, 5, 9, 9, 9} };
            double[,] refIL = new double[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
                { 3, 3, 3, 5, 5, 5},
                { 3, 3, 3, 5, 5, 5},
                { 3, 3, 3, 5, 5, 5},
                { 3, 3, 3, 5, 5, 5},
                { 3, 3, 3, 5, 5, 5} };
            CollectionAssert.AreEqual(refLBP, mappedLBP);
            CollectionAssert.AreEqual(refIS, app.LBPISMapped);
            CollectionAssert.AreEqual(refIR, app.LBPIRMapped);
            CollectionAssert.AreEqual(refIL, app.LBPILMapped);
        }
    }
}
