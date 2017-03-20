using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Itemprefix
    {
        // ItemPrefix Command
        // Added: Pre-Alpha R3
        // Rewritten as try-catch in Pre-Alpha R3
        // Added a list function in Pre-Alpha R3, to list all compatible prefixes for the selected weapon
        public static void Run(string[] args, int count)
        {
            try
            {
                Player player = Main.player[Main.myPlayer];
                int prefix = 0;
                int selItem = player.selectedItem; // inventory index
                Item curItem = player.inventory[selItem]; // actual item
                if (curItem.type == 0) throw new NumericException(); // no item selected
                byte curPre = curItem.prefix; // save cur prefix
                string text = "Use the ARROW-UP and ARROW-DOWN keys to scroll.";

                if (args.Length > 0)
                {
                    if (Tools.EditDistance(args[0], "list", 3) != int.MaxValue) // list valid prefixes
                    {
                        StringBuilder valid = new StringBuilder();

                        for (int k = 0; k < Lang.prefix.Length; k++)
                        {
                            if (!Tools.GetValidPrefix(curItem.type, k)) continue;
                            valid.Append(" " + Lang.prefix[k] + ":" + k + " |"); // append valid prefix name and type
                        }
                        curItem.prefix = curPre;
                        if (valid.Length > 1) Main.NewText(valid.ToString().Substring(1).Substring(0, valid.ToString().Length - 3));
                        else text = "No valid prefixes!";
                        Main.NewText(text, 255, 255, 0);
                        return;
                    }
                    else if (int.TryParse(args[0], out prefix) && !(prefix < 1 & prefix > Lang.prefix.Length))
                    {
                        prefix = int.Parse(args[0]);
                    }
                    else
                    {
                        string result = Tools.ConvertStringArrayToStringJoin(args);
                        string cleanname = result.Replace("\"", "").Replace(".", "").ToLower();
                        //string lowercase = Char.ToLowerInvariant(cleanname[0]) + cleanname.Substring(1);
                        string uppercase = Char.ToUpperInvariant(cleanname[0]) + cleanname.Substring(1);

                        for (int k = 1; k < Lang.prefix.Length; k++)
                        {
                            if (Lang.prefix[k] != cleanname && Lang.prefix[k] != uppercase) continue;
                            prefix = k;
                            break;
                        }
                    }
                }

                if (prefix < 1 || prefix > Lang.prefix.Length) throw new NumericException();
                //player.inventory[item].prefix = (byte)prefix; <-- not preferred since it'll work for non-usable prefixes on items as well
                if (!Tools.GetValidPrefix(curItem.type, prefix)) Main.NewText("Selected item is not compatible with the chosen prefix.", 255, 0, 0);
                else
                {
                    bool flag2 = curItem.favorited;
                    curItem.netDefaults(curItem.netID);
                    curItem.Prefix(prefix);
                    curItem.position.X = player.position.X + (float)(player.width / 2) - (float)(curItem.width / 2);
                    curItem.position.Y = player.position.Y + (float)(player.height / 2) - (float)(curItem.height / 2);
                    curItem.favorited = flag2;
                    ItemText.NewText(curItem, curItem.stack, true, false);
                    Main.PlaySound(2, -1, -1, 37);
                }
            }
            catch (Exception e)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }

            /*try
            {
                Player player = Main.player[Main.myPlayer];
                int selItem = player.selectedItem, prefix;
                bool listAll = false;

                if (player.inventory[selItem].type == 0)
                {
                    Main.NewText("No item currently selected", 255, 255, 0);
                    return;
                }

                Item item = player.inventory[selItem];

                if (args[0] == "list")
                {
                    byte curPre = item.prefix;

                    StringBuilder valid = new StringBuilder();

                    if (args.Length > 1 & args[1] == "all") listAll = true;

                    for (int i = 1; i < Lang.prefix.Length; i++)
                    {
                        if (!listAll)
                        {
                            item.Prefix(i);
                            if (item.prefix != i) continue;
                        }
                        valid.Append("" + Lang.prefix[i] + ":" + i + " - ");
                    }
                    item.prefix = curPre;
                    string prefixes = valid.ToString().Substring(0, valid.ToString().Length - 2);
                    Main.NewText(prefixes);
                    Main.NewText("Use the ARROW-UP and ARROW-DOWN keys to scroll.", 255, 255, 0);
                    return;
                }
                else if (args.Length ==1 && int.TryParse(args[0], out prefix))
                {
                    prefix = int.Parse(args[0]);
                }
                else
                {
                    prefix = 0;
                    string result = Tools.ConvertStringArrayToStringJoin(args);
                    string cleanname = result.Replace("\"", "").Replace(".", "").ToLower();
                    //string lowercase = Char.ToLowerInvariant(cleanname[0]) + cleanname.Substring(1);
                    string uppercase = Char.ToUpperInvariant(cleanname[0]) + cleanname.Substring(1);

                    for (int k = 1; k < Lang.prefix.Length; k++)
                    {
                        if (Lang.prefix[k] != cleanname & Lang.prefix[k] != uppercase) continue;
                        prefix = k;
                        break;
                    }
                }

                if (prefix < 1 || prefix > Lang.prefix.Length) throw new NumericException();

                byte curPrefix = item.prefix;
                item.Prefix(prefix);
                //player.inventory[item].prefix = (byte)prefix; <-- not preferred since it'll work for non-usable prefixes on items as well
                if (item.prefix != prefix)
                {
                    item.prefix = (byte)curPrefix;
                    Main.NewText("Selected item is not compatible with the chosen prefix.", 255, 0, 0);
                }
                else item.prefix = (byte)prefix;

            }
            catch (NumericException)
            {
                Main.NewText("Error: prefix not found or prefix id invalid (valid: 1-83)", 255, 255, 0);
                return;
            }
            catch (Exception e)
            {
                WriteCommand(command, count);
                Main.NewText(e.ToString());
                return;
            }
            
            /*Player player = Main.player[Main.myPlayer];
            int itemType = player.selectedItem, pre = 0;
            if (args.Length < 1)
            {
                WriteCommand(command, count);
                return;
            }
            else if (args.Length == 1 && !int.TryParse(args[0], out pre)) // Name
            {
                string result = Tools.ConvertStringArrayToStringJoin(args); // Array to string in format "Item.Name.With.Spaces".AmountToSpawn or "Item.Name.With.Spaces".
                string cleanname = result.Replace("\"", "").Replace(".", "");

                for (int k = 1; k < Lang.prefix.Length; k++) // Look for item with name
                {
                    if (Lang.prefix[k] == cleanname)
                    {
                        pre = k;
                        break;
                    }
                }
            }
            else if (int.TryParse(args[0], out pre))
            {
                pre = int.Parse(args[0]);
            }
            else
            {
                pre = 0;
            }

            if (pre == 0 || pre < 1 || pre > 83)  // prefix no guud
            {
                Main.NewText("Prefix was invalid! (Not possible on item or pre==0) Error: 2", 255, 0, 0);
                return;
            }
            else
            {
                byte curPre = player.inventory[itemType].prefix;
                player.inventory[itemType].Prefix(pre);
                if (player.inventory[itemType].prefix != pre)
                {
                    player.inventory[itemType].prefix = (byte)curPre;
                    Main.NewText("Selected item is not compatible with the chosen prefix.", 255, 0, 0);
                }
            } 
            */




            /*
       Lang.prefix[1] = "Large";
       Lang.prefix[2] = "Massive";
       Lang.prefix[3] = "Dangerous";
       Lang.prefix[4] = "Savage";
       Lang.prefix[5] = "Sharp";
       Lang.prefix[6] = "Pointy";
       Lang.prefix[7] = "Tiny";
       Lang.prefix[8] = "Terrible";
       Lang.prefix[9] = "Small";
       Lang.prefix[10] = "Dull";
       Lang.prefix[11] = "Unhappy";
       Lang.prefix[12] = "Bulky";
       Lang.prefix[13] = "Shameful";
       Lang.prefix[14] = "Heavy";
       Lang.prefix[15] = "Light";
       Lang.prefix[16] = "Sighted";
       Lang.prefix[17] = "Rapid";
       Lang.prefix[18] = "Hasty";
       Lang.prefix[19] = "Intimidating";
       Lang.prefix[20] = "Deadly";
       Lang.prefix[21] = "Staunch";
       Lang.prefix[22] = "Awful";
       Lang.prefix[23] = "Lethargic";
       Lang.prefix[24] = "Awkward";
       Lang.prefix[25] = "Powerful";
       Lang.prefix[58] = "Frenzying";
       Lang.prefix[26] = "Mystic";
       Lang.prefix[27] = "Adept";
       Lang.prefix[28] = "Masterful";
       Lang.prefix[29] = "Inept";
       Lang.prefix[30] = "Ignorant";
       Lang.prefix[31] = "Deranged";
       Lang.prefix[32] = "Intense";
       Lang.prefix[33] = "Taboo";
       Lang.prefix[34] = "Celestial";
       Lang.prefix[35] = "Furious";
       Lang.prefix[52] = "Manic";
       Lang.prefix[36] = "Keen";
       Lang.prefix[37] = "Superior";
       Lang.prefix[38] = "Forceful";
       Lang.prefix[53] = "Hurtful";
       Lang.prefix[54] = "Strong";
       Lang.prefix[55] = "Unpleasant";
       Lang.prefix[39] = "Broken";
       Lang.prefix[40] = "Damaged";
       Lang.prefix[56] = "Weak";
       Lang.prefix[41] = "Shoddy";
       Lang.prefix[57] = "Ruthless";
       Lang.prefix[42] = "Quick";
       Lang.prefix[43] = "Deadly";
       Lang.prefix[44] = "Agile";
       Lang.prefix[45] = "Nimble";
       Lang.prefix[46] = "Murderous";
       Lang.prefix[47] = "Slow";
       Lang.prefix[48] = "Sluggish";
       Lang.prefix[49] = "Lazy";
       Lang.prefix[50] = "Annoying";
       Lang.prefix[51] = "Nasty";
       Lang.prefix[59] = "Godly";
       Lang.prefix[60] = "Demonic";
       Lang.prefix[61] = "Zealous";
       Lang.prefix[62] = "Hard";
       Lang.prefix[63] = "Guarding";
       Lang.prefix[64] = "Armored";
       Lang.prefix[65] = "Warding";
       Lang.prefix[66] = "Arcane";
       Lang.prefix[67] = "Precise";
       Lang.prefix[68] = "Lucky";
       Lang.prefix[69] = "Jagged";
       Lang.prefix[70] = "Spiked";
       Lang.prefix[71] = "Angry";
       Lang.prefix[72] = "Menacing";
       Lang.prefix[73] = "Brisk";
       Lang.prefix[74] = "Fleeting";
       Lang.prefix[75] = "Hasty";
       Lang.prefix[76] = "Quick";
       Lang.prefix[77] = "Wild";
       Lang.prefix[78] = "Rash";
       Lang.prefix[79] = "Intrepid";
       Lang.prefix[80] = "Violent";
       Lang.prefix[81] = "Legendary";
       Lang.prefix[82] = "Unreal";
       Lang.prefix[83] = "Mythical";
           */
        }
    }
}
