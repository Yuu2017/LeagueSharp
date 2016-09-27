using System;
using System.Drawing;
using System.Linq;
using HikiCarry.Core.Plugins;
using HikiCarry.Core.Plugins.JhinModes;
using HikiCarry.Core.Predictions;
using HikiCarry.Core.Utilities;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Utilities = HikiCarry.Core.Utilities.Utilities;

namespace HikiCarry.Champions
{
    internal class Jhin
    {
        internal static Spell Q;
        internal static Spell W;
        internal static Spell E;
        internal static Spell R;

        public Jhin()
        {
            Q = new Spell(SpellSlot.Q, 550);
            W = new Spell(SpellSlot.W, 2500);
            E = new Spell(SpellSlot.E, 2000);
            R = new Spell(SpellSlot.R, 3500);

            W.SetSkillshot(0.75f, 40, float.MaxValue, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.23f, 120, 1600, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.21f, 80, 5000, false, SkillshotType.SkillshotLine);

            var comboMenu = new Menu("Combo Settings", ":: Combo Settings");
            {

                comboMenu.AddItem(new MenuItem("q.combo", "Use (Q)", true).SetValue(true));

                comboMenu.AddItem(new MenuItem("w.combo", "Use (W)", true).SetValue(true));
                comboMenu.AddItem(
                    new MenuItem("w.combo.min.distance", "Min. Distance", true).SetValue(new Slider(400, 1, 2500)));
                comboMenu.AddItem(
                    new MenuItem("w.combo.max.distance", "Max. Distance", true).SetValue(new Slider(1000, 1, 2500)));
                comboMenu.AddItem(new MenuItem("w.passive.combo", "Use (W) If Enemy Is Marked", true).SetValue(true));
                comboMenu.AddItem(new MenuItem("e.combo", "Use (E)", true).SetValue(true));
                Initializer.Config.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Harass Settings", ":: Harass Settings");
            {

                harassMenu.AddItem(new MenuItem("w.harass", "Use (W)",true).SetValue(true));

                harassMenu.AddItem(
                    new MenuItem("harass.mana", "Min. Mana Percentage",true).SetValue(new Slider(50, 1, 99)));
                Initializer.Config.AddSubMenu(harassMenu);
            }

            var clearMenu = new Menu("Clear Settings", ":: Clear Settings");
                {
                    var laneclearMenu = new Menu("Wave Clear", ":: Wave Clear");
                    {
                        laneclearMenu.AddItem(
                            new MenuItem("keysinfo1", "                  (Q) Settings").SetTooltip("Q Settings"));
                        laneclearMenu.AddItem(new MenuItem("q.clear", "Use (Q)",true).SetValue(true));
                        laneclearMenu.AddItem(
                            new MenuItem("keysinfo2", "                  (W) Settings").SetTooltip("W Settings"));
                        laneclearMenu.AddItem(new MenuItem("w.clear", "Use (W)",true).SetValue(true));
                        laneclearMenu.AddItem(new MenuItem("w.hit.x.minion", "Min. Minion",true).SetValue(new Slider(4, 1, 5)));
                        clearMenu.AddSubMenu(laneclearMenu);
                    }

                    var jungleClear = new Menu("Jungle Clear", ":: Jungle Clear");
                    {
                        jungleClear.AddItem(
                            new MenuItem("keysinfo1X", "                  (Q) Settings").SetTooltip("Q Settings"));
                        jungleClear.AddItem(new MenuItem("q.jungle", "Use (Q)", true).SetValue(true));
                        jungleClear.AddItem(
                            new MenuItem("keysinfo2X", "                  (W) Settings").SetTooltip("W Settings"));
                        jungleClear.AddItem(new MenuItem("w.jungle", "Use (W)", true).SetValue(true));
                        clearMenu.AddSubMenu(jungleClear);
                    }
                    clearMenu.AddItem(
                        new MenuItem("clear.mana", "LaneClear Min. Mana Percentage", true).SetValue(new Slider(50, 1, 99)));
                    clearMenu.AddItem(
                        new MenuItem("jungle.mana", "Jungle Min. Mana Percentage", true).SetValue(new Slider(50, 1, 99)));
                Initializer.Config.AddSubMenu(clearMenu);
                }

                var ksMenu = new Menu("Kill Steal", ":: Kill Steal");
                {
                ksMenu.AddItem(new MenuItem("q.ks", "Use (Q)", true).SetValue(true));
                ksMenu.AddItem(new MenuItem("w.ks", "Use (W)", true).SetValue(true));
                Initializer.Config.AddSubMenu(ksMenu);
                }

                var miscMenu = new Menu("Miscellaneous", ":: Miscellaneous");
                {
                miscMenu.AddItem(new MenuItem("auto.e.immobile", "Auto Cast (E) Immobile Target", true).SetValue(true));
                //miscMenu.AddItem(new MenuItem("ezevade.hijacker", "ezEvade Hijacker").SetValue(true)).SetTooltip("When Jhin using (R) Disabling ezEvade for max. damage ");
                //miscMenu.AddItem(new MenuItem("evadesharp.hijacker", "Evade# Hijacker").SetValue(true)).SetTooltip("When Jhin using (R) Disabling Evade# for max. damage ");
                Initializer.Config.AddSubMenu(miscMenu);
                }
                var rComboMenu = new Menu("Ultimate Settings", ":: Ultimate Settings").SetFontStyle(FontStyle.Bold,
                    SharpDX.Color.Yellow);
                {
                    var rComboWhiteMenu = new Menu(":: R - Whitelist", ":: R - Whitelist");
                    {
                        foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValid))
                        {
                            rComboWhiteMenu.AddItem(
                                new MenuItem("r.combo." + enemy.ChampionName, "(R): " + enemy.ChampionName, true).SetValue(
                                    true));
                        }
                        rComboMenu.AddSubMenu(rComboWhiteMenu);
                    }
                rComboMenu.AddItem(new MenuItem("r.combo", "Use (R)", true).SetValue(true));
                    rComboMenu.AddItem(
                        new MenuItem("auto.shoot.bullets", "If Jhin Casting (R) Auto Cast Bullets", true).SetValue(true));
                Initializer.Config.AddSubMenu(rComboMenu);
                }

