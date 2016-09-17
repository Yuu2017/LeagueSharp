using hYasuo.Extensions;
using LeagueSharp;
using LeagueSharp.Common;

namespace hYasuo.Logics
{
    internal class YasuoIncomingDamage
    {
        public static void IncomingDamage (Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Hero && sender.IsEnemy && sender.Type == GameObjectType.obj_AI_Hero
                && args.End.Distance(ObjectManager.Player.Position) < 400f && 
                args.SData.TargettingType != SpellDataTargetType.Unit
                && Spells.W.IsReady())
            {
                var calcmagicaldamage = ((Obj_AI_Hero) args.Target).CalcDamage(ObjectManager.Player, Damage.DamageType.Magical,
                    ((Obj_AI_Hero) args.Target).GetSpellDamage(ObjectManager.Player, args.Slot));

                var calcpsydamage = ((Obj_AI_Hero)args.Target).CalcDamage(ObjectManager.Player, Damage.DamageType.Physical,
                    ((Obj_AI_Hero)args.Target).GetSpellDamage(ObjectManager.Player, args.Slot));

                var calctrue = ((Obj_AI_Hero)args.Target).CalcDamage(ObjectManager.Player, Damage.DamageType.True,
                    ((Obj_AI_Hero)args.Target).GetSpellDamage(ObjectManager.Player, args.Slot));

                // no way to get enemy spell damage type. spagetti code 10/10

                if (calcmagicaldamage > ObjectManager.Player.Health || 
                    calcpsydamage > ObjectManager.Player.Health || calctrue > ObjectManager.Player.Health)
                {
                    Spells.W.Cast(ObjectManager.Player.Position.Extend(args.Start, Spells.W.Range));
                }

            }
        }

    }
}
