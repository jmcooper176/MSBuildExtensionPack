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
namespace MSBuild.ExtensionPack.ErrorMessage
{
    using System.Text;

    public class Origin
    {
        public Origin(string path)
            : this(path, 0, 0, 0, 0)
        {
        }

        public Origin(string path, int lineNumber)
            : this(path, lineNumber, 0, 0, 0)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber)
            : this(path, lineNumber, columnNumber, 0, 0)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber, int endColumnNumber)
            : this(path, lineNumber, columnNumber, 0, endColumnNumber)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber)
        {
            Path = path;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            EndLineNumber = endLineNumber;
            EndColumnNumber = endColumnNumber;
        }

        public int ColumnNumber { get; }

        public int EndColumnNumber { get; }

        public int EndLineNumber { get; }

        public int LineNumber { get; }

        public string Path { get; }

        public override string ToString()
        {
            StringBuilder builder = new(Path);
            builder.Append('(');

            if (LineNumber <= 0)
            {
                builder.Append(')');
                return builder.ToString();
            }
            else
            {
                builder.Append(LineNumber);
            }

            if (ColumnNumber > 0 && EndColumnNumber > 0)
            {
                builder.Append(", ").Append(ColumnNumber).Append('-').Append(EndColumnNumber).Append(')');
                return builder.ToString();
            }
            else if (ColumnNumber > 0)
            {
                builder.Append(", ").Append(ColumnNumber);
            }

            if (EndLineNumber > 0)
            {
                builder.Append(", ").Append(EndLineNumber);
            }

            if (EndColumnNumber > 0)
            {
                builder.Append(", ").Append(EndColumnNumber);
            }

            builder.Append(')');
            return builder.ToString();
        }
    }
}
