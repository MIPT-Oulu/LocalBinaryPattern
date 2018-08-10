using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;
using Xunit;


namespace LBP.UnitTests.Tests
{
    public class Pipeline
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        // FilterImage test
        [Fact]
        public void FilteringPipelineSubtraction_SmallQuarter_EqualsModifiedPythonArray()
        {   /// Test subtraction of mean from filtering pipeline
            testImg.New("Quarters", new int[] { 6, 6 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            var param = new Parameters() { W_c = 3 };
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble(),
                d = (param.W_r[0] - 1) / 2,
                Param = param
            };
            app.xSize = w - 2 * app.d;
            app.ySize = l - 2 * app.d;

            // Filtering pipeline
            app.FilterImage();

            double[,] refArray = new double[6, 6] // Here, actually columns are written out
                {{ 0, 1, 1, 1, 3, 0},
            { 1, 1, 1, 3, 3, 3},
            { 1, 1, 2, 3, 3, 3},
            { 1, 2, 2, 3, 4, 3},
            { 2, 2, 2, 4, 4, 4},
            { 0, 2, 2, 2, 4, 0} };
            refArray = refArray // Subtraction is done only from center pixels not affected by edge artifacts
                .Subtract(
                Functions.Mean(
                    Functions.GetSubMatrix(refArray, app.d, w - app.d - 1, app.d, l - app.d - 1)));
            Console.WriteLine("Reference:"); Functions.DisplayArray(refArray);
            CollectionAssert.AreEqual(refArray, app.imageCenter);
        }

        // GetMapping test
        [Fact]
        public void GetMapping_ScaledQuarterArray_EqualsReferenceArray()
        {
            testImg.New("Quarters");
            testImg.Image = testImg.Image.Multiply(256 / 4).Subtract(1); // Scale to 8-bit range
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            var param = new Parameters() { Neighbours = 8 };
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble(),
                Param = param
            };

            app.GetMapping(); // Get Mapping table
            double[,] mapped = new double[w, l];
            for (int i = 0; i < w; i++) // Apply mapping
            {
                for (int j = 0; j < l; j++)
                {
                    mapped[i, j] = app.mappingTable[(int)app.Image[i, j]];
                }
            }

            testImg.New("Quarters"); // Create reference array with known values
            float[,] refArray = testImg.Image;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    if (refArray[i, j] == 1) refArray[i, j] = 6;
                    if (refArray[i, j] == 2) refArray[i, j] = 7;
                    if (refArray[i, j] == 3) refArray[i, j] = 7;
                    if (refArray[i, j] == 4) refArray[i, j] = 8;
                }
            }
            Functions.DisplayArray(mapped);
            CollectionAssert.AreEqual(mapped, refArray);
        }

        [Fact]
        public void GetBinMapping_Testmap_EqualsReferenceMapping()
        {
            for (int n = 8; n < 17; n += 4)
            {
                var param = new Parameters() { Neighbours = n };
                var app = new LBPApplication() { Param = param };
                app.GetMapping(); // GetMapping function

                // Loop for mapping (reference)
                int[] mappingTable = new int[(int)Math.Pow(2, param.Neighbours)];
                for (int i = 0; i < Math.Pow(2, param.Neighbours); i++)
                {
                    // Binary and shifted binary strings (rotation invariance)
                    string binary = Convert.ToString(i, 2).PadLeft(param.Neighbours, '0');
                    string binaryShift = binary.Substring(1, binary.Length - 1) + binary.Substring(0, 1);

                    // Convert strings to double arrays
                    int[] binList = new int[binary.Length];
                    int[] binShiftList = new int[binaryShift.Length];
                    for (int ii = 0; ii < binary.Length; ii++)
                    {
                        binList[ii] = Convert.ToInt32(binary.Substring(ii, 1));
                        binShiftList[ii] = Convert.ToInt32(binaryShift.Substring(ii, 1));
                    }

                    // Calculate sum of different bits (uniformity)
                    int sum = 0;
                    for (int ii = 0; ii < binList.Length; ii++)
                    {
                        if (binList[ii] != binShiftList[ii])
                            sum++;
                    }

                    // Binning
                    if (sum <= 2)
                    {
                        int c = 0;
                        for (int ii = 0; ii < binary.Length; ii++)
                        {
                            c = c + (int)binList[ii];
                        }
                        mappingTable[i] = c;
                    }
                    else
                        mappingTable[i] = param.Neighbours + 1;
                }

                CollectionAssert.AreEqual(mappingTable, app.mappingTable);
            }

        }

        // GetHistogram test
        [Fact]
        public void GetHistogram_ScaledQuarterArray_FullHistogramBin()
        {
            testImg.New("Ones");
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            var param = new Parameters() { Neighbours = 8 };
            LBPApplication app = new LBPApplication
            {
                Image = testImg.Image.ToDouble(),
                LBPILMapped = testImg.Image.ToDouble(),
                LBPIRMapped = testImg.Image.Add(1).ToDouble(),
                LBPISMapped = testImg.Image.Add(2).ToDouble(),
                Param = param,
                xSize = w,
                ySize = l,
                MRE = false
            };

            app.GetMapping(); // Get Mapping table
            app.GetHistogram(); // Get Histograms
            int[] histLBP = app.histS;
            app.LBPISMapped = testImg.Image.Add(3).ToDouble();
            app.MRE = true;
            app.GetHistogram();

            Functions.DisplayVector(app.histL);
            Functions.DisplayVector(app.histR);
            Functions.DisplayVector(app.histS);
            Xunit.Assert.Equal(150, histLBP[3]);
            Xunit.Assert.Equal(150, app.histL[1]);
            Xunit.Assert.Equal(150, app.histR[2]);
            Xunit.Assert.Equal(150, app.histS[4]);
        }

        // Full pipeline test
        [Fact]
        public void CalculateImage_QuarterArray_EqualsReferenceMappedImages()
        {
            testImg.New("Quarters", new int[] { 28, 28 });
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);
            var param = new Parameters();


            LBPApplication.PipelineMRELBP(testImg.Image.ToDouble(), param, // MRELBP pipeline
            out double[,] LBPIL, out double[,] LBPIS, out double[,] LBPIR, out int[] histL, out int[] histS, out int[] histR, out int[] histCenter);
            testImg.New("Quarters", new int[] { 12, 12 });
            LBPApplication.PipelineLBP(testImg.Image.ToDouble(), param, // LBP pipeline
                out double[,] LBPresult, out int[] LBPhistogram);

            Functions.DisplayArray(LBPresult);
            Functions.DisplayArray(LBPIS);
            Functions.DisplayArray(LBPIR);
            Functions.DisplayArray(LBPIL);
            float[,] refLBP = new float[6, 6] // Here, actually columns are written out as rows
                {{ 8, 8, 8, 5, 5, 5},
            { 8, 8, 8, 5, 5, 6},
            { 8, 8, 8, 5, 5, 6},
            { 5, 6, 6, 3, 3, 3},
            { 5, 6, 6, 3, 3, 3},
            { 6, 6, 6, 3, 3, 3} };
            float[,] refIS = new float[6, 6]
                {{ 3, 4, 4, 5, 5, 6},
            { 4, 3, 3, 5, 5, 2},
            { 4, 3, 3, 5, 5, 2},
            { 6, 3, 3, 5, 5, 4},
            { 6, 3, 3, 5, 5, 4},
            { 2, 3, 3, 4, 4, 5} };
            float[,] refIR = new float[6, 6]
                {{ 8, 8, 8, 8, 8, 9},
            { 8, 8, 8, 8, 8, 5},
            { 8, 8, 8, 8, 7, 5},
            { 8, 8, 8, 8, 7, 9},
            { 8, 8, 7, 7, 7, 9},
            { 7, 6, 5, 9, 9, 9} };
            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            int[] refLBPHist = new int[] { 0, 0, 0, 9, 0, 9, 9, 0, 9, 0 };
            int[] refSHist = new int[] { 0, 0, 3, 11, 8, 11, 3, 0, 0, 0 };
            int[] refRHist = new int[] { 0, 0, 0, 0, 0, 3, 1, 6, 20, 6 };
            int[] refLHist = new int[] { 0, 0, 0, 18, 0, 18, 0, 0, 0, 0 };
            CollectionAssert.AreEqual(refLBP, LBPresult);
            CollectionAssert.AreEqual(refIS, LBPIS);
            CollectionAssert.AreEqual(refIR, LBPIR);
            CollectionAssert.AreEqual(refIL, LBPIL);
            CollectionAssert.AreEqual(refLBPHist, LBPhistogram);
            CollectionAssert.AreEqual(refSHist, histS);
            CollectionAssert.AreEqual(refRHist, histR);
            CollectionAssert.AreEqual(refLHist, histL);
        }

        [Fact]
        public void MRELBP_QuarterArray_EqualsPythonReference()
        {
            testImg.New("Quarters", new int[] { 16, 16 });
            var param = new Parameters()
            {
                LargeRadius = 2,
                Radius = 1
            };

            LBPApplication.PipelineMRELBP(testImg.Image.ToDouble(), param, // MRELBP pipeline
            out double[,] LBPIL, out double[,] LBPIS, out double[,] LBPIR, out int[] histL, out int[] histS, out int[] histR, out int[] histCenter);

            Console.WriteLine("Images");
            Functions.DisplayArray(LBPIS);
            Functions.DisplayArray(LBPIR);
            Functions.DisplayArray(LBPIL);
            Console.WriteLine("Histograms");
            Functions.DisplayVector(histL);
            Functions.DisplayVector(histS);
            Functions.DisplayVector(histR);
            Functions.DisplayVector(histCenter);
            float[,] refLBP = new float[6, 6] // Here, actually columns are written out as rows
                {{ 8, 8, 8, 5, 5, 5},
            { 8, 8, 8, 5, 5, 6},
            { 8, 8, 8, 5, 5, 6},
            { 5, 6, 6, 3, 3, 3},
            { 5, 6, 6, 3, 3, 3},
            { 6, 6, 6, 3, 3, 3} };
            float[,] refIS = new float[6, 6]
                {{ 3, 4, 4, 5, 5, 6},
            { 4, 3, 3, 5, 5, 2},
            { 4, 3, 3, 5, 5, 2},
            { 6, 3, 3, 5, 5, 4},
            { 6, 3, 3, 5, 5, 4},
            { 2, 3, 3, 4, 4, 5} };
            float[,] refIR = new float[6, 6]
                {{ 8, 8, 8, 8, 8, 9},
            { 8, 8, 8, 8, 8, 5},
            { 8, 8, 8, 8, 7, 5},
            { 8, 8, 8, 8, 7, 9},
            { 8, 8, 7, 7, 7, 9},
            { 7, 6, 5, 9, 9, 9} };
            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            int[] refLBPHist = new int[] { 0, 0, 0, 9, 0, 9, 9, 0, 9, 0 };
            int[] refSHist = new int[] { 0, 0, 3, 11, 8, 11, 3, 0, 0, 0 };
            int[] refRHist = new int[] { 0, 0, 0, 0, 0, 3, 1, 6, 20, 6 };
            int[] refLHist = new int[] { 0, 0, 0, 18, 0, 18, 0, 0, 0, 0 };
            //CollectionAssert.AreEqual(refLBP, LBPresult);
            //CollectionAssert.AreEqual(refIS, LBPIS);
            //CollectionAssert.AreEqual(refIR, LBPIR);
            //CollectionAssert.AreEqual(refIL, LBPIL);
            //CollectionAssert.AreEqual(refLBPHist, LBPhistogram);
            //CollectionAssert.AreEqual(refSHist, histS);
            //CollectionAssert.AreEqual(refRHist, histR);
            //CollectionAssert.AreEqual(refLHist, histL);
        }
    }
}
