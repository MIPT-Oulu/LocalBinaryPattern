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
        string load = @"C:\temp\test\load";
        string save = @"C:\temp\test\save";

        public RunLBPTests()
        {
            Directory.CreateDirectory(@"C:\temp\test\load");
            Directory.CreateDirectory(@"C:\temp\test\save");
        }

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
            var runlbp = new RunLBP(load + @"\Test7\Test1.png", save + @"\Test7");
            Directory.CreateDirectory(@"C:\temp\test\load\Test7");
            Directory.CreateDirectory(@"C:\temp\test\save\Test7");

            testImg.New("Quarters", new int[] { 28, 28 });
            Functions.Save(runlbp.path, testImg.Image.ToDouble(), false);
            runlbp.param.Mre = true;
            runlbp.param.Scale = false;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1 };
            runlbp.param.ImageType = ".png";
            Functions.Save(load + @"\Test7\Test1.png", testImg.Image.ToDouble(), false);

            runlbp.CalculateSingle();
            float[,] result = Functions.Load(save + @"\Test7\Test1_LBPIS.png");
            int[,] features = Functions.ReadCSV(save + @"\Test7\features.csv").ToInt32();

            float[,] refIS = new float[6, 6]
                {{ 9, 7, 6, 6, 6, 1},
            { 8, 1, 9, 6, 6, 1},
            { 6, 2, 3, 6, 3, 1},
            { 7, 5, 2, 5, 6, 2},
            { 7, 2, 2, 9, 7, 8},
            { 7, 2, 2, 2, 1, 9} };
            Assert.Equal(refIS, result);
        }

        [Fact]
        public void RunLBPCalculateSingle_LBP_png_EqualsReference()
        {
            var runlbp = new RunLBP()
            {
                path = load + @"\Test1.png",
                savepath = save
            };
            testImg.New("Quarters", new int[] { 12, 12 });
            Functions.Save(runlbp.path, testImg.Image.ToDouble(), false);
            runlbp.param.Mre = false;
            runlbp.param.Scale = false;
            runlbp.param.ImageType = ".png";

            runlbp.CalculateSingle();
            float[,] result = Functions.Load(save + @"\Test1_LBP.png");
            int[,] features = Functions.ReadCSV(save + @"\features.csv").ToInt32();

            float[,] refIS = new float[6, 6]
                {{ 8, 8, 8, 5, 5, 5},
            { 8, 8, 8, 5, 5, 6},
            { 8, 8, 8, 5, 5, 6},
            { 5, 6, 6, 3, 3, 3},
            { 5, 6, 6, 3, 3, 3},
            { 6, 6, 6, 3, 3, 3} };
            Assert.Equal(refIS, result);
        }


        [Fact]
        public void RunLBPCalculateSingle_MRE_dat_EqualsReference()
        {
            var runlbp = new RunLBP(load + @"\Test6\Test1.dat", save + @"\Test6");
            Directory.CreateDirectory(@"C:\temp\test\load\Test6");
            Directory.CreateDirectory(@"C:\temp\test\save\Test6");

            testImg.New("Quarters", new int[] { 28, 28 });
            var bin = new BinaryWriterApp() { filename = load + @"\Test6\Test1.dat" };
            bin.SaveBinary(testImg.Image.ToDouble());
            runlbp.param.Mre = true;
            runlbp.param.Scale = false;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1 };
            runlbp.param.ImageType = ".dat";

            runlbp.CalculateSingle();
            bin.filename = save + @"\Test6\features.dat";
            float[,] result = Functions.Load(save + @"\Test6\Test1_LBPIS.png");
            bin.ReadLBPFeatures("uint32");
            int[,] features = bin.features;

            float[,] refIS = new float[6, 6]
                {{ 9, 7, 6, 6, 6, 1},
            { 8, 1, 9, 6, 6, 1},
            { 6, 2, 3, 6, 3, 1},
            { 7, 5, 2, 5, 6, 2},
            { 7, 2, 2, 9, 7, 8},
            { 7, 2, 2, 2, 1, 9} };
            Assert.Equal(refIS, result);
        }

        [Fact]
        public void RunLBPCalculateBatch_MRE_png_EqualsReference()
        {
            var runlbp = new RunLBP(load + @"\Test5", save + @"\Test5");
            Directory.CreateDirectory(@"C:\temp\test\load\Test5");
            Directory.CreateDirectory(@"C:\temp\test\save\Test5");

            runlbp.param.Mre = true;
            runlbp.param.Scale = false;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1};
            testImg.New("Quarters", new int[] { 28, 28 });
            Functions.Save(load + @"\Test5\Test1.png", testImg.Image.ToDouble(), false);
            Functions.Save(load + @"\Test5\Test2.png", testImg.Image.ToDouble(), false);
            Functions.Save(load + @"\Test5\Test3.png", testImg.Image.ToDouble(), false);

            runlbp.CalculateBatch();
            float[,] result1 = Functions.Load(save + @"\Test5\Test1_small.png");
            float[,] result2 = Functions.Load(save + @"\Test5\Test2_small.png");
            float[,] result3 = Functions.Load(save + @"\Test5\Test3_small.png");

            float[,] refIS = new float[6, 6]
                {{ 9, 7, 6, 6, 6, 1},
            { 8, 1, 9, 6, 6, 1},
            { 6, 2, 3, 6, 3, 1},
            { 7, 5, 2, 5, 6, 2},
            { 7, 2, 2, 9, 7, 8},
            { 7, 2, 2, 2, 1, 9} };
            Assert.Equal(refIS, result1);
            Assert.Equal(refIS, result2);
            Assert.Equal(refIS, result3);
        }

        [Fact]
        public void RunLBPCalculateBatch_LBP_dat_EqualsReference()
        {

            var runlbp = new RunLBP(load + @"\Test1", save + @"\Test1");
            Directory.CreateDirectory(@"C:\temp\test\load\Test1");
            Directory.CreateDirectory(@"C:\temp\test\save\Test1");

            runlbp.param.Mre = false;
            runlbp.param.Scale = false;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1 };
            runlbp.param.ImageType = ".dat";
            // save images
            testImg.New("Quarters", new int[] { 12, 12 });
            var bin = new BinaryWriterApp() { filename = load + @"\Test1\Test4.dat" };
            bin.SaveBinary(testImg.Image.ToDouble());
            bin.filename = load + @"\Test1\Test5.dat";
            bin.SaveBinary(testImg.Image.ToDouble());
            bin.filename = load + @"\Test1\Test6.dat";
            bin.SaveBinary(testImg.Image.ToDouble());

            runlbp.CalculateBatch();
            float[,] result1 = Functions.Load(save + @"\Test1\Test4_LBP.png");
            float[,] result2 = Functions.Load(save + @"\Test1\Test5_LBP.png");
            float[,] result3 = Functions.Load(save + @"\Test1\Test6_LBP.png");

            float[,] refIS = new float[6, 6]
                {{ 8, 8, 8, 5, 5, 5},
            { 8, 8, 8, 5, 5, 6},
            { 8, 8, 8, 5, 5, 6},
            { 5, 6, 6, 3, 3, 3},
            { 5, 6, 6, 3, 3, 3},
            { 6, 6, 6, 3, 3, 3} };
            Assert.Equal(refIS, result1);
            Assert.Equal(refIS, result2);
            Assert.Equal(refIS, result3);
        }

        [Fact]
        public void RunLBPCalculateBatch_LBP_png_EqualsReference()
        {

            var runlbp = new RunLBP()
            {
                path = load + @"\Test3",
                savepath = save + @"\Test3"
            };
            Directory.CreateDirectory(@"C:\temp\test\load\Test3");
            Directory.CreateDirectory(@"C:\temp\test\save\Test3");

            runlbp.param.Mre = true;
            runlbp.param.Scale = false;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1 };
            testImg.New("Quarters", new int[] { 28, 28 });
            Functions.Save(load + @"\Test3\Test1.png", testImg.Image.ToDouble(), false);
            Functions.Save(load + @"\Test3\Test2.png", testImg.Image.ToDouble(), false);
            Functions.Save(load + @"\Test3\Test3.png", testImg.Image.ToDouble(), false);

            runlbp.CalculateBatch();
            float[,] result1 = Functions.Load(save + @"\Test3\Test1_small.png");
            float[,] result2 = Functions.Load(save + @"\Test3\Test2_small.png");
            float[,] result3 = Functions.Load(save + @"\Test3\Test3_small.png");

            float[,] refIS = new float[6, 6]
                {{ 9, 7, 6, 6, 6, 1},
            { 8, 1, 9, 6, 6, 1},
            { 6, 2, 3, 6, 3, 1},
            { 7, 5, 2, 5, 6, 2},
            { 7, 2, 2, 9, 7, 8},
            { 7, 2, 2, 2, 1, 9} };
            Assert.Equal(refIS, result1);
            Assert.Equal(refIS, result2);
            Assert.Equal(refIS, result3);
        }

        [Fact]
        public void RunLBPCalculateBatch_LBP_datOverride_EqualsReference()
        {
            var runlbp = new RunLBP(load + @"\Test2", save + @"\Test2");
            Directory.CreateDirectory(@"C:\temp\test\load\Test2");
            Directory.CreateDirectory(@"C:\temp\test\save\Test2");

            runlbp.param.Mre = false;
            runlbp.param.Scale = false;
            runlbp.param.Meanstd = true;
            runlbp.param.W_stand = new int[] { 5, 3, 2, 1 };
            runlbp.param.ImageType = ".dat";
            // save images
            testImg.New("Quarters", new int[] { 12, 12 });
            var bin = new BinaryWriterApp() { filename = load + @"\Test2\Test4.dat" };
            bin.SaveBinary(testImg.Image.ToDouble());
            bin.filename = load + @"\Test2\Test5.dat";
            bin.SaveBinary(testImg.Image.ToDouble());

            runlbp.CalculateBatch();
            float[,] result1 = Functions.Load(save + @"\Test2\Test4_LBP.png");

            float[,] refIS = new float[6, 6]
                {{ 8, 8, 8, 5, 5, 5},
            { 8, 8, 8, 5, 5, 6},
            { 8, 8, 8, 5, 5, 6},
            { 5, 6, 6, 3, 3, 3},
            { 5, 6, 6, 3, 3, 3},
            { 6, 6, 6, 3, 3, 3} };
            Assert.Equal(refIS, result1);
        }
    }
}
