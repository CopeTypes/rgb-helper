using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Colore;
using Colore.Data;
using Colore.Effects.Keyboard;

namespace RGBHelper
{
    public class RGBControl
    {

        private static IChroma rgb;
        
        
        // Keyboard Grid
        private static CustomKeyboardEffect baseKeyboard = CustomKeyboardEffect.Create();



        public static async Task prep()
        {
            Console.WriteLine("Initializing chroma connection...\n");
            rgb = await ColoreProvider.CreateNativeAsync();
            Console.WriteLine("Waiting for SDK...\n");
            Thread.Sleep(2500); // https://github.com/chroma-sdk/Colore/issues/274
            Console.WriteLine("Connection established with sdk version " + rgb.SdkVersion + "\n");
            await solidEverything(Color.Black); // clean leftover (frozen) effects
        }


        public static IChroma getChroma()
        {
            return rgb;
        }
        
        //GLOBAL
        public static async Task solidEverything(Color color)
        {
            await rgb.SetAllAsync(color);
        }
        //END GLOBAL
        
        
        //MOUSE & MOUSEPAD
        public static async Task solidMouse(Color color)
        {
            await rgb.Mouse.SetAllAsync(color);
        }

        public static async Task solidMousePad(Color color)
        {
            await rgb.Mousepad.SetAllAsync(color);
        }
        //END MOUSE & MOUSEPAD
        
        
        //HEADSET
        public static async Task solidHeadset(Color color)
        {
            await rgb.Headset.SetAllAsync(color);
        }
        //END HEADSET
        
        
        //KEYBOARD 
        
        public static async Task flashKb(Color color, int times, int side, int delay = 250)
        { // 1 = left , 2 = right , 3 = top , 4 = bottom


            while (times > 0)
            {
                await KeyboardUtils.setSide(Color.Black, side, baseKeyboard);
                Thread.Sleep(delay);
                await KeyboardUtils.setSide(color, side, baseKeyboard);
                Thread.Sleep(delay);
                times--;
            }
            
            
        }
        
        
        public static async Task restoreKb()
        {
            await solidKb(Program.DefaultColor);
        }

        public static async Task solidTop(Color color)
        {
            int r = 0;
            int c = 0;
            while (c < KeyboardConstants.MaxColumns)
            {
                baseKeyboard[r, c] = color;
                c++;
            }

            await rgb.Keyboard.SetCustomAsync(baseKeyboard);
        }
        
        public static async Task solidKb(Color color)
        {
            baseKeyboard.Set(color);
            await rgb.Keyboard.SetCustomAsync(baseKeyboard);
        }
        
        public static async Task blinkKb(Color color, int times, int delay = 280)
        {
            while (times > 0)
            {
                await solidKb(color);
                Thread.Sleep(delay);
                await solidKb(Color.Black);
                Thread.Sleep(delay);
                times--;
            }

            await restoreKb();
        }
        
        public static async Task scrollKb(Color first, Color second, int times, int mode, int delay = 1)
        { // 1 = bottom to top, 2 = top to bottom, 3 = left to right, 4 = right to left
            while (times > 0)
            {
                await smoothKb(baseKeyboard, first, mode, delay);
                await smoothKb(baseKeyboard, second, mode, delay);
                times--;
            }
        }

        
        public static async Task smoothKb(CustomKeyboardEffect effect, Color color, int mode, int delay)
        { //Smoothly set the keyboard color in one of four directions.
            
            if (effect == null) effect = CustomKeyboardEffect.Create();
            
            int r = 0;
            int c = 0;
                
            switch (mode)
            {
                // going up
                case 1:
                {
                    if (delay == 1) delay = 90;
                    r = KeyboardConstants.MaxRows;
                    while (r >= 0) // go from bottom to top row
                    {
                        while (c < KeyboardConstants.MaxColumns) // set all the keys in that row
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            c++;
                        }
                        c = 0; // reset key colum
                        r--; // move up to next row
                        await rgb.Keyboard.SetCustomAsync(effect);
                        Thread.Sleep(delay);
                    }

                    break;
                }
                // going down
                case 2:
                {
                    if (delay == 1) delay = 90;
                    while (r < KeyboardConstants.MaxRows)
                    {
                        while (c < KeyboardConstants.MaxColumns)
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            c++;
                        }
                        c = 0;
                        r++;
                        await rgb.Keyboard.SetCustomAsync(effect);
                        Thread.Sleep(delay);
                    }
                    break;
                }
                // going left
                case 3:
                {
                    if (delay == 1) delay = 45;
                    while (c < KeyboardConstants.MaxColumns)
                    {
                        while (r < KeyboardConstants.MaxRows)
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            r++;
                        }
                        r = 0;
                        c++;
                        await rgb.Keyboard.SetCustomAsync(effect);
                        Thread.Sleep(delay);
                        
                    }
                    break;
                }
                // going right
                case 4:
                {
                    if (delay == 1) delay = 45;
                    c = KeyboardConstants.MaxColumns - 1;
                    while (c >= 0)
                    {
                        while (r < KeyboardConstants.MaxRows)
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            r++;
                        }
                        r = 0;
                        c--;
                        await rgb.Keyboard.SetCustomAsync(effect);
                        Thread.Sleep(delay);
                    }
                    break;
                }

            }
        }
        //END KEYBOARD
        
        
    }
}