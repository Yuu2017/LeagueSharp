using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using  Jhin___The_Virtuoso.Extensions;
using LeagueSharp;

namespace Jhin___The_Virtuoso.Extensions
{
    static class Ultimate
    {
        public static void ComboUltimate()
        {
            if (ObjectManager.Player.IsActive(Spells.R))
            {
                if (Menus.Config.Item("auto.shoot.bullets").GetValue<bool>())
                {
                    foreach (var tstarget in HeroManager.Enemies.Where(x => Menus.Config.Item("r.combo." + x.ChampionName).GetValue<bool>()
                    && x.IsValidTarget(Spells.R.Range) && !FogOfWar.InFog(x.Position)))
                    {
                        var pred = Spells.R.GetPrediction(tstarget);
                        if (pred.Hitchance >= Menus.Config.HikiChance("r.hit.chance"))
                        {
                            Spells.R.Cast(pred.CastPosition);
                            return;
                        }
                    }
                   
                }
            }
            else
            {
                if (Spells.R.IsReady() && Menus.Config.Item("semi.manual.ult").GetValue<KeyBind>().Active)
                {
                    foreach (var tstarget in HeroManager.Enemies.Where(x => Menus.Config.Item("r.combo." + x.ChampionName).GetValue<bool>()
                    && x.IsValidTarget(Spells.R.Range) && !FogOfWar.InFog(x.Position)))
                    {
                        var pred = Spells.R.GetPrediction(tstarget);
                        if (pred.Hitchance >= Menus.Config.HikiChance("r.hit.chance"))
                        {
                            Spells.R.Cast(pred.CastPosition);
                            return;
                        }
                    }
                }
            }
        }
    }
}
