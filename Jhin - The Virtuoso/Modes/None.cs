using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jhin___The_Virtuoso.Extensions;
using LeagueSharp;
using LeagueSharp.Common;

namespace Jhin___The_Virtuoso.Modes
{
    static class None
    {
        public static void ImmobileExecute()
        {
            if (Spells.E.IsReady() && Menus.Config.Item("auto.e.immobile").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.E.Range) && x.IsEnemyImmobile()))
                {
                    var pred = Spells.E.GetPrediction(enemy);
                    if (pred.Hitchance >= Menus.Config.HikiChance("e.hit.chance"))
                    {
                        Spells.E.Cast(pred.CastPosition);
                    }
                }
            }
        }

        public static void KillSteal()
        {
            if (Spells.Q.IsReady() && Menus.Config.Item("q.ks").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.Q.Range) &&
                    x.Health < Spells.Q.GetDamage(x)))
                {
                    Spells.Q.CastOnUnit(enemy);
                }
            }
            if (Spells.W.IsReady() && Menus.Config.Item("w.ks").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.Distance(ObjectManager.Player) < Menus.Config.Item("w.combo.max.distance").GetValue<Slider>().Value
                        && x.Distance(ObjectManager.Player) > Menus.Config.Item("w.combo.min.distance").GetValue<Slider>().Value
                        && x.IsValid && Spells.W.GetPrediction(x).Hitchance >= Menus.Config.HikiChance("w.hit.chance")
                        && x.Health < Spells.W.GetDamage(x) && !x.IsDead && !x.IsZombie && x.IsValid))
                {
                    Spells.W.Cast(enemy);
                }
            }
        }

       
    }
}
