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
