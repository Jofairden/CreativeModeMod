using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Achievements;

namespace CreativeMode.Commands
{
    class Event
    {
        // Event Command
        // Added: Pre-Alpha R1
        public static void Run(string[] args, int count)
        {
            if (args.Length == 0)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
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
            else if (cmd == "stop") // stop all
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
        }

    }
}
