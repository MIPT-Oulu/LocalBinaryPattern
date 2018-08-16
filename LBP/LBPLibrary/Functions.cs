using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;

using Accord.Math;

namespace LBPLibrary
{
    /// <summary>
    /// Contains utility functions that are used in calculating LBP images.
    /// </summary>
    public class Functions
    {   

        // Convert bitmap to float[,]
        public static float[,] BitmapToFloatMatrix(Bitmap image)
        {
            float[,] convMatrix = new float[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    string str = newColor.R.ToString();
                    convMatrix[x, y] = Convert.ToSingle(str);
                }
            }
            return convMatrix;
        }

        // Convert bitmap to byte[,]
        public static byte[,] BitmapToByteMatrix(Bitmap image)
        {
            byte[,] convMatrix = new byte[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    string str = newColor.R.ToString();
                    convMatrix[x, y] = Convert.ToByte(str);
                }
            }
            image.Dispose(); // Delete previous image
            return convMatrix;
        }

        //Convert byte[,] to bitmap
        public static Bitmap ByteMatrixToBitmap(byte[,] matrix)
        {   /// Converts byte arrays to bitmap images.

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format8bppIndexed;

            // Create image
            Bitmap img = new Bitmap(matrix.GetLength(0), matrix.GetLength(1), PixelFormat.Format8bppIndexed);

            // Lock bits
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData imgData = img.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Image metadata
            int stride = imgData.Stride; // Width of the row, rounded up to four-byte boundary. Sign tells order of rows.
            int numBytes = imgData.Stride * img.Height;
            Int64 scan0 = imgData.Scan0.ToInt64(); // First line as 64bit integer
            Int32 newDataWidth = ((Image.GetPixelFormatSize(pxf) * img.Width) + 7) / 8; // Image width in bits

            // Set matrix values row by row
            for (Int32 y = 0; y < img.Height; y++) // 32bit integer includes all four channels
                Marshal.Copy(Matrix.GetColumn(matrix, y), 0, new IntPtr(scan0 + y * stride), newDataWidth); // Columns and rows are mixed in Matrix class

            // Unlock the bits.
            img.UnlockBits(imgData);

            // Define grayscale palette
            ColorPalette _palette = img.Palette;
            Color[] _entries = _palette.Entries;
            for (int i = 0; i < 256; i++)
            {
                Color b = new Color();
                b = Color.FromArgb(i, i, i);
                _entries[i] = b;
            }
            img.Palette = _palette;

            return img;
        }

        // Convert 2D array to vector
        public static T[] ArrayToVector<T>(T[,] array)
        {   /// Transform image into vector
            T[] vector = new T[array.GetLength(0) * array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    vector[i + array.GetLength(0) * j] = array[i, j];
                }
            }
            return vector;
        }

        // Convert vector to 2D array
        public static T[,] VectorToArray<T>(T[] vector, int width)
        {   /// Transform image into vector
            T[,] array;
            if (vector.Length % width == 0)
            {
                array = new T[width, vector.Length / width];
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        array[i, j] = vector[i + array.GetLength(0) * j];
                    }
                }
            }
            else
                throw new Exception("Array width not compatible!");
            return array;
        }

        /// <summary>
        /// Get submatrix from 2D matrix.
        /// Gets smaller submatrix using given limit indices.
        /// Equal to Matlab operation matrix(ylim1:ylim2, xlim1:xlim2)
        /// </summary>
        public static T[,] GetSubMatrix<T>(T[,] matrix, int xlim1, int xlim2, int ylim1, int ylim2)
        {   
            if (xlim2 - xlim1 + 1 == matrix.GetLength(0) && ylim2 - ylim1 + 1 == matrix.GetLength(1))
            {
                Console.WriteLine("Array was not cropped");
                return matrix;
            }
            if (xlim1 > xlim2 || ylim1 > ylim2)
                throw new Exception("Limits not compatible!");
            try
            {
                T[,] smallMatrix = new T[xlim2 - xlim1 + 1, ylim2 - ylim1 + 1];
                for (int i = xlim1; i <= xlim2; i++)
                {
                    for (int j = ylim1; j <= ylim2; j++)
                    {
                        smallMatrix[i - xlim1, j - ylim1] = matrix[i, j];
                    }
                }
                return smallMatrix;
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Limits exceed the length of input array");
            }
        }

        // Normalize array
        public static double[,] Normalize(double[,] array)
        {
            double[,] norm_array = array.Subtract(array.Min()).Multiply(1 / (array.Max() - array.Min())); ;
            return norm_array;
        }

        /// <summary>
        /// Pads image by extending border pixels.
        /// Give "Reflect" as method string if you want to reflect array borders.
        /// Nearest = extend edge values
        /// </summary>
        public static T[,] ArrayPadding<T>(T[,] array, int padding, string method)
        {   

            // Zero padding (default)

            int w = array.GetLength(0);
            int l = array.GetLength(1);
            T[,] paddedImage = new T[w + 2 * padding, l + 2 * padding];

            // Fill center of array
            for (int i = padding; i < w + padding; i++)
            {
                for (int j = padding; j < l + padding; j++)
                {
                    paddedImage[i, j] = array[i - padding, j - padding];
                }
            }

            if (method == "Nearest")
            {
                // Top and bottom borders
                T[] top = Matrix.GetColumn(array, 0);
                T[] bottom = Matrix.GetColumn(array, array.GetLength(1) - 1);
                T[] left = Matrix.GetRow(array, 0);
                T[] right = Matrix.GetRow(array, array.GetLength(0) - 1);

                // Loop for border values
                for (int i = 0; i < paddedImage.GetLength(0); i++)
                {
                    for (int j = 0; j < paddedImage.GetLength(1); j++)
                    {
                        if (j < padding && i >= padding && i < padding + w) // top row
                        {
                            paddedImage[i, j] = top[i - padding];
                        }
                        else if (j >= padding + l && i >= padding && i < padding + w) // bottom row
                        {
                            paddedImage[i, j] = bottom[i - padding];
                        }
                        else if (i < padding && j >= padding && j < padding + l) // left column
                        {
                            paddedImage[i, j] = left[j - padding];
                        }
                        else if (i >= padding + w && j >= padding && j < padding + l) // right column
                        {
                            paddedImage[i, j] = right[j - padding];
                        }
                        else if (i < padding && j < padding) // top left corner
                        {
                            paddedImage[i, j] = array[0, 0];
                        }
                        else if (j < padding && i >= padding + w) // top right corner
                        {
                            paddedImage[i, j] = array[w - 1, 0];
                        }
                        else if (i < padding && j >= padding + l) // bottom left corner
                        {
                            paddedImage[i, j] = array[0, l - 1];
                        }
                        else if (i >= padding + w && j >= padding + l) // bottom right corner
                        {
                            paddedImage[i, j] = array[w - 1, l - 1];
                        }
                    }
                }
            }

            if (method == "Reflect")
            {
                if (padding > w || padding > l)
                    throw new Exception("Cannot reflect over array size!");

                // Loop for border values
                for (int i = 0; i < paddedImage.GetLength(0); i++)
                {
                    for (int j = 0; j < paddedImage.GetLength(1); j++)
                    {
                        if (j < padding && i >= padding && i < padding + w) // top row
                        {
                            int top = padding - j;
                            paddedImage[i, j] = array[i - padding, top - 1];
                        }
                        else if (j >= padding + l && i >= padding && i < padding + w) // bottom row
                        {
                            int bottom = j - padding - 2 * Math.Abs(l - 1 - (j - padding));
                            paddedImage[i, j] = array[i - padding, bottom + 1];
                        }
                        else if (i < padding && j >= padding && j < padding + l) // left column
                        {
                            int left = padding - i;
                            paddedImage[i, j] = array[left - 1, j - padding];
                        }
                        else if (i >= padding + w && j >= padding && j < padding + l) // right column
                        {
                            int right = i - padding - 2 * Math.Abs(w - 1 - (i - padding));
                            paddedImage[i, j] = array[right + 1, j - padding];
                        }
                        else if (i < padding && j < padding) // top left corner
                        {
                            int top = padding - j; int left = padding - i;
                            paddedImage[i, j] = array[left - 1, top - 1];
                        }
                        else if (j < padding && i >= padding + w) // top right corner
                        {
                            int top = padding - j; int right = i - padding - 2 * Math.Abs(w - 1 - (i - padding));
                            paddedImage[i, j] = array[right + 1, top - 1];
                        }
                        else if (i < padding && j >= padding + l) // bottom left corner
                        {
                            int bottom = j - padding - 2 * Math.Abs(l - 1 - (j - padding)); int left = padding - i;
                            paddedImage[i, j] = array[left - 1, bottom + 1];
                        }
                        else if (i >= padding + w && j >= padding + l) // bottom right corner
                        {
                            int bottom = j - padding - 2 * Math.Abs(l - 1 - (j - padding)); int right = i - padding - 2 * Math.Abs(w - 1 - (i - padding));
                            paddedImage[i, j] = array[right + 1, bottom + 1];
                        }
                    }
                }
            }
            return paddedImage;
        }

        // Bilinear interpolation
        public static double Bilinear(double[,] array, double x, double y, int i, int j, double e)
        {
            double interpolated, R1, R2;
            int fx = (int)Math.Floor(x), cx = (int)Math.Ceiling(x);
            int fy = (int)Math.Floor(y), cy = (int)Math.Ceiling(y);

            double Q11 = array[i + fx, j + fy], Q21 = array[i + cx, j + fy],
                Q12 = array[i + fx, j + cy], Q22 = array[i + cx, j + cy];
            R1 = Q11 * ((cx - x) / (cx - fx + e)) + Q21 * ((x - fx) / (cx - fx + e));
            R2 = Q12 * ((cx - x) / (cx - fx + e)) + Q22 * ((x - fx) / (cx - fx + e));

            return interpolated = R1 * ((cy - y) / (cy - fy + e)) + R2 * ((y - fy) / (cy - fy + e));
        }

        // Mean
        public static double Mean(double[,] array)
        {
            double mean = array
                .Sum()
                / array.Length;
            return mean;
        }

        // Std
        public static double Std(double[,] array)
        {
            double mean = array
                .Sum()
                / array.Length;

            double std = Math.Sqrt(array
                .Subtract(mean)
                .Pow(2)
                .Sum()
                / (array.Length - 1));
            return std;
        }

        // Save bitmap image
        public static void Save(string filename, double[,] image, bool scale)
        {   /// Saves images to selected path
            
            if (scale) // scale image from 0 to 255
                image = Normalize(image).Multiply(255);
            byte[,] bytearray = image.Round().ToByte(); // Round and convert to byte
            Bitmap bmp = new Bitmap(ByteMatrixToBitmap(bytearray));
            bmp.Save(filename, ImageFormat.Png);
            bmp.Dispose(); // Delete bitmap object
        }

        // Load bitmap image
        public static float[,] Load(string filename)
        {   /// Saves images to selected path

            Bitmap bmp;
            bmp = (Bitmap)Image.FromFile(filename, true); // Load image
            float[,] image = BitmapToFloatMatrix(bmp); // Matlab LBP image
            bmp.Dispose();

            return image;
        }

        // Read CSV file
        public static float[,] ReadCSV(string filename)
        {
            float[,] array = new float[0, 0];
            foreach (string line in File.ReadLines(filename))
            {
                var commaseparated = line.Split(',');
                float[] values;
                values = new float[commaseparated.Length];
                for (int j = 0; j < commaseparated.Length; j++)
                {
                    values[j] = float.Parse(commaseparated[j], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                array = Matrix.Concatenate(array, values);
            }
            return array.Transpose();
        }

        // Write array to CSV file
        public static void WriteCSV(float[,] array, string filename)
        {
            using (StreamWriter file = new StreamWriter(filename))
            {
                int w = array.GetLength(0);
                int l = array.GetLength(1);
                
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        if ((j + 1) % l == 0) // Last value of the row, change line
                        {
                            file.Write(array[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
                            file.Write(Environment.NewLine);
                        }
                        else // Write value
                        {
                            file.Write(array[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + ","); 
                        }
                    }
                }
            }
        }

        // Convolution
        public static double[,] Convolution2D(double[,] kernel, double[,] image, string method)
        {
            int block = kernel.GetLength(0);
            int w = image.GetLength(0), l = image.GetLength(1);
            int d = (block - 1) / 2;
            double[,] im_pad = ArrayPadding(image, d, method);
            double[,] result = new double[w, l];

            if (block % 2 == 0) // Check for odd kernel
                throw new Exception("Kernel width is not odd!");
            if (d > image.GetLength(0) || d > image.GetLength(1))
                throw new Exception("Kernel radius is larger than input array!");

            Parallel.For(0, w, i =>
            {
                Parallel.For(0, l, j =>
                {
                    double[,] im = GetSubMatrix(im_pad, i, i + block - 1, j, j + block - 1);
                    result[i, j] = ArrayToVector(im
                        .Multiply(kernel))
                        .Sum();
                });
            });
            return result;
        }
    }
}