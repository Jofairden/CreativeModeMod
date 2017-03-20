using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CreativeMode.Commands.Handling.Builder
{
    class BuilderPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            // Max stacks at all times
            if (CreativeMode.BUILDER)
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].active & player.inventory[i].stack < player.inventory[i].maxStack)
                    {
                        player.inventory[i].stack = player.inventory[i].maxStack;
                    }
                }

                Player.tileRangeX = 999;
                Player.tileRangeY = 999;

                player.wingTimeMax = 60 * 3600 * 24;
            }

        }
    }
}
