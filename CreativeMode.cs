using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
// Snippet is no longer used!


/*
        Hey guys, this is Gorateron.
        If you're reading this, hi.
        I'm not done documenting and shortening code, my apologies!
        (you can always ask me on Discord though)
        Have fun!
 */

namespace CreativeMode
{
    public class CreativeMode : Mod
    {
        /*
            Pre-Alpha R3:
            
            GENERAL:
            * Added tModLoader v0.7 support
            * Mod should now be readable using the tModReader application
            * Simplified /help, non-working arguments now just use ! and optional listed arguments are no longer between brackets {}.
            
            BUG FIXES:
            * /giveitem works again and now uses edit distance to guess which item you meant (not perfect)
            * /npc should no longer spawn npcs an incorrect amount of times

            OTHER CHANGES:
            * A lot of code has been moved to ModPlayer
            * I have rewritten almost everything as try-catch which should translate for easier programming for me, and no crashes for you

            GOD MODE:
            * /godmode [true|false] or /gm [true|false] now completely shifted to ModPlayer. Remember that true/false can be omitted for fast use!
            * You now ignore any contact from mobs, deal quadripled damage and suffer from increased critical strikes.
            * Also a neat buff should appear while God mode is active. Eventually I want this to be a crown or divine circlet hovering your character.
            * God mode features:
                * Infinite mana and health pool
                * Infinite flight (24 hours)
                * Quadripled damage
                * 100% more critical strike chance
                * No more damage taken by any source, even spikes
                * No more gore and no more damage sound noises
                * No more aggro drawn
                * Ammo is not consumed
                * Immune to all negative debuffs (buffs with a positive effect such as 'Feral Bite' should still work)

            TILE RANGE COMMAND:
            * /tilerange x [y] or /tr
            * Change your tilerange! Set an x and y value and set nothing to return to default.
            * eg: /tr 999 999 (999x16 = 15984 tiles.. nuff said)

            BUILDER COMMAND:
            * /builder or /bm
            * Now lets you enter build mode! This mode gives you:
                * Infinite flight (24 hours)
                * Infinite inventory stacks (non-depleting stacks)
                * Infinite tilerange

            ITEM PREFIX COMMAND:
            * /itemprefix name|type|"list" or /itempre name|type|"list"
            * This command sets a prefix on your currently selected item! (eg: /itempre 81 this will give 'Legendary')
            * Index ranges from 1 to 83, but you can also enter the name of the prefix! (eg: /itempre legendary)
            * Use /itempre list to see all possible prefixes for your selected item

            ADDITIONS:
            * Added 'day' and 'night' for /time (/time day and /time night)
            * Added a teleport to the temple! Use /tp temple
            * Added edit distance calculation for /item
            * Added /tilerange x y (eg: /tilerange 150 150 use /tilerange or /tr to reset it to your normal values)
            * Added skeletron, brain of cthulhu/eater of worlds and eye of cthulhu to /down or /npcd

            Sorry for the late update, but it is finally here!
            -Gorateron on 15-01-2016
        */

        public static bool ROD = false, GM = false, PEACE = false, SHINE = false, DEV = false, BUILDER = false, TILERANGE = false, FREEZETIME = false, updatedayTime;
        public static int tileRangeX, tileRangeY;
        public static double updateTime;

        // data of commands
        // name, command, slangs, description
        public static string[,] command =
        {
            { "help", "/help", "", "Shows a list of commands"},
            { "npc", "/npc type [~][x] [~][y] [amount]", "", "Spawns an X amount (default:1) of NPCs at a given location (default:player). Coordinates may be preceded by '~' to become relative to the player's position." },
            { "npctype", "/npctype mod npc", "", "Returns the ID of a mod's NPC" },
            { "addTime", "", "", "" }, // removed PA R2 -> time (unfinished)
            { "subTime", "", "", "" }, // removed PA R2 -> time (unfinished)
            { "setTime", "/setTime dayTime numTicks", "", "Sets time (ticks) [t: 0.0 - 54000.0, f: 0.0 - 32400.0]" }, // removed PA R2 -> time (unfinished)
            { "giveitem", "/giveitem type|name [amount] !player]", "", "Gives an item" },
            { "rod", "/rod [true/false]", "", "Toggles 'Chaos State' immunity, preventing you from taking Rod of Discord's damage." },
            { "setHP", "", "shp", "" }, // removed PA R2 -> /stat (unfinished)
            { "setMP", "", "smp", "" }, // removed PA R2 -> /stat (unfinished)
            { "setHPmax", "", "shpm", "" }, // removed PA R2 -> /stat (unfinished)
            { "setMPmax", "", "smpm", "" }, // removed PA R2 -> /stat (unfinished)
            { "godmode", "/godmode [true/false]", "gm", "Toggles an exclusive godmode, granting you extreme powers." },
            { "down", "/down npc [true/false]", "npcd", "Toggles a boss' downed state. " },
            { "event", "/event name", "e", "Forces to start an event, ignoring its requirements. ('goblininvasion', 'bloodmoon', 'slimerain', 'frostlegion', 'frostmoon', 'pumpkinmoon', 'solareclipse', 'pirateinvasion', 'martianmadness', 'lunar')" }, // lunar event'
            { "rain", "/rain [true/false]", "downfall", "Toggles rain on or off." },
            { "spawn", "", "recall", "" }, // removed PA R2 -> /tp (done)
            { "random", "", "", "" }, // removed PA R2 -> /tp (done)
            { "teleport", "/teleport location !player", "tp", "Teleports a player to the specified location. ('temple', 'dungeon', 'hell', 'spawn', 'random')" },
            { "butcher", "/butcher [true/false]", "killall", "Butcher everything. Use 'true' to also butcher town NPCs, use 'false' to ONLY butcher town NPCs." },
            { "vacuum", "/vacuum type|-1 [type2] !player, /vac type|-1 [type2] !player", "vac", "Vacuums the specified item to a player, picking it up automatically. Use '-1' to vacuum everything." },
            { "peace", "/peace [true/false]", "harmony", "Makes the world peaceful, preventing NPCs to from spawning." },
            { "stopevents", "/stopevents", "estop", "Stops all ongoing events, including invasions."},
            { "itemtype", "/itemtype mod item", "moditem", "Returns the ID of a mod's item. e.g: /itemtype ExampleMod ExampleSword" },
            { "position", "/position !player", "pos", "Returns a player's position." },
            { "shine", "/shine [true/false] !player", "", "Makes a player shine as if a shine potion is active." },
            { "tilerange", "/tilerange x [y] !player", "tr", "Modifies a player's tilerange." },
            { "stat", "/stat health|hp|mana|mp|hpmax|hpm|mpmax|mpm !player]", "", "Modifies a player's statistic, such as health or mana." },
            { "time", "/time add|sub|set|day|night numticks [daytime]", "", "Modifies the in-game time. Use 'day' or 'night' for fast use." },
            { "builder", "/builder [true|false]", "bm", "Toggles builder mode. (infinite flight, item stacks and tilerange)" },
            { "itemprefix", "/itemprefix type|name|\"list\"", "itempre", "Sets the prefix for your currently selected item. Use 'list' to display possible prefixes." },
            { "itemsound", "/sound type", "sound", "Enables or disables an item's sound" },
        };

