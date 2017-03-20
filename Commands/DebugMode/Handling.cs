using System;

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeMode.Commands.Handling.DebugMode
{
    public class DebugModeGlobalNPC : GlobalNPC 
    {
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (CreativeMode.DM)
            {
                try
                {
                    if ((!CreativeMode.DM1 && npc.friendly && !CreativeMode.DM2) || !npc.active)
                    {
                        return;
                    }

                    Vector2 center = npc.Center - Main.screenPosition;
                    Vector2 pos = npc.position;
                    int type = npc.type;
                    int index = npc.whoAmI;
                    int ai = npc.aiStyle;
                    float ai0 = npc.ai[0];
                    float ai1 = npc.ai[1];
                    float lai0 = npc.localAI[0];
                    float lai1 = npc.localAI[1];
                    int hp = npc.life;
                    int hpm = npc.lifeMax;
                    int dir = npc.direction;

                    string namet = npc.name;
                    string typet = string.Format("type: {0}", type);
                    string ait = string.Format("aiStyle: {0}[{1}][{2}][{3}][{4}]", ai, ai0, ai1, lai0, lai1);
                    string indext = string.Format("index: {0}", index);
                    string hpt = string.Format("{0}/{1}", hp, hpm);
                    string post = string.Format("pos: {0}x / {1}y", pos.X, pos.Y);
                    string dirt = (dir == 1) ? ">" : "<";

                    float cVar = (float)CreativeMode.cVar;

                    Vector2 cbase = new Vector2(center.X - npc.width * npc.scale * 2 / 16f, center.Y - npc.height / 2);

                    spriteBatch.DrawString(Main.fontMouseText, typet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(typet).X / 2, cbase.Y - Main.fontMouseText.MeasureString(typet).Y), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, ait, new Vector2(cbase.X - Main.fontMouseText.MeasureString(ait).X / 2, cbase.Y - Main.fontMouseText.MeasureString(ait).Y  - (Main.fontMouseText.MeasureString(ait).Y * cVar * 1)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, indext, new Vector2(cbase.X - Main.fontMouseText.MeasureString(indext).X / 2, cbase.Y - Main.fontMouseText.MeasureString(indext).Y - (Main.fontMouseText.MeasureString(indext).Y * cVar * 2)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, post, new Vector2(cbase.X - Main.fontMouseText.MeasureString(post).X / 2, cbase.Y - Main.fontMouseText.MeasureString(post).Y - (Main.fontMouseText.MeasureString(post).Y * cVar * 3)), Color.White);

                    if (npc.velocity != Vector2.Zero)
                    {
                        Color mainColor = (npc.target == CreativeMode.curPlr.whoAmI) ? Color.OrangeRed : Color.White;

                        if (dir == 1)
                        {
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + npc.width * npc.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X, center.Y), mainColor);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + npc.width * npc.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X + (Main.fontMouseText.MeasureString(dirt).X * cVar * 1), center.Y), Color.LightSlateGray);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + npc.width * npc.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X + (Main.fontMouseText.MeasureString(dirt).X * cVar * 2), center.Y), Color.SlateGray * .4f);
                        }
                        else
                        {
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - npc.width * npc.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X, center.Y), mainColor);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - npc.width * npc.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X - (Main.fontMouseText.MeasureString(dirt).X * cVar * 1), center.Y), Color.LightSlateGray);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - npc.width * npc.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X - (Main.fontMouseText.MeasureString(dirt).X * cVar * 2), center.Y), Color.SlateGray * .4f);
                        }
                    }

                    if (true) //(npc.life < npc.lifeMax)
                    {
                        spriteBatch.DrawString(Main.fontMouseText, namet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(namet).X / 2, center.Y + npc.height / 2 + Main.fontMouseText.MeasureString(namet).Y * cVar), Color.White);
                        spriteBatch.DrawString(Main.fontMouseText, hpt, new Vector2(cbase.X - Main.fontMouseText.MeasureString(hpt).X / 2, center.Y + npc.height / 2 + Main.fontMouseText.MeasureString(hpt).Y * cVar * 2), Color.White);
                    }
                }
                catch (Exception e)
                {
                    CreativeMode.RunError(this.GetType(), e);
                    return;
                }
            }
        }
    }

    public class DebugModeGlobalProjectile : GlobalProjectile
    {

        public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            if (CreativeMode.DMproj)
            {
                try
                {
                    if (!CreativeMode.DMproj1 && projectile.friendly)
                    {
                        return;
                    }

                    Vector2 center = projectile.Center - Main.screenPosition;
                    Vector2 pos = projectile.position;
                    int type = projectile.type;
                    int index = projectile.whoAmI;
                    int ai = projectile.aiStyle;
                    float ai0 = projectile.ai[0];
                    float ai1 = projectile.ai[1];
                    float lai0 = projectile.localAI[0];
                    float lai1 = projectile.localAI[1];
                    int dir = projectile.direction;

                    string namet = projectile.name;
                    string typet = string.Format("type: {0}", type);
                    string ait = string.Format("aiStyle: {0}[{1}][{2}][{3}][{4}]", ai, ai0, ai1, lai0, lai1);
                    string indext = string.Format("index: {0}", index);
                    string post = string.Format("pos: {0}x / {1}y", pos.X, pos.Y);
                    string dirt = (dir == 1) ? ">" : "<";

                    float cVar = (float)CreativeMode.cVar;

                    Vector2 cbase = new Vector2(center.X - projectile.width * projectile.scale * 2 / 16f, center.Y - projectile.height / 2);

                    spriteBatch.DrawString(Main.fontMouseText, typet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(typet).X / 2, cbase.Y - Main.fontMouseText.MeasureString(typet).Y), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, ait, new Vector2(cbase.X - Main.fontMouseText.MeasureString(ait).X / 2, cbase.Y - Main.fontMouseText.MeasureString(ait).Y - (Main.fontMouseText.MeasureString(ait).Y * cVar * 1)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, indext, new Vector2(cbase.X - Main.fontMouseText.MeasureString(indext).X / 2, cbase.Y - Main.fontMouseText.MeasureString(indext).Y - (Main.fontMouseText.MeasureString(indext).Y * cVar * 2)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, post, new Vector2(cbase.X - Main.fontMouseText.MeasureString(post).X / 2, cbase.Y - Main.fontMouseText.MeasureString(post).Y - (Main.fontMouseText.MeasureString(post).Y * cVar * 3)), Color.White);

                    if (projectile.velocity != Vector2.Zero)
                    {
                        Color mainColor =  Color.White;

                        if (dir == 1)
                        {
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + projectile.width * projectile.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X, center.Y), mainColor);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + projectile.width * projectile.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X + (Main.fontMouseText.MeasureString(dirt).X * cVar * 1), center.Y), Color.LightSlateGray);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X + projectile.width * projectile.scale * 2 / 16f + Main.fontMouseText.MeasureString(dirt).X + (Main.fontMouseText.MeasureString(dirt).X * cVar * 2), center.Y), Color.SlateGray * .4f);
                        }
                        else
                        {
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - projectile.width * projectile.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X, center.Y), mainColor);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - projectile.width * projectile.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X - (Main.fontMouseText.MeasureString(dirt).X * cVar * 1), center.Y), Color.LightSlateGray);
                            spriteBatch.DrawString(Main.fontMouseText, dirt, new Vector2(cbase.X - projectile.width * projectile.scale * 2 / 16f - Main.fontMouseText.MeasureString(dirt).X - (Main.fontMouseText.MeasureString(dirt).X * cVar * 2), center.Y), Color.SlateGray * .4f);
                        }
                    }

                    if (true)
                    {
                        spriteBatch.DrawString(Main.fontMouseText, namet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(namet).X / 2, center.Y + projectile.height / 2 + Main.fontMouseText.MeasureString(namet).Y * cVar), Color.White);
                    }
                }
                catch (Exception e)
                {
                    CreativeMode.RunError(this.GetType(), e);
                    return;
                }
            }
        }
    }

    public class DebugModeGlobalItem : GlobalItem
    {
        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale)
        {
            if (CreativeMode.DMitem)
            {
                try
                {
                    Vector2 center = item.Center - Main.screenPosition;
                    Vector2 pos = item.position;
                    int type = item.type;
                    int index = item.whoAmI;
                    int value = item.value;
                    //int anim = Main.itemFrame[type];
                    //int curA = (Main.itemAnimations[type].FrameCount <= 1) ? 1 : Main.itemAnimations[type].FrameCount;

                    string namet = item.name;
                    string typet = string.Format("type: {0}", type);
                    string indext = string.Format("index: {0}", index);
                    string post = string.Format("pos: {0}x / {1}y", pos.X, pos.Y);
                    string valt = string.Format("value: {0} copper", value);

                    float cVar = (float)CreativeMode.cVar;

                    Vector2 cbase = new Vector2(center.X - item.width * item.scale * 2 / 16f, center.Y - item.height / 2);

                    spriteBatch.DrawString(Main.fontMouseText, typet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(typet).X / 2, cbase.Y - Main.fontMouseText.MeasureString(typet).Y), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, valt, new Vector2(cbase.X - Main.fontMouseText.MeasureString(valt).X / 2, cbase.Y - Main.fontMouseText.MeasureString(valt).Y - (Main.fontMouseText.MeasureString(valt).Y * cVar * 1)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, indext, new Vector2(cbase.X - Main.fontMouseText.MeasureString(indext).X / 2, cbase.Y - Main.fontMouseText.MeasureString(indext).Y - (Main.fontMouseText.MeasureString(indext).Y * cVar * 2)), Color.White);
                    spriteBatch.DrawString(Main.fontMouseText, post, new Vector2(cbase.X - Main.fontMouseText.MeasureString(post).X / 2, cbase.Y - Main.fontMouseText.MeasureString(post).Y - (Main.fontMouseText.MeasureString(post).Y * cVar * 3)), Color.White);

                    //spriteBatch.DrawString(Main.fontMouseText, anim.ToString(), new Vector2(cbase.X + Main.itemTexture[type].Width * 2 / 16f + Main.fontMouseText.MeasureString(anim.ToString()).X, center.Y), Color.White);

                    if (true)
                    {
                        spriteBatch.DrawString(Main.fontMouseText, namet, new Vector2(cbase.X - Main.fontMouseText.MeasureString(namet).X / 2, cbase.Y + item.height / 2 + Main.fontMouseText.MeasureString(namet).Y * cVar), Color.White);
                    }
                }
                catch (Exception e)
                {
                    CreativeMode.RunError(this.GetType(), e);
                    return;
                }
            }
        }
    }
}
