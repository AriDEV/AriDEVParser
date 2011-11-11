using System;
using AriDEVParser.Util;
using AriDEVParser.Enums;

namespace AriDEVParser.Parsing.Parsers
{
    public static class MiscHandler
    {
        [Parser(Opcode.SMSG_PLAYED_TIME)]
        public static void HandlePlayedTime(Packet packet)
        {
            packet.ReadInt32("Time Played");
            packet.ReadInt32("Total");
            packet.ReadByte("Level");
        }
    }
}
