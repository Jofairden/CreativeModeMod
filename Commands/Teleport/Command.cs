using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CreativeMode.Commands
{
    public class Teleport
    {
        // Teleport Command
        // Added: Pre-Alpha R1
        // Locations R1: dungeon, hell
        // Locations R2: spawn(shifted), random(shifted)
        // Locations R3: temple (thanks Eldrazi)
        public static void Run(string[] args, int count)
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
                CreativeMode.WriteCommand(CreativeMode.command, count);
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
                    Extra.Tools.LeaveTeleportDust(player);
                    Extra.Tools.KillPlayerHooks(player);
                    player.Teleport(newPos, 2, 0);
                    player.velocity = Vector2.Zero;
                    Extra.Tools.LeaveTeleportDust(player);
                    Extra.Tools.ReportLocation(player);
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
                    Extra.Tools.ReportLocation(player);
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

            if (!noCalc) newPos = Extra.Tools.CalculateTilePos(player, tileX, tileY);
            else newPos = new Vector2(tileX, tileY);

            Extra.Tools.LeaveTeleportDust(player);
            Extra.Tools.KillPlayerHooks(player);
            flag2 = player.immune;
            num2 = player.immuneTime;
            player.Teleport(newPos, 2, 0);
            player.velocity = Vector2.Zero;
            player.immune = flag2;
            player.immuneTime = num2;
            Extra.Tools.LeaveTeleportDust(player);
            Extra.Tools.ReportLocation(player);
            if (Main.netMode != 2)
                return;
            RemoteClient.CheckSection(player.whoAmI, player.position, 1);
            NetMessage.SendData(65, -1, -1, "", 0, (float)player.whoAmI, newPos.X, newPos.Y, 3, 0, 0);
        }

    }

    namespace Extra
    {
        public partial class Tools
        {
            public static Vector2 CalculateTilePos(Player player, int tileX, int tileY)
            {
                Vector2 returnPos = new Vector2(tileX * 16 + 8 - player.width / 2, tileY * 16 - player.height);
                return returnPos;
            }

            public static void KillPlayerHooks(Player player)
            {
                player.grappling[0] = -1;
                player.grapCount = 0;
                for (int index = 0; index < 1000; ++index)
                {
                    if (Main.projectile[index].active && Main.projectile[index].owner == player.whoAmI && Main.projectile[index].aiStyle == 7)
                        Main.projectile[index].Kill();
                }
            }

            public static void LeaveTeleportDust(Player player)
            {
                for (int index = 0; index < 70; ++index)
                    Main.dust[Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
            }

            public static void ReportLocation(Player player)
            {
                Point playerPos = Utils.ToTileCoordinates(new Vector2(player.position.X, player.position.Y));

                Main.NewText("Pos X:" + playerPos.X + " Y:" + playerPos.Y, 255, 255, 0);
            }
        }
    }
}
