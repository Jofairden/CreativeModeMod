using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;

namespace CreativeMode.Commands
{
    public class Butcher
    {
        // Butcher Command
        // Added: Pre-Alpha R1
        // Pre-Alpha R3: Added the option to also butcher town NPCs
        public static void Run(string[] args, int count)
        {
            /* 
                Iterate through all npcs, and butcher all the active ones
            */
            int npcCount = 0;
            bool incTown = false;
            if (args.Length == 1 && bool.TryParse(args[0], out incTown))
            {
                incTown = bool.Parse(args[0]);
            }
            for (int i = 0; i < Main.npc.Length; i++) // Iteration
            {
                if (Main.npc[i].active && Main.npc[i].type != NPCID.TargetDummy)
                {
                    if (!incTown && Main.npc[i].townNPC) continue;
                    Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, 1, true); // NoInteraction to avoid Banners being hit
                    npcCount++;
                }
            }
            Main.NewText("Butchered " + npcCount + " NPCs", 255, 255, 0);
        }

    }
}
