using System.Threading.Tasks;
using Colore.Data;
using Colore.Effects.Keyboard;

namespace RGBHelper
{
    public class KeyboardUtils
    {


        public static async Task setSide(Color color, int side, CustomKeyboardEffect effect)
        { // 1 = left , 2 = right , 3 = top , 4 = bottom

            int r = 0;
            int c = 0;
            
            switch (side)
            {
                case 1: // left
                {
                    r = 0;
                    c = 1;
                    while (r < KeyboardConstants.MaxRows)
                    {
                        try { effect[r, c] = color; }
                        catch { }
                        r++;
                    }
                    break;
                }

                case 2: // right
                {
                    r = 0;
                    c = KeyboardConstants.MaxColumns - 1;
                    while (r < KeyboardConstants.MaxRows)
                    {
                        try { effect[r, c] = color; }
                        catch { }
                        r++;
                    }
                    break;
                }

                case 3: // top
                {
                    r = 0;
                    c = 0;
                    while (c < KeyboardConstants.MaxColumns)
                    {
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            c++;
                        }
                    }
                    break;
                }

                case 4: // bottom
                {
                    r = KeyboardConstants.MaxRows - 1;
                    c = 0;
                    while (c < KeyboardConstants.MaxColumns)
                    {
                        {
                            try { effect[r, c] = color; }
                            catch { }
                            c++;
                        }
                    }
                    break;
                }
            }

            await RGBControl.getChroma().Keyboard.SetCustomAsync(effect);

        }
        
        
    
    }
}

