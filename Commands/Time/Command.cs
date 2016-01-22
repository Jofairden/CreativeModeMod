using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Time
    {
        // Time Command
        // Added: Pre-Alpha R2
        // Added a day and night shortcur in Pre-Alpha R3
        // An intermediate way of controlling the in-game time.
        // Rewritten as try-catch in Pre-Alpha R3

        /*
        Used in conjuction with Main.dayTime to determine time of day. (Terraria v. 1.1.2)
        If Main.dayTime == true, Main.time goes from 0.0 (dawn) to 54000.0 (nightfall) and resets.
        If Main.dayTime == false, Main.time goes from 0.0 (nightfall) to 32400.0 (dawn) and resets. 16200.0 (midnight) is in the middle. 
        */
        public static void Run(string[] args, int count)
        {
            string[] time =
            {
                "add", "sub", "set", "day", "night", "freeze"
            };

            try
            {
                string arg = args[0];
                if (time.Contains(arg))
                {
                    if (arg == time[0]) // add
                    {
                        Main.time += int.Parse(args[1]);
                    }
                    else if (arg == time[1]) // subtract
                    {
                        Main.time -= int.Parse(args[1]);
                    }
                    else if (arg == time[2]) // set
                    {
                        if (args.Length > 2)
                        {
                            Main.dayTime = bool.Parse(args[2]);
                        }
                        Main.time = int.Parse(args[1]);
                    }
                    else if (arg == time[3]) // day
                    {
                        Main.dayTime = true;
                        Main.time = 24000.0;
                    }
                    else if (arg == time[4]) // night
                    {
                        Main.dayTime = false;
                        Main.time = 16200.0;
                    }
                    else if (arg == time[5]) // freeze
                    {
                        CreativeMode.FREEZETIME = true;
                        CreativeMode.updatedayTime = Main.dayTime;
                        CreativeMode.updateTime = Main.time;
                    }
                }
                else throw new ArgumentException();
            }
            catch
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
