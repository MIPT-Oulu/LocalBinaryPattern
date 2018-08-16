using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accord.Math;

namespace LBPLibrary
{
    /// <summary>
    /// Class that is used to call different LBP pipelines for single image
    /// or image batches. Takes in the Parameters class variable.
    /// </summary>
    public class RunLBP
    {   

        public string path, savepath, meanpath, stdpath;
        public Parameters param;

        int[] f; int[,] features = new int[0, 0];
        BinaryWriterApp lbpreader = new BinaryWriterApp();

        public RunLBP()
        {
            param = new Parameters();
            path = Directory.GetCurrentDirectory();
            savepath = Directory.GetCurrentDirectory();
        }

        public RunLBP(string dir, string savedir)
        {
            param = new Parameters();
            path = dir;
            savepath = savedir;
        }

        /// <summary>
        /// Calculate single LBP image. For LBP image, path and savepath should be defined first.
        /// For MRELBP image meanpath, stdpath and savepath should be defined first.
        /// </summary>
        public void CalculateSingle()
        {
            // Timer
            var time = Stopwatch.StartNew();

            // Load image
            double[,] image, imagemean, imagestd;
            if (param.Meanstd)
            {
                if (param.ImageType == ".dat" && meanpath.EndsWith(".dat") && stdpath.EndsWith(".dat"))
                {
                    lbpreader.ReadLBPFeatures(param.Precision, meanpath); // Read binary image
                    imagemean = lbpreader.image_double;
                    lbpreader.ReadLBPFeatures(param.Precision, stdpath); // Read binary image
                    imagestd = lbpreader.image_double;
                    image = imagemean.Add(imagestd); // Combine mean and std images
                }
                else if ((meanpath.EndsWith(".png") && stdpath.EndsWith(".png")) || (meanpath.EndsWith(".bmp") && stdpath.EndsWith(".png")))
                {
                    imagemean = Functions.Load(meanpath).ToDouble();
                    imagestd = Functions.Load(stdpath).ToDouble();
                    image = imagemean.Add(imagestd); // Combine mean and std images
                }
                else
                {
                    Console.WriteLine("One of image types not compatible.\n");
                    return;
                }
            }
            else
            {
                if (param.ImageType == ".dat" && path.EndsWith(".dat"))
                {
                    lbpreader.ReadLBPFeatures(param.Precision, path); // Read binary image
                    image = lbpreader.image_double;
                }
                else if (path.EndsWith(".png") || path.EndsWith(".bmp"))
                    image = Functions.Load(path).ToDouble();
                else
                {
                    Console.WriteLine("Image type not compatible.\n");
                    return;
                }
            }
            

            // LBP
            if (param.Mre)
            {
                if (param.Stand)
                {
                    // Grayscale normalization (weights and sigmas from parameters class)
                    var standrd = new LocalStandardization(param.W_stand[0], param.W_stand[1], param.W_stand[2], param.W_stand[3]);
                    standrd.Standardize(ref image, "Reflect"); // standardize given image
                }

                // Run MRELBP
                Console.WriteLine("\nRunning MRELBP:\n");
                LBPApplication.PipelineMRELBP(image, param,
                    out double[,] LBPIL, out double[,] LBPIS, out double[,] LBPIR, out int[] histL, out int[] histS, out int[] histR, out int[] histCenter);

                if (param.Meanstd) // update save path for mean and std images
                    path = meanpath;
                if (param.Save) // Save images
                {
                    Functions.Save(savepath + "\\" + Path.GetFileName(path).Replace(Path.GetExtension(path), "") + "_LBPIL.png", LBPIL, param.Scale);
                    Functions.Save(savepath + "\\" + Path.GetFileName(path).Replace(Path.GetExtension(path), "") + "_LBPIS.png", LBPIS, param.Scale);
                    Functions.Save(savepath + "\\" + Path.GetFileName(path).Replace(Path.GetExtension(path), "") + "_LBPIR.png", LBPIR, param.Scale);
                }

                // Concatenate histograms
                f = Matrix.Concatenate(histCenter, Matrix.Concatenate(histL, Matrix.Concatenate(histS, histR)));
            }
            else
            {
                // Run LBP
                Console.WriteLine("\nRunning LBP:\n");
                LBPApplication.PipelineLBP(image, param,
                    out double[,] LBPresult, out int[] LBPhistogram); // Get LBP for test image;

                if (param.Save) // Save images
                    Functions.Save(savepath + "\\" + Path.GetFileName(path).Replace(Path.GetExtension(path), "") + "_LBP.png", LBPresult, param.Scale);

                f = LBPhistogram;
            }

            // Results
            features = Matrix.Concatenate(features, f);

            // Write features to csv
            Functions.WriteCSV(features.ToSingle(), savepath + "\\features.csv");

            // Write features to binary file
            var binwriter = new BinaryWriterApp() { filename = savepath + "\\features.dat" };
            binwriter.SaveLBPFeatures(features);

            Console.WriteLine("LBP images calculated and results saved.\nElapsed time: {0}min {1}sec", time.Elapsed.Minutes, time.Elapsed.Seconds);
            time.Stop();
        }

