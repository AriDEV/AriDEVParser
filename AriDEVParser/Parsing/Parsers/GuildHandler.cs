using System;
using AriDEVParser.Util;
using AriDEVParser.Enums;

namespace AriDEVParser.Parsing.Parsers
{
    public static class GuildHandler
    {
        [Parser(Opcode.SMSG_GUILD_INFO)]
        public static void HandleGuildInfo(Packet packet)
        {
            packet.ReadCString("Name");
            packet.ReadTime("Created");
            packet.ReadInt32("Number of members");
            packet.ReadInt32("Number of accounts");
        }
    }
}
