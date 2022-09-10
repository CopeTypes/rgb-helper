using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Colore.Data;

namespace RGBHelper
{
    public class CommandSystem
    {

        

        public static async Task start()
        {
            var cmd = string.Empty;
            while (cmd != "quit")
            {
                Console.WriteLine("\n");
                Console.WriteLine("Command?\n");
                cmd = Console.ReadLine();
                Console.WriteLine("\n");
                Console.WriteLine("BUSY.\n");
                await runCmd(cmd);
            }
            Console.WriteLine("\n");
            Console.WriteLine("Shutting down...");
        }


        private static async Task runCmd(string command)
        {
            
            Console.WriteLine("runCmd(): " + command + "\n");
            
            string[] cmdD = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var cmd = cmdD[0];
            
            Console.WriteLine("cmdD len: " + cmdD.Length + "\n");
            Console.WriteLine("cmd: " + cmd + "\n");

            
            //Console.ReadLine();
            
            if (cmdD.Length < 2) // single commands
            {
                Console.WriteLine("single command mode (<2 args)\n");
                if (cmd == "level_up")
                {
                    Console.WriteLine("Playing level up animation\n");
                    await Effects.levelUp();
                }
            }
            else
            {
                Console.WriteLine("multi command mode\n");
                if (cmd == "macro") //custom list of commands to execute together
                {
                    Console.WriteLine("macro mode\n");
                    command = command.Replace("macro ", "");
                    cmdD = command.Split(','); // macro uses , to separate (ex solid green, blink red 3, solid white)
                    foreach (var subc in cmdD)
                    {
                        if (subc == "macro") continue;

                        if (subc.StartsWith("delay "))
                        { //delay x millis
                            int toSleep = int.Parse(subc.Replace("delay ", ""));
                            Thread.Sleep(toSleep);
                        }

                        if (subc.StartsWith("topbar "))
                        {
                            Color c = Utils.getColorFromString(subc.Replace("topbar ", ""));
                            await RGBControl.solidTop(c);
                        }

                        if (subc.Equals("restore"))
                        {
                            await RGBControl.restoreKb();
                        }
                        
                        
                        if (subc.StartsWith("solid "))
                        { // solid color (custom #color)
                            string[] temp = subc.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Color c;
                            c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                            await RGBControl.solidKb(c);
                        }


                        if (subc.StartsWith("blink "))
                        { // blink color (custom #color) times
                            string[] temp = subc.Replace("blink ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Color c;
                            int times;
                            if (temp[0].Equals("custom")) // parse custom color
                            {
                                c = Utils.getColorFromString("#" + temp[1]);
                                times = int.Parse(temp[2]);
                            }
                            else
                            {
                                c = Utils.getColorFromString(temp[0]);
                                times = int.Parse(temp[1]);
                            }
                            if (subc.Contains("delay ")) // custom scroll delay
                            {
                                Console.WriteLine("using custom scroll delay\n");
                                var ti = Array.IndexOf(temp, "delay");
                                var delay = temp[ti + 1].Replace("delay ", "");
                                Console.WriteLine(delay);
                                await RGBControl.blinkKb(c, times, int.Parse(delay));
                            }
                            else
                            {
                                await RGBControl.blinkKb(c, times);
                            }
                        }

                        if (subc.StartsWith("scroll "))
                        { // scroll color1 color2 times direction
                            string[] temp = subc.Replace("scroll ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Color color1;
                            Color color2;
                            int i = 0; // need to offset the arg locations if using custom colors
                            if (temp[0].Equals("custom")) // parse custom color(s)
                            {
                                color1 = Utils.getColorFromString("#" + temp[1]);
                                i++;
                            }
                            else { color1 = Utils.getColorFromString(temp[0]); }
                            if (temp[1 + i].Equals("custom"))
                            {
                                color2 = Utils.getColorFromString("#" + temp[2 + i]);
                                i++;
                            }
                            else
                            { color2 = Utils.getColorFromString(temp[1 + i]); }
                            int times = int.Parse(temp[2 + i]);
                            int mode = 1;
                            switch (temp[3 + i])
                            {
                                case "bottom_to_top":
                                    mode = 1;
                                    break;
                                case "top_to_bottom":
                                    mode = 2;
                                    break;
                                case "left_to_right":
                                    mode = 3;
                                    break;
                                case "right_to_left":
                                    mode = 4;
                                    break;
                            }
                            if (subc.Contains("delay ")) // custom scroll delay
                            {
                                Console.WriteLine("using custom scroll delay\n");
                                var ti = Array.IndexOf(temp, "delay");
                                var delay = temp[ti + 1].Replace("delay ", "");
                                Console.WriteLine(delay);
                                await RGBControl.scrollKb(color1, color2, times, mode, int.Parse(delay));
                            }
                            else
                            {
                                await RGBControl.scrollKb(color1, color2, times, mode);
                            }
                            
                        }
                    }
                }
            }

        }
        
    }
}