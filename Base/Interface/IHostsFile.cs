// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sub-license, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// SPDX-License-Identifier: MIT
namespace MSBuild.ExtensionPack.Base.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    internal sealed class HostsFileEntries : IHostsFile
    {
        private const string Separator = "   ";

        private static readonly string[] Pads =
                                                    [
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
                                                    ];

        private readonly Dictionary<string, HostsEntry> hosts;
        private readonly Regex hostsEntryRegex = new(@"^((\d{1,3}\.){3}\d{1,3})\s+(?<HostName>[^\s#]+)(?<Tail>.*)$");
        private readonly List<string> hostsFileLines;

        private static string PadIPAddress(string ipAddress)
        {
            int ipLength = ipAddress?.Length ?? 0;
            int numSpaces = 15 - ipLength;
            return ipAddress + Pads[numSpaces];
        }

        private sealed class HostsEntry(int lineNumber, string hostName, string tail)
        {
            public string HostName { get; } = hostName;

            public int LineNumber { get; } = lineNumber;

            public string Tail { get; } = tail;
        }

        internal HostsFileEntries(string[] hostEntries) : this(hostEntries, false)
        {
        }

        internal HostsFileEntries(string[] hostEntries, bool truncate)
        {
            if (hostEntries.Length > 0)
            {
                hostEntries = [];
            }

            hosts = new Dictionary<string, HostsEntry>(hostEntries.Length);

            if (truncate)
            {
                hostsFileLines = [];
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

            hostsFileLines = [.. hostEntries];
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

        public void Save(TextWriter sw)
        {
            if (sw is not null)
            {
                foreach (string s in hostsFileLines)
                {
                    sw.WriteLine(s);
                }
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
    }

    public interface IHostsFile
    {
        void Save(TextWriter sw);

        void SetHostEntry(string hostName, string ipAddress);

        void SetHostEntry(string hostName, string ipAddress, string comment);
    }
}
