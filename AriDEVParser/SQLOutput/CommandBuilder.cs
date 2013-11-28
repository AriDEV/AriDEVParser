using System;
using System.Collections.Generic;
using System.Globalization;

namespace AriDEVParser.SQLOutput
{
    public sealed class CommandBuilder
    {
        public readonly List<KeyValuePair<string, object>> InsertValues =
            new List<KeyValuePair<string, object>>();

        public readonly List<KeyValuePair<string, object>> UpdateValues =
            new List<KeyValuePair<string, object>>();

        public readonly string Table;

        public CommandBuilder(string table)
        {
            Table = table;
        }

        public void AddColumnValue(string name, object value)
        {
            InsertValues.Add(new KeyValuePair<string, object>(name, value));
        }

        public void AddUpdateValue(string name, object value)
        {
            UpdateValues.Add(new KeyValuePair<string, object>(name, value));
        }

        public static string EscapeString(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\n", "\\n");
            return str;
        }

        public string BuildInsert(bool ignore)
        {
            var ignoreStr = ignore ? "IGNORE " : string.Empty;
            var str = "INSERT " + ignoreStr + "INTO " + Table + " (";

            for (var i = 0; i < InsertValues.Count; i++)
            {
                var val = InsertValues[i];
                var comma = i == InsertValues.Count - 1 ? string.Empty : ", ";

                str += val.Key + comma;
            }

            str += ") VALUES (";

            for (var i = 0; i < InsertValues.Count; i++)
            {
                var val = InsertValues[i];
                var comma = i == InsertValues.Count - 1 ? string.Empty : ", ";

                string value;
                if (val.Value is float)
                    value = ((float)val.Value).ToString("R", CultureInfo.InvariantCulture);
                else if (val.Value is string)
                    value = "'" + EscapeString((string)val.Value) + "'";
                else
                    value = val.Value.ToString();

                str += value + comma;
            }

            str += ");";

            return str;
        }

        public string BuildUpdate(string where)
        {
            var str = "UPDATE " + Table + " SET ";

            for (var i = 0; i < UpdateValues.Count; i++)
            {
                var val = UpdateValues[i];
                var comma = i == UpdateValues.Count - 1 ? string.Empty : ", ";

                str += val.Key + " = " + val.Value + comma;
            }

            str += " WHERE " + where + ";";

            return str;
        }
    }
}
