using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CreativeMode.Commands
{
    public class ItemType
    {
        // ItemType Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch in Pre-Alpha R3
        // Returns the ID of the given mod's item
        public static void Run(string[] args, int count)
        {
            try
            {
                string item = args[1];
                //bool isMod = ModLoader.GetMod(args[0]) == null ? false : true;
                Mod mod = ModLoader.GetMod(args[0]);
                //if (!isMod) throw new NoModException();

                Main.NewText("Item ID: " + mod.ItemType(item).ToString(), 255, 255, 0);
            }

            catch
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }
        }

    }
}
