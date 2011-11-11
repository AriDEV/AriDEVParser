using System;
using System.Collections.Generic;
using AriDEVParser.Util;

namespace AriDEVParser.Loading
{
    public abstract class Loader
    {
        public readonly string FileToParse;

        protected Loader(string file)
        {
            FileToParse = file;
        }

        public abstract IEnumerable<Packet> ParseFile();
    }
}
