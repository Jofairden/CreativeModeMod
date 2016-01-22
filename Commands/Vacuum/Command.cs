using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Vacuum
    {
        // Vacuum Command
        // Added: Pre-Alpha R1
        // Fixes: PA R2 fixed vacuumed items count
        public static void Run(string[] args, int count)
        {
            /* 
                Iterate through specified items in the world, and change their position to the player.
                For tiles you need to divide positions by 16 due to the fact that a tile is 16x16 pixels.
            */

            Player player = Main.player[Main.myPlayer];
            int item, itemI, itemJ, itemCount = 0;
            bool itemRange = false, itemsAll = false;
            IEnumerable<int> itemList = Enumerable.Empty<int>();

            // Command check
            if ((args.Length == 0 || !int.TryParse(args[0], out item)) || (args.Length == 2 && !int.TryParse(args[1], out item)))
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }

            itemI = int.Parse(args[0]); // parse arg 1

            if (itemI == -1) itemsAll = true; // -1 == vacuum all items
            else if (args.Length == 2)
            {
                itemJ = int.Parse(args[1]);
                itemList = Enumerable.Range(itemI, itemJ).OrderBy(x => itemI).ToArray(); // Save all ids, in ascending order
                itemRange = true;
            }

            for (int i = 0; i < Main.item.Length; i++) // Iteration
            {
                if (Main.item[i].active)
                {
                    itemCount++;
                    if (itemsAll)
                    {
                        Main.item[i].position = new Vector2((int)player.position.X, (int)player.position.Y); // ALL ITEMS
                    }
                    else
                    {
                        if (!itemRange && Main.item[i].type == item)
                        {
                            Main.item[i].position = new Vector2((int)player.position.X, (int)player.position.Y); // ITEM RANGE
                        }
                        else if (itemList.Contains(Main.item[i].type))
                        {
                            Main.item[i].position = new Vector2((int)player.position.X, (int)player.position.Y); // SINGLE ITEM
                        }
                    }
                }
            }

            Main.NewText("Vacuumed " + itemCount + " items", 255, 255, 0);
        }

    }
}
