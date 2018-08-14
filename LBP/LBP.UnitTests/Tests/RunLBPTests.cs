using System;
using System.IO;
using Xunit;
using LBPLibrary;
using Accord.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBP.UnitTests
{

    public class RunLBPTests
    {
        string filename = Directory.GetCurrentDirectory() + @"\Test.png";
        string save = Directory.GetCurrentDirectory();
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Fact]
        public void RunLBP_DefaultInput_OutputsDefaultParameters()
        {
            var param = new Parameters();

            var runlbp = new RunLBP();

            // Write more assertions
            Assert.Equal(param.LargeRadius, runlbp.param.LargeRadius);
            Assert.Equal(param.Neighbours, runlbp.param.Neighbours);
            Assert.Equal(param.Save, runlbp.param.Save);
        }

        [Fact]
        public void RunLBPCalculateSingle_MRE_png_EqualsReference()
        {
            var runlbp = new RunLBP()
            {
                path = filename,
                savepath = save
            };
            testImg.New("Quarters", new int[] { 28, 28 });
            Functions.Save(filename, testImg.Image.ToDouble(), false);
            runlbp.param.Mre = true;

            runlbp.CalculateSingle();
            float[,] result = Functions.Load(save + "\\" + Path.GetFileName(save).Replace(Path.GetExtension(save), "") + "_LBPIL.png");
            int[,] features = Functions.ReadCSV(save + "\\features.csv").ToInt32();

            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            int[,] reffeat = new int[1, 32]
                { { 18, 18, 0, 0, 0, 18, 0, 18, 0, 0, 0, 0, 0, 0, 3, 11, 8, 11, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 6, 20, 6} };
            Assert.Equal(refIL, result);
            Assert.Equal(reffeat, features);
        }


        [Fact]
        public void RunLBPCalculateSingle_MRE_dat_EqualsReference()
        {
            var runlbp = new RunLBP()
            {
                path = filename,
                savepath = save
            };
            testImg.New("Quarters", new int[] { 28, 28 });
            var bin = new BinaryWriterApp();
            bin.SaveLBPFeatures(testImg.Image);
            runlbp.param.Mre = true;
            runlbp.param.ImageType = ".dat";

            runlbp.CalculateSingle();
            bin.filename = save + "\\" + Path.GetFileName(save).Replace(Path.GetExtension(save), "") + "_LBPIL.png";
            float[,] result = Functions.Load(save + "\\" + Path.GetFileName(save).Replace(Path.GetExtension(save), "") + "_LBPIL.png");
            bin.ReadLBPFeatures("uint32");
            int[,] features = bin.features;

            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            int[,] reffeat = new int[1, 32]
                { { 18, 18, 0, 0, 0, 18, 0, 18, 0, 0, 0, 0, 0, 0, 3, 11, 8, 11, 3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1, 6, 20, 6} };

            Assert.Equal(refIL, result);
            Assert.Equal(reffeat, features);
        }

        [Fact]
        public void RunLBPCalculateBatch_MRE_png_EqualsReference()
        {
            var runlbp = new RunLBP()
            {
                path = filename,
                savepath = save
            };
            runlbp.param.Mre = true;
            testImg.New("Quarters", new int[] { 28, 28 });
            Functions.Save(save + @"\Test1.png", testImg.Image.ToDouble(), false);
            Functions.Save(save + @"\Test2.png", testImg.Image.ToDouble(), false);
            Functions.Save(save + @"\Test3.png", testImg.Image.ToDouble(), false);

            runlbp.CalculateBatch();
            float[,] result1 = Functions.Load(save + @"\Test1_large.png");
            float[,] result2 = Functions.Load(save + @"\Test2_large.png");
            float[,] result3 = Functions.Load(save + @"\Test3_large.png");

            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            Assert.Equal(refIL, result1);
            Assert.Equal(refIL, result2);
            Assert.Equal(refIL, result3);
        }
    }
}
