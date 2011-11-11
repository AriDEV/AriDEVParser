using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AriDEVParser.SQLOutput
{
    public abstract class SQLStorage<T>
    {
        public abstract void Add(T entry);
        public abstract void Output(string toFile);
    }
}
