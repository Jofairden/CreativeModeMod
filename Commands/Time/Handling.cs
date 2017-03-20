using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace CreativeMode.Commands.Handling.Time
{
    public class TimeGlobal : ModPlayer
    {
        public override void ResetEffects()
        {
            //Main.dayRate = 1;
        }

        public override void PostUpdate()
        {
            if (CreativeMode.updatedayRate) Main.dayRate = CreativeMode.dayRate;
            if (CreativeMode.FREEZETIME)
            {
                Main.dayTime = CreativeMode.updatedayTime;
                Main.time = CreativeMode.updateTime;
            }
        }
    }
}
