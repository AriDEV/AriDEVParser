using System.Collections.Generic;
using System.IO;
using AriDEVParser.Enums;
using AriDEVParser.Util;

namespace AriDEVParser.Loading.Loaders
{
    [Loader("aridev")]
    public sealed class AriDEVLoader : Loader
    {
        public AriDEVLoader(string file)
            : base(file)
        {
        }

        public override IEnumerable<Packet> ParseFile()
        {
            var bin = new BinaryReader(new FileStream(FileToParse, FileMode.Open));
            var packets = new List<Packet>();

            while (bin.BaseStream.Position != bin.BaseStream.Length)
            {
                var buildnumber = bin.ReadInt32();
                var opcode = (Opcode)bin.ReadInt32();
                var direction = (Direction)bin.ReadChar();
                var length = bin.ReadInt32();
                var time = Utilities.GetDateTimeFromUnixTime(bin.ReadInt32());
                var data = bin.ReadBytes(length);

                var packet = new Packet(data, opcode, time, direction);
                packets.Add(packet);
            }

            return packets;
        }
    }
}
