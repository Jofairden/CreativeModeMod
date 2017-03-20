using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

namespace CreativeMode.Commands
{
    namespace Extra
    {
        public class NPC
        {
            // NPC Command
            // Added: Pre-Alpha R1
            // Thanks: BlueMagic123 / Jopojelly
            // Spawns an x amount of NPCs at a given location.
            public static void Run(string[] args, int count)
            {
                int type, x, y, num = 1;
                Player player = Main.player[Main.myPlayer];
                if (args.Length == 0 || !int.TryParse(args[0], out type))
                {
                    CreativeMode.WriteCommand(CreativeMode.command, count);
                    return;
                }
                if (args.Length >= 1 && int.TryParse(args[0], out type))
                {
                    type = int.Parse(args[0]);

                    if (args.Length > 2)
                    {
                        bool relativeX = false;
                        bool relativeY = false;
                        if (args[1][0] == '~') // relative to plr
                        {
                            relativeX = true;
                            args[1] = args[1].Substring(1);
                        }
                        if (args[2][0] == '~') // relative to plr
                        {
                            relativeY = true;
                            args[2] = args[2].Substring(1);
                        }
                        if (!int.TryParse(args[1], out x))
                        {
                            x = 0;
                            relativeX = true;
                        }
                        if (!int.TryParse(args[2], out y))
                        {
                            y = 0;
                            relativeY = true;
                        }
                        if (relativeX)
                        {
                            x += (int)player.Bottom.X;
                        }
                        if (relativeY)
                        {
                            y += (int)player.Bottom.Y;
                        }
                        if (args.Length >= 3) // amount of npcs is given
                        {
                            if (!int.TryParse(args[3], out num))
                            {
                                num = 1;
                            }
                        }
                    }
                    else
                    {
                        x = (int)player.Bottom.X;
                        y = (int)player.Bottom.Y;
                    }
                    for (int k = 0; k < num; k++)
                    {
                        Terraria.NPC.NewNPC(x, y, type);
                    }
                }
                else
                {
                    CreativeMode.WriteCommand(CreativeMode.command, count);
                    return;
                }
            }

        }
    }
}
