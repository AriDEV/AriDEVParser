using System;
using AriDEVParser.Enums;

namespace AriDEVParser.Parsing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class ParserAttribute : Attribute
    {
        public ParserAttribute(Opcode opcode)
        {
            Opcode = opcode;
        }

        public Opcode Opcode { get; private set; }
    }
}
