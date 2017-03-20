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
    public class Buff
    {
        // Buff command
        // Added: Alpha 1.0
        public static void Run(string[] args, int count)
        {
            if (string.IsNullOrEmpty(args.ToString()))
            {
                throw new Exception();
            }

            try
            {
                int result;
                int bType = 0;
                string bName = "";
                bool isInt = false;
                bool isFound = false;
                int bTime = 600;
                int threshold = 5;

                if (int.TryParse(args[0], out result))
                {
                    bType = int.Parse(args[0]); // store type
                    isInt = true;
                }
                else if (args[0] is string)
                {
                    bName = args[0].ToString(); // store name
                }

                if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[0])) bTime = int.Parse(args[1]);

                if (!isInt)
                {
                    List<int> distanceList = new List<int> { };
                    List<int> rangeList = new List<int> { };
                    var buffid = typeof(BuffID);
                    var field = buffid.GetField(bName);
                    string nameFilter = Char.ToUpperInvariant(bName[0]) + bName.Substring(1).ToLower();
                    if (field != null)
                    {
                        bType = (short)field.GetValue(null);
                    }
                    else
                    {
                        for (int k = 0; k < Main.maxBuffTypes; k++) // Look for buff with name
                        {
                            if (bName == Main.buffName[k] || nameFilter == Main.buffName[k] || bName.ToLower() == Main.buffName[k].ToLower())
                            {
                                bType = k;
                                isFound = true;
                                break;
                            }
                            // Use Levenshtein's edit distance
                            else if (Tools.EditDistance(bName.ToLower(), Main.buffName[k].ToLower(), threshold) <= threshold)
                            {
                                // Add any match to our list and continue
                                bool wordMatch = Tools.Contains(Main.buffName[k], bName, StringComparison.OrdinalIgnoreCase);
                                if (wordMatch)
                                {
                                    bType = k;
                                    isFound = true;
                                    break;
                                }
                                distanceList.Add(Tools.EditDistance(bName.ToLower(), Main.buffName[k].ToLower(), threshold));
                                rangeList.Add(k);
                                continue;
                            }
                        }

                        // Not found yet? 
                        if (!isFound)
                        {
                            // Loop our created list
                            int distIndex = Array.IndexOf(distanceList.ToArray(), distanceList.Min());
                            bType = rangeList[distIndex];
                        }
                    }
                }

                Main.player[Main.myPlayer].AddBuff(bType, bTime);
                Main.NewText(string.Format("Added {0}[{1}] for {2} ticks to {3}", Main.buffName[bType], bType, bTime, Main.player[Main.myPlayer].name), 255, 255, 0);
            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                //Main.NewText(new StackTrace(e, true).GetFrame(0).GetFileLineNumber().ToString(), 255, 255, 0);
                return;
            }
        }
    }
}
