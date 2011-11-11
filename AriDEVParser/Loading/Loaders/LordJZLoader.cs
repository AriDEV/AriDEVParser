using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AriDEVParser.Util;

namespace AriDEVParser.Loading.Loaders
{
    [Loader("lordjz")]
    public sealed class LordJZLoader : Loader
    {
        public uint Build { get; private set; }

        public LordJZLoader(string file)
            : base(file)
        {
        }

        public override IEnumerable<Packet> ParseFile()
        {
            using (var gr = new BinaryReader(new FileStream(FileToParse, FileMode.Open, FileAccess.Read), Encoding.ASCII))
            {
                gr.ReadBytes(3);                        // PKT
                var version = gr.ReadUInt16();          // sniff version (0x0201, 0x0202, 0x0300)

                switch (version)
                {
                    case 0x0201:
                        Build = gr.ReadUInt16();        // build
                        gr.ReadBytes(40);               // session key
                        break;
                    case 0x0202:
                        gr.ReadByte();                  // 0x06
                        Build = gr.ReadUInt16();        // build
                        gr.ReadBytes(4);                // client locale
                        gr.ReadBytes(20);               // packet key
                        gr.ReadBytes(64);               // realm name
                        break;
                    case 0x0300:
                        gr.ReadByte();                  // snifferId
                        Build = gr.ReadUInt32();        // client build
                        gr.ReadBytes(4);                // client locale
                        gr.ReadBytes(40);               // session key
                        var optionalHeaderLength = gr.ReadInt32();
                        gr.ReadBytes(optionalHeaderLength);
                        break;
                    default:
                        throw new Exception(String.Format("Unknown sniff version {0:X2}", version));
                }

                var packets = new List<Packet>();

                if (version != 0x0300)
                {
                    while (gr.PeekChar() >= 0)
                    {
                        byte direction = (byte)(gr.ReadByte() == 0xff ? 0 : 1);
                        DateTime time = Utilities.GetDateTimeFromUnixTime(gr.ReadUInt32());
                        uint tickcount = gr.ReadUInt32();
                        uint size = gr.ReadUInt32();
                        ushort opcode = (direction == 1) ? (ushort)gr.ReadUInt32() : gr.ReadUInt16();
                        byte[] data = gr.ReadBytes((int)size - ((direction == 1) ? 4 : 2));
                        Packet p = new Packet(data, opcode, time, direction);
                        packets.Add(p);
                    }
                }
                else
                {
                    while (gr.PeekChar() >= 0)
                    {
                        byte direction = (byte)(gr.ReadUInt32() == 0x47534d53 ? 0 : 1);
                        DateTime time = Utilities.GetDateTimeFromUnixTime(gr.ReadUInt32());
                        uint tickcount = gr.ReadUInt32();
                        int optionalSize = gr.ReadInt32();
                        int dataSize = gr.ReadInt32();
                        gr.ReadBytes(optionalSize);
                        ushort opcode = (ushort)gr.ReadUInt32();
                        byte[] data = gr.ReadBytes(dataSize - 4);
                        Packet p = new Packet(data, opcode, time, direction);
                        packets.Add(p);
                    }
                }

                return packets;
            }
        }

    }
}
