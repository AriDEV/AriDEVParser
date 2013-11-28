using System;
using AriDEVParser.Enums;
using AriDEVParser.Util;
using AriDEVParser.Parsing;
using AriDEVParser.SQLOutput;

namespace AriDEVParser
{
    public static class GameObjectHandler
    {
        [Parser(Opcode.SMSG_GAMEOBJECT_QUERY_RESPONSE)] // 5.4.1 17538
        public static void HandleGameObjectQueryResponse(Packet packet)
        {
            var entry = packet.ReadEntry();
            Console.WriteLine("Entry: " + entry.Key);

            if (entry.Value)
                return;

            var unkInt = packet.ReadInt32();
            Console.WriteLine("unkInt: " + unkInt);

            var type = (GameObjectType)packet.ReadInt32();
            Console.WriteLine("Type: " + type);

            var dispId = packet.ReadInt32();
            Console.WriteLine("Display ID: " + dispId);

            var name = new string[4];
            for (var i = 0; i < 4; i++)
            {
                name[i] = packet.ReadCString();
                Console.WriteLine("Name " + i + ": " + name[i]);
            }

            var iconName = packet.ReadCString();
            Console.WriteLine("Icon Name: " + iconName);

            var castCaption = packet.ReadCString();
            Console.WriteLine("Cast Caption: " + castCaption);

            var unkStr = packet.ReadCString();
            Console.WriteLine("Unk String: " + unkStr);

            var data = new int[32];
            for (var i = 0; i < 32; i++)
            {
                data[i] = packet.ReadInt32();
                Console.WriteLine("Data " + i + ": " + data[i]);
            }

            var size = packet.ReadSingle();
            Console.WriteLine("Size: " + size);

            var qItemCount = packet.ReadByte();
            Console.WriteLine("Quest Item Count: " + qItemCount);

            var qItem = new int[qItemCount];
            for (var i = 0; i < qItemCount; i++)
            {
                qItem[i] = packet.ReadInt32();
                Console.WriteLine("Quest Item " + i + ": " + qItem[i]);
            }

            var expansion = packet.ReadInt32();
            Console.WriteLine("Expansion: " + expansion);

            SQLOutput.SQLOutput.WriteData(SQLOutput.SQLOutput.GameObjects.GetCommand(entry.Key, type, dispId, name[0], iconName,
                castCaption, unkStr, data, size, qItem));
        }
    }
}
