using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using Xunit;

namespace LBP.UnitTests
{
    public class StandardizationTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        [Fact]
        public void Stand_DefaultInput_OutputsDefaultParameters()
        {
            var stand = new LocalStandardization();

            // Write more assertions
            Assert.Equal(23, stand.w1);
            Assert.Equal(5, stand.w2);
            Assert.Equal(5, stand.s1);
            Assert.Equal(1, stand.s2);
        }

        [Fact]
        public void Stand_SmallKernels_Equalsreference()
        {   
            testImg.New("Quarters", new int[] { 6, 6 });
            var stand = new LocalStandardization() { w1 = 5, w2 = 3};

            double[,] image = testImg.Image.ToDouble();
            stand.Standardize(ref image, "Reflect");

            double[,] refimage = new double[6, 6]
                {{ 0, -0.713520621472151, -1.0860460420492704, 1.1431629051894177, 0.84311553277412787, -2.2355540831313907E-15},
            { -0.51110271561126808, -0.858138042569251, -1.2191098967853371, 0.88522909002589645, 0.50327268512720325, -0.79096598944498153},
            { -0.90778462979852093, -1.1519031239765429, -1.4403470961301064, 0.52956642547784116, -0.023010004835671567, -1.0641502996862582},
            { 1.0641502996862577, 0.023010004835671564, -0.52956642547784061, 1.4403470961301061, 1.1519031239765429, 0.90778462979852015},
            { 0.79096598944498064, -0.50327268512720447, -0.88522909002589667, 1.2191098967853371, 0.85813804256925119, 0.51110271561126719},
            { 0, -0.84311553277412821, -1.1431629051894172, 1.0860460420492706, 0.71352062147215034, 0} };
            Assert.Equal(image, refimage);
        }

        [Fact]
        public void Stand_SmallKernelsOverride_Equalsreference()
        {
            testImg.New("Quarters", new int[] { 6, 6 });
            var stand = new LocalStandardization(5, 3, 5, 1);

            double[,] image = testImg.Image.ToDouble();
            stand.Standardize(ref image, "Reflect");

            double[,] refimage = new double[6, 6]
                {{ 0, -0.713520621472151, -1.0860460420492704, 1.1431629051894177, 0.84311553277412787, -2.2355540831313907E-15},
            { -0.51110271561126808, -0.858138042569251, -1.2191098967853371, 0.88522909002589645, 0.50327268512720325, -0.79096598944498153},
            { -0.90778462979852093, -1.1519031239765429, -1.4403470961301064, 0.52956642547784116, -0.023010004835671567, -1.0641502996862582},
            { 1.0641502996862577, 0.023010004835671564, -0.52956642547784061, 1.4403470961301061, 1.1519031239765429, 0.90778462979852015},
            { 0.79096598944498064, -0.50327268512720447, -0.88522909002589667, 1.2191098967853371, 0.85813804256925119, 0.51110271561126719},
            { 0, -0.84311553277412821, -1.1431629051894172, 1.0860460420492706, 0.71352062147215034, 0} };
            Assert.Equal(image, refimage);
        }
    }
}
