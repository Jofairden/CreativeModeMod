using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands.Handling.Peace
{
    class PeaceGlobalNPC : GlobalNPC
    {
        // CanHitPlayer shifted to ModPlayer [Pre-Alpha R3]

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            // No Spawns in peace mode
            if (CreativeMode.PEACE)
            {
                spawnRate = 0;
                maxSpawns = 0;
            }
        }
    }
}
