using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CreativeMode.Commands
{
    public class DebugMode
    {
        // DebugMode Command
        // Added: Alpha 1.0.3
        // Debugs npc/proj/item
        public static void Run(string[] args, int count)
        {
            try
            {
                bool? arg1 = null, arg2 = null, arg3 = null;
                bool result;
                bool toggle = true;
                string setting = "";
                string text = CreativeMode.curPlr.name + " toggled ";

                if (args.Length >= 1)
                {
                    if (bool.TryParse(args[0], out result)) arg1 = bool.Parse(args[0]);
                    else setting = args[0];

                    if (args.Length >= 2)
                    {
                        arg2 = bool.Parse(args[1]);
                    }

                    if (args.Length >= 3)
                    {
                        arg3 = bool.Parse(args[2]);
                    }
                }

                if (setting != "") toggle = false;

                if (setting == "projectile" | setting == "proj" | setting == "-p" | setting == "p")
                {
                    if (arg2.HasValue) CreativeMode.DMproj = arg2.Value;
                    else CreativeMode.DMproj = !CreativeMode.DMproj;
                    if (arg3.HasValue) CreativeMode.DMproj1 = arg3.Value;
                    text += "debugmode(projectile) " + CreativeMode.NuGet(CreativeMode.DMproj);
                }
                else if (setting == "item" | setting == "i" | setting == "-i")
                {
                    if (arg2.HasValue) CreativeMode.DMitem = arg2.Value;
                    else CreativeMode.DMitem = !CreativeMode.DMitem;
                    text += "debugmode(item) " + CreativeMode.NuGet(CreativeMode.DMitem);
                }
                else if (setting == "reset")
                {
                    CreativeMode.DM = false;
                    CreativeMode.DM1 = false;
                    CreativeMode.DM2 = false;
                    CreativeMode.DMitem = false;
                    CreativeMode.DMproj = false;
                    CreativeMode.DMproj1 = false;
                    Main.NewText(CreativeMode.curPlr.name + " reset debugmode(all)", 255, 255, 0);
                    return;
                }
                else if (setting == "all" | setting == "-all" | setting == "-a")
                {
                    if (arg2.HasValue)
                    {
                        CreativeMode.DM = arg2.Value;
                        CreativeMode.DM1 = arg2.Value;
                        CreativeMode.DM2 = arg2.Value;
                        CreativeMode.DMitem = arg2.Value;
                        CreativeMode.DMproj = arg2.Value;
                        CreativeMode.DMproj1 = arg2.Value;
                        Main.NewText(CreativeMode.curPlr.name + " toggled debugmode(all) " + CreativeMode.NuGet(arg2.Value), 255, 255, 0);
                        return;
                    }
                    else
                    {
                        CreativeMode.DM = !CreativeMode.DM;
                        text += "debugmode(npc) " + CreativeMode.NuGet(CreativeMode.DM);
                        CreativeMode.DM1 = !CreativeMode.DM1;
                        text += ", debugmode(npcfriendly) " + CreativeMode.NuGet(CreativeMode.DM1);
                        CreativeMode.DM2 = !CreativeMode.DM2;
                        text += ", debugmode(npcany) " + CreativeMode.NuGet(CreativeMode.DM2);
                        CreativeMode.DMitem = !CreativeMode.DMitem;
                        text += ", debugmode(item) " + CreativeMode.NuGet(CreativeMode.DMitem);
                        CreativeMode.DMproj = !CreativeMode.DMproj;
                        text += "debugmode(projectile) " + CreativeMode.NuGet(CreativeMode.DMproj);
                        CreativeMode.DMproj1 = !CreativeMode.DMproj1;
                        text += "debugmode(projectilefriendly) " + CreativeMode.NuGet(CreativeMode.DMproj1);
                    }
                }
                
                if (toggle)
                {
                    if (arg1.HasValue) CreativeMode.DM = arg1.Value;
                    else CreativeMode.DM = !CreativeMode.DM;
                    text += "debugmode(npc) " + CreativeMode.NuGet(CreativeMode.DM);

                    if (args.Length >= 2)
                    {
                        if (arg2.HasValue) CreativeMode.DM1 = arg2.Value;
                        else CreativeMode.DM1 = !CreativeMode.DM1;
                        text += ", debugmode(npcfriendly) " + CreativeMode.NuGet(CreativeMode.DM1);

                        if (args.Length >= 3)
                        {
                            if (arg3.HasValue) CreativeMode.DM2 = arg3.Value;
                            else CreativeMode.DM2 = !CreativeMode.DM2;
                            text += ", debugmode(npcany) " + CreativeMode.NuGet(CreativeMode.DM2);
                        }
                    }

                }

                Main.NewText(text, 255, 255, 0);

            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }

        }
    }
}
