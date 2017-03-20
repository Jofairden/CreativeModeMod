using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

namespace CreativeMode.Commands
{
    public class ItemSound
    {

        // ItemSound Command
        // Added: Alpha V1.0
        public static void Run(string[] args, int count)
        {
            try
            {
                int r;
                if (args.Length < 1 || int.TryParse(args[0], out r)) throw new NumericException();

                int itemType = int.Parse(args[0]);

            }
            catch (Exception e)
            {
                Main.NewText(e.ToString());
                return;
            }

        }
    }
}
