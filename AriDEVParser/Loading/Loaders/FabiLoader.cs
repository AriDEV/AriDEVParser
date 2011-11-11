using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AriDEVParser.Util;

namespace AriDEVParser.Loading.Loaders
{
    [Loader("fabi")]
    public sealed class FabiLoader : Loader
    {
        public FabiLoader(string file)
            : base(file)
        {
        }

        public override IEnumerable<Packet> ParseFile()
        {
            var packets = new List<Packet>();

            using (TextReader tr = new StreamReader(FileToParse))
            {
                while (tr.Peek() != -1)
                {
                    string line = tr.ReadLine();
                    string[] data = line.Split(';');
                    //0123
                    string[] data0 = data[0].Split(' ');
                    string[] data1 = data[1].Split(' ');
                    string[] data2 = data[2].Split(' ');
                    string[] data3 = data[3].Split(' ');

                    DateTime time = Utilities.GetDateTimeFromUnixTime(UInt32.Parse(data0[1]));
                    byte direction = (byte)(data1[1].Equals("SMSG") ? 0 : 1);
                    ushort opcode = UInt16.Parse(data2[1]);
                    string directdata = data3[1];
                    byte[] byteData = ParseHex(directdata);
                    Packet p = new Packet(byteData, opcode, time, direction);
                    packets.Add(p);
                }
            }
            return packets;
        }

        public static byte[] ParseHex(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length: " + hex.Length);
            }
            byte[] ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseNybble(hex[offset]) << 4)
                                 | ParseNybble(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }

        static int ParseNybble(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            throw new ArgumentException("Invalid hex digit: " + c);
        }

    }
}
