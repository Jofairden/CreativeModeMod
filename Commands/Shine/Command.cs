using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Shine  
    {

        // Shine Command
        // Added: Pre-Alpha R1
        // Requires: ModPlayer support
        // Rewritten as try-catch in Pre-Alpha R3
        public static void Run(string[] args, int count)
        {
            try
            {
                bool toggle;
                if (CreativeMode.SHINE) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                CreativeMode.SHINE = toggle;
                Main.NewText("Shine toggled.", 255, 255, 0);
            }
            catch
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
            // Handling: Handling.cs
        }
    }
}