        public static string[,] versions =
        {
            { "Pre-Alpha", "Not released" },
            { "Pre-Alpha R1", "First Alpha Release" },
            { "Pre-Alpha R2", "Second Alpha Release" },
            { "Pre-Alpha R3", "Third Alpha Release" },
            { "Alpha V1.0", "First official Alpha" },
        };

        public static string[,] shifts =
        {
            { "addTime", "time", "add", "numTicks" },
            { "subTime", "time", "sub", "numTicks" },
            { "setTime", "time", "set", "dayTime numTicks" },
            { "setHP", "stat", "hp", "amount" },
            { "setMP", "stat", "mp", "amount" },
            { "setHPmax", "stat", "hpmax", "amount" },
            { "setMPmax", "stat", "mpmax", "amount" },
            { "spawn", "tp", "spawn", "" },
            { "random", "tp", "random", "" },
        };

        public static int commandCount = command.GetLength(0),
                          commandProp = command.GetLength(1);
        public static int versionCount = versions.GetLength(0),
                          versionProp = versions.GetLength(1);
        public static int shiftCount = shifts.GetLength(0),
                          shiftProp = shifts.GetLength(1);

        public override void SetModInfo(out string name, ref ModProperties properties)
        {
            name = "CreativeMode";
            properties.Autoload = true;
            properties.AutoloadGores = true;
            properties.AutoloadSounds = true;
        }

        public override void Load()
        {
            if (DEV)
            {
                ErrorLogger.Log(commandCount.ToString());
                ErrorLogger.Log(versionCount.ToString());
                ErrorLogger.Log(shiftCount.ToString());
            }
        }

        public override void Unload()
        {
        }

        public override void AddCraftGroups()
        {
            
        }

        public override void AddRecipes()
        {
            if (DEV)
            {

            }
            /* for (int i = 0; i < Main.item.Length; i++)
            {
                if (Main.item[i].type > 0)
                {
                    //Recipes.QuickRecipe(null, Main.item[i].type, Main.item[i].maxStack, null, null, TileID.WorkBenches);
                    ModRecipe recipe = new ModRecipe(this);
                    recipe.SetResult(Main.item[i].type);
                    recipe.AddRecipe();
                }
            } */

            ModRecipe recipe = new ModRecipe(this);
            recipe.anyIronBar = true;
        }

        // Write command info
        private static void WriteCommand(string[,] haystack, int needle)
        {
            Main.NewText("Usage: " + haystack[needle, 1]);
            Main.NewText(haystack[needle, 3]);
            if (command[needle, 2] != "") Main.NewText("Command slangs: " + command[needle, 2]);
        }

        public override void ChatInput(string text)
        {
            // Seperate command
            if (text[0] != '/' || text == "/")
            {
                return;
            }
            text = text.Substring(1);
            int index = text.IndexOf(' ');
            string cmd;
            string[] args;
            if (index < 0)
            {
                cmd = text;
                args = new string[0];
            }
            else
            {
                cmd = text.Substring(0, index);
                args = text.Substring(index + 1).Split(' ');
            }

            // Command Check, loop command
            for (int i = 0; i < commandCount; i++)
            {
                if (cmd == command[i, 0] || command[i, 2].Contains(cmd)) // if command == command or is in command slangs
                {
                    switch (i)
                    {
                        case 0:
                            HelpCommand(args);
                            break;
                        case 1:
                            NPCCommand(args, i);
                            break;
                        case 2:
                            NPCTypeCommand(args, i);
                            break;
                        case 3:
                            AddTimeCommand(args, i); // removed PA R2 -> time
                            break;
                        case 4:
                            SubTimeCommand(args, i); // removed PA R2 -> time
                            break;
                        case 5:
                            SetTimeCommand(args, i); // removed PA R2 -> time
                            break;
                        case 6:
                            GiveItemCommand(args, i);
                            break;
                        case 7:
                            RoDCommand(args, i);
                            break;
                        case 8:
                            SetHPCommand(args, i); // removed PA R2 -> stat
                            break;
                        case 9:
                            SetMPCommand(args, i); // removed PA R2 -> stat
                            break;
                        case 10:
                            SetHPMaxCommand(args, i); // removed PA R2 -> stat
                            break;
                        case 11:
                            SetMPMaxCommand(args, i); // removed PA R2 -> stat
                            break;
                        case 12:
                            GodModeCommand(args, i);
                            break;
                        case 13:
                            DownNPCCommand(args, i);
                            break;
                        case 14:
                            EventCommand(args, i);
                            break;
                        case 15:
                            RainCommand(args, i);
                            break;
                        case 16:
                            SpawnCommand(args, i); // removed PA R2 -> tp
                            break;
                        case 17:
                            RandomCommand(args, i); // removed PA R2 -> tp
                            break;
                        case 18:
                            TeleportCommand(args, i);
                            break;
                        case 19:
                            ButcherCommand(args, i);
                            break;
                        case 20:
                            VacuumCommand(args, i);
                            break;
                        case 21:
                            PeaceCommand(args, i);
                            break;
                        case 22:
                            StopEventsCommand(args, i);
                            break;
                        case 23:
                            ItemTypeCommand(args, i);
                            break;
                        case 24:
                            PositionCommand(args, i);
                            break;
                        case 25:
                            ShineCommand(args, i);
                            break;
                        case 26:
                            TileRangeCommand(args, i);
                            break;
                        case 27:
                            StatCommand(args, i);
                            break;
                        case 28:
                            TimeCommand(args, i);
                            break;
                        case 29:
                            BuilderCommand(args, i);
                            break;
                        case 30:
                            ItemPrefixCommand(args, i);
                            break;
                        case 31:
                            ItemSoundCommand(args, i);
                            break;
                        default:
                            HelpCommand(args);
                            break;
                    }
                }
            }
        }

