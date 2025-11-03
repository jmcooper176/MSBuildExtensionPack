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
namespace MSBuild.ExtensionPack.ErrorMessage.Message
{
    using System;
    using System.Diagnostics;

    using Environment = Utility.Environment;

    public static class Cause
    {
        public static bool GetLogExceptionDetail(bool defaultValue = false)
        {
            return Environment.TestEnvironmentValue("LogExceptionDetail", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }

        public static bool GetLogExceptionStackTrace(bool defaultValue = false)
        {
            return Environment.TestEnvironmentValue("LogExceptionStackTrace", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }

        public static int GetSourceColumnNumber(Exception exception, int compilerColumn, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? frame.GetFileColumnNumber() : compilerColumn;
        }

        public static string GetSourceFileName(Exception exception, string? compilerPath, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? (frame.GetFileName() ?? string.Empty) : (compilerPath ?? string.Empty);
        }

        public static int GetSourceLineNumber(Exception exception, int compilerLine, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? frame.GetFileLineNumber() : compilerLine;
        }

        public static string GetSourceMethod(Exception exception, string? compilerMemberName, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasMethod() == true ? (frame.GetFileName() ?? string.Empty) : (compilerMemberName ?? string.Empty);
        }

        public static bool GetSuppressTaskMessages(bool defaultValue = true)
        {
            return Environment.TestEnvironmentValue("SuppressTaskMessages", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }
    }
}
