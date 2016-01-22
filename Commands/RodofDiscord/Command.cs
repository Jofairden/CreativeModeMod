using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class RodOfDiscord
    {
        // RoD Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch in Pre-Alpha R3
        public static void Run(string[] args, int count)
        {
            try
            {
                bool toggle = false;
                if (CreativeMode.ROD) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                CreativeMode.ROD = toggle;
                Main.NewText("'Chaos State' immunity toggled.", 255, 255, 0);
            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                //ErrorLogger.Log(e.ToString());
                return;
            }
            // Handling: Handling.cs
        }
    }
}
