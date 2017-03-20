using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Reflection;

namespace CreativeMode.Commands
{
    public class Cache
    {
        // Cache command
        // Added: Alpha 1.0
        public static void Run(string[] args, string cmd)
        {
            try
            {
                cmd = cmd ?? null;
                args = args ?? new string[0];

                if ((args == null || args.Length == 0) | string.IsNullOrEmpty(cmd) | string.IsNullOrEmpty(CreativeMode.commandCache) | (CreativeMode.commandCacheArgs == null || CreativeMode.commandCacheArgs.Length == 0))
                {
                    Main.NewText("No command is in cache.", 255, 255, 0);
                    return;
                }
                else CreativeMode.CRB216(args, cmd);
            }
            catch (Exception e)
            {
                CreativeMode.RunError(MethodBase.GetCurrentMethod().GetType(), e);
                return;
            }
        }
    }
}
