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


            switch (cmd)
            { // determine base command type
                case "keyboard":
                    await parseKeyboardCmd(command);
                    break;
                case "mouse":
                    await parseMouseCmd(command);
                    break;
                case "mousepad":
                    await parseMousePadCmd(command);
                    break;
                case "headset":
                    await parseHeadsetCmd(command);
                    break;
                case "macro":
                    await parseMacro(command);
                    break;
                case "global":
                    await parseGlobalCmd(command);
                    break;
                case "set":
                    await parseSetCmd(command);
                    break;
            }
            
        }
        
        
        
        // command parsing by type

        private static async Task parseHeadsetCmd(string cmd)
        { // headset commands
            cmd = cmd.Replace("headset ", "");

            if (cmd.StartsWith("solid "))
            {
                if (cmd.StartsWith("solid "))
                {
                    string[] temp = cmd.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Color c;
                    c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                    await RGBControl.solidHeadset(c);
                }
            }
        }
        
        private static async Task parseMouseCmd(string cmd)
        { // mouse commands
            cmd = cmd.Replace("mouse ", "");

            if (cmd.StartsWith("solid "))
            {
                if (cmd.StartsWith("solid "))
                {
                    string[] temp = cmd.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Color c;
                    c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                    await RGBControl.solidMouse(c);
                }
            }
        }
        
        private static async Task parseMousePadCmd(string cmd)
        { // mouse pad commands
            cmd = cmd.Replace("mousepad ", "");

            if (cmd.StartsWith("solid "))
            {
                if (cmd.StartsWith("solid "))
                {
                    string[] temp = cmd.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Color c;
                    c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                    await RGBControl.solidMousePad(c);
                }
            }
        }
        
        private static async Task parseSetCmd(string cmd)
        { // setting commands
            await Task.Run(() =>
            {
                cmd = cmd.Replace("set ", "");
                if (cmd.StartsWith("default_color "))
                {
                    string[] temp = cmd.Replace("default_color ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Color c;
                    c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                    Program.DefaultColor = c;
                }
            });
        }
        
        
        private static async Task parseGlobalCmd(string cmd)
        { // global commands (controlling all devices)
            cmd = cmd.Replace("global ", "");

            if (cmd.StartsWith("solid "))
            {
                string[] temp = cmd.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Color c;
                c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
                await RGBControl.solidEverything(c);
            }
        }
        
        private static async Task parseMacro(string command)
        { // macro commands (list of commands to run at once)
            command = command.Replace("macro ", "");
            string[] cmdD = command.Split(','); // macro uses , to separate (ex solid green, blink red 3, solid white)
            foreach (var subc in cmdD)
            {
                if (subc == "macro") continue;

                if (subc.StartsWith("delay "))
                { //delay x millis
                    int toSleep = int.Parse(subc.Replace("delay ", ""));
                    Thread.Sleep(toSleep);
                }
                //parse these as normal, macros just contain multiple commands together
                if (subc.StartsWith("keyboard ")) { await parseKeyboardCmd(subc); }
                if (subc.StartsWith("global ")) { await parseGlobalCmd(subc); }
            }
        }
        
        
        //keyboard command parsing (should cleanup more later)
        private static async Task parseKeyboardCmd(string cmd)
        {//ex keyboard solid arg1 arg2
            //Console.WriteLine("parseKeyboardCmd()\n");
            cmd = cmd.Replace("keyboard ", "");
            //Console.WriteLine("new cmd: " + cmd + "\n");
            
            if (cmd.StartsWith("topbar "))
            {
                Color c = Utils.getColorFromString(cmd.Replace("topbar ", ""));
                await RGBControl.solidTop(c);
            }

            if (cmd.Equals("restore")) { await RGBControl.restoreKb(); }
            if (cmd.StartsWith("solid ")) { await parseKeyboardSolid(cmd); } // solid color (custom #color)
            if (cmd.StartsWith("blink ")) { await parseKeyboardBlink(cmd); } // blink color (custom #color) times
            if (cmd.StartsWith("scroll ")) { await parseKeyboardScroll(cmd); } // scroll color1 color2 times direction
            if (cmd.StartsWith("flash ")) { await parseKeyboardFlash(cmd); } // flash color times side delay
        }

        
        
        private static async Task parseKeyboardSolid(string subc)
        {
            string[] temp = subc.Replace("solid ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Color c;
            c = temp[0].Equals("custom") ? Utils.getColorFromString("#" + temp[1]) : Utils.getColorFromString(temp[0]); //ease
            await RGBControl.solidKb(c);
        }

        private static async Task parseKeyboardBlink(string subc)
        {
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
        
        private static async Task parseKeyboardFlash(string subc)
        {
            string[] temp = subc.Replace("flash ", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Color c;
            int times, side, delay;
            int i = 0;
            if (temp[0] == "custom")
            {
                i++;
                c = Utils.getColorFromString(temp[1]);
            }
            else
            {
                c = Utils.getColorFromString(temp[0]);
            }

            times = int.Parse(temp[1 + i]);
            side = int.Parse(temp[2 + i]); //todo string alias like the directions for scroll
            if (subc.Contains("delay "))
            {
                var ti = Array.IndexOf(temp, "delay");
                delay = int.Parse(temp[ti + 1].Replace("delay ", ""));
            }
            else
            {
                delay = 250;
            }
            await RGBControl.flashKb(c, times, side, delay);
        }

        private static async Task parseKeyboardScroll(string subc)
        {
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
                //Console.WriteLine("using custom scroll delay\n");
                var ti = Array.IndexOf(temp, "delay");
                var delay = temp[ti + 1].Replace("delay ", "");
                //Console.WriteLine(delay);
                await RGBControl.scrollKb(color1, color2, times, mode, int.Parse(delay));
            }
            else
            {
                await RGBControl.scrollKb(color1, color2, times, mode);
            }
        }
    }
}