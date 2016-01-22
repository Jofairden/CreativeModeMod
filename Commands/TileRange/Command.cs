using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class TileRange
    {
        // TileRange Command
        // Added: Pre-Alpha R3
        // Rewritten as try-catch in Pre-Alpha R3
        public static void Run(string[] args, int count)
        {
            try
            {
                int x = Player.tileRangeX, y = Player.tileRangeY;
                if (args.Length == 0)
                {
                    CreativeMode.TILERANGE = false;
                    return;
                }
                x = int.Parse(args[0]);
                if (args.Length > 1) y = int.Parse(args[1]);
                CreativeMode.tileRangeX = x;
                CreativeMode.tileRangeY = y;
                CreativeMode.TILERANGE = true;
                Main.NewText("Tilerange X: " + x.ToString() + " Tilerange Y: " + y.ToString(), 255, 255, 0);
            }
            catch (NumericException)
            {
                Main.NewText("Error: values must range from 0 to 999", 255, 255, 0);
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
