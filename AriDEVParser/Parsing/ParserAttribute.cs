using System;
using AriDEVParser.Enums;

namespace AriDEVParser.Parsing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class ParserAttribute : Attribute
    {
        public ParserAttribute(ushort i)
        {
            index = i;
        }

        public ParserAttribute(Opcode i)
        {
            index = (ushort)i;
        }

        public int index { get; private set; }
    }
}
