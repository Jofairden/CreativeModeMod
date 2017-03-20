using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands.Handling.TileRange
{
    class HandlingPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            if (CreativeMode.TILERANGE)
            {
                Player.tileRangeX = CreativeMode.tileRangeX;
                Player.tileRangeY = CreativeMode.tileRangeY;
            }

        }
    }
}
