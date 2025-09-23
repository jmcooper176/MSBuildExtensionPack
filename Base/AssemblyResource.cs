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
using System.Globalization;
using System.Resources;

using MSBuild.ExtensionPack.Base.Shared.MSBuild.ExtensionPack.Base.Shared;

namespace MSBuild.ExtensionPack.Base
{
    internal static class AssemblyResource
    {
        #region Internal Properties

        /// <summary>
        /// Gets the assembly's primary resources i.e. the resources exclusively owned by this assembly.
        /// </summary>
        /// <remarks>This property is thread-safe.</remarks>
        /// <value>ResourceManager for primary resources.</value>
        internal static ResourceManager PrimaryResources { get; } = new ResourceManager("Microsoft.Build.Utilities.Core.Strings", typeof(AssemblyResource).GetType().Assembly);

        /// <summary>
        /// Gets the assembly's shared resources i.e. the resources this assembly shares with other assemblies.
        /// </summary>
        /// <remarks>This property is thread-safe.</remarks>
        /// <value>ResourceManager for shared resources.</value>
        internal static ResourceManager SharedResources { get; } = new ResourceManager("Microsoft.Build.Utilities.Core.Strings.shared", typeof(AssemblyResource).GetType().Assembly);

        #endregion Internal Properties

        #region Internal Methods

        /// <summary>
        /// Loads the specified resource string and optionally formats it using the given arguments. The current thread's culture is
        /// used for formatting.
        /// </summary>
        /// <remarks>
        /// 1) This method requires the owner task to have registered its resources either via the Task (or TaskMarshalByRef) base
        /// class constructor, or the Task.TaskResources (or AppDomainIsolatedTask.TaskResources) property.
        /// 2) This method is thread-safe.
        /// </remarks>
        /// <param name="resourceName">The name of the string resource to load.</param>
        /// <param name="args">        Optional arguments for formatting the loaded string.</param>
        /// <returns>The formatted string.</returns>
        internal static string FormatResourceString(string resourceName, params object[] args)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(resourceName);

            // NOTE: the ResourceManager.GetString() method is thread-safe
            string resourceString = GetString(resourceName);

            return FormatString(resourceString, args);
        }

        /// <summary>
        /// Formats the given string using the variable arguments passed in. The current thread's culture is used for formatting.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="unformatted">The string to format.</param>
        /// <param name="args">       Arguments for formatting.</param>
        /// <returns>The formatted string.</returns>
        internal static string FormatString(string unformatted, params object[] args)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(unformatted);

            return ResourceUtilities.FormatString(unformatted, args);
        }

        /// <summary>
        /// Loads the specified resource string, either from the assembly's primary resources, or its shared resources.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="name"></param>
        /// <returns>The resource string, or <see langref="null"/> if not found.</returns>
        internal static string GetString(string name)
        {
            string? resource = PrimaryResources.GetString(name, CultureInfo.CurrentUICulture)
                ?? SharedResources.GetString(name, CultureInfo.CurrentUICulture);

            ArgumentNullException.ThrowIfNull(resource, nameof(resource));

            return resource;
        }

        #endregion Internal Methods
    }
}
