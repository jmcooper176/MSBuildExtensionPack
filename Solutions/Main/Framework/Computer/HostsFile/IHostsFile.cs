//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IHostsFile.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Computer.HostsFile.HostsFile
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    public interface IHostsFile
    {
        void SetHostEntry(string hostName, string ipAddress);

        void SetHostEntry(string hostName, string ipAddress, string comment);

        void Save(TextWriter sw);
    }

    internal sealed class HostsFileEntries : IHostsFile
    {
        private const string Separator = "   ";
        private static readonly string[] Pads = new[]
                                                    {
                                                        string.Empty,
                                                        " ",
                                                        "  ",
                                                        "   ",
                                                        "    ",
                                                        "     ",
                                                        "      ",
                                                        "       ",
                                                        "        ",
                                                        "         ",
                                                        "          ",
                                                        "           ",
                                                        "            ",
                                                        "             ",
                                                        "              ",
                                                        "               "
                                                    };

        private readonly Regex hostsEntryRegex = new Regex(@"^((\d{1,3}\.){3}\d{1,3})\s+(?<HostName>[^\s#]+)(?<Tail>.*)$");
        private readonly Dictionary<string, HostsEntry> hosts;
        private readonly List<string> hostsFileLines;

        internal HostsFileEntries(string[] hostEntries) : this(hostEntries, false)
        {
        }

        internal HostsFileEntries(string[] hostEntries, bool truncate)
        {
            if (hostEntries == null)
            {
                hostEntries = new string[0];
            }

            hosts = new Dictionary<string, HostsEntry>(hostEntries.Length);

            if (truncate)
            {
                hostsFileLines = new List<string>();
                foreach (var line in hostEntries)
                {
                    if (line.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                    {
                        hostsFileLines.Add(line);
                    }
                    else
                    {
                        break;
                    }
                }

                hostsFileLines.Add(string.Empty);
                SetHostEntry("localhost", "127.0.0.1");
                return;
            }

            hostsFileLines = new List<string>(hostEntries);
            var lineNum = 0;
            foreach (var line in hostsFileLines)
            {
                var match = hostsEntryRegex.Match(line);
                if (match.Success)
                {
                    var hostsEntry = new HostsEntry(lineNum, match.Groups["HostName"].Value, match.Groups["Tail"].Value);
                    var hostsEntryKey = hostsEntry.HostName.ToLower(CultureInfo.InvariantCulture);
                    if (!hosts.ContainsKey(hostsEntryKey))
                    {
                        hosts[hostsEntryKey] = hostsEntry;
                    }
                }

                lineNum++;
            }
        }

        public void SetHostEntry(string hostName, string ipAddress)
        {
            SetHostEntry(hostName, ipAddress, string.Empty);
        }

        public void SetHostEntry(string hostName, string ipAddress, string comment)
        {
            string hostsKey = hostName.ToLower(CultureInfo.InvariantCulture);
            string tail = string.IsNullOrEmpty(comment) ? null : "\t# " + comment;
            string hostsLine = PadIPAddress(ipAddress) + Separator + hostName;
            if (hosts.ContainsKey(hostsKey))
            {
                HostsEntry hostEntry = hosts[hostsKey];
                hostsFileLines[hostEntry.LineNumber] = hostsLine + (tail ?? hostEntry.Tail);
            }
            else
            {
                hostsFileLines.Add(hostsLine + tail);
                hosts[hostsKey] = new HostsEntry(hostsFileLines.Count - 1, hostName, tail);
            }
        }

        public void Save(TextWriter sw)
        {
            if (sw != null)
            {
                foreach (string s in hostsFileLines)
                {
                    sw.WriteLine(s);
                }
            }
        }
        
        private static string PadIPAddress(string ipAddress)
        {
            int ipLength = ipAddress?.Length ?? 0;
            int numSpaces = 15 - ipLength;
            return ipAddress + Pads[numSpaces];
        }

        private sealed class HostsEntry
        {
            public HostsEntry(int lineNumber, string hostName, string tail)
            {
                LineNumber = lineNumber;
                HostName = hostName;
                Tail = tail;
            }

            public string HostName { get; }

            public int LineNumber { get; }

            public string Tail { get; }
        }
    }
}
