using System;
using LBPLibrary;
using LBP.UnitTests;
using Accord.Math;
using Xunit;

namespace LBP.UnitTests
{
    public class ArrayTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function

        // Array to vector conversions

        [Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void ArrayToVector_ConvertBack_EqualsInputArray(string pattern)
        {   /// It's actually better to use default conversion functions from System namespace.
            testImg.New(pattern);

            float[] vector = Functions.ArrayToVector(testImg.Image);
            float[,] array = Functions.VectorToArray(vector, testImg.Image.GetLength(0));

            //Functions.DisplayArray(testImg.Image);
            //Console.WriteLine("Vector\n");
            //Functions.DisplayVector(vector);
            //Console.WriteLine("Result\n");
            //Functions.DisplayArray(array);
            Assert.Equal(testImg.Image, array);
        }

        // Submatrix tests

        [Xunit.Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void GetSubMatrix_GetFullArray_EqualsInputArray(string pattern)
        {
            testImg.New(pattern);
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);

            float[,] subMatrix = Functions.GetSubMatrix(testImg.Image, 0, w - 1, 0, l - 1);

           Assert.Equal(testImg.Image, subMatrix);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Quarters")]
        [InlineData("Ones")]
        [InlineData("Running numbers")]
        public void GetSubMatrix_GetFirstindex_EqualsInputArrayFirstIndex(string pattern)
        {
            testImg.New(pattern);

            float[,] subMatrix = Functions.GetSubMatrix(testImg.Image, 0, 0, 0, 0);
            int w = subMatrix.GetLength(0), l = subMatrix.GetLength(1);

            Assert.Equal(1, w);
            Assert.Equal(1, l);
            Assert.Equal(testImg.Image[0, 0], subMatrix[w - 1, l - 1]);
        }

        [Fact]
        public void GetSubMatrix_MiddleArray_EqualsReference()
        {
            string pattern = "Quarters";
            testImg.New(pattern);
            int w = testImg.Image.GetLength(0), l = testImg.Image.GetLength(1);

            int w1 = (int)Math.Floor((double)w / 4), w2 = (int)Math.Floor((double)3 * w / 4);
            int l1 = (int)Math.Floor((double)l / 4), l2 = (int)Math.Floor((double)3 * l / 4);
            float[,] subMatrix = Functions.GetSubMatrix(testImg.Image, w1, w2, l1, l2);
            int ww = subMatrix.GetLength(0), ll = subMatrix.GetLength(1);
            testImg.New(pattern, new int[] { 6, 9 }); // Test if possible also to use smaller matrix

            float[,] refArray = new float[6, 9] 
            { { 1, 1, 1, 1, 3, 3, 3, 3, 3 },
            { 1, 1, 1, 1, 3, 3, 3, 3, 3 },
            { 1, 1, 1, 1, 3, 3, 3, 3, 3 },
            { 2, 2, 2, 2, 4, 4, 4, 4, 4 },
            { 2, 2, 2, 2, 4, 4, 4, 4, 4 },
            { 2, 2, 2, 2, 4, 4, 4, 4, 4 }};

            Assert.Equal(6, ww);
            Assert.Equal(9, ll);
            Assert.Equal(refArray, subMatrix);
            Assert.Equal(subMatrix, testImg.Image);
        }
    }
}