        // Help Command
        // Added: Pre-Alpha R1
        // Displays all current commands with their usage.
        // Shifted commands are ignored because they are usually deprecated
        private void HelpCommand(string[] args)
        {
            try
            {
                for (int i = 0; i < command.GetLength(0); i++)
                {
                    if (Tools.SliceColumn(shifts, 0).ToArray().Contains(command[i, 0]) == false) // not a shifted command
                    {
                        Main.NewText(command[i, 1].ToString());
                    }
                }
                Main.NewText("Use the ARROW-UP and ARROW-DOWN keys to scroll.", 255, 255, 0);
                Main.NewText("Visit the TCF thread for more information and help.", 255, 255, 0);
            }
            catch (Exception e)
            {
                ErrorLogger.Log(e.ToString());
                return;
            }
        }

        // NPC Command
        // Added: Pre-Alpha R1
        // Thanks: BlueMagic123 / Jopojelly
        // Spawns an x amount of NPCs at a given location.
        private void NPCCommand(string[] args, int count)
        {
            int type, x, y, num = 1;
            Player player = Main.player[Main.myPlayer];
            if (args.Length == 0 || !int.TryParse(args[0], out type))
            {
                WriteCommand(command, count);
                return;
            }
            if (args.Length >= 1 && int.TryParse(args[0], out type))
            {
                type = int.Parse(args[0]);

                if (args.Length > 2)
                {
                    bool relativeX = false;
                    bool relativeY = false;
                    if (args[1][0] == '~')
                    {
                        relativeX = true;
                        args[1] = args[1].Substring(1);
                    }
                    if (args[2][0] == '~')
                    {
                        relativeY = true;
                        args[2] = args[2].Substring(1);
                    }
                    if (!int.TryParse(args[1], out x))
                    {
                        x = 0;
                        relativeX = true;
                    }
                    if (!int.TryParse(args[2], out y))
                    {
                        y = 0;
                        relativeY = true;
                    }
                    if (relativeX)
                    {
                        x += (int)player.Bottom.X;
                    }
                    if (relativeY)
                    {
                        y += (int)player.Bottom.Y;
                    }
                    if (args.Length > 3)
                    {
                        if (!int.TryParse(args[3], out num))
                        {
                            num = 1;
                        }
                    }
                }
                else
                {
                    x = (int)player.Bottom.X;
                    y = (int)player.Bottom.Y;
                }
                for (int k = 0; k < num; k++)
                {
                    NPC.NewNPC(x, y, type);
                }
            }
            else
            {
                WriteCommand(command, count);
                return;
            }
        }

        // NPCType Command
        // Added: Pre-Alpha R1
        // Thanks: BlueMagic123 / Jopojelly
        // Returns the ID for an NPC of a mod.
        private void NPCTypeCommand(string[] args, int count)
        {
            if (args.Length < 2)
            {
                WriteCommand(command, count);
                return;
            }
            Mod mod = ModLoader.GetMod(args[0]);
            string message;
            bool isMod = mod == null ? false : true;
            if (!isMod) message = "Given mod is null";
            else message = "NPCType: " + mod.NPCType(args[1]);

            Main.NewText(message, 255, 255, 0);
        }

