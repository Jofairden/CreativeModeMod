using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands.Handling.Shine
{
    class ShinePlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (CreativeMode.SHINE)
            {
                Lighting.AddLight((int)((double)player.position.X + (double)(player.width / 2)) / 16, (int)((double)player.position.Y + (double)(player.height / 2)) / 16, 0.8f, 0.95f, 1f);
            }
        }
    }
}
