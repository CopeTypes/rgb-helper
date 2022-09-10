using System;
using System.Threading;
using System.Threading.Tasks;
using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;

namespace RGBHelper
{
    internal class Program
    {


        public static Color kbDefault = Utils.getColorFromString("#913de2");
        
        public static async Task Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("No default keyboard color set, using meteor.");
            }

            if (args.Length > 0)
            {
                kbDefault = Utils.getColorFromString(args[0]);
            }
            
            await RGBControl.prep();
            await CommandSystem.start();
        }


    }
}