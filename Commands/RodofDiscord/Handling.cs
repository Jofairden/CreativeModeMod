using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CreativeMode.Commands.Handling.RodofDiscord
{
    class RodofDiscordPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            if (CreativeMode.ROD)
            {
                player.buffImmune[BuffID.ChaosState] = true;
            }
        }
    }
}