        /// <summary>
        /// Calculate all images from given path directory with selected extension.
        /// Savepath sould also be defined.
        /// </summary>
        public void CalculateBatch()
        {
            // Timer
            var time = Stopwatch.StartNew();
            var timeFull = Stopwatch.StartNew();

            // Get files from sample directory
            string[] dir;
            if (param.ImageType == ".dat")
                dir = Directory.GetFiles(path, "*.dat");
            else
                dir = Directory.GetFiles(path, "*.png");
            Array.Sort(dir);

            if (param.Mre)
            {
                // Loop for calculating MRELBP for whole dataset
                for (int k = 0; k < dir.Length; k++)
                {
                    // Load images
                    double[,] imagemean, imagestd, image = null;
                    if (param.ImageType == ".dat" && param.Meanstd)
                    {
                        lbpreader.ReadLBPFeatures(param.Precision, dir[k]); // Read binary mean image
                        imagemean = lbpreader.image_double;
                        k++;
                        lbpreader.ReadLBPFeatures(param.Precision, dir[k]); // Read binary std image
                        imagestd = lbpreader.image_double;
                        image = imagemean.Add(imagestd); // Combine mean and std images
                    }
                    else if (param.Meanstd) // Check whether to sum two adjacent images
                    {
                        imagemean = Functions.Load(dir[k]).ToDouble();
                        k++;
                        imagestd = Functions.Load(dir[k]).ToDouble();
                        image = imagemean.Add(imagestd); // Combine mean and std images
                    }
                    else if (param.ImageType == ".dat" && !param.Meanstd) // Don't combine images
                    {
                        lbpreader.ReadLBPFeatures(param.Precision, dir[k]); // Read binary mean image
                        image = lbpreader.image_double;
                    }
                    else if (!param.Meanstd)
                    {
                        image = Functions.Load(dir[k]).ToDouble();
                    }

                    if (param.Stand)
                    {
                        // Grayscale normalization (weights and sigmas from parameters class)
                        var standrd = new LocalStandardization(param.W_stand[0], param.W_stand[1], param.W_stand[2], param.W_stand[3]);
                        standrd.Standardize(ref image, "Reflect"); // standardize given image
                    }

                    // Calculate MRELBP
                    LBPApplication.PipelineMRELBP(image, param,
                    out double[,] LBPIL, out double[,] LBPIS, out double[,] LBPIR, out int[] histL, out int[] histS, out int[] histR, out int[] histCenter);

                    // Concatenate histograms
                    f = Matrix.Concatenate(histCenter, Matrix.Concatenate(histL, Matrix.Concatenate(histS, histR)));
                    features = Matrix.Concatenate(features, f);

                    if (param.Save) // Save LBP image
                    {
                        Functions.Save(savepath + "\\" + Path.GetFileName(dir[k]).Replace(Path.GetExtension(dir[k]), "") + "_small.png", LBPIS, param.Scale);
                        Functions.Save(savepath + "\\" + Path.GetFileName(dir[k]).Replace(Path.GetExtension(dir[k]), "") + "_large.png", LBPIL, param.Scale);
                        Functions.Save(savepath + "\\" + Path.GetFileName(dir[k]).Replace(Path.GetExtension(dir[k]), "") + "_radial.png", LBPIR, param.Scale);
                    }

                    Console.WriteLine("Image: {0}, elapsed time: {1}ms", dir[k], time.ElapsedMilliseconds);
                    time.Restart();
                }
            }
            else
            {
                // Loop for calculating LBP for whole dataset
                for (int k = 0; k < dir.Length; k++)
                {
                    // Load images
                    double[,] image;
                    if (param.ImageType == ".dat")
                    {
                        lbpreader.ReadLBPFeatures(param.Precision, dir[k]); // Read binary image
                        image = lbpreader.image_double;
                    }
                    else
                    {
                        image = Functions.Load(dir[k]).ToDouble(); // Read png or bmp image
                    }
                    LBPApplication.PipelineLBP(image, param,
                        out double[,] LBPresult, out int[] LBPhistogram); // Get LBP for test image;

                    if (param.Save) // Save images
                        Functions.Save(savepath + "\\" + Path.GetFileName(dir[k]).Replace(Path.GetExtension(dir[k]), "") + "_LBP.png", LBPresult, param.Scale);

                    Console.WriteLine("Image: {0}, elapsed time: {1}ms", dir[k], time.ElapsedMilliseconds);
                    time.Restart();
                }
            }


            // Write features to csv
            Functions.WriteCSV(features.ToSingle(), savepath + "\\features.csv");

            // Write features to binary file
            var binwriter = new BinaryWriterApp() { filename = savepath + "\\features.dat" };
            binwriter.SaveLBPFeatures(features);

            Console.WriteLine("All LBP images calculated and results saved.\nElapsed time: {0}min {1}sec", timeFull.Elapsed.Minutes, timeFull.Elapsed.Seconds);
            time.Stop(); timeFull.Stop();
        }
    }
}
