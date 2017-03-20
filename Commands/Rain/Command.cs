using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Rain
    {
        // Rain Command
        // Added: Pre-Alpha R1
        public static void Run(string[] args, int count)
        {
            bool toggle = false, v;
            if (args.Length == 0)
                if (!Main.raining) toggle = true;
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
                Extra.Tools.ChangeRain();
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

    }

    namespace Extra
    {
        public partial class Tools
        {
            // ChangeRain by Terraria
            // Copy needed due to origional being private
            public static void ChangeRain()
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
        }
    }
}
