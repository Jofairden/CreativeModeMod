using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

namespace CreativeMode.Commands
{
    public class DownNPC
    {
        
        // DownNPC Command
        // Added: Pre-Alpha R1
        public static void Run(string[] args, int count)
        {
            bool toggle, dcur, bnew;
            int call;
            string t = "", bcur = "";
            if (args.Length == 0)
            {
                CreativeMode.WriteCommand(CreativeMode.command, count);
                return;
            }

            // SET
            if (args[0].ToString() == "plantera" || args[0].ToString() == "plant")
            {
                dcur = Terraria.NPC.downedPlantBoss;
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
                    CreativeMode.WriteCommand(CreativeMode.command, count);
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
            if (call == 0) CreativeMode.WriteCommand(CreativeMode.command, count);
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

    }
}
