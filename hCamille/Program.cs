using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hCamille.Champions;
using LeagueSharp;
using LeagueSharp.Common;

namespace hCamille
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName == "Camille")
            {
                new Camille();
            }
            else
            {
                Console.WriteLine("XD");
                return;
            }
        }
    }
}
