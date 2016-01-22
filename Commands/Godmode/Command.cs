using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands
{
    public class Godmode
    {
        // GodMode Command
        // Added: Pre-Alpha R1
        // Shifted to ModPlayer in [Pre-Alpha R3] and rewritten as try-catch
        public static void Run(string[] args, int count)
        {
            try
            {
                bool toggle = false;
                Player player = Main.player[Main.myPlayer];
                ModBuff buff = ModLoader.GetMod("CreativeMode").GetBuff("GodmodeBuff");
                if (CreativeMode.GM) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                CreativeMode.GM = toggle;
                if (CreativeMode.GM)
                {
                    player.AddBuff(buff.Type, 3600 * 60 * 24);
                }
                else
                {
                    player.ClearBuff(buff.Type);
                }
                Main.NewText("God mode toggled.", 255, 255, 0);
            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
