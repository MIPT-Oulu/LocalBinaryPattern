using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBPLibrary
{
    public class Parameters
    {   /// 
        /// Class variable that contains necessary parameters for running LBP calculations
        /// and running the demo application. Initializes default values.
        /// 

        // Class properties
        public int Radius { get; set; }
        public int LargeRadius { get; set; }
        public int Neighbours { get; set; }
        public int W_c { get; set; }
        public int[] W_r { get; set; }
        public int[] W_stand { get; set; }
        public string Precision { get; set; }
        public string Method { get; set; }
        public string ImageType { get; set; }
        public double Eps1 { get; set; }
        public double Eps2 { get; set; }
        public bool Mre { get; set; }
        public bool Stand { get; set; }
        public bool Save { get; set; }
        public bool Scale { get; set; }
        public bool Meanstd { get; set; }

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
            Meanstd = false;
        }
    }
}
