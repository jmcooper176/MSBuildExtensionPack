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
namespace MSBuild.ExtensionPack.Framework
{
    using System.Globalization;
    using System.Security.Cryptography;

    using Microsoft.Build.Framework;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>Create</i> ( <b>Output:</b> GuidString, FormattedGuidString)</para>
    /// <para><i>CreateCrypto</i> ( <b>Output:</b> GuidString, FormattedGuidString)</para>
    /// <para><b>Remote Execution Support:</b> NA</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" DefaultTargets="Default" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<Target Name="Default">
    ///<!-- Create a new Guid and get the formatted and unformatted values -->
    ///<MSBuild.ExtensionPack.Framework.Guid TaskAction="Create">
    ///<Output TaskParameter="FormattedGuidString" PropertyName="FormattedGuidString1" />
    ///<Output TaskParameter="GuidString" PropertyName="GuidStringItem" />
    ///</MSBuild.ExtensionPack.Framework.Guid>
    ///<Message Text="GuidStringItem: $(GuidStringItem)"/>
    ///<Message Text="FormattedGuidString: $(FormattedGuidString1)"/>
    ///<!-- Create a new cryptographically strong Guid and get the formatted and unformatted values -->
    ///<MSBuild.ExtensionPack.Framework.Guid TaskAction="CreateCrypto">
    ///<Output TaskParameter="FormattedGuidString" PropertyName="FormattedGuidString1" />
    ///<Output TaskParameter="GuidString" PropertyName="GuidStringItem" />
    ///</MSBuild.ExtensionPack.Framework.Guid>
    ///<Message Text="GuidStringItem Crypto: $(GuidStringItem)"/>
    ///<Message Text="FormattedGuidString Crypto: $(FormattedGuidString1)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class Guid : BaseTask
    {
        #region Private Fields

        private System.Guid internalGuid;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Gets this instance.
        /// </summary>
        private void Get()
        {
            this.LogTaskMessage("Getting random GUID");
            this.internalGuid = System.Guid.NewGuid();
        }

        /// <summary>
        /// Gets the crypto.
        /// </summary>
        private void GetCrypto()
        {
            this.LogTaskMessage("Getting Cryptographically Secure GUID");
            byte[] data = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
                this.internalGuid = new System.Guid(data);
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            switch (this.TaskAction)
            {
                case "Create":
                    this.Get();
                    break;

                case "CreateCrypto":
                    this.GetCrypto();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// 32 digits separated by hyphens: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// </summary>
        [Output]
        public string[] FormattedGuidString => new[] { this.internalGuid.ToString("D", CultureInfo.CurrentCulture) };

        /// <summary>
        /// 32 digits: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        /// </summary>
        [Output]
        public string[] GuidString => new[] { this.internalGuid.ToString("N", CultureInfo.CurrentCulture) };

        #endregion Public Properties
    }
}
