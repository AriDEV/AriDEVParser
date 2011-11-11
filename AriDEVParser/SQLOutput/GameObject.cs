﻿using System;
using System.Collections.Generic;
using AriDEVParser.Enums;

namespace AriDEVParser.SQLOutput
{
    public sealed class GameObject
    {
        public GameObject()
        {
            Name = new FourStrings();
            Data = new ThirtyTwoInts();
            QuestItems = new SixInts();
        }

        public int Entry { get; set; }
        public GameObjectType Type { get; set; }
        public int DisplayID { get; set; }
        public FourStrings Name { get; private set; }
        public string IconName { get; set; }
        public string CastCaption { get; set; }
        public string UnkString { get; set; }
        public ThirtyTwoInts Data { get; private set; }
        public float Size { get; set; }
        public SixInts QuestItems { get; private set; }
        public int Exp { get; set; }

        public string ToSQL()
        {
            string sql = "REPLACE INTO gameobjectcache VALUES (";
            sql += Entry + ", ";
            sql += (int)Type + ", ";
            sql += DisplayID + ", ";
            sql += Name.ToSQL() + ", ";
            sql += "'" + IconName.ToSQL() + "',";
            sql += "'" + CastCaption.ToSQL() + "',";
            sql += "'" + UnkString.ToSQL() + "',";
            sql += Data.ToSQL() + ", ";
            sql += Size + ", ";
            sql += QuestItems.ToSQL() + ", ";
            sql += Exp + ");";
            return sql;
        }
    }
}