            Initializer.Config.AddItem(
                    new MenuItem("semi.manual.ult", "Semi-Manual (R)!", true).SetValue(new KeyBind("A".ToCharArray()[0],
                        KeyBindType.Press)));
            Initializer.Config.AddItem(new MenuItem("use.combo", "Combo (Active)", true).SetValue(new KeyBind(32, KeyBindType.Press)));

            var drawMenu = new Menu("Draw Settings", "Draw Settings");
            {
                var drawDamageMenu = new MenuItem("RushDrawEDamage", "Combo Damage").SetValue(true);
                var drawFill = new MenuItem("RushDrawEDamageFill", "Combo Damage Fill").SetValue(new Circle(true, System.Drawing.Color.Gold));

                drawMenu.SubMenu("Damage Draws").AddItem(drawDamageMenu);
                drawMenu.SubMenu("Damage Draws").AddItem(drawFill);

                DamageIndicator.DamageToUnit = TotalDamage;
                DamageIndicator.Enabled = drawDamageMenu.GetValue<bool>();
                DamageIndicator.Fill = drawFill.GetValue<Circle>().Active;
                DamageIndicator.FillColor = drawFill.GetValue<Circle>().Color;

                drawDamageMenu.ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    DamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                };

                drawFill.ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    DamageIndicator.Fill = eventArgs.GetNewValue<Circle>().Active;
                    DamageIndicator.FillColor = eventArgs.GetNewValue<Circle>().Color;
                };
                Initializer.Config.AddSubMenu(drawMenu);
            }
           

            Game.OnUpdate += JhinOnUpdate;

        }

        private static float TotalDamage(Obj_AI_Hero hero)
        {
            var damage = 0d;
            if (Q.IsReady())
            {
                damage += Q.GetDamage(hero);
            }
            if (W.IsReady())
            {
                damage += W.GetDamage(hero);
            }
            if (E.IsReady())
            {
                damage += W.GetDamage(hero);
            }
            if (R.IsReady())
            {
                damage += R.GetDamage(hero);
            }
            return (float)damage;
        }
    

        private void JhinOnUpdate(EventArgs args)
        {
            #region Orbwalker & Modes 
            switch (Initializer.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo.ExecuteCombo();
                    None.ImmobileExecute();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Jungle.ExecuteJungle();
                    Clear.ExecuteClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Mixed.ExecuteHarass();
                    break;
            }
            #endregion
            #region General Functions
            None.KillSteal();
            Ultimate.ComboUltimate();
            #endregion
            #region Check Ultimate
            if (ObjectManager.Player.IsActive(R))
            {
                Initializer.Orbwalker.SetAttack(false);
                Initializer.Orbwalker.SetMovement(false);
            }
            else
            {
                Initializer.Orbwalker.SetAttack(true);
                Initializer.Orbwalker.SetMovement(true);
            }
            #endregion
        }
    }
}
