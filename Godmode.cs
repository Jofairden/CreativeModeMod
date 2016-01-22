using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CreativeMode
{
    public class GodmodePlayer : ModPlayer
    {
        int[] debuffList =
        {
            BuffID.Bleeding,
            BuffID.Poisoned,
            BuffID.OnFire,
            BuffID.Venom,
            BuffID.Darkness,
            BuffID.Blackout,
            BuffID.Silenced,
            BuffID.Cursed,
            BuffID.Confused,
            BuffID.Slow,
            BuffID.Weak,
            BuffID.BrokenArmor,
            BuffID.Horrified,
            BuffID.TheTongue,
            BuffID.CursedInferno,
            BuffID.Ichor,
            BuffID.Chilled,
            BuffID.Frozen,
            BuffID.Webbed,
            BuffID.Stoned,
            164, // distorted
            BuffID.Obstructed,
            BuffID.Electrified,
            BuffID.ShadowFlame,
            //148, // feral bite
            145, // moon bite
            BuffID.ManaSickness,
            BuffID.PotionSickness,
            BuffID.Suffocation,
            BuffID.Burning,
            BuffID.Stinky,
        };

        // Below we make absolutely sure the player cannot be damaged in any way.
        // GMs damage is quadripled, always crits and is always 666

            // Cannot be hit by projectiles
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return (CreativeMode.GM) ? false : base.CanBeHitByProjectile(proj);
        }

            // Cannot be hit by NPCs
        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return (CreativeMode.GM) ? false : base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

            // Player cannot be killed
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref string deathText)
        {
            return (CreativeMode.GM) ? false : base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref deathText);
        }

            // Do not consume any ammo
        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return (CreativeMode.GM) ? false : base.ConsumeAmmo(weapon, ammo);
        }

            // Stats remain maxed at all times, max mana and health
        public override void PreUpdate()
        {
            if (CreativeMode.GM)
            {
                if (player.statLife < player.statLifeMax)
                {
                    player.statLife = player.statLifeMax;
                }
                if (player.statMana < player.statManaMax)
                {
                    player.statMana = player.statManaMax;
                }
            }
        }

            // Before buffs are tinkered with, the player becomes immune to all debuffs
        public override void PreUpdateBuffs()
        {
            if (CreativeMode.GM)
            {
                for (int i = 0; i < debuffList.Length; i++)
                {
                    player.buffImmune[debuffList[i]] = true;
                }

                for (int i = 0; i < player.npcTypeNoAggro.Length; i++)
                {
                    player.npcTypeNoAggro[i] = true;
                }

            }
        }

            // Remove annoying damage gore and sound, since the player is immortal
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref string deathText)
        {
            if (CreativeMode.GM)
            {
                damage = 0;
                crit = false;
                playSound = false;
                genGore = false;
                return false;
            }
            else
            {
                return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref deathText);
            }
        }

            // When hurt, no damage and crit
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (CreativeMode.GM)
            {
                damage = 0;
                crit = false;
            }
        }
            // When hit by an NPC, no damage and crit
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (CreativeMode.GM)
            {
                damage = 0;
                crit = false;
            }
        }
            // When hit by a projectile, no damage and crit
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (CreativeMode.GM)
            {
                damage = 0;
                crit = false;
            }
        }

            // When hitting an npc, quadriple damage and crit
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (CreativeMode.GM)
            {
                damage *= 4;
                crit = true;
            }
        }

            // When hitting an npc with a projectile, quadriple damage and crit
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (CreativeMode.GM)
            {
                damage *= 4;
                crit = true;
            }
        }

            // Change all items damage to be 666 and have 100 crit
        public override void GetWeaponDamage(Item item, ref int damage)
        {
            if (CreativeMode.GM && item.damage > 1)
            {
                item.damage = 666;
                item.crit = 100;
            }
        }
            // Everything non-important
        public override void PostUpdateMiscEffects()
        {
            if (CreativeMode.GM)
            {
                player.wingTimeMax = 60 * 3600 * 24;
            }
        }
    }
}
