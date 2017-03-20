using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Peace
    {
        // Peace Command
        // Added: Pre-Alpha R1
        // Requires: ModPlayer support
        public static void Run(string[] args, int count)
        {
            try
            {
                bool toggle;
                if (CreativeMode.PEACE) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                CreativeMode.PEACE = toggle;
                Main.NewText("Peace mode toggled.", 255, 255, 0);
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
