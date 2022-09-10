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


        public static Color DefaultColor;
        
        public static async Task Main(string[] args)
        {

            DefaultColor = Color.Green;
            
            await RGBControl.prep();
            await CommandSystem.start();
        }


    }
}