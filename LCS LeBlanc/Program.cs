using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace LCS_LeBlanc
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Leblanc")
            {
                return;
            }
            else
            {
                // ReSharper disable once ObjectCreationAsStatement
                new LeBlanc();
            }
        }
    }
}
