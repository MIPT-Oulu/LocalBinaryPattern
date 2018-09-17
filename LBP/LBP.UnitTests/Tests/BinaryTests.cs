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

        public BinaryTests()
        {
            Directory.CreateDirectory(@"C:\temp\test\load");
            Directory.CreateDirectory(@"C:\temp\test\save");
        }

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

        [Fact]
        public void SaveAndRead_WeightsOverride_EqualsInputArray()
        {
            testImg.New("Quarters", new int[] { 12, 12 });
            string filename = @"C:\temp\test\load\TestWeight2.dat";
            // Write feature file
            int w = 20, nComp = 10;
            float[,] eigenVectors = new float[w, nComp].Add(1);
            float[] singularValues = new float[nComp].Add(2);
            double[] weights = new double[nComp];
            double[] mean = new double[w];
            using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create))) // BinaryWriter is little endian
            {
                writer.Write(w); // write array width
                writer.Write(10);
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < nComp; j++)
                    {
                         writer.Write(eigenVectors[i, j]);
                    }
                }
                for (int i = 0; i < nComp; i++)
                {
                    writer.Write(singularValues[i]);
                }
                for (int i = 0; i < nComp; i++)
                {
                    writer.Write(weights[i]);
                }
                for (int i = 0; i < w; i++)
                {
                    writer.Write(mean[i]);
                }
            }

            lbpreader.ReadWeights(filename);

            Assert.Equal(w, lbpreader.w);
            Assert.Equal(nComp, lbpreader.ncomp);
            Assert.Equal(eigenVectors, lbpreader.eigenVectors);
            Assert.Equal(singularValues, lbpreader.singularValues);
            Assert.Equal(weights, lbpreader.weights);
        }

        [Fact]
        public void SaveAndRead_Weights_EqualsInputArray()
        {
            testImg.New("Quarters", new int[] { 12, 12 });
            string filename = @"C:\temp\test\load\TestWeight.dat";
            // Write feature file
            int w = 20, nComp = 10;
            float[,] eigenVectors = new float[w, nComp].Add(1);
            float[] singularValues = new float[nComp].Add(2);
            double[] weights = new double[nComp];
            double[] mean = new double[w];
            using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create))) // BinaryWriter is little endian
            {
                writer.Write(w); // write array width
                writer.Write(10);
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < nComp; j++)
                    {
                        writer.Write(eigenVectors[i, j]);
                    }
                }
                for (int i = 0; i < nComp; i++)
                {
                    writer.Write(singularValues[i]);
                }
                for (int i = 0; i < nComp; i++)
                {
                    writer.Write(weights[i]);
                }
                for (int i = 0; i < w; i++)
                {
                    writer.Write(mean[i]);
                }
            }

            lbpreader.filename = filename;
            lbpreader.ReadWeights();

            Assert.Equal(w, lbpreader.w);
            Assert.Equal(nComp, lbpreader.ncomp);
            Assert.Equal(eigenVectors, lbpreader.eigenVectors);
            Assert.Equal(singularValues, lbpreader.singularValues);
            Assert.Equal(weights, lbpreader.weights);
        }
    }
}
