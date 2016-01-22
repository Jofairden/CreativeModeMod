using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeMode
{
    public class NumericException : Exception { }
    public class NoModException : Exception { }
    public class NoModItemException : Exception { }

    public class HandlePlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (CreativeMode.FREEZETIME)
            {
                Main.dayTime = CreativeMode.updatedayTime;
                Main.time = CreativeMode.updateTime;
            }
        }

        public override void PreUpdateBuffs()
        {

        }

    }
}
