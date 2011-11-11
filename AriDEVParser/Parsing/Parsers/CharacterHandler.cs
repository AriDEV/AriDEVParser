using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using AriDEVParser.Enums;
using AriDEVParser.Util;

namespace AriDEVParser.Parsing.Parsers
{
    public static class CharacterHandlers
    {
        enum AccountDataType : uint
        {
            GLOBAL_CONFIG_CACHE = 0,                    // 0x01 g
            PER_CHARACTER_CONFIG_CACHE = 1,                    // 0x02 p
            GLOBAL_BINDINGS_CACHE = 2,                    // 0x04 g
            PER_CHARACTER_BINDINGS_CACHE = 3,                    // 0x08 p
            GLOBAL_MACROS_CACHE = 4,                    // 0x10 g
            PER_CHARACTER_MACROS_CACHE = 5,                    // 0x20 p
            PER_CHARACTER_LAYOUT_CACHE = 6,                    // 0x40 p
            PER_CHARACTER_CHAT_CACHE = 7,                    // 0x80 p
        };

        private const int NUM_ACCOUNT_DATA_TYPES = 8;

        [Parser(Opcode.SMSG_ACCOUNT_DATA_TIMES)]
        public static void HandleAccountDataTimes(Packet packet)
        {
            packet.ReadTime("Time");
            packet.ReadByte("Unk byte (1)");
            int mask = packet.ReadInt32("Mask");
            for (int i = 0; i < NUM_ACCOUNT_DATA_TYPES; ++i)
            {
                if ((mask & (1 << i)) != 0)
                    packet.ReadTime(((AccountDataType)i).ToString());
            }
        }

        [Parser(Opcode.CMSG_REQUEST_ACCOUNT_DATA)]
        public static void HandleReqAccountData(Packet packet)
        {
            packet.ReadEnum<AccountDataType>("Type");
        }

        static UTF8Encoding encoder = new System.Text.UTF8Encoding();

        [Parser(Opcode.SMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleUpdateAccountData(Packet packet)
        {
            packet.ReadGuid("GUID");
            packet.ReadEnum<AccountDataType>("Type");
            packet.ReadTime("Time");
            int inflatedSize = packet.ReadInt32("Size");
            byte[] compressedData = packet.ReadBytes((int)packet.GetLength() - (int)packet.GetPosition());
            byte[] data = new byte[inflatedSize];
            var inflater = new Inflater();
            inflater.SetInput(compressedData, 0, compressedData.Length);
            inflater.Inflate(data, 0, inflatedSize);
            Console.WriteLine("Data: {0}", encoder.GetString(data));
        }

        [Parser(Opcode.CMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleReqUpdateAccountData(Packet packet)
        {
            packet.ReadEnum<AccountDataType>("Type");
            packet.ReadTime("Time");
            int inflatedSize = packet.ReadInt32("Size");
            byte[] compressedData = packet.ReadBytes((int)packet.GetLength() - (int)packet.GetPosition());
            byte[] data = new byte[inflatedSize];
            var inflater = new Inflater();
            inflater.SetInput(compressedData, 0, compressedData.Length);
            inflater.Inflate(data, 0, inflatedSize);
            Console.WriteLine("Data: {0}", encoder.GetString(data));
        }
        
    }
}
