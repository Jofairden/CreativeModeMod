using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace CreativeMode.Commands
{
    public class GiveItem
    {
        // GiveItem Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch and fixed in Pre-Alpha R3
        public static void Run(string[] args, int count)
        {
            try
            {
                int amount = 1, call = 0, threshold = 5;
                int? type = null;
                string name = null;
                Player player = Main.player[Main.myPlayer];

                if (!int.TryParse(args[0], out call)) // item used as a name
                {
                    string result = Tools.ConvertStringArrayToString(args);
                    // if we type '/giveitem Rod of Discord 5' we get:
                    // Rod.of.Discord.5
                    string checker = args[args.Length - 1].ToString();
                    string[] split;
                    bool isAmount, isFound = false;
                    List<int> distanceList = new List<int> { };
                    List<int> rangeList = new List<int> { };

                    if (int.TryParse(checker, out call)) // last value an integer?
                    {
                        result = result + "." + (int.Parse(args[args.Length - 1]) * 2).ToString(); // Rod.Of.Discord.5.10
                        split = result.Split(new string[] { "." + checker }, StringSplitOptions.RemoveEmptyEntries); // Split at .5
                                                                                                                     // Rod.of.Discord
                                                                                                                     // .10
                        isAmount = true;
                        Main.NewText(split[0]);//
                    }
                    else // not amount
                    {
                        split = result.Split(new string[] { "\"." }, StringSplitOptions.RemoveEmptyEntries); // Split at ".
                        // Rod.of.Discord
                        // .5
                        isAmount = false;
                        Main.NewText(split[0]);//
                    }

                    split[0] = split[0].Replace("\"", ""); // Remove "
                    string itemtype = split[0].Replace(".", ""); // Get itemtype name
                    string itemname = split[0].Replace(".", " "); // Get item name to lowercase and first letter uppercase
                    string nameFilter = Char.ToUpperInvariant(itemname[0]) + itemname.Substring(1).ToLower();
                    name = itemname;
                    Main.NewText(itemtype);//
                    if (split.Length == 2)
                    {
                        amount = int.Parse(split[1].Replace(".", "")); // Get amount
                    }
                    if (isAmount)
                    {
                        amount /= 2; // 10/2 = 5
                    }

                    // Find item
                    var itemid = typeof(ItemID);
                    var field = itemid.GetField(itemtype);
                    if (field != null)
                    {
                        type = (short)field.GetValue(null);
                    }
                    else
                    {
                        for (int k = 0; k < Main.itemName.Length; k++) // Look for item with name
                        {
                            if (itemname == Main.itemName[k] || nameFilter == Main.itemName[k] || itemname.ToLower() == Main.itemName[k].ToLower())
                            {
                                type = k;
                                isFound = true;
                                break;
                            }
                            // Use Levenshtein's edit distance
                            else if (Tools.EditDistance(itemname.ToLower(), Main.itemName[k].ToLower(), threshold) <= threshold) // (int)(Main.itemName[k].Length/2 * (2/3) + 1)
                            {
                                // Add any match to our list and continue
                                bool wordMatch = Tools.Contains(Main.itemName[k], itemname, StringComparison.OrdinalIgnoreCase);
                                if (wordMatch)
                                {
                                    type = k;
                                    isFound = true;
                                    //Main.NewText(k.ToString());//
                                    break;
                                }
                                distanceList.Add(Tools.EditDistance(itemname.ToLower(), Main.itemName[k].ToLower(), threshold));
                                rangeList.Add(k);
                                continue;
                            }
                        }

                        // Not found yet? 
                        if (!isFound)
                        {
                            // Loop our created list
                            int distIndex = Array.IndexOf(distanceList.ToArray(), distanceList.Min());
                            type = rangeList[distIndex];
                            //Main.NewText(rangeList[distIndex].ToString()); //
                        }
                    }

                    //Main.NewText(itemname);//
                    //Main.NewText(itemtype.ToString());//
                }
                else // Type given
                {
                    type = int.Parse(args[0]);
                    if (args.Length == 2) amount = int.Parse(args[1]);
                }

                int item = int.Parse(type.ToString()); // ITEM TYPE
                Item reference = new Item();
                reference.SetDefaults(item);
                if (amount > reference.maxStack)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        player.QuickSpawnItem(item);
                    }
                }
                else player.QuickSpawnItem(item, amount);

            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                //ErrorLogger.Log(e.ToString());
                return;
            }
            // Thanks Jopojelly :)
        }

    }
}
