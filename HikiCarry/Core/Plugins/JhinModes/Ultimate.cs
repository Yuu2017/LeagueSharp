using System.Linq;
using HikiCarry.Champions;
using HikiCarry.Core.Utilities;
using LeagueSharp;
using LeagueSharp.Common;

namespace HikiCarry.Core.Plugins.JhinModes
{
    static class Ultimate
    {
        public static void ComboUltimate()
        {
            if (ObjectManager.Player.IsActive(Jhin.R))
            {
                if (Initializer.Config.Item("auto.shoot.bullets").GetValue<bool>())
                {
                    foreach (var tstarget in HeroManager.Enemies.Where(x => Initializer.Config.Item("r.combo." + x.ChampionName,true).GetValue<bool>()
                    && x.IsValidTarget(Jhin.R.Range) && !FogOfWar.InFog(x.Position)))
                    {
                        var pred = Jhin.R.GetPrediction(tstarget);
                        if (pred.Hitchance >= Utilities.Utilities.HikiChance("hitchance"))
                        {
                            Jhin.R.Cast(pred.CastPosition);
                            return;
                        }
                    }
                   
                }
            }
            else
            {
                if (Jhin.R.IsReady() && Initializer.Config.Item("semi.manual.ult",true).GetValue<KeyBind>().Active)
                {
                    foreach (var tstarget in HeroManager.Enemies.Where(x => Initializer.Config.Item("r.combo." + x.ChampionName,true).GetValue<bool>()
                    && x.IsValidTarget(Jhin.R.Range) && !FogOfWar.InFog(x.Position)))
                    {
                        var pred = Jhin.R.GetPrediction(tstarget);
                        if (pred.Hitchance >= Utilities.Utilities.HikiChance("hitchance"))
                        {
                            Jhin.R.Cast(pred.CastPosition);
                            return;
                        }
                    }
                }
            }
        }
    }
}
