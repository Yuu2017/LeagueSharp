using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Illaoi___Tentacle_Kitty.Handlers;
using LeagueSharp.Common;

namespace Illaoi___Tentacle_Kitty.Champion
{
    internal class Illaoi
    {
        public static Menu Config;
        public Menus Menus = new Menus(Config);
        public Spells Spell = new Spells();


        public Illaoi()
        {
            Menus.Init();

        }
    }
}
