using System;
using System.IO;
using Accord.Math;

namespace LBPLibrary
{
    /// <summary>
    /// Read and write binary .dat files 
    /// </summary>
    public class BinaryWriterApp
    {
        /// <summary>
        /// Path to the file to be loaded or writed.
        /// </summary>
        public string filename;
        /// <summary>
        /// Loaded 2D integer array.
        /// </summary>
        public int[,] features;
        /// <summary>
        /// Loaded 2D float array. Eigenvectors are used in pretrained PCA with ReadWeights method.
        /// </summary>
        public float[,] image, eigenVectors;
        /// <summary>
        /// Loaded 2D double array.
        /// </summary>
        public double[,] image_double;
        /// <summary>
        /// w = array width, l = length, ncomp = number of PCA components.
        /// </summary>
        public int w, l, ncomp;
        /// <summary>
        /// Loaded 1D float vector. Sigularvalues are used in pretrained PCA with ReadWeights method.
        /// </summary>
        public float[] singularValues;
        /// <summary>
        /// Pretrained linear regression weights. Readed with ReadWeights method from .dat file.
        /// </summary>
        public double[] weights;
        /// <summary>
        /// Mean feature vector.
        /// </summary>
        public double[] mean;

        /// <summary>
        /// Defult directory (current)
        /// </summary>
        public BinaryWriterApp()
        { // Working path as default
            filename = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Set directory.
        /// </summary>
        /// <param name="filename">Directory for reading/writing</param>
        public BinaryWriterApp(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Save integer array.
        /// </summary>
        /// <param name="array">Array to be saved.</param>
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

        /// <summary>
        /// Save float array.
        /// </summary>
        /// <param name="array">Array to be saved.</param>
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

        /// <summary>
        /// Save double array.
        /// </summary>
        /// <param name="array">Array to be saved.</param>
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

        /// <summary>
        /// Read array. Include precision argument string to read "float" or "double".
        /// Pass any other string to read integer array.
        /// </summary>
        /// <param name="precision">Data type to be read. "float", "double" or "int".</param>
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

        /// <summary>
        /// Read array. Include precision argument string to read "float" or "double".
        /// Pass any other string to read integer array.
        /// </summary>
        /// <param name="precision">Data type to be read. "float", "double" or "int".</param>
        /// <param name="fname">Path to file to be read.</param>
        public void ReadLBPFeatures(string precision, string fname)
        {
            filename = fname;
            ReadLBPFeatures(precision);
        }

        /// <summary>
        /// Read pretrained model including number of components, eigenvectors,
        /// singularvalues, and regression weights.
        /// </summary>
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
                        mean = new double[w];
                        for (int i = 0; i < w; i++)
                        {
                            mean[i] = reader.ReadDouble();
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Invalid file");
                    }

                }
            }
        }

        /// <summary>
        /// Read pretrained model including number of components, eigenvectors,
        /// singularvalues, and regression weights.
        /// Include path to file.
        /// </summary>
        /// <param name="fname">Full path to .dat file.</param>
        public void ReadWeights(string fname)
        {
            filename = fname;
            ReadWeights();
        }
    }
}
