﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AriDEVParser.Enums
{
    public enum QuestType : int
    {
        Group = 1,
        Class = 21,
        PvP = 41,
        Raid = 62,
        Dungeon = 81,
        WorldEvent = 82,
        Legendary = 83,
        Escort = 84,
        Heroic = 85,
        Raid10 = 88,
        Raid25 = 89,
		Scenario = 98,
		Account = 102
    }
}
