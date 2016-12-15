using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using hCamille.Extensions;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Utilities = hCamille.Extensions.Utilities;

namespace hCamille.Champions
{
    class Camille
    {
        public static string WallBuff => "camilleedashtoggle";
        public static string DashName => "camilleedash";
        public static bool OnWall => ObjectManager.Player.HasBuff(WallBuff) || Spells.E.Instance.Name == "CamilleEDash2";

        public Camille()
        {
            Spells.Initializer();
            Menus.Initializer();

            Game.OnUpdate += CamilleOnUpdate;
            Obj_AI_Base.OnDoCast += CamilleOnDoCast;
            Obj_AI_Base.OnIssueOrder += OnIssueOrder;

        }

        private void OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (sender.IsMe && OnWall && Menus.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && 
                Spells.E.IsReady() && args.Order == GameObjectOrder.MoveTo)
            {
                var target = TargetSelector.GetTarget(Utilities.Slider("enemy.search.range"), TargetSelector.DamageType.Physical);
                if (target != null)
                {
                    args.Process = false;
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, target.ServerPosition, false);
                }
                else
                {
                    args.Process = true;
                }
            }
        }

        private void CamilleOnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.IsAutoAttack() && Menus.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo
                && Menus.Config.Item("q.mode").GetValue<StringList>().SelectedIndex == 0 && Spells.Q.IsReady() && Utilities.Enabled("q.combo"))
            {
                Spells.Q.Cast();
            }
        }

        private void CamilleOnUpdate(EventArgs args)
        {
            switch (Menus.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    OnCombo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    OnMixed();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    OnJungle();
                    OnClear();
                    break;
            }

            if (Menus.Config.Item("flee").GetValue<KeyBind>().Active)
            {
                Orbwalking.MoveTo(Game.CursorPos);
                if (Spells.E.IsReady())
                {
                    FleeE();
                }
               
            }
        }

        private static void OnCombo()
        {
            var target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Physical);
            if (target != null)
            {
                if (Spells.Q.IsReady() && Utilities.Enabled("q.combo") && target.IsValidTarget(ObjectManager.Player.AttackRange)
                    && Menus.Config.Item("q.mode").GetValue<StringList>().SelectedIndex == 1)
                {
                    Spells.Q.Cast();
                }

                if (Spells.W.IsReady() && Utilities.Enabled("w.combo") && target.IsValidTarget(Spells.W.Range))
                {
                    switch (Menus.Config.Item("w.mode").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            if (OnWall)
                            {
                                var pred = Spells.W.GetPrediction(target);
                                if (pred.Hitchance >= HitChance.Medium)
                                {
                                    Spells.W.Cast(pred.CastPosition);
                                }
                            }
                            break;
                        case 1:
                            var predx = Spells.W.GetPrediction(target);
                            if (predx.Hitchance >= HitChance.Medium)
                            {
                                Spells.W.Cast(predx.CastPosition);
                            }
                            break;
                    }
                    
                }

                if (Spells.E.IsReady() && Utilities.Enabled("e.combo") && target.IsValidTarget(Utilities.Slider("enemy.search.range")))
                {
                    if (ObjectManager.Player.CountEnemiesInRange(Utilities.Slider("enemy.search.range")) <= Utilities.Slider("max.enemy.count"))
                    {
                        UseE();
                    }
                }

                if (Spells.R.IsReady() && Utilities.Enabled("r.combo"))
                {
                    switch (Menus.Config.Item("r.mode").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            if (target.IsValidTarget(Spells.R.Range) && target.HealthPercent < Utilities.Slider("enemy.health.percent") && Utilities.Enabled("r." + target.ChampionName))
                            {
                                Spells.R.CastOnUnit(target);
                            }
                            break;
                        case 1:
                            var selectedtarget = TargetSelector.SelectedTarget;
                            if (selectedtarget != null && selectedtarget.IsValidTarget(Spells.R.Range) && selectedtarget.HealthPercent < Utilities.Slider("enemy.health.percent") && Utilities.Enabled("r." + selectedtarget.ChampionName))
                            {
                                Spells.R.CastOnUnit(selectedtarget);
                            }
                            break;
                    }
                    
                }
            }
            
        }

        private static void UseE()
        {
            var result = ObjectManager.Player;
            var rng = Utilities.Slider("wall.search.range");
            var listPoint = new List<Tuple<Vector2, float>>();
            for (var i = 0; i <= 360; i += 1)
            {
                var cosX = Math.Cos(i * Math.PI / 180);
                var sinX = Math.Sin(i * Math.PI / 180);
                var pos1 = new Vector3(
                    (float)(result.Position.X + rng * cosX), (float)(result.Position.Y + rng * sinX),
                    ObjectManager.Player.Position.Z);
                var time = Utils.TickCount;
                for (int j = 0; j < rng; j += 100)
                {
                    var pos = new Vector3(
                        (float)(result.Position.X + j * cosX), (float)(result.Position.Y + j * sinX),
                        ObjectManager.Player.Position.Z);
                    if (NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Wall))
                    {
                        if (j != 0)
                        {
                            int left = j - 99, right = j;
                            do
                            {
                                var middle = (left + right) / 2;
                                pos = new Vector3(
                                    (float)(result.Position.X + middle * cosX), (float)(result.Position.Y + middle * sinX),
                                    ObjectManager.Player.Position.Z);
                                if (NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Wall))
                                {
                                    right = middle;
                                }
                                else
                                {
                                    left = middle + 1;
                                }
                            } while (left < right);
                        }
                        pos1 = pos;
                        time = Utils.TickCount;
                        break;
                    }
                }

                listPoint.Add(new Tuple<Vector2, float>(pos1.To2D(), time));
            }
            var target = TargetSelector.GetTarget(Utilities.Slider("enemy.search.range"), TargetSelector.DamageType.Physical);
            if (target != null && !OnWall)
            {
                for (int i = 0; i < listPoint.Count - 1; i++)
                {
                    if (listPoint[i].Item1.IsWall() && listPoint[i].Item1.Distance(ObjectManager.Player.Position) < Utilities.Slider("wall.distance.to.enemy"))
                    {
                        var i1 = i;
                        var starttick = listPoint[i1].Item2;
                        var startpos = target.ServerPosition.To2D();
                        var speed = target.MoveSpeed;
                        var pathshit = target.Path.OrderBy(x => starttick + (int)(1000 * (new Vector3(x.X, x.Y, x.Z).
                        Distance(startpos.To3D()) / speed))).FirstOrDefault();

                        var endpos = new Vector3(pathshit.X, pathshit.Y, pathshit.Z);
                        var endtick = starttick + (int)(1000 * (endpos.Distance(startpos.To3D())
                            / speed));
                        var camilleendtic = starttick + (int)(1000 * (listPoint[i].Item1.Distance(ObjectManager.Player.Position)
                            / Spells.E.Speed));

                        if (listPoint[i].Item1.Distance(endpos) < 500 && camilleendtic > endtick)
                        {
                            Spells.E.Cast(listPoint[i].Item1);
                        }
                    }
                }
            }
        }

        private static void FleeE()
        {
            var result = ObjectManager.Player;
            var rng = Utilities.Slider("wall.search.range");
            var listPoint = new List<Tuple<Vector2, float>>();
            for (var i = 0; i <= 360; i += 1)
            {
                var cosX = Math.Cos(i * Math.PI / 180);
                var sinX = Math.Sin(i * Math.PI / 180);
                var pos1 = new Vector3(
                    (float)(result.Position.X + rng * cosX), (float)(result.Position.Y + rng * sinX),
                    ObjectManager.Player.Position.Z);
                var time = Utils.TickCount;
                for (int j = 0; j < rng; j += 100)
                {
                    var pos = new Vector3(
                        (float)(result.Position.X + j * cosX), (float)(result.Position.Y + j * sinX),
                        ObjectManager.Player.Position.Z);
                    if (NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Wall))
                    {
                        if (j != 0)
                        {
                            int left = j - 99, right = j;
                            do
                            {
                                var middle = (left + right) / 2;
                                pos = new Vector3(
                                    (float)(result.Position.X + middle * cosX), (float)(result.Position.Y + middle * sinX),
                                    ObjectManager.Player.Position.Z);
                                if (NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Wall))
                                {
                                    right = middle;
                                }
                                else
                                {
                                    left = middle + 1;
                                }
                            } while (left < right);
                        }
                        pos1 = pos;
                        time = Utils.TickCount;
                        break;
                    }
                }

                listPoint.Add(new Tuple<Vector2, float>(pos1.To2D(), time));
            }

            if (!OnWall)
            {
                for (int i = 0; i < listPoint.Count - 1; i++)
                {
                    var rectangle = new Geometry.Polygon.Rectangle(ObjectManager.Player.Position, ObjectManager.Player.Position.Extend(Game.CursorPos, Spells.E.Range), Spells.E.Width);
                    if (listPoint[i].Item1.IsWall() && listPoint[i].Item1.Distance(ObjectManager.Player.Position) < Utilities.Slider("wall.distance.to.enemy") && rectangle.IsInside(listPoint[i].Item1))
                    {
                        Spells.E.Cast(listPoint[i].Item1);
                    }
                }
            }

        }

        private static void OnMixed()
        {
            var target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Physical);
            if (target != null)
            {
                if (Spells.Q.IsReady() && Utilities.Enabled("q.combo") && target.IsValidTarget(ObjectManager.Player.AttackRange))
                {
                    Spells.Q.Cast();
                }

                if (Spells.W.IsReady() && Utilities.Enabled("w.combo") && target.IsValidTarget(Spells.W.Range))
                {
                    var pred = Spells.W.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Spells.W.Cast(pred.CastPosition);
                    }
                }
            }
        }

        private static void OnJungle()
        {
            var mob = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) + 100, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (mob == null || mob.Count == 0)
            {
                return;
            }

            if (Spells.Q.IsReady() && Utilities.Enabled("q.jungle"))
            {
                Spells.Q.Cast();
            }

            if (Spells.W.IsReady() && Utilities.Enabled("w.jungle"))
            {
                Spells.W.Cast(mob[0].Position);
            }
            
        }

        private static void OnClear()
        {
            if (Spells.Q.IsReady() && Utilities.Enabled("q.clear"))
            {
                var minions = MinionManager.GetMinions(ObjectManager.Player.Position, Spells.W.Range, MinionTypes.All,
                MinionTeam.NotAlly);

                var minioncount = Spells.W.GetLineFarmLocation(minions);
                if (minions == null || minions.Count == 0)
                {
                    return;
                }

                if (minioncount.MinionsHit >= Utilities.Slider("min.count"))
                {
                    Spells.W.Cast(minioncount.Position);
                }
            }
        }
    }
}
