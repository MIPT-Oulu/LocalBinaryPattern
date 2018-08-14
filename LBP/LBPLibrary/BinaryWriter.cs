using System;
using System.IO;
using Accord.Math;

namespace LBPLibrary
{
    // Read and write binary files
    public class BinaryWriterApp
    {
        public string filename;
        public int[,] features;
        public float[,] image, eigenVectors;
        public double[,] image_double;
        public int w, l, ncomp;
        public float[] singularValues;
        public double[] weights;

        public BinaryWriterApp()
        { // Working path as default
            filename = Directory.GetCurrentDirectory();
        }

        public BinaryWriterApp(string filename)
        {
            this.filename = filename;
        }

        // Save LBP features
        public void SaveLBPFeatures(int[,] array)
        {   // Save int array to binary file, width written as Int32
            w = array.GetLength(0); l = array.GetLength(1);
            using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create))) // BinaryWriter is little endian
            {
                writer.Write(w); // write array width
                // Loop to write values one by one
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        writer.Write(array[i, j]);
                    }
                }
            }
        }

        public void SaveLBPFeatures(float[,] array)
        {   // Float overload for SaveLBPFeatures
            w = array.GetLength(0); l = array.GetLength(1);
            using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create))) // BinaryWriter is little endian
            {
                writer.Write(w); // write array width
                // Loop to write values one by one
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        writer.Write(array[i, j]);
                    }
                }
            }
        }

        public void SaveLBPFeatures(double[,] array)
        {   // Double overload for SaveLBPFeatures
            w = array.GetLength(0); l = array.GetLength(1);
            using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create))) // BinaryWriter is little endian
            {
                writer.Write(w); // write array width
                // Loop to write values one by one
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        writer.Write(array[i, j]);
                    }
                }
            }
        }

        // Load LBP features
        public void ReadLBPFeatures(string precision)
        {
            if (File.Exists(filename))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filename);
                    using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                    {
                        w = reader.ReadInt32();
                        l = (bytes.Length * 8 / 32 - 1) / w;
                        if (precision == "float")
                        {
                            image = new float[w, l];
                            // Loop to read values one by one
                            for (int i = 0; i < w; i++)
                            {
                                for (int j = 0; j < l; j++)
                                {
                                    image[i, j] = reader.ReadSingle();
                                }
                            }
                        }
                        else if (precision == "double")
                        {
                            l = (bytes.Length * 8 / 64 - 1 / 2) / w;
                            image_double = new double[w, l];
                            // Loop to read values one by one
                            for (int i = 0; i < w; i++)
                            {
                                for (int j = 0; j < l; j++)
                                {
                                    image_double[i, j] = reader.ReadDouble();
                                }
                            }
                            image = image_double.ToSingle(); // Convert to single
                        }
                        else // integer type as default
                        {
                            features = new int[w, l];
                            // Loop to read values one by one
                            for (int i = 0; i < w; i++)
                            {
                                for (int j = 0; j < l; j++)
                                {
                                    features[i, j] = reader.ReadInt32();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Invalid file");
                }

            }
        }

        public void ReadLBPFeatures(string precision, string fname)
        {
            filename = fname;
            ReadLBPFeatures(precision);
        }

        public void ReadWeights()
        {
            if (File.Exists(filename))
            {
                byte[] bytes = File.ReadAllBytes(filename);
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    try
                    {
                        w = reader.ReadInt32();
                        ncomp = reader.ReadInt32();
                        // Loop to read values one by one
                        eigenVectors = new float[w, ncomp];
                        for (int i = 0; i < w; i++)
                        {
                            for (int j = 0; j < ncomp; j++)
                            {
                                eigenVectors[i, j] = reader.ReadSingle();
                            }
                        }
                        singularValues = new float[ncomp];
                        for (int i = 0; i < ncomp; i++)
                        {
                            singularValues[i] = reader.ReadSingle();
                        }
                        weights = new double[ncomp];
                        for (int i = 0; i < ncomp; i++)
                        {
                            weights[i] = reader.ReadDouble();
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Invalid file");
                    }

                }
            }
        }

        public void ReadWeights(string fname)
        {
            filename = fname;
            ReadWeights();
        }
    }
}
