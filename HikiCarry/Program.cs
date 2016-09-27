using System;
using LeagueSharp.Common;

namespace HikiCarry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }
        private static void OnGameLoad(EventArgs args)
        {
            Initializer.Load();
        }
    }
}
