using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.InformationalServices
{
    public struct Pixel : IComparable<Pixel>
    {
        public byte Green { get; set; }
        public byte Red { get; set; }
        public byte Blue { get; set; }

        public double Luminence { get; set; }

        public int CompareTo(Pixel other)
        {
            return this.Luminence.CompareTo(other.Luminence);
        }
    }

    public static class PixelInfo
    {
        public const double LIGHTNESS = 15;
        public static double ConvertToDecimal(int colorChannel)
        {
            return ((double)colorChannel / 255);
        }

        public static double ConvertToLinear(double colorChannel)
        {
            if (colorChannel <= 0.04045)
            {
                return colorChannel / 12.92;
            }

            return Math.Pow((colorChannel + 0.055) / 1.055, 2.4);
        }

        public static double CalculateYLuminence(double rlin, double glin, double blin)
        {
            return 0.2126 * rlin + 0.7152 * glin + 0.0722 * blin;
        }

        public static double CalculatePerceivedLightness(double yLuminnce)
        {
            if (yLuminnce <= 0.008856)
            {
                return yLuminnce * 903.3;
            }

            return (Math.Pow(yLuminnce, 0.33) * 116) - 16;
        }
    }
}
