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
        public string Precision { get; set; }
        public string Method { get; set; }
        public string ImageType { get; set; }
        public double Eps1 { get; set; }
        public double Eps2 { get; set; }
        public bool Mre { get; set; }
        public bool Save { get; set; }
        public bool Scale { get; set; }
        public bool Meanstd { get; set; }

        public Parameters()
        {
            Radius = 3;
            LargeRadius = 9;
            Neighbours = 8;
            W_c = 5;
            W_r = new int[] { 5, 5 };
            Precision = "double";
            Method = "Reflect";
            ImageType = ".png";
            Eps1 = 1E-06;
            Eps2 = 1E-12;
            Mre = true;
            Save = true;
            Scale = true;
            Meanstd = false;
        }
    }
}