        // AddTime Command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        // Not my command, just simplified some parts credit: BlueMagic123 / Jopojelly
        private void AddTimeCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 0).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // SubtractTime Command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SubTimeCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 1).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // SetTime Command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SetTimeCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 2).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // GiveItem Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch and fixed in Pre-Alpha R3
        private void GiveItemCommand(string[] args, int count)
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
                WriteCommand(command, count);
                //ErrorLogger.Log(e.ToString());
                return;
            }
            // Thanks Jopojelly :)
        }

        // RoD Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch in Pre-Alpha R3
        private void RoDCommand(string[] args, int count)
        {
            try
            {
                bool toggle = false;
                if (ROD) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                ROD = toggle;
                Main.NewText("'Chaos State' immunity toggled.", 255, 255, 0);
            }
            catch (Exception e)
            {
                WriteCommand(command, count);
                //ErrorLogger.Log(e.ToString());
                return;
            }
            // Handling: Handling.cs
        }

        // Set HP Commmand
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SetHPCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 3).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // Set MP Command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SetMPCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 4).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // Set HPMax Command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SetHPMaxCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 5).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // Set MPMax command
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2
        private void SetMPMaxCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 6).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // GodMode Command
        // Added: Pre-Alpha R1
        // Shifted to ModPlayer in [Pre-Alpha R3] and rewritten as try-catch
        private void GodModeCommand(string[] args, int count)
        {
            try
            {
                bool toggle = false;
                Player player = Main.player[Main.myPlayer];
                ModBuff buff = ModLoader.GetMod("CreativeMode").GetBuff("GodmodeBuff");
                if (GM) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                GM = toggle;
                if (GM)
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
                WriteCommand(command, count);
                //ErrorLogger.Log(e.ToString());
                return;
            }
            // Handling: Godmode.cs
        }

        // DownNPC Command
        // Added: Pre-Alpha R1
        private void DownNPCCommand(string[] args, int count)
        {
            bool toggle, dcur, bnew;
            int call;
            string t = "", bcur = "";
            if (args.Length == 0)
            {
                WriteCommand(command, count);
                return;
            }

            // SET
            if (args[0].ToString() == "plantera" || args[0].ToString() == "plant")
            {
                dcur = NPC.downedPlantBoss;
                call = 1;
                bcur = "Plantera";
            }
            else if (args[0].ToString() == "moonlord" || args[0].ToString() == "moon")
            {
                dcur = NPC.downedMoonlord;
                call = 2;
                bcur = "Moonlord";
            }
            else if (args[0].ToString() == "golem")
            {
                dcur = NPC.downedGolemBoss;
                call = 3;
                bcur = "Golem";
            }
            else if (args[0].ToString() == "cultist" || args[0].ToString() == "cult")
            {
                dcur = NPC.downedAncientCultist;
                call = 4;
                bcur = "Ancient Cultist";
            }
            else if (args[0].ToString() == "thedestroyer" || args[0].ToString() == "destroyer" || args[0].ToString() == "des")
            {
                dcur = NPC.downedMechBoss1;
                call = 5;
                bcur = "The Destroyer";
            }
            else if (args[0].ToString() == "thetwins" || args[0].ToString() == "twins")
            {
                dcur = NPC.downedMechBoss2;
                call = 6;
                bcur = "The Twins";
            }
            else if (args[0].ToString() == "skeletronprime" || args[0].ToString() == "skeletronp" || args[0].ToString() == "sp" || args[0].ToString() == "skellyp")
            {
                dcur = NPC.downedMechBoss3;
                call = 7;
                bcur = "Skeletron Prime";
            }
            else if (args[0].ToString() == "mechanicalbosses" || args[0].ToString() == "mechs" || args[0].ToString() == "m3")
            {
                dcur = NPC.downedMechBossAny;
                call = 8;
                bcur = "All Mechanical Bosses";
            }
            else if (args[0].ToString() == "nebula")
            {
                dcur = NPC.downedTowerNebula;
                call = 9;
                bcur = "Lunar Nebula Tower";
            }
            else if (args[0].ToString() == "solar")
            {
                dcur = NPC.downedTowerSolar;
                call = 10;
                bcur = "Lunar Solar Tower";
            }
            else if (args[0].ToString() == "stardust")
            {
                dcur = NPC.downedTowerStardust;
                call = 11;
                bcur = "Lunar Stardust Tower";
            }
            else if (args[0].ToString() == "vortex")
            {
                dcur = NPC.downedTowerVortex;
                call = 12;
                bcur = "Lunar Vortex Tower";
            }
            else if (args[0].ToString() == "eaterofworlds" || args[0].ToString() == "eow" || args[0].ToString() == "brainofcthulhu" || args[0].ToString() == "boc")
            {
                dcur = NPC.downedBoss2;
                call = 13;
                bcur = "Eater of Worlds / Brain of Cthulhu";
            }
            else if (args[0].ToString() == "skeletron" || args[0].ToString() == "skele" || args[0].ToString() == "skelly")
            {
                dcur = NPC.downedBoss3;
                call = 14;
                bcur = "Skeletron";
            }
            else if (args[0].ToString() == "eyeofcthulhu" || args[0].ToString() == "eye" || args[0].ToString() == "cthulhu")
            {
                dcur = NPC.downedBoss1;
                call = 15;
                bcur = "Eye of Cthulhu";
            }
            else
            {
                call = 0;
                dcur = false;
                bnew = false;
            }

            // Set bnew to arg
            if (args.Length == 2)
            {
                if (bool.TryParse(args[1], out toggle))
                {
                    bnew = toggle;
                }
                else
                {
                    WriteCommand(command, count);
                    return;
                }
            }
            else
            {
                // Inverse
                if (dcur) bnew = false;
                else bnew = true;
            }

            if (bnew) t = "downed";
            else t = "undowned";

            // SET
            if (call == 0) WriteCommand(command, count);
            else if (call == 1) NPC.downedPlantBoss = bnew;
            else if (call == 2) NPC.downedMoonlord = bnew;
            else if (call == 3) NPC.downedGolemBoss = bnew;
            else if (call == 4) NPC.downedAncientCultist = bnew;
            else if (call == 5) NPC.downedMechBoss1 = bnew;
            else if (call == 6) NPC.downedMechBoss2 = bnew;
            else if (call == 7) NPC.downedMechBoss3 = bnew;
            else if (call == 8) NPC.downedMechBossAny = bnew;
            else if (call == 9) NPC.downedTowerNebula = bnew;
            else if (call == 10) NPC.downedTowerSolar = bnew;
            else if (call == 11) NPC.downedTowerStardust = bnew;
            else if (call == 12) NPC.downedTowerVortex = bnew;
            else if (call == 13) NPC.downedBoss2 = bnew;
            else if (call == 14) NPC.downedBoss3 = bnew;
            else if (call == 15) NPC.downedBoss1 = bnew;

            // NOTE
            if (bcur.Length > 0)
            {
                if (call != 8) Main.NewText(bcur + " has been " + t + "!", 255, 255, 0);
                else Main.NewText(bcur + " have been " + t + "!", 255, 255, 0);
            }
        }

        // Event Command
        // Added: Pre-Alpha R1
        private void EventCommand(string[] args, int count)
        {
            if (args.Length == 0)
            {
                WriteCommand(command, count);
                return;
            }
            string cmd = args[0].ToString();

            if (cmd == "goblininvasion" || cmd == "goblins") // Goblins
            {
                if (Main.CanStartInvasion()) Main.StartInvasion();
                else Main.NewText("Unable to start Goblin Invasion", 255, 0, 0);
            }
            else if (cmd == "bloodmoon") // BM
            {
                Player player = Main.player[Main.myPlayer];
                //if (player.statLifeMax2 >= 120 && Main.moonPhase != MoonPhases.FullMoon && !WorldGen.spawnEye && !Main.bloodMoon) //1 = Full Moon
                
                Main.dayTime = false;
                Main.time = 0.0;
                if (Main.moonPhase == 0) Main.moonPhase++;
                WorldGen.spawnEye = false;
                Main.bloodMoon = true;
                AchievementsHelper.NotifyProgressionEvent(4);
                if (Main.netMode == 0)
                    Main.NewText(Lang.misc[8], (byte)50, byte.MaxValue, (byte)130, false);
                else if (Main.netMode == 2)
                    NetMessage.SendData(25, -1, -1, Lang.misc[8], (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                
                //}else Main.NewText("Unable to start Blood Moon", 255, 255, 0);
            }
            else if (cmd == "slimerain") // Slimes
            {
                if (Main.netMode != 1 && !Main.gameMenu || Main.netMode == 2)
                {
                    //(!Main.raining && Main.slimeRainTime == 0.0 && (!Main.bloodMoon && !Main.eclipse) && (!Main.snowMoon && !Main.pumpkinMoon && Main.invasionType == 0))
                    Main.raining = false;
                    Main.slimeRainTime = 0.0;
                    Main.stopMoonEvent(); // pumpkin / snow
                    Main.bloodMoon = false;
                    Main.eclipse = false;
                    Main.invasionType = 0;
                    //Actual
                    Main.StartSlimeRain(true);
                }
            }
            else if (cmd == "frostlegion" || cmd == "snowlegion") // FL / SL
            {
                if (Main.CanStartInvasion(2)) Main.StartInvasion(2);
                else Main.NewText("Unable to start Frost Legion Invasion", 255, 0, 0);
            }
            else if (cmd == "solareclipse") // SE
            {
                // !! REQUIRED
                Main.dayTime = true;
                Main.time = 32400.1;

                if (Main.netMode != 1)
                {
                    AchievementsHelper.NotifyProgressionEvent(1);
                    Main.eclipse = true;
                    AchievementsHelper.NotifyProgressionEvent(2);
                    if (Main.eclipse)
                    {
                        if (Main.netMode == 0)
                        {
                            Main.NewText(Lang.misc[20], (byte)50, byte.MaxValue, (byte)130, false);
                        }
                        else if (Main.netMode == 2)
                        {
                            NetMessage.SendData(25, -1, -1, Lang.misc[20], (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                        }
                    }
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(7, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
            else if (cmd == "pirateinvasion") // Pirates
            {
                if (Main.CanStartInvasion(3)) Main.StartInvasion(3);
                else Main.NewText("Unable to start Pirate Invasion", 255, 0, 0);
            }
            else if (cmd == "pumpkinmoon") // PM
            {
                // REQUIRED
                Main.dayTime = false;
                Main.stopMoonEvent(); 

                if (Main.netMode != 1)
                {
                    Main.NewText(Lang.misc[31], (byte)50, byte.MaxValue, (byte)130, false);
                    Main.startPumpkinMoon();
                }
                else
                {
                    NetMessage.SendData(61, -1, -1, "", Main.player[Main.myPlayer].whoAmI, -4f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
            else if (cmd == "frostmoon") // FM
            {
                // REQUIRED
                Main.dayTime = false;
                Main.stopMoonEvent();
               
                if (Main.netMode != 1)
                {
                    Main.NewText(Lang.misc[34], (byte)50, byte.MaxValue, (byte)130, false);
                    Main.startSnowMoon();
                }
                else
                    NetMessage.SendData(61, -1, -1, "", Main.player[Main.myPlayer].whoAmI, -5f, 0.0f, 0.0f, 0, 0, 0);
            }
            else if (cmd == "martianmadness" || cmd == "martians" || cmd == "mm") // MM
            {
                if (Main.CanStartInvasion(4)) Main.StartInvasion(4);
                else Main.NewText("Unable to start Martian Madness", 255, 0, 0);
            }
            else if (cmd == "lunar") // Lunar
            {
                Main.NewText("Lunar events are not supported yet", 255, 255, 0);
            }
        }

        // Rain Command
        // Added: Pre-Alpha R1
        private void RainCommand(string[] args, int count)
        {
            bool toggle = false, v;
            if (args.Length == 0) if (!Main.raining) toggle = true;
            else if (args.Length > 0 && bool.TryParse(args[0], out v)) toggle = v;
            
            if (toggle)
            {
                int maxValue1 = 86400;
                int maxValue2 = maxValue1 / 24;
                Main.rainTime = Main.rand.Next(maxValue2 * 8, maxValue1);
                if (Main.rand.Next(3) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2);
                if (Main.rand.Next(4) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2 * 2);
                if (Main.rand.Next(5) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2 * 2);
                if (Main.rand.Next(6) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2 * 3);
                if (Main.rand.Next(7) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2 * 4);
                if (Main.rand.Next(8) == 0)
                    Main.rainTime += Main.rand.Next(0, maxValue2 * 5);
                float num = 1f;
                if (Main.rand.Next(2) == 0)
                    num += 0.05f;
                if (Main.rand.Next(3) == 0)
                    num += 0.1f;
                if (Main.rand.Next(4) == 0)
                    num += 0.15f;
                if (Main.rand.Next(5) == 0)
                    num += 0.2f;
                Main.rainTime = (int)((double)Main.rainTime * (double)num);
                ChangeRain();
                Main.raining = true;
                Main.NewText("Rain toggled ON", 255, 255, 0);
            }
            else
            {
                Main.rainTime = 0;
                Main.raining = false;
                Main.maxRaining = 0.0f;
                Main.NewText("Rain toggled OFF", 255, 255, 0);
            }
        }

        // ChangeRain by Terraria
        // Copy needed due to origional being private
        private static void ChangeRain()
        {
            if ((double)Main.cloudBGActive >= 1.0 || (double)Main.numClouds > 150.0)
            {
                if (Main.rand.Next(3) == 0)
                    Main.maxRaining = (float)Main.rand.Next(20, 90) * 0.01f;
                else
                    Main.maxRaining = (float)Main.rand.Next(40, 90) * 0.01f;
            }
            else if ((double)Main.numClouds > 100.0)
            {
                if (Main.rand.Next(3) == 0)
                    Main.maxRaining = (float)Main.rand.Next(10, 70) * 0.01f;
                else
                    Main.maxRaining = (float)Main.rand.Next(20, 60) * 0.01f;
            }
            else if (Main.rand.Next(3) == 0)
                Main.maxRaining = (float)Main.rand.Next(5, 40) * 0.01f;
            else
                Main.maxRaining = (float)Main.rand.Next(5, 30) * 0.01f;
        }

        // Spawn Command
        // To be: /tp spawn (done)
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2 (done)
        private static void SpawnCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 7).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // Random Command
        // To be: /tp random (done)
        // Added: Pre-Alpha R1
        // Removed: Pre-Alpha R2 (done)
        private static void RandomCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 8).ToArray(), Tools.SliceRow(versions, 2).ToArray());
        }

        // Teleport Command
        // Added: Pre-Alpha R1
        // Locations R1: dungeon, hell
        // Locations R2: spawn(shifted), random(shifted)
        // Locations R3: temple (thanks Eldrazi)
        private static void TeleportCommand(string[] args, int count)
        {
            string[] locations =
            {
                "dungeon", "hell", "spawn", "random", "temple"
            };

            Player player = Main.player[Main.myPlayer];
            int tileX = (int)player.position.X, 
                tileY = (int)player.position.Y, val;
            Vector2 newPos = new Vector2((float)tileX, (float)tileY);

            bool flag1 = false, 
                 flag2, 
                 noCalc = false;
            int num1 = 0,
                num2 = 0,
                num3 = 0,
                Width = player.width;

            if ((args.Length == 0) || 
                (args.Length == 1 && !locations.Contains(args[0])) || 
                (args.Length == 2 && (!int.TryParse(args[0], out val) || !int.TryParse(args[1], out val))))
            {
                WriteCommand(command, count);
                return;
            }

            if (args.Length == 1)
            {
                if (args[0] == locations[0]) // DUNGEON
                {
                    tileX = Main.dungeonX;
                    tileY = Main.dungeonY;
                }
                else if (args[0] == locations[1]) // HELL
                {
                    Vector2 Position = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-(double)Width / 2.0 + 8.0), -(float)player.height);
                    while (!flag1 && num1 < 1000)
                    {
                        ++num1;
                        int index1 = Main.rand.Next(Main.maxTilesX - 200);
                        int index2 = Main.rand.Next(Main.maxTilesY - 200, Main.maxTilesY);
                        Position = new Vector2((float)index1, (float)index2) * 16f + new Vector2((float)(-(double)Width / 2.0 + 8.0), -(float)player.height);
                        if (!Collision.SolidCollision(Position, Width, player.height))
                        {
                            if (Main.tile[index1, index2] == null)
                                Main.tile[index1, index2] = new Tile();
                            if (((int)Main.tile[index1, index2].wall != 87 || (double)index2 <= Main.worldSurface || NPC.downedPlantBoss) && (!Main.wallDungeon[(int)Main.tile[index1, index2].wall] || (double)index2 <= Main.worldSurface || NPC.downedBoss3))
                            {
                                int num4 = 0;
                                while (num4 < 100)
                                {
                                    if (Main.tile[index1, index2 + num4] == null)
                                        Main.tile[index1, index2 + num4] = new Tile();
                                    Tile tile = Main.tile[index1, index2 + num4];
                                    Position = new Vector2((float)index1, (float)(index2 + num4)) * 16f + new Vector2((float)(-(double)Width / 2.0 + 8.0), -(float)player.height);
                                    Vector4 vector4 = Collision.SlopeCollision(Position, player.velocity, Width, player.height, player.gravDir, false);
                                    flag2 = !Collision.SolidCollision(Position, Width, player.height);
                                    if ((double)vector4.Z == (double)player.velocity.X)
                                    {
                                        double num5 = (double)player.velocity.Y;
                                    }
                                    if (flag2)
                                        ++num4;
                                    else if (!tile.active() || tile.inActive() || !Main.tileSolid[(int)tile.type])
                                        ++num4;
                                    else
                                        break;
                                }
                                if (!Collision.LavaCollision(Position, Width, player.height) && (double)Collision.HurtTiles(Position, player.velocity, Width, player.height, false).Y <= 0.0)
                                {
                                    Collision.SlopeCollision(Position, player.velocity, Width, player.height, player.gravDir, false);
                                    if (Collision.SolidCollision(Position, Width, player.height) && num4 < 99)
                                    {
                                        Vector2 Velocity1 = Vector2.UnitX * 16f;
                                        if (!(Collision.TileCollision(Position - Velocity1, Velocity1, player.width, player.height, false, false, (int)player.gravDir) != Velocity1))
                                        {
                                            Vector2 Velocity2 = -Vector2.UnitX * 16f;
                                            if (!(Collision.TileCollision(Position - Velocity2, Velocity2, player.width, player.height, false, false, (int)player.gravDir) != Velocity2))
                                            {
                                                Vector2 Velocity3 = Vector2.UnitY * 16f;
                                                if (!(Collision.TileCollision(Position - Velocity3, Velocity3, player.width, player.height, false, false, (int)player.gravDir) != Velocity3))
                                                {
                                                    Vector2 Velocity4 = -Vector2.UnitY * 16f;
                                                    if (!(Collision.TileCollision(Position - Velocity4, Velocity4, player.width, player.height, false, false, (int)player.gravDir) != Velocity4))
                                                    {
                                                        flag1 = true;
                                                        int num5 = index2 + num4;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    if (!flag1)
                        return;
                    newPos = Position;
                    LeaveTeleportDust(player);
                    KillPlayerHooks(player);
                    player.Teleport(newPos, 2, 0);
                    player.velocity = Vector2.Zero;
                    LeaveTeleportDust(player);
                    ReportLocation(player);
                    if (Main.netMode != 2)
                        return;
                    RemoteClient.CheckSection(player.whoAmI, player.position, 1);
                    NetMessage.SendData(65, -1, -1, "", 0, (float)player.whoAmI, newPos.X, newPos.Y, 3, 0, 0);
                    return;
                }
                else if (args[0] == locations[2]) // SPAWN
                {
                    if (player.SpawnX >= 0 && player.SpawnY >= 0)
                    {
                        tileX = (player.SpawnX * 16 + 8 - player.width / 2);
                        tileY = (player.SpawnY * 16 - player.height);
                    }
                    else
                    {
                        tileX = (Main.spawnTileX * 16 + 8 - player.width / 2);
                        tileY = (Main.spawnTileY * 16 - player.height);
                        for (int i = Main.spawnTileX - 1; i < Main.spawnTileX + 2; ++i)
                        {
                            for (int j = Main.spawnTileY - 3; j < Main.spawnTileY; ++j)
                            {
                                if (Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
                                    WorldGen.KillTile(i, j, false, false, false);
                                if ((int)Main.tile[i, j].liquid > 0)
                                {
                                    Main.tile[i, j].lava(false);
                                    Main.tile[i, j].liquid = (byte)0;
                                    WorldGen.SquareTileFrame(i, j, true);
                                }
                            }
                        }
                    }
                    noCalc = true;
                }
                else if (args[0] == locations[3]) // RANDOM
                {
                    player.TeleportationPotion(); // RND
                    ReportLocation(player);
                    if (Main.netMode != 2)
                        return;
                    RemoteClient.CheckSection(player.whoAmI, player.position, 1);
                    NetMessage.SendData(65, -1, -1, "", 0, (float)player.whoAmI, newPos.X, newPos.Y, 3, 0, 0);
                    return;
                }
                else if (args[0] == locations[4]) // TEMPLE GOLEM 
                {
                    Vector2 pos = newPos;
                    for (int x = 0; x < Main.tile.GetLength(0); ++x) // LOOP WORLD X
                    {
                        for (int y = 0; y < Main.tile.GetLength(1); ++y) // LOOP WORLD Y
                        {
                            if (Main.tile[x, y] == null) continue;
                            if (Main.tile[x, y].type != 237) continue;
                            //if (Main.tile[x, y].wall != 87) continue;
                            pos = new Vector2((x + 2) * 16, y * 16); // get temple pos
                            break;
                        }
                    }
                    if (pos != newPos)
                    {
                        //Main.player[Main.myPlayer].Teleport(telepos);
                        tileX = (int)pos.X;
                        tileY = (int)pos.Y;
                        noCalc = true;
                    }
                    else return;
                    // thanks Eldrazi for helping :)
                }
            }
            else if (args.Length == 2) // PARSE VALUES
            {
                tileX = int.Parse(args[0]);
                tileY = int.Parse(args[1]);
            }

            if (!noCalc) newPos = CalculateTilePos(player, tileX, tileY);
            else newPos = new Vector2(tileX, tileY);

            LeaveTeleportDust(player);
            KillPlayerHooks(player);
            flag2 = player.immune;
            num2 = player.immuneTime;
            player.Teleport(newPos, 2, 0);
            player.velocity = Vector2.Zero;
            player.immune = flag2;
            player.immuneTime = num2;
            LeaveTeleportDust(player);
            ReportLocation(player);
            if (Main.netMode != 2)
                return;
            RemoteClient.CheckSection(player.whoAmI, player.position, 1);
            NetMessage.SendData(65, -1, -1, "", 0, (float)player.whoAmI, newPos.X, newPos.Y, 3, 0, 0);
        }

        // Butcher Command
        // Added: Pre-Alpha R1
        // Pre-Alpha R3: Added the option to also butcher town NPCs
        private static void ButcherCommand(string[] args, int count)
        {
            /* 
                Iterate through all npcs, and butcher all the active ones
            */
            int npcCount = 0;
            bool incTown = false;
            if (args.Length == 1 && bool.TryParse(args[0], out incTown))
            {
                incTown = bool.Parse(args[0]);
            }
            for (int i = 0; i < Main.npc.Length; i++) // Iteration
            {
                if (Main.npc[i].active && Main.npc[i].type != NPCID.TargetDummy)
                {
                    if (!incTown && Main.npc[i].townNPC) continue;
                    Main.npc[i].StrikeNPCNoInteraction(Main.npc[i].lifeMax, 0f, 1, true); // NoInteraction to avoid Banners being hit
                    npcCount++;
                }
            }
            Main.NewText("Butchered " + npcCount + " NPCs", 255, 255, 0);
        }

        // Vacuum Command
        // Added: Pre-Alpha R1
        // Fixes: PA R2 fixed vacuumed items count
        private static void VacuumCommand(string[] args, int count)
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
                WriteCommand(command, count);
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

        // Peace Command
        // Added: Pre-Alpha R1
        // Requires: ModPlayer support
        private static void PeaceCommand(string[] args, int count)
        {
            try
            {
                bool toggle;
                if (PEACE) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                PEACE = toggle;
                Main.NewText("Peace mode toggled.", 255, 255, 0);
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
            // Handling: Handling.cs
        }

        // StopEvents Command
        // Added: Pre-Alpha R1
        private static void StopEventsCommand(string[] args, int count)
        {
            //Reset invasions
            Main.invasionType = 0;
            Main.invasionX = 0.0;
            Main.invasionSize = 0;
            Main.invasionDelay = 0;
            Main.invasionWarn = 0;
            Main.invasionSizeStart = 0;
            Main.invasionProgressNearInvasion = false;
            Main.invasionProgressMode = 2;
            Main.invasionProgressIcon = 0;
            Main.invasionProgress = 0;
            Main.invasionProgressMax = 0;
            Main.invasionProgressWave = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0.0f;
            //Reset others
            Main.stopMoonEvent(); // pumpkin / snow - moon
            Main.eclipse = false;
            Main.StopSlimeRain();
            Main.bloodMoon = false;

            Main.NewText("All events stopped and reset", 255, 255, 0);
        }

        // ItemType Command
        // Added: Pre-Alpha R1
        // Rewritten as try-catch in Pre-Alpha R3
        // Returns the ID of the given mod's item
        private static void ItemTypeCommand(string[] args, int count)
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
                WriteCommand(command, count);
                return;
            }
        }

        // Position Command
        // Added: Pre-Alpha R1
        // Returns the coordinates of a player
        private static void PositionCommand(string[] args, int count)
        {
            ReportLocation(Main.player[Main.myPlayer]);
        }

        // Shine Command
        // Added: Pre-Alpha R1
        // Requires: ModPlayer support
        // Rewritten as try-catch in Pre-Alpha R3
        private static void ShineCommand(string[] args, int count)
        {
            try
            {
                bool toggle;
                if (SHINE) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    toggle = bool.Parse(args[0]);
                }
                SHINE = toggle;
                Main.NewText("Shine toggled.", 255, 255, 0);
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
            // Handling: Handling.cs
        }

        // TileRange Command
        // Added: Pre-Alpha R3
        // Rewritten as try-catch in Pre-Alpha R3
        private static void TileRangeCommand(string[] args, int count)
        {
            try
            {
                int x = Player.tileRangeX, y = Player.tileRangeY;
                if (args.Length == 0) {
                    TILERANGE = false;
                    return;
                }
                x = int.Parse(args[0]);
                if (args.Length > 1) y = int.Parse(args[1]);
                tileRangeX = x;
                tileRangeY = y;
                TILERANGE = true;
                Main.NewText("Tilerange X: " + x.ToString() + " Tilerange Y: " + y.ToString(), 255, 255, 0);
            }
            catch (NumericException)
            {
                Main.NewText("Error: values must range from 0 to 999", 255, 255, 0);
                return;
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
        }

        // Stat Command
        // Added: Pre-Alpha R2
        // Rewritten as try-catch in Pre-Alpha R3
        private static void StatCommand(string[] args, int count)
        {
            string[] stat =
            {
                "health", "hp", "mana", "mp", "hpmax", "mpmax", "hpm", "mpm"
            };

            try
            {
                Player player = Main.player[Main.myPlayer];
                string arg = args[0];
                int value = int.Parse(args[1]);

                int minHP = 100, minMP = 20, maxHP = 500, maxMP = 240;
                if (stat.Contains(arg))
                {
                    // statLife
                    if (arg == stat[0] || arg == stat[1])
                    {
                        if (value > 0 & value <= player.statLifeMax & value <= maxHP)
                        {
                            player.statLife = value;
                        }
                        else throw new NumericException();
                    }
                    // statMana
                    else if (arg == stat[2] || arg == stat[3])
                    {
                        if (value > 0 & value <= player.statManaMax & value <= maxMP)
                        {
                            player.statMana = value;
                        }
                        else throw new NumericException();
                    }
                    // statLifeMax
                    else if (arg == stat[4] || arg == stat[5])
                    {
                        if (value > minHP & value <= maxHP)
                        {
                            player.statLifeMax = value;
                        }
                        else throw new NumericException();
                    }
                    // statManaMax
                    else if (arg == stat[6] || arg == stat[7])
                    {
                        if (value > minMP & value <= maxMP)
                        {
                            player.statManaMax = value;
                        }
                        else throw new NumericException();
                    }
                }
                else throw new ArgumentException();
            }
            catch (NumericException){
                Main.NewText("Passed value was invalid.", 255, 255, 0);
                return;
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
        }

        // Time Command
        // Added: Pre-Alpha R2
        // Added a day and night shortcur in Pre-Alpha R3
        // An intermediate way of controlling the in-game time.
        // Rewritten as try-catch in Pre-Alpha R3

        /*
        Used in conjuction with Main.dayTime to determine time of day. (Terraria v. 1.1.2)
        If Main.dayTime == true, Main.time goes from 0.0 (dawn) to 54000.0 (nightfall) and resets.
        If Main.dayTime == false, Main.time goes from 0.0 (nightfall) to 32400.0 (dawn) and resets. 16200.0 (midnight) is in the middle. 
        */
        private static void TimeCommand(string[] args, int count)
        {
            string[] time =
            {
                "add", "sub", "set", "day", "night", "freeze"
            };

            try
            {
                string arg = args[0];
                if (time.Contains(arg))
                {
                    if (arg == time[0]) // add
                    {
                        Main.time += int.Parse(args[1]);
                    }
                    else if (arg == time[1]) // subtract
                    {
                        Main.time -= int.Parse(args[1]);
                    }
                    else if (arg == time[2]) // set
                    {
                        if (args.Length > 2)
                        {
                            Main.dayTime = bool.Parse(args[2]);
                        }
                        Main.time = int.Parse(args[1]);
                    }
                    else if (arg == time[3]) // day
                    {
                        Main.dayTime = true;
                        Main.time = 24000.0;
                    }
                    else if (arg == time[4]) // night
                    {
                        Main.dayTime = false;
                        Main.time = 16200.0;
                    }
                    else if (arg == time[5]) // freeze
                    {
                        FREEZETIME = true;
                        updatedayTime = Main.dayTime;
                        updateTime = Main.time;
                    }
                }
                else throw new ArgumentException();
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
        }

        // Builder Command
        // Added: Pre-Alpha R2
        // Rewritten as try-catch in Pre-Alpha R3
        private static void BuilderCommand(string[] args, int count)
        {
            try
            {
                bool toggle;
                if (BUILDER) toggle = false;
                else toggle = true;
                if (args.Length > 0)
                {
                    BUILDER = bool.Parse(args[0]);
                }
                BUILDER = toggle;
                Main.NewText("Builder mode toggled.", 255, 255, 0);
            }
            catch
            {
                WriteCommand(command, count);
                return;
            }
        }

        // ItemPrefix Command
        // Added: Pre-Alpha R3
        // Rewritten as try-catch in Pre-Alpha R3
        // Added a list function in Pre-Alpha R3, to list all compatible prefixes for the selected weapon
        private static void ItemPrefixCommand(string[] args, int count)
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
                WriteCommand(command, count);
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

        // ItemSound Command
        // Added: Alpha V1.0
        private static void ItemSoundCommand(string[] args, int count)
        {
            try
            {
                int r;
                if (args.Length < 1 || int.TryParse(args[0], out r)) throw new NumericException();

                int soundType = int.Parse(args[0]);
                
            }
            catch (Exception e)
            {
                Main.NewText(e.ToString());
                return;
            }
            
        }


        /* private void pumpkinSword(int i, int dmg, float kb)
        {
            int num1 = Main.rand.Next(100, 300);
            int num2 = Main.rand.Next(100, 300);
            int num3 = Main.rand.Next(2) != 0 ? num1 + (Main.maxScreenW / 2 - num1) : num1 - (Main.maxScreenW / 2 + num1);
            int num4 = Main.rand.Next(2) != 0 ? num2 + (Main.maxScreenH / 2 - num2) : num2 - (Main.maxScreenH / 2 + num2);
            int num5 = num3 + (int)this.position.X;
            int num6 = num4 + (int)this.position.Y;
            float num7 = 8f;
            Vector2 vector2 = new Vector2((float)num5, (float)num6);
            float num8 = Main.npc[i].position.X - vector2.X;
            float num9 = Main.npc[i].position.Y - vector2.Y;
            float num10 = (float)Math.Sqrt((double)num8 * (double)num8 + (double)num9 * (double)num9);
            float num11 = num7 / num10;
            float SpeedX = num8 * num11;
            float SpeedY = num9 * num11;
            Projectile.NewProjectile((float)num5, (float)num6, SpeedX, SpeedY, 321, dmg, kb, this.whoAmI, (float)i, 0.0f);
        }*/

        private static Vector2 CalculateTilePos(Player player, int tileX, int tileY)
        {
            Vector2 returnPos = new Vector2(tileX * 16 + 8 - player.width / 2, tileY * 16 - player.height);
            return returnPos;
        }

        private static void KillPlayerHooks(Player player)
        {
            player.grappling[0] = -1;
            player.grapCount = 0;
            for (int index = 0; index < 1000; ++index)
            {
                if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI && Main.projectile[index].aiStyle == 7)
                    Main.projectile[index].Kill();
            }
        }

        private static void LeaveTeleportDust(Player player)
        {
            for (int index = 0; index < 70; ++index)
                Main.dust[Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
        }

        private static void ReportLocation(Player player)
        {
            Point playerPos = Utils.ToTileCoordinates(new Vector2(player.position.X, player.position.Y));

            Main.NewText("Pos X:" + playerPos.X + " Y:" + playerPos.Y, 255, 255, 0);
        }

        private static void ReportShift(string[] command, string[] version)
        {
            Main.NewText("The command /" + command[0] + " was removed in " + version[0] + ", use /" + command[1] + " " + command[2] + " " + command[3] + " instead.", 255, 255, 0);
        }

    }

}
