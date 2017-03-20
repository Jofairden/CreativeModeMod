using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CreativeMode.ModHelper
{
    public static class Gen
    {
        [ThreadStatic]
        public static Random rand = new Random();
    }

    public class Math
    {
        public const double PI = System.Math.PI;
        public const double RAD = 0.0174532925;
        public const int maxDegrees = 360;

        public double toRadians(float degrees)
        {
            return degrees * RAD;
        }

        public double toRadians(double degrees)
        {
            return toRadians((float)degrees);
        }

        public double toRadians(decimal degrees)
        {
            return toRadians((float)degrees);
        }

        public double percDegrees(float degrees)
        {
            return degrees / maxDegrees * 100;
        }

        public double percDegrees(double degrees)
        {
            return percDegrees((float)degrees);
        }

        public double percDegrees(decimal degrees)
        {
            return percDegrees((float)degrees);
        }
        
        public double circumferenceCircle(float radius)
        {
            return 2 * PI * radius;
        }

        public double circumferenceCircle(double radius)
        {
            return circumferenceCircle((float)radius);
        }

        public double circumferenceCircle(decimal radius)
        {
            return circumferenceCircle((float)radius);
        }
    }

    public class Tiles
    {
        public Vector2 ToTileCoordinates(Vector2 pos)
        {
            return pos / 16f;
        }

        public Vector2 ToTileCoordinates(int x, int y)
        {
            return ToTileCoordinates(new Vector2((float)x, (float)y));
        }

        public Vector2 ToTileCoordinates(float x, float y)
        {
            return ToTileCoordinates(new Vector2(x, y));
        }

        public Vector2 ToPixelCoordinates(Vector2 pos)
        {
            return pos * 16f;
        }

        public Vector2 ToPixelCoordinates(int x, int y)
        {
            return ToPixelCoordinates(new Vector2((float)x, (float)y));
        }

        public Vector2 ToPixelCoordinates(float x, float y)
        {
            return ToPixelCoordinates(new Vector2(x, y));
        }
    }

    public class String
    {
        public const double cVar = 2 / 3;

        public Vector2 CalcBaseToDir(int dir, string text, Vector2 pos, Microsoft.Xna.Framework.Graphics.SpriteFont spritefont, int useSpace = 0, double cvar = cVar)
        {
            Vector2 newPos;
            switch (dir)
            {
                case 0: // n
                    newPos = new Vector2((float)pos.X, (float)(pos.Y - spritefont.MeasureString(text).Y));
                    newPos.Y *= useSpace > 0 ? (float)cvar : 1;
                    return newPos;
                case 1: // e
                    newPos = new Vector2((float)(pos.X + spritefont.MeasureString(text).X), (float)pos.Y);
                    newPos.X *= useSpace > 0 ? (float)cvar : 1;
                    return newPos;
                case 2: // s
                    newPos = new Vector2((float)pos.X, (float)(pos.Y + spritefont.MeasureString(text).Y));
                    newPos.Y *= useSpace > 0 ? (float)cvar : 1;
                    return newPos;
                case 3: // w
                    newPos = new Vector2((float)(pos.X - spritefont.MeasureString(text).X), (float)pos.Y);
                    newPos.X *= useSpace > 0 ? (float)cvar : 1;
                    return newPos;
                default:
                    return Vector2.Zero;
            }
        }
    }

    public class Projectile
    {
        /*
            Example use:

                int amount = 5;
                Vector2[] speeds = ModHelper.Projectile.evenArc(speedX, speedY, 45, amount);
                for (int i; i < amount; ++i)
                {
                    Terraria.Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, item.shoot, damage, knockBack, item.owner)
                }
        */

        public static Vector2[] evenSpread (float speedX,  float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * Math.PI);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double startAngle = System.Math.Atan2(speedX, speedY) - spread / 2;
            double deltaAngle = spread / (float)num;
            double offsetAngle;
            for (int i = 0; i < num; ++i)
            {
                offsetAngle = startAngle + deltaAngle * i;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(offsetAngle), baseSpeed * (float)System.Math.Cos(offsetAngle));
            }
            return (Vector2[])posArray;
        }

        public static Vector2[] evenSpread2 (float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float rotation = MathHelper.ToRadians(45);
            for (int i = 0; i < num; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (num - 1)));
                posArray[i] = perturbedSpeed;
            }
            return (Vector2[])posArray;
        }

        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * Math.PI);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }
    }
}
