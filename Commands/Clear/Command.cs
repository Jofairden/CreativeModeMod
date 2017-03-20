using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Diagnostics;

namespace CreativeMode.Commands
{
    public class Clear
    {
        // Clear command
        // Added: Alpha 1.0
        // Run true for only items, false for only projectiles
        public static void Run(string[] args, int count)
        {
            try
            {
                int cItem = 0;
                int cProj = 0;
                bool flag = false, flag1 = false;
                bool parse;

                if (args.Length >= 1 && bool.TryParse(args[0], out parse))
                {
                    flag = true;
                    flag1 = bool.Parse(args[0]);
                }

                if (!flag || (flag && flag1))
                {
                    for (int i = 0; i < Main.item.Length; i++)
                    {
                        if (Main.item[i].active)
                        {
                            Main.item[i].active = false;
                            cItem++;
                        }
                    }
                }

                if (!flag || (flag && !flag1))
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].active)
                        {
                            Main.projectile[i].Kill();
                            cProj++;
                        }
                    }
                }

                Main.NewText(string.Format("Killed {0} items and {1} projectiles", cItem, cProj), 255, 255, 0);
            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
