using System;
using Colore.Data;

namespace RGBHelper
{
    public class Utils
    {


        public static Color getColorFromString(string s)
        { //todo add support for a custom hex or something for the macro command 

            if (s.StartsWith("#"))
            {
                s = s.Replace("#", "");
                uint color = Convert.ToUInt32(s, 16);
                return Color.FromRgb(color);
            }
            
            
            switch (s)
            {
                case "red":
                    return Color.Red;
                case "orange":
                    return Color.Orange;
                case "yellow":
                    return Color.Yellow;
                case "green":
                    return Color.Green;
                case "blue":
                    return Color.Blue;
                case "purple":
                    return Color.Purple;
                case "pink":
                    return Color.Pink;
                case "blank":
                    return Color.Black;
                case "white":
                    return Color.White;
            }

            return Color.Green;
        }
        
        //slightly improved from https://stackoverflow.com/a/39412657
        public static string getBetween(string strSource, string strStart, string strEnd) {
            const int kNotFound = -1;
            var startIdx = strSource.IndexOf(strStart, StringComparison.Ordinal);
            if (startIdx == kNotFound) return string.Empty;
            startIdx += strStart.Length;
            var endIdx = strSource.IndexOf(strEnd, startIdx, StringComparison.Ordinal);
            return endIdx > startIdx ? strSource.Substring(startIdx, endIdx - startIdx) : string.Empty;
        }
        
    }
}