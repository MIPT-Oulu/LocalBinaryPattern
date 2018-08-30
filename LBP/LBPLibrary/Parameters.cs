using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBPLibrary
{
    /// <summary>
    /// Class variable that contains necessary parameters for running LBP calculations
    /// and running the demo application. Initializes default values.
    /// </summary>
    public class Parameters
    {   
        /// <summary>
        /// LBP radius or small radius on MRELBP.
        /// </summary>
        public int Radius { get; set; }
        /// <summary>
        /// Large radius used in MRELBP.
        /// </summary>
        public int LargeRadius { get; set; }
        /// <summary>
        /// Number of LBP neighbours.
        /// </summary>
        public int Neighbours { get; set; }
        /// <summary>
        /// Kernel size for median filter on center image.
        /// </summary>
        public int W_c { get; set; }
        /// <summary>
        /// Kernel size for median filter on small [0] and large [1] image.
        /// </summary>
        public int[] W_r { get; set; }
        /// <summary>
        /// Parameters for grayscale standardization. See also class LocalStandardization.
        /// </summary>
        /// <seealso cref="LocalStandardization"/>
        public int[] W_stand { get; set; }
        /// <summary>
        /// Option to use "double" or "float" precision.
        /// </summary>
        public string Precision { get; set; }
        /// <summary>
        /// Padding method. See method ArrayPadding.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Extension to be used when loading image files.
        /// </summary>
        public string ImageType { get; set; }
        /// <summary>
        /// Error residual.
        /// </summary>
        public double Eps1 { get; set; }
        /// <summary>
        /// Error residual.
        /// </summary>
        public double Eps2 { get; set; }
        /// <summary>
        /// Option to use Median Robust Extended LBP.
        /// </summary>
        public bool Mre { get; set; }
        /// <summary>
        /// Option to use Grayscale normalization. See also class LocalStandardization.
        /// </summary>
        public bool Stand { get; set; }
        /// <summary>
        /// Option to save LBP images.
        /// </summary>
        public bool Save { get; set; }
        /// <summary>
        /// Option to scale images from 0 to 255.
        /// </summary>
        public bool Scale { get; set; }
        /// <summary>
        /// Option to use mean and standard deviation images in LBP calculation.
        /// </summary>
        public bool Meanstd { get; set; }

        /// <summary>
        /// Initialize default parameters.
        /// </summary>
        public Parameters()
        {
            // LBP
            Radius = 3;
            LargeRadius = 9;
            Neighbours = 8;
            // Filter weights
            W_c = 5; 
            W_r = new int[] { 5, 5 };
            W_stand = new int[] { 23, 5, 5, 1 }; // normalization (kernel size 1, 2 and sigma 1, 2)
            // Methods
            Precision = "double";
            Method = "Reflect";
            ImageType = ".png";
            // Error
            Eps1 = 1E-06;
            Eps2 = 1E-12;
            //MRELBP
            Mre = true;
            // Standardization
            Stand = true;
            // Save
            Save = true;
            Scale = true;
            // Load
            Meanstd = true;
        }
    }
}
