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
namespace MSBuild.ExtensionPack
{
    using System;
    using System.Configuration;
    using System.Globalization;

    using Microsoft.Build.Framework;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// Task used to work with the .NET framework web.config and machine config files <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>ProtectConfigSection</i> ( <b>Required:</b> Section <b>Optional:</b> Site, Path, ConfigurationFileType,
    /// ProtectionProvider, SaveMode)
    /// </para>
    /// <para><i>RemoveAppSetting</i> ( <b>Required:</b> SettingName <b>Optional:</b> Site, Path, ConfigurationFileType, SaveMode)</para>
    /// <para><i>RemoveConnectionString</i> ( <b>Required:</b> SettingName <b>Optional:</b> Site, Path, ConfigurationFileType, SaveMode)</para>
    /// <para>
    /// <i>SetAppSetting</i> ( <b>Required:</b> SettingName <b>Optional:</b> Site, Path, SettingValue, ConfigurationFileType, SaveMode)
    /// </para>
    /// <para>
    /// <i>SetConnectionString</i> ( <b>Required:</b> SettingName <b>Optional:</b> Site, Path, SettingValue, ConfigurationFileType, SaveMode)
    /// </para>
    /// <para><i>UnprotectConfigSection</i> ( <b>Required:</b> Section <b>Optional:</b> Site, Path, ConfigurationFileType, SaveMode)</para>
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
    ///<ItemGroup>
    ///<MachineConfigSettings Include="settingName" >
    ///<Value>settingValue</Value>
    ///</MachineConfigSettings>
    ///</ItemGroup>
    ///<!-- Update machine.config app settings -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="SetAppSetting" SettingName="%(MachineConfigSettings.Identity)" SettingValue="%(Value)" SaveMode="Full"/>
    ///<ItemGroup>
    ///<ConnectionStrings Include="myAppDB">
    ///<Value>Server=MyServer;</Value>
    ///</ConnectionStrings>
    ///</ItemGroup>
    ///<!-- Update a website's connection strings -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="SetConnectionString" SettingName="%(ConnectionStrings.Identity)" SettingValue="%(Value)" ConfigurationFileType="WebConfig" Site="NewSite" Path="/" />
    ///<!-- Encrypt a website's connection strings -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="ProtectConfigSection" Section="connectionStrings"  ProtectionProvider="DataProtectionConfigurationProvider" ConfigurationFileType="WebConfig" Site="NewSite" Path="/" />
    ///<!-- Un-encrypt a website's connection strings -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="UnprotectConfigSection" Section="connectionStrings" ConfigurationFileType="WebConfig" Site="NewSite" Path="/" />
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="RemoveConnectionString" SettingName="%(ConnectionStrings.Identity)" ConfigurationFileType="WebConfig"  Site="NewSite" Path="/" />
    ///<!--- Remove a setting from a website -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="RemoveAppSetting" SettingName="removeMe" ConfigurationFileType="WebConfig"  Site="NewSite" Path="/" />
    ///<!-- Remove connection string 'obsoleteConnection' from machine.config file -->
    ///<MSBuild.ExtensionPack.Framework.ConfigManager TaskAction="RemoveConnectionString" SettingName="obsoleteConnection" />
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public sealed class ConfigManager : BaseTask
    {
        #region Private Fields

        private const string ProtectConfigSectionAction = "ProtectConfigSection";
        private const string RemoveAppSettingTaskAction = "RemoveAppSetting";
        private const string RemoveConnectionStringTaskAction = "RemoveConnectionString";
        private const string SetAppSettingTaskAction = "SetAppSetting";
        private const string SetConnectionStringTaskAction = "SetConnectionString";
        private const string UnprotectConfigSectionAction = "UnprotectConfigSection";

        private DotNetConfigurationFile configurationFileType = DotNetConfigurationFile.MachineConfig;
        private ConfigurationSaveMode saveMode = ConfigurationSaveMode.Minimal;

        #endregion Private Fields

        #region Private Properties

        private KeyValueConfigurationCollection AppSettings => this.Config.AppSettings.Settings;

        private Configuration Config { get; set; }

        private ConnectionStringSettingsCollection ConnectionStrings => this.Config.ConnectionStrings.ConnectionStrings;

        #endregion Private Properties

        #region Private Methods

        private void RemoveAppSetting(bool save)
        {
            if (this.AppSettings[this.SettingName] is null)
            {
                this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.InvariantCulture, "Setting not found '{0}' in {1}.", this.SettingName, this.Config.FilePath));
                return;
            }

