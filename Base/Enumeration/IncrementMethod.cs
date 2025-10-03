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
namespace MSBuild.ExtensionPack.Base.Enumeration
{
    /// <summary>
    /// Specifies how certain version numbers are incremented by the task.
    /// </summary>
    public enum IncrementMethod
    {
        /// <summary>
        /// Do not auto-increment the number.
        /// </summary>
        NoIncrement = 0,

        /// <summary>
        /// Add one to the current number.
        /// </summary>
        AutoIncrement = 1,

        /// <summary>
        /// Format the current date and time using a formatting string, and use that as the number.
        /// </summary>
        DateString = 2,

        /// <summary>
        /// Format the current date as the two digit year and the day of the year, and use that as the number, i.e. the revision
        /// number for 7/03/2009 is 09184
        /// </summary>
        Julian = 3,

        /// <summary>
        /// Format the current date as YYWWDW where YY is the year, WW is the week number and DW is the day of the week e.g. 2 Feb
        /// 2010 would be 10062. 15 March 2010 will be 10121 and 19 December 2010 10475.
        /// </summary>
        YearWeekDay = 4,

        /// <summary>
        /// Calculate the number of days elapsed since a given StartDate. Take note of the StartDate, PaddingCount and PaddingDigit parameters.
        /// </summary>
        ElapsedDays = 5
    }
}
