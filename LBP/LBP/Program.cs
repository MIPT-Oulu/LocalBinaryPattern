using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using LBP.Components;
using Microsoft.VisualBasic;

namespace LBP
{

    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var param = new Parameters(); // Default parameters

            while (true)
            {
                Display();
                var selection = Console.ReadKey(true);
                if (selection.KeyChar == '1') // Batch
                {
                    param.Mre = true;
                    Console.WriteLine("\n\nSelection: Calculate MRELBP batch\n");
                    Batch(param);
                }
                else if (selection.KeyChar == '2') // Single
                {
                    param.Mre = true;
                    Console.WriteLine("\n\nSelection: Calculate MRELB single image\n");
                    Single(param);
                }
                if (selection.KeyChar == '3') // Batch
                {
                    param.Mre = false;
                    Console.WriteLine("\n\nSelection: Calculate LBP batch\n");
                    Batch(param);
                }
                else if (selection.KeyChar == '4') // Single
                {
                    param.Mre = false;
                    Console.WriteLine("\n\nSelection: Calculate LBP single image\n");
                    Single(param);
                }
                else if (selection.KeyChar == '5') // Parameters
                {
                    Console.WriteLine("\n\nSelection: Define parameters\n");
                    ParameterForm.DefineParam(ref param);
                }
                else if (selection.KeyChar == '6') // Show default
                {
                    Console.WriteLine("\n\nSelection: Display parameters\n");
                    Displayparam(param);
                }
                else if (selection.KeyChar == '7') // Exit app
                {
                    Console.WriteLine("\n\nSelection: Exit\n");
                    System.Threading.Thread.Sleep(500); Console.Write(". ");
                    System.Threading.Thread.Sleep(500); Console.Write(". ");
                    System.Threading.Thread.Sleep(500); Console.Write(". ");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Select one of the possible operations.");
                }
            }
        }

        public static void Display()
        {
            Console.WriteLine("\nChoose operation:\n");
            Console.WriteLine("1: Calculate a batch of MRELBP images");
            Console.WriteLine("2: Calculate single MRELBP image");
            Console.WriteLine("3: Calculate a batch of LBP images");
            Console.WriteLine("4: Calculate single LBP image");
            Console.WriteLine("5: Define parameters");
            Console.WriteLine("6: Show selected parameters");
            Console.WriteLine("7: Exit\n");
            Console.Write("Select one of operations 1-7...");
        }

        public static void Displayparam(Parameters param)
        {
            Console.WriteLine("Selected parameters:\n");
            Console.WriteLine("Number of Neighbours: {0}", param.Neighbours);
            Console.WriteLine("Small radius (MRELBP) / LBP radius: {0}", param.Radius);

            Console.WriteLine("MRELBP:");
            Console.WriteLine("Large radius: {0}", param.LargeRadius);
            Console.WriteLine("Filter size (center image): {0}", param.W_c);
            Console.WriteLine("Filter size (Large image): {0}", param.W_r[0]);
            Console.WriteLine("Filter size (Small image): {0}", param.W_r[1]);
            Console.WriteLine("Image type: {0}", param.Type);
            Console.WriteLine("Loaded image type: {0}", param.ImageType);
            Console.WriteLine("Padding method: {0}", param.Method);
        }

        public static void Batch(Parameters param)
        {
            // Initialize paths
            string path = "", savepath = "";

            // Select load path
            var fbd = new FolderBrowserDialog() { Description = "Select the directory to load images" };
            if (fbd.ShowDialog() == DialogResult.OK)
                path = fbd.SelectedPath;
            else
            {
                Console.WriteLine("No directory selected.\n");
                return;
            }
                        
            // Select save path
            fbd = new FolderBrowserDialog() { Description = "Select the directory to save results" };
            if (fbd.ShowDialog() == DialogResult.OK)
                savepath = fbd.SelectedPath;
            else
            {
                Console.WriteLine("No directory selected.\n");
                return;
            }

            // Calculate batch of LBP images
            RunLBP run = new RunLBP()
            {
                path = path, // image path and result path
                savepath = savepath,
                param = param, // pipeline parameters
            };
            run.CalculateBatch();
        }

        public static void Single(Parameters param)
        {
            // Initialize paths
            string path = "", savepath = "";

            // Select load path
            var openfile = new OpenFileDialog() { Title = "Select the image to be calculated" };
            if (openfile.ShowDialog() == DialogResult.OK)
                path = openfile.FileName;
            else
            {
                Console.WriteLine("No directory selected.\n");
                return;
            }

            // Select save path
            var fbd = new FolderBrowserDialog() { Description = "Select the directory to save results" };
            if (fbd.ShowDialog() == DialogResult.OK)
                savepath = fbd.SelectedPath;
            else
            {
                Console.WriteLine("No directory selected.\n");
                return;
            }

            // Calculate single LBP image
            RunLBP run2 = new RunLBP()
            {
                path = path,
                savepath = savepath,
                param = param,
            };
            run2.CalculateSingle();
        }
    }
}
