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

        public override void PreUpdate()
        {
            if (CreativeMode.SHINE)
            {
                Lighting.AddLight((int)((double)player.position.X + (double)(player.width / 2)) / 16, (int)((double)player.position.Y + (double)(player.height / 2)) / 16, 0.8f, 0.95f, 1f);
            }
        }

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


            if (CreativeMode.TILERANGE)
            {
                Player.tileRangeX = CreativeMode.tileRangeX;
                Player.tileRangeY = CreativeMode.tileRangeY;
            }

        }

        public override void PostUpdateEquips()
        {
            if (CreativeMode.ROD)
            {
                player.buffImmune[BuffID.ChaosState] = true;
            }

        }
    }

    public class CreativeModeGlobalProjectile : GlobalProjectile
    {
        // CanHitPlayer shifted to ModPlayer [Pre-Alpha R3]
    }

    public class CreativeModeNPC : GlobalNPC
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
