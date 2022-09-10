using System;
using Colore.Data;

namespace RGBHelper
{
    public class Utils
    {


        public static Color getColorFromString(string s)
        { 

            if (s.StartsWith("#")) //parse custom hex
            {
                s = s.Replace("#", "");
                uint color = Convert.ToUInt32(s, 16);
                return Color.FromRgb(color);
            }
            
            
            switch (s) //built in-s
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
    }
}