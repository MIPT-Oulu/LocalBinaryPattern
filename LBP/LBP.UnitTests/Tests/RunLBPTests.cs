using System;
using System.IO;
using Xunit;
using LBPLibrary;
using Accord.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBP.UnitTests.Tests
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
        public void RunLBP_SingleImage_EqualsReference()
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

            float[,] refIL = new float[6, 6]
                {{ 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5},
            { 3, 3, 3, 5, 5, 5} };
            Assert.Equal(refIL, result);
        }
    }
}
