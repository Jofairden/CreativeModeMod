using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands
{
    public class Builder
    {
        // GodMode Command
        // Added: Pre-Alpha R1
        // Shifted to ModPlayer in [Pre-Alpha R3] and rewritten as try-catch
        public static void Run(string[] args, int count)
        {
            // Builder Command
            // Added: Pre-Alpha R2
            // Rewritten as try-catch in Pre-Alpha R3
            try
            {
                bool toggle;
                if (CreativeMode.BUILDER) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    CreativeMode.BUILDER = bool.Parse(args[0]);
                }
                CreativeMode.BUILDER = toggle;
                Main.NewText("Builder mode toggled.", 255, 255, 0);
            }
            catch
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
