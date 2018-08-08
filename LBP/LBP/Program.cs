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
                    Console.WriteLine("\nSelection: Calculate MRELBP batch\n");
                    Batch(param);
                }
                else if (selection.KeyChar == '2') // Single
                {
                    param.Mre = true;
                    Console.WriteLine("\nSelection: Calculate MRELB single image\n");
                    Single(param);
                }
                if (selection.KeyChar == '3') // Batch
                {
                    param.Mre = false;
                    Console.WriteLine("\nSelection: Calculate LBP batch\n");
                    Batch(param);
                }
                else if (selection.KeyChar == '4') // Single
                {
                    param.Mre = false;
                    Console.WriteLine("\nSelection: Calculate LBP single image\n");
                    Single(param);
                }
                else if (selection.KeyChar == '5') // Parameters
                {
                    Console.WriteLine("\nSelection: Define parameters\n");
                    DefineParam(ref param);
                }
                else if (selection.KeyChar == '6') // Show default
                {
                    Console.WriteLine("\nSelection: Display parameters\n");
                    Displayparam(param);
                }
                else if (selection.KeyChar == '7') // Exit app
                {
                    Console.WriteLine("\nSelection: Exit\n");
                    System.Threading.Thread.Sleep(800); Console.Write(". ");
                    System.Threading.Thread.Sleep(800); Console.Write(". ");
                    System.Threading.Thread.Sleep(800); Console.Write(". ");
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

        public static void DefineParam(ref Parameters param)
        {
            Button buttonOk = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right
            };
            Button buttonCancel = new Button()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Right
            };
            Form form = new Form()
            {
                Text = "Parameters",
                ClientSize = new Size(400, 450),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false,
                AcceptButton = buttonOk,
                CancelButton = buttonCancel
            };

            Label neighbourLabel = new Label()
            {
                Text = "Set number of neighbours",
                AutoSize = true
            };
            TextBox neighbourTextBox = new TextBox(){ Text = param.Neighbours.ToString() };
            neighbourTextBox.Anchor = neighbourTextBox.Anchor | AnchorStyles.Right;

            Label radiusLabel = new Label()
            {
                Text = "Set small radius / LBP radius",
                AutoSize = true
            };
            TextBox radiusTextBox = new TextBox() { Text = param.Radius.ToString() };
            radiusTextBox.Anchor = radiusTextBox.Anchor | AnchorStyles.Right;

            Label largeRadiusLabel = new Label()
            {
                Text = "Set large radius",
                AutoSize = true
            };
            TextBox largeRadiusTextBox = new TextBox() { Text = param.LargeRadius.ToString() };
            largeRadiusTextBox.Anchor = largeRadiusTextBox.Anchor | AnchorStyles.Right;

            Label filterLabel = new Label()
            {
                Text = "Set filter size (center)",
                AutoSize = true
            };
            TextBox filterTextBox = new TextBox() { Text = param.W_c.ToString() };
            filterTextBox.Anchor = filterTextBox.Anchor | AnchorStyles.Right;

            Label filterLargeLabel = new Label()
            {
                Text = "Set filter size (large)",
                AutoSize = true
            };
            TextBox filterLargeTextBox = new TextBox() { Text = param.W_r[0].ToString() };
            filterLargeTextBox.Anchor = filterLargeTextBox.Anchor | AnchorStyles.Right;

            Label filterSmallLabel = new Label()
            {
                Text = "Set filter size (small)",
                AutoSize = true
            };
            TextBox filterSmallTextBox = new TextBox() { Text = param.W_r[1].ToString() };
            filterSmallTextBox.Anchor = filterSmallTextBox.Anchor | AnchorStyles.Right;

            Label imageLabel = new Label()
            {
                Text = "Loaded image type",
                AutoSize = true
            };
            ComboBox imageBox = new ComboBox()
            {
                Location = new Point(10, 340), Size = new Size(370, 12),
                DropDownWidth = 200, DropDownHeight = 40,
                Items = { ".dat", ".png"},
                Text = ".dat"
            };
            imageBox.Anchor = imageBox.Anchor | AnchorStyles.Right;

            var save = new RadioButton()
            {
                Location = new Point(200, 90), Size = new Size(100, 30),
                Text = "Save images",
                Checked = true
            };

            var scale = new RadioButton()
            {
                Location = new Point(200, 140), Size = new Size(100, 30),
                Text = "Scale image values",
                Checked = true
            };

            // Set size
            neighbourLabel.SetBounds(10, 20, 170, 12);
            neighbourTextBox.SetBounds(10, 40, 170, 20);
            radiusLabel.SetBounds(10, 70, 170, 12);
            radiusTextBox.SetBounds(10, 90, 170, 12);
            largeRadiusLabel.SetBounds(10, 120, 170, 12);
            largeRadiusTextBox.SetBounds(10, 140, 170, 12);
            filterLabel.SetBounds(10, 170, 170, 12);
            filterTextBox.SetBounds(10, 190, 170, 12);
            filterLargeLabel.SetBounds(10, 220, 170, 12);
            filterLargeTextBox.SetBounds(10, 240, 170, 12);
            filterSmallLabel.SetBounds(10, 270, 170, 12);
            filterSmallTextBox.SetBounds(10, 290, 170, 12);
            imageLabel.SetBounds(10, 320, 170, 12);
            buttonOk.SetBounds(230, 420, 75, 25);
            buttonCancel.SetBounds(310, 420, 75, 25);

            // Set all controls to form
            form.Controls.AddRange(new Control[] 
            { neighbourLabel, neighbourTextBox, radiusLabel, radiusTextBox, largeRadiusLabel, largeRadiusTextBox, imageLabel, imageBox, save, scale,
                filterLabel, filterTextBox, filterLargeLabel, filterLargeTextBox, filterSmallLabel, filterSmallTextBox, buttonOk, buttonCancel });

            form.ClientSize = new Size(Math.Max(300, neighbourLabel.Right + 10), form.ClientSize.Height);

            DialogResult dialogResult = form.ShowDialog();
            if (form.DialogResult == buttonOk.DialogResult)
            {
                param.Neighbours = Convert.ToInt32(neighbourTextBox.Text);
                param.Radius = Convert.ToInt32(radiusTextBox.Text);
                param.LargeRadius = Convert.ToInt32(largeRadiusTextBox.Text);
                param.W_c = Convert.ToInt32(filterTextBox.Text);
                param.W_r[0] = Convert.ToInt32(filterLargeTextBox.Text);
                param.W_r[1] = Convert.ToInt32(filterSmallTextBox.Text);
                param.ImageType = imageBox.Text;
                param.Save = save.Checked;
                param.Scale = scale.Checked;
            }
        }

        public static void Batch(Parameters param)
        {
            // Initialize paths
            string path = "", savepath = "";

            // Select load path
            var fbd = new FolderBrowserDialog() { Description = "Select the directory to load images" };
            if (fbd.ShowDialog() == DialogResult.OK)
                path = fbd.SelectedPath;

            // Select save path
            fbd = new FolderBrowserDialog() { Description = "Select the directory to save results" };
            if (fbd.ShowDialog() == DialogResult.OK)
                savepath = fbd.SelectedPath;

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

            // Select save path
            var fbd = new FolderBrowserDialog() { Description = "Select the directory to save results" };
            if (fbd.ShowDialog() == DialogResult.OK)
                savepath = fbd.SelectedPath;

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
