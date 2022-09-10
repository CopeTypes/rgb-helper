using System.Threading.Tasks;
using Colore.Data;

namespace RGBHelper
{
    public class Effects
    {


        
        
        
        
        public static async Task levelUp()
        {
            await RGBControl.solidKb(Color.Black);
            await RGBControl.blinkKb(Color.Yellow, 2);
            await RGBControl.scrollKb(Color.Green, Color.Yellow, 2, 1);
            await restoreDefault();
        }
        
        private static async Task restoreDefault()
        {
            await RGBControl.solidKb(Program.kbDefault);
        }
    }
}