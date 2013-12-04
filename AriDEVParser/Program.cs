﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AriDEVParser.Enums;
using AriDEVParser.Loading;
using AriDEVParser.Loading.Loaders;
using AriDEVParser.Util;
using AriDEVParser.Parsing;
using AriDEVParser.SQLOutput;
using Guid=AriDEVParser.Util.Guid;
using AriDEVParser.Parsing.Parsers;

namespace AriDEVParser
{
    public class Program
    {
        public static CommandLine CmdLine { get; private set; }

        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            CmdLine = new CommandLine(args);

            string file;
            string loader;
            string filters;
            string format;
            string nodump;

            try
            {
                file = CmdLine.GetValue("-file");
                loader = CmdLine.GetValue("-loader");
                filters = CmdLine.GetValue("-filters");
                format = CmdLine.GetValue("-sql");
                nodump = CmdLine.GetValue("-nodump");
            }
            catch (IndexOutOfRangeException)
            {
                PrintUsage("All command line options require an argument.");
                return;
            }

            try
            {
                var packets = Reader.Read(loader, file);
                if (packets == null)
                {
                    PrintUsage("Could not open file " + file + " for reading.");
                    return;
                }

                if (packets.Count() > 0)
                {
                    var fullPath = Utilities.GetPathFromFullPath(file);
                    Handler.InitializeLogFile(Path.Combine(fullPath, file + ".txt"), nodump);
                    SQLOutput.SQLOutput.Initialize(Path.Combine(fullPath, file + ".sql"), format);

                    var appliedFilters = filters.Split(',');

                    foreach (var packet in packets)
                    {
                        var opcode = packet.GetOpcode().ToString();
                        if (!string.IsNullOrEmpty(filters))
                        {
                            foreach (var opc in appliedFilters)
                            {
                                if (!opcode.Contains(opc))
                                    continue;

                                Handler.Parse(packet);
                                break;
                            }
                        }
                        else
                            Handler.Parse(packet);
                    }

                    SQLOutput.SQLOutput.WriteToFile();
                    Handler.WriteToFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ResetColor();
        }

        public static void PrintUsage(string error)
        {
            var n = Environment.NewLine;

            if (!string.IsNullOrEmpty(error))
                Console.WriteLine(error + n);

            var usage = "Usage: AriDEVParser -file <input file> -loader <loader type> " +
                "[-filters opcode1,opcode2,...] [-sql <SQL format>] [-nodump <boolean>]" + n + n +
                "-file\t\tThe file to read packets from." + n +
                "-loader\t\tThe loader to use (AriDEV)." + n +
                "-filters\tComma-separated list of opcodes to parse." + n +
                "-sql\t\tSQL query format (Mangos/Trinity). Activates SQL dumping." + n +
                "-nodump\t\tSet to True to disable file logging.";

            Console.WriteLine(usage);
        }
    }
}
