using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands
{
    public class NPCType
    {
        // NPCType Command
        // Added: Pre-Alpha R1
        // Thanks: BlueMagic123 / Jopojelly
        // Returns the ID for an NPC of a mod.
        public static void Run(string[] args, int count)
        {
            if (args.Length < 2)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
            Mod mod = ModLoader.GetMod(args[0]);
            string message;
            bool isMod = mod == null ? false : true;
            if (!isMod) message = "Given mod is null";
            else message = "NPCType: " + mod.NPCType(args[1]);

            Main.NewText(message, 255, 255, 0);
        }
    }
}