            this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Removing app setting '{0}' from {1}.", this.SettingName, this.Config.FilePath));
            this.AppSettings.Remove(this.SettingName);
            if (save)
            {
                this.Save();
            }
        }

        private void RemoveConnectionString(bool save)
        {
            if (this.ConnectionStrings[this.SettingName] is null)
            {
                this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.InvariantCulture, "Setting not found '{0}' in {1}.", this.SettingName, this.Config.FilePath));
                return;
            }

            this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Removing connection string '{0}' from {1}.", this.SettingName, this.Config.FilePath));
            this.ConnectionStrings.Remove(this.SettingName);
            if (save)
            {
                this.Save();
            }
        }

        private void Save()
        {
            this.Config.Save(this.saveMode);
        }

        private void SetAppSetting()
        {
            this.RemoveAppSetting(false);
            this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Setting app setting '{0}' in {1}.", this.SettingName, this.Config.FilePath));
            this.AppSettings.Add(this.SettingName, this.SettingValue);
            this.Save();
        }

        private void SetConnectionString()
        {
            this.RemoveConnectionString(false);
            this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Setting connection string '{0}' in {1}.", this.SettingName, this.Config.FilePath));
            this.ConnectionStrings.Add(new ConnectionStringSettings(this.SettingName, this.SettingValue));
            this.Save();
        }

        #endregion Private Methods

        #region Protected Methods

        protected override void InternalExecute()
        {
            switch (this.configurationFileType)
            {
                case DotNetConfigurationFile.MachineConfig:
                    this.Config = WebConfigurationManager.OpenMachineConfiguration();
                    break;

                case DotNetConfigurationFile.WebConfig:
                    this.Config = WebConfigurationManager.OpenWebConfiguration(this.Path, this.Site);
                    break;

                default:
                    this.Log.LogError("Task parameter ConfigurationFile has an unrecognized value.");
                    return;
            }

            switch (this.TaskAction)
            {
                case RemoveAppSettingTaskAction:
                    this.RemoveAppSetting(true);
                    break;

                case RemoveConnectionStringTaskAction:
                    this.RemoveConnectionString(true);
                    break;

                case SetAppSettingTaskAction:
                    this.SetAppSetting();
                    break;

                case SetConnectionStringTaskAction:
                    this.SetConnectionString();
                    break;

                case ProtectConfigSectionAction:
                    this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Protecting section '{0}' in {1}.", this.Section, this.Config.FilePath));
                    ConfigurationSection cs = this.Config.Sections[this.Section];
                    cs.SectionInformation.ProtectSection(this.ProtectionProvider);
                    this.Save();
                    break;

                case UnprotectConfigSectionAction:
                    this.LogTaskMessage(string.Format(CultureInfo.InvariantCulture, "Unprotecting section '{0}' in {1}.", this.Section, this.Config.FilePath));
                    ConfigurationSection cs2 = this.Config.Sections[this.Section];
                    cs2.SectionInformation.UnprotectSection();
                    this.Save();
                    break;

                default:
                    this.Log.LogError("Invalid task action: {0}.", this.TaskAction);
                    break;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Which .NET framework configuration file to update. Supports WebConfig and MachineConfig. Default is MachineConfig
        /// </summary>
        public string ConfigurationFileType
        {
            get => this.configurationFileType.ToString();
            set => this.configurationFileType = (DotNetConfigurationFile)Enum.Parse(typeof(DotNetConfigurationFile), value);
        }

        /// <summary>
        /// Sets the Path to work on. Leave blank to target the .net framework web.config
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The encryption provider. Supports RSAProtectedConfigurationProvider and DataProtectionConfigurationProvider. Default is RSAProtectedConfigurationProvider
        /// </summary>
        public string ProtectionProvider { get; set; } = "RSAProtectedConfigurationProvider";

        /// <summary>
        /// How should changes to the config file be saved? See
        /// http://msdn.microsoft.com/en-us/library/system.configuration.configurationsavemode.aspx for the list of values. Default
        /// is Minimal
        /// </summary>
        public string SaveMode
        {
            get => this.saveMode.ToString();
            set => this.saveMode = (ConfigurationSaveMode)Enum.Parse(typeof(ConfigurationSaveMode), value);
        }

        /// <summary>
        /// The config section to protect or unprotect
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// The setting name to update.
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// The setting's value.
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        /// Sets the Site to work on. Leave blank to target the .net framework web.config
        /// </summary>
        public string Site { get; set; }

        #endregion Public Properties
    }
}
