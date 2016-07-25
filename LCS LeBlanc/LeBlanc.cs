using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCS_LeBlanc.Extensions;
using LCS_LeBlanc.Modes;
using LCS_LeBlanc.Modes.Combo;
using LeagueSharp.Common;
using LeagueSharp;

namespace LCS_LeBlanc
{
    public class LeBlanc
    {
        public LeBlanc()
        {
            OnLoad();
        }

        private static void OnLoad()
        {
            Spells.Initialize();
            Menus.Initialize();

            Game.OnUpdate += OnUpdate;
            AntiGapcloser.OnEnemyGapcloser += OnGapcloser;
        }
        private static void OnGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsEnemy && gapcloser.Sender.IsValidTarget(Spells.E.Range) &&
                (gapcloser.Sender.Target.IsMe || ObjectManager.Player.Distance(gapcloser.End) < 100) && Spells.E.IsReady()
                && Utilities.Enabled("anti-gapcloser.e"))
            {
                Spells.E.Cast(gapcloser.Sender.Position);
            }
        }

        private static void ComboSelector()
        {
            switch (Menus.Config.Item("combo.mode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    QRWE.Init();
                    break;
                case 1:
                    WRQE.WRQECombo();
                    break;
            }
        }

        private static void OnUpdate(EventArgs args)
        {
           
            Utilities.UpdateUltimateVariable();

            switch (Menus.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    ComboSelector();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Mixed.Init();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Clear.WaveInit();
                    Clear.JungleInit();
                    break;
            }
        }

    }
}
