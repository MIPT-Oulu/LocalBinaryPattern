﻿using System;
using System.Numerics;
using LBP.Components;
using NUnit.Framework;
using Accord.Math;

namespace LBPTesting.Tests
{
    [TestFixture]
    class BMPWriterTests
    {
        TestImage testImg = new TestImage(); // Initialize testimage function
        string filename = @"C:\Users\sarytky\Desktop\trials\Test.png";

        [Test]
        public void SaveAndRead_IntegerArray_EqualsInputArray()
        {
            testImg.New();

            Functions.Save(filename, testImg.Image.ToDouble(), false);
            float[,] readedArray = Functions.Load(filename);

            Functions.DisplayArray(testImg.Image);
            Functions.DisplayArray(readedArray);
            CollectionAssert.AreEqual(testImg.Image, readedArray);
        }

        [Test]
        public void SaveAndRead_FloatArray_EqualsInputArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.Save(filename, testImg.Image.ToDouble(), false);
            float[,] readedArray = Functions.Load(filename);

            testImg.Image = testImg.Image.Round().ToSingle(); // Round
            Functions.DisplayArray(testImg.Image);
            Functions.DisplayArray(readedArray);
            CollectionAssert.AreEqual(testImg.Image, readedArray);
        }

        [Test]
        public void SaveAndRead_FloatArrayNormalized_EqualsNormalizedArray()
        {   // Float type's precision is only 7 digits!!
            testImg.New("Add residual");

            Functions.Save(filename, testImg.Image.ToDouble(), true);
            float[,] readedArray = Functions.Load(filename);

            testImg.Image = Functions.Normalize(testImg.Image.ToDouble()).Multiply(255).Round().ToSingle(); // Normalize array
            Functions.DisplayArray(testImg.Image);
            Functions.DisplayArray(readedArray);
            CollectionAssert.AreEqual(testImg.Image, readedArray);
        }
    }
}
