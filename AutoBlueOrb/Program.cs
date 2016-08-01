using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace AutoBlueOrb
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
        public static List<OrbLocation> OrbLocations { get; set; }

        public static int OrbRange = 3500;
        private static Menu _config;

        private static void Menus()
        {
            _config = new Menu("Random Blue Orb", "Random Blue Orb",true);
            {
                _config.AddItem(new MenuItem("cast.random.blue.orb", "(CAST) Random Blue Orb").SetValue(new KeyBind('T', KeyBindType.Press)));
                _config.AddItem(new MenuItem("draw.bush.place", "(DRAW) Bush Places").SetValue(new Circle(true, Color.Gold)));
                _config.AddToMainMenu();
            }
        }
        private static void OnLoad(EventArgs args)
        {
            Init();
            Menus();
            Drawing.OnEndScene += OnEnd;
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (_config.Item("cast.random.blue.orb").GetValue<KeyBind>().Active)
            {
                ExecuteRandomBlueOrb();
            }
            
        }

        private static void ExecuteRandomBlueOrb()
        {
            var pos =
                OrbLocations.Where(x => ObjectManager.Player.Distance(x.Position) < OrbRange)
                    .OrderBy(o => ObjectManager.Player.Position.Distance(o.Position)).FirstOrDefault();

            if (pos != null && ObjectManager.Player.GetSpell(SpellSlot.Trinket).SData.Name == "TrinketOrbLvl3")
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Trinket, pos.Position);
            }
        }

        private static void OnEnd(EventArgs args)
        {
            if (_config.Item("draw.bush.place").GetValue<Circle>().Active)
            {
                var pos =
                OrbLocations.Where(x => ObjectManager.Player.Distance(x.Position) < OrbRange)
                    .OrderBy(o => ObjectManager.Player.Position.Distance(o.Position)).FirstOrDefault();

                if (pos != null)
                {
                    Drawing.DrawCircle(pos.Position, 100, _config.Item("draw.bush.place").GetValue<Circle>().Color);
                }
            }
        }

        private static void Init()
        {
            OrbLocations = new List<OrbLocation>
            {
                /*blue jungle right start*/
                new OrbLocation(new Vector3(5674, 3508, (float)51.40868),
                    "BlueTeamRight1",
                    Utility.Map.MapType.SummonersRift),
                    
                    new OrbLocation(new Vector3(6880, 3156, (float)51.72251),
                    "BlueTeamRight2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(7972, 3508, (float)51.6178),
                    "BlueTeamRight3",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(8576, 4716, (float)51.76864),
                    "BlueTeamRight4",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(6562, 4752, (float)48.527),
                    "BlueTeamRight5",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(9180, 2154, (float)58.35512),
                    "BlueTeamRight6",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(10360, 3054, (float)49.53857),
                    "BlueTeamRight7",
                    Utility.Map.MapType.SummonersRift),
                    /*blue jungle right end*/

                    /*blue jungle left start*/
                     new OrbLocation(new Vector3(4840, 7114, (float)50.71838),
                    "BlueTeamLeft1",
                    Utility.Map.MapType.SummonersRift),

                     new OrbLocation(new Vector3(3426, 7776, (float)53.11991),
                    "BlueTeamLeft2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(2306, 9694, (float)53.33098),
                    "BlueTeamLeft3",
                    Utility.Map.MapType.SummonersRift),

                     new OrbLocation(new Vector3(5010, 8474, (float)-9.862988),
                    "BlueTeamLeft4",
                    Utility.Map.MapType.SummonersRift),

                    /*blue jungle left end*/

                    /*red jungle left start*/
                    new OrbLocation(new Vector3(9946, 7880, (float)51.6483),
                    "RedTeamLeft1",
                    Utility.Map.MapType.SummonersRift),

                     new OrbLocation(new Vector3(9804, 6476, (float)14.52098),
                    "RedTeamLeft2",
                    Utility.Map.MapType.SummonersRift),

                     new OrbLocation(new Vector3(11478, 7150, (float)51.7254),
                    "RedTeamLeft3",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(12492, 5216, (float)51.7294),
                    "RedTeamLeft4",
                    Utility.Map.MapType.SummonersRift),

                    /*red jungle left end*/

                    /*red jungle right start*/
                    new OrbLocation(new Vector3(9172, 11406, (float)52.94517),
                    "RedTeamRight1",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(7942, 11864, (float)56.4768),
                    "RedTeamRight2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(8260, 10284, (float)49.91746),
                    "RedTeamRight3",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(6216, 10288, (float)54.32497),
                    "RedTeamRight4",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(4428, 11790, (float)56.60786),
                    "RedTeamRight5",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(5656, 12752, (float)52.8381),
                    "RedTeamRight6",
                    Utility.Map.MapType.SummonersRift),
                    /*red jungle right end*/

                    /*neutral sectors*/
                    new OrbLocation(new Vector3(2988, 11052, (float)-70.47139),
                    "BaronToplaneBush",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(5199, 9150, (float)-71.24072),
                    "BaronMiniBush",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(9414, 5674, (float)-71.2406),
                    "DragonMiniBush",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(11886, 3892, (float)-67.16178),
                    "DragonBotBush1",
                    Utility.Map.MapType.SummonersRift),
                    /*neutrals end*/

                    /*lane bush start*/
                    new OrbLocation(new Vector3(6456, 8296, (float)-70.70168),
                    "MidBush1",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(8332, 6454, (float)-71.2406),
                    "MidBush2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(12398, 1408, (float)52.57143),
                    "BotBush1",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(13372, 2482, (float)51.3669),
                    "BotBush2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(14112, 7004, (float)52.3063),
                    "BotBush3",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(1172, 12304, (float)52.8381),
                    "TopBush1",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(1678, 13016, (float)52.8381),
                    "TopBush2",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(2402, 13518, (float)52.8381),
                    "TopBush3",
                    Utility.Map.MapType.SummonersRift),

                    new OrbLocation(new Vector3(7074, 14106, (float)52.8381),
                    "TopBush4",
                    Utility.Map.MapType.SummonersRift),
                    /*lane bush end*/

            };
        }

    }

    public class OrbLocation
    {
        public OrbLocation(Vector3 pos, string bushname, Utility.Map.MapType maptype)
        {
            Position = pos;
            BushName = bushname;
            MapType = maptype;
        }
        public Vector3 Position { get; set; }
        public string BushName { get; set; }
        public Utility.Map.MapType MapType { get; set; }
    }
}
