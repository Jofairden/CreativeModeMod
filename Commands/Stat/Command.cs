using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Stat
    {
        // Stat Command
        // Added: Pre-Alpha R2
        // Rewritten as try-catch in Pre-Alpha R3
        public static void Run(string[] args, int count)
        {
            string[] stat =
            {
                "health", "hp", "mana", "mp", "hpmax", "mpmax", "hpm", "mpm"
            };

            try
            {
                Player player = Main.player[Main.myPlayer];
                string arg = args[0];
                int value = int.Parse(args[1]);

                int minHP = 100, minMP = 20, maxHP = 500, maxMP = 240;
                if (stat.Contains(arg))
                {
                    // statLife
                    if (arg == stat[0] || arg == stat[1])
                    {
                        if (value > 0 & value <= player.statLifeMax & value <= maxHP)
                        {
                            player.statLife = value;
                        }
                        else throw new NumericException();
                    }
                    // statMana
                    else if (arg == stat[2] || arg == stat[3])
                    {
                        if (value > 0 & value <= player.statManaMax & value <= maxMP)
                        {
                            player.statMana = value;
                        }
                        else throw new NumericException();
                    }
                    // statLifeMax
                    else if (arg == stat[4] || arg == stat[5])
                    {
                        if (value > minHP & value <= maxHP)
                        {
                            player.statLifeMax = value;
                        }
                        else throw new NumericException();
                    }
                    // statManaMax
                    else if (arg == stat[6] || arg == stat[7])
                    {
                        if (value > minMP & value <= maxMP)
                        {
                            player.statManaMax = value;
                        }
                        else throw new NumericException();
                    }
                }
                else throw new ArgumentException();
            }
            catch (NumericException)
            {
                Main.NewText("Passed value was invalid.", 255, 255, 0);
                return;
            }
            catch
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
