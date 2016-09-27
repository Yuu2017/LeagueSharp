using System;
using System.Drawing;
using System.Linq;
using HikiCarry.Core.Plugins;
using HikiCarry.Core.Predictions;
using HikiCarry.Core.Utilities;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace HikiCarry.Champions
{
    internal class _Blitzcrank
    {
        internal static Spell Q;
        internal static Spell W;
        internal static Spell E;
        internal static Spell R;

        public _Blitzcrank()
        {

            Q = new Spell(SpellSlot.Q, 1075);
            W = new Spell(SpellSlot.W, 950);
            E = new Spell(SpellSlot.E, 480);
            R = new Spell(SpellSlot.R, 400);

            Q.SetSkillshot(0.25f, 80f, 1800f, true, SkillshotType.SkillshotLine);



            var miscMenu = new Menu("Misc Settings", "Misc Settings");
            {
                var dashinterrupter = new Menu("Dash Interrupter", "Dash Interrupter");
                {
                    dashinterrupter.AddItem(new MenuItem("dash.block", "Use (Q) for Block Dash!", true).SetValue(true));
                    dashinterrupter.AddItem(new MenuItem("info.ashe.1", "                       Blockable Dash Spells", true)).SetFontStyle(FontStyle.Bold,SharpDX.Color.Yellow);
                    foreach (var enemy in HeroManager.Enemies.Where(x=> x.IsValid))
                    {
                        foreach (var dash in DashInterrupter.Spells.Where(x => x.championname == enemy.ChampionName))
                        {
                            dashinterrupter.AddItem(new MenuItem("dash."+dash.spellname, "Block ("+dash.championname+") - ("+dash.slot+")").SetValue(true));
                        }
                    }
                    miscMenu.AddSubMenu(dashinterrupter);
                }
                
                Initializer.Config.AddSubMenu(miscMenu);
            }

            Obj_AI_Base.OnNewPath += ObjAiHeroOnOnNewPath;

        }

        private void ObjAiHeroOnOnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsEnemy && sender is Obj_AI_Hero && args.IsDash)
            {
                var starttick = Utils.TickCount;
                var speed = args.Speed;
                var startpos = sender.ServerPosition.To2D();
                var path = args.Path;
                var forch = args.Path.OrderBy(x => starttick + (int) (1000*(new Vector3(x.X, x.Y, x.Z).
                    Distance(startpos.To3D())/speed))).FirstOrDefault();
                {
                    var endpos = new Vector3(forch.X, forch.Y, forch.Z);
                    var endtick = starttick + (int)(1000 * (endpos.Distance(startpos.To3D())
                        / speed));
                    var duration = endtick - starttick;

                    if (duration < starttick)
                    {
                        Q.Cast(endpos);
                    }
                }
            }
        }
    }
}
