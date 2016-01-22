using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Buffs
{
    public class GodmodeBuff : ModBuff
    {
        // A buff displayed while godmode is active
        public override void SetDefaults()
        {
            Main.buffName[Type] = "Godly Appearance";
            Main.buffTip[Type] = "Aren't you stunning? Quadripled damage, critical hits and being immortal!";
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
        }

    }
}
