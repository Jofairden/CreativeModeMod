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

        /* 
            Official Alpha V1.0
            
            [x] All commands are now in their own namespace and folder! hooray
            [x] Made a GitHub repo you scan stroll through: https://github.com/gorateron/CreativeMode
            [x] Fixed the '/npc' not spawning the number of npcs you entered
            [] Added '/itemsound type' to disable an item's sounds! (e.g: /itemsound meowmere)
            [] Rewritten the NPC down command
            [x] Shifted '/stopevents' to '/event stop'
        */
        public static bool ROD = false, GM = false, PEACE = false, SHINE = false, DEV = false, BUILDER = false, TILERANGE = false, FREEZETIME = false, updatedayTime;
        public static int tileRangeX, tileRangeY;
        public static double updateTime;

        // data of commands
        // name, command, slangs, description
        public static string[,] command =
        {
            { "help", "/help", "?", "Shows a list of commands"},
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
            { "stopevents", "/stopevents", "estop", "Stops all ongoing events, including invasions."}, // removed PA3 -> /event (done)
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
            { "stopevents", "event", "stop", "" },
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

        // Write command info
        public static void WriteCommand(string[,] haystack, int needle)
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
                            Commands.Extra.NPC.Run(args, i);
                            break;
                        case 2:
                            Commands.NPCType.Run(args, i);
                            break;
                        case 3:
                            AddTimeCommand(args, i); // removed PA R2 -> time
                            // no change
                            break;
                        case 4:
                            SubTimeCommand(args, i); // removed PA R2 -> time
                            // no change
                            break;
                        case 5:
                            SetTimeCommand(args, i); // removed PA R2 -> time
                            // no change
                            break;
                        case 6:
                            Commands.GiveItem.Run(args, i);
                            break;
                        case 7:
                            Commands.RodOfDiscord.Run(args, i);
                            break;
                        case 8:
                            SetHPCommand(args, i); // removed PA R2 -> stat
                            // no change
                            break;
                        case 9:
                            SetMPCommand(args, i); // removed PA R2 -> stat
                            // no change
                            break;
                        case 10:
                            SetHPMaxCommand(args, i); // removed PA R2 -> stat
                            // no change
                            break;
                        case 11:
                            SetMPMaxCommand(args, i); // removed PA R2 -> stat
                            // no change
                            break;
                        case 12:
                            Commands.Godmode.Run(args, i);
                            break;
                        case 13:
                            Commands.DownNPC.Run(args, i);
                            break;
                        case 14:
                            Commands.Event.Run(args, i);
                            break;
                        case 15:
                            Commands.Rain.Run(args, i);
                            break;
                        case 16:
                            SpawnCommand(args, i); // removed PA R2 -> tp
                            // no change
                            break;
                        case 17:
                            RandomCommand(args, i); // removed PA R2 -> tp
                            // no change
                            break;
                        case 18:
                            Commands.Teleport.Run(args, i);
                            break;
                        case 19:
                            Commands.Butcher.Run(args, i);
                            break;
                        case 20:
                            Commands.Vacuum.Run(args, i);
                            break;
                        case 21:
                            Commands.Peace.Run(args, i);
                            break;
                        case 22:
                            StopEventsCommand(args, i); // removed PA R3 -> event
                            // no change
                            break;
                        case 23:
                            Commands.ItemType.Run(args, i);
                            break;
                        case 24:
                            PositionCommand(args, i);
                            // no change
                            break;
                        case 25:
                            Commands.Shine.Run(args, i);
                            break;
                        case 26:
                            Commands.TileRange.Run(args, i);
                            break;
                        case 27:
                            Commands.Stat.Run(args, i);
                            break;
                        case 28:
                            Commands.Time.Run(args, i);
                            break;
                        case 29:
                            Commands.Builder.Run(args, i);
                            break;
                        case 30:
                            Commands.Itemprefix.Run(args, i);
                            break;
                        case 31:
                            Commands.ItemSound.Run(args, i);
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

        // StopEvents Command
        // Added: Pre-Alpha R1
        private static void StopEventsCommand(string[] args, int count)
        {
            ReportShift(Tools.SliceRow(shifts, 9).ToArray(), Tools.SliceRow(versions, 4).ToArray());
        }

        // Position Command
        // Added: Pre-Alpha R1
        // Returns the coordinates of a player
        private static void PositionCommand(string[] args, int count)
        {
            Commands.Extra.Tools.ReportLocation(Main.player[Main.myPlayer]);
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

        private static void ReportShift(string[] command, string[] version)
        {
            Main.NewText("The command /" + command[0] + " was removed in " + version[0] + ", use /" + command[1] + " " + command[2] + " " + command[3] + " instead.", 255, 255, 0);
        }

    }

}
