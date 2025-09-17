// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

namespace MSBuild.ExtensionPack.Computer
{
    using Microsoft.Build.Framework;
    using Microsoft.Win32;

    using MSBuild.ExtensionPack.Base;

    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>CheckEmpty</i> ( <b>Required:</b> RegistryHive, Key <b>Optional:</b> RegistryView <b>Output:</b> Empty)</para>
    /// <para>
    /// <i>CheckValueExists</i> ( <b>Required:</b> RegistryHive, Key, Value <b>Optional:</b> RegistryView <b>Output:</b> Empty (true
    /// iff the value does not exist))
    /// </para>
    /// <para><i>CreateKey</i> ( <b>Required:</b> RegistryHive, Key <b>Optional:</b> RegistryView)</para>
    /// <para><i>DeleteKey</i> ( <b>Required:</b> RegistryHive, Key <b>Optional:</b> RegistryView)</para>
    /// <para><i>DeleteKeyTree</i> ( <b>Required:</b> RegistryHive, Key <b>Optional:</b> RegistryView )</para>
    /// <para>
    /// <i>DeleteValue</i> ( <b>Required:</b> RegistryHive, Key, Value <b>Optional:</b> RegistryView <b>Output:</b> Empty (true iff
    /// the Delete was redundant))
    /// </para>
    /// <para><i>Get</i> ( <b>Required:</b> RegistryHive, Key, Value <b>Optional:</b> RegistryView <b>Output:</b> Data)</para>
    /// <para><i>Set</i> ( <b>Required:</b> RegistryHive, Key, Value <b>Optional:</b> DataType, RegistryView)</para>
    /// <para><b>Remote Execution Support:</b> Yes</para>
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
    ///<!-- Create a key -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="CreateKey" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp"/>
    ///<!-- Check if a key is empty -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="CheckEmpty" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp">
    ///<Output PropertyName="REmpty" TaskParameter="Empty"/>
    ///</MSBuild.ExtensionPack.Computer.Registry>
    ///<Message Text="SOFTWARE\ANewTemp is empty: $(REmpty)"/>
    ///<!-- Set a value -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="Set" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" Value="MySetting" Data="21"/>
    ///<!-- Check if the value exists -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="CheckValueExists" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" Value="MySetting">
    ///<Output PropertyName="RExists" TaskParameter="Exists"/>
    ///</MSBuild.ExtensionPack.Computer.Registry>
    ///<Message Text="SOFTWARE\ANewTemp\@MySetting exists: $(RExists)"/>
    ///<!-- Get the value out -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="Get" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" Value="MySetting">
    ///<Output PropertyName="RData" TaskParameter="Data"/>
    ///</MSBuild.ExtensionPack.Computer.Registry>
    ///<Message Text="Registry Value: $(RData)"/>
    ///<!-- Check if a key is empty again -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="CheckEmpty" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp">
    ///<Output PropertyName="REmpty" TaskParameter="Empty"/>
    ///</MSBuild.ExtensionPack.Computer.Registry>
    ///<Message Text="SOFTWARE\ANewTemp is empty: $(REmpty)"/>
    ///<!-- Set some Binary Data -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="Set" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" DataType="Binary" Value="binval" Data="10, 43, 44, 45, 14, 255" />
    ///<!--Get some Binary Data-->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="Get" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" Value="binval">
    ///<Output PropertyName="RData" TaskParameter="Data"/>
    ///</MSBuild.ExtensionPack.Computer.Registry>
    ///<Message Text="Registry Value: $(RData)"/>
    ///<!-- Delete a value -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="DeleteValue" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp" Value="MySetting" />
    ///<!-- Delete a key -->
    ///<MSBuild.ExtensionPack.Computer.Registry TaskAction="DeleteKey" RegistryHive="LocalMachine" Key="SOFTWARE\ANewTemp"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class Registry : BaseTask
    {
        #region Private Fields

        private const string CheckEmptyTaskAction = "CheckEmpty";
        private const string CheckValueExistsTaskAction = "CheckValueExists";
        private const string CreateKeyTaskAction = "CreateKey";
        private const string DeleteKeyTaskAction = "DeleteKey";
        private const string DeleteKeyTreeTaskAction = "DeleteKeyTree";
        private const string DeleteValueTaskAction = "DeleteValue";
        private const string GetTaskAction = "Get";
        private const string SetTaskAction = "Set";
        private RegistryHive hive;
        private RegistryKey registryKey;
        private RegistryView view = Microsoft.Win32.RegistryView.Default;

        #endregion Private Fields

        #region Private Methods

        private static string GetRegistryKeyValue(RegistryKey subkey, string value)
        {
            var v = subkey.GetValue(value);
            if (v is null)
            {
                return null;
            }

            RegistryValueKind valueKind = subkey.GetValueKind(value);
            if (valueKind == RegistryValueKind.Binary && v is byte[])
            {
                byte[] valueBytes = (byte[])v;
                StringBuilder bytes = new StringBuilder(valueBytes.Length * 2);
                foreach (byte b in valueBytes)
                {
                    bytes.Append(b.ToString(CultureInfo.InvariantCulture));
                    bytes.Append(',');
                }

                return bytes.ToString(0, bytes.Length - 1);
            }

            if (valueKind == RegistryValueKind.MultiString && v is string[])
            {
                var itemList = new StringBuilder();
                foreach (string item in (string[])v)
                {
                    itemList.Append(item);
                    itemList.Append(',');
                }

                return itemList.ToString(0, itemList.Length - 1);
            }

            return v.ToString();
        }

        /// <summary>
        /// Checks if a Registry Key contains values or subkeys.
        /// </summary>
        private void CheckEmpty()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Checking if Registry Key: {0} is empty in Hive: {1}, View: {2} on: {3}",
                arguments: [this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            RegistryKey? subKey = this.registryKey.OpenSubKey(this.Key, true);
            if (subKey is not null)
            {
                if (subKey.SubKeyCount <= 0)
                {
                    this.Empty = subKey.ValueCount <= 0;
                }
                else
                {
                    this.Empty = false;
                }
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Registry Key: {0} not found in Hive: {1}, View: {2} on: {3}", this.Key, this.RegistryHive, this.RegistryView, this.MachineName));
            }
        }

        private void CheckValueExists()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Checking if Registry Value: {0} for Key {1} exists in Hive: {2} on: {3}",
                arguments: [this.Value, this.Key, this.RegistryHive, this.MachineName]);
            RegistryKey? subKey = this.registryKey.OpenSubKey(this.Key, false);
            this.Exists = subKey?.GetValue(this.Value) is not null;
        }

        private void CreateKey()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Creating Registry Key: {0} in Hive: {1}, View: {2} on: {3}",
                arguments: [this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            using RegistryKey r = RegistryKey.OpenRemoteBaseKey(this.hive, this.MachineName, this.view);
            using RegistryKey r2 = r.CreateSubKey(this.Key);
        }

        private void DeleteKey()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Deleting Registry Key: {0} in Hive: {1}, View: {2} on: {3}",
                arguments: [this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            using RegistryKey r = RegistryKey.OpenRemoteBaseKey(this.hive, this.MachineName, this.view);
            r.DeleteSubKey(this.Key, false);
        }

        private void DeleteKeyTree()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Deleting Key Tree: {0} in Hive: {1}, View: {2} on: {3}",
                arguments: [this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            using RegistryKey r = RegistryKey.OpenRemoteBaseKey(this.hive, this.MachineName, this.view);
            r.DeleteSubKeyTree(this.Key);
        }

        private void DeleteValue()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Deleting Registry value: {0} from Key: {1} in Hive: {2} on: {3}",
                arguments: [this.Value, this.Key, this.RegistryHive, this.MachineName]);
            RegistryKey? subKey = this.registryKey.OpenSubKey(this.Key, true);
            var val = subKey?.GetValue(this.Value);
            if (val is not null)
            {
                subKey?.DeleteValue(this.Value);
            }
        }

        private void Get()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Getting Registry value: {0} from Key: {1} in Hive: {2}, View: {3} on: {4}",
                arguments: [this.Value, this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            RegistryKey? subKey = this.registryKey.OpenSubKey(this.Key, false);
            if (subKey is null)
            {
                this.Log.LogTaskError("The Registry Key provided is not valid: {0}", this.Key);
                return;
            }

            if (subKey.GetValue(this.Value) is null)
            {
                this.Log.LogTaskMessage(
                    predicate: () => string.IsNullOrEmpty(this.Value),
                    messageImportance: MessageImportance.Normal,
                    message: "A Default value was not found for the Registry Key: {0}",
                    arguments: this.Key);
                this.Log.LogTaskMessage(
                    predicate: () => !string.IsNullOrEmpty(this.Value),
                    messageImportance: MessageImportance.Normal,
                    message: "The Registry value provided is not valid: {0}",
                    arguments: this.Value);
                return;
            }

            this.Data = GetRegistryKeyValue(subKey, this.Value);
            subKey.Close();
            this.registryKey.Close();
        }

        private void Set()
        {
            this.Log.LogTaskMessage(
                predicate: () => true,
                messageImportance: MessageImportance.Normal,
                message: "Setting Registry Value: {0} for Key: {1} in Hive: {2}, View: {3} on: {4}",
                arguments: [this.Value, this.Key, this.RegistryHive, this.RegistryView, this.MachineName]);
            bool changed = false;
            RegistryKey? subKey = this.registryKey.OpenSubKey(this.Key, true);
            if (subKey is not null)
            {
                string oldData = GetRegistryKeyValue(subKey, this.Value);
                if (oldData is null || oldData != this.Data)
                {
                    if (string.IsNullOrEmpty(this.DataType))
                    {
                        subKey.SetValue(this.Value, this.Data ?? string.Empty);
                    }
                    else
                    {
                        // assumption that ',' is separator for binary and multistring value types.
                        char[] separator = { ',' };
                        object registryValue;

                        RegistryValueKind valueKind = Enum.Parse<RegistryValueKind>(this.DataType, true);
                        switch (valueKind)
                        {
                            case RegistryValueKind.Binary:
                                string[] parts = this.Data.Split(separator);
                                byte[] val = new byte[parts.Length];
                                for (int i = 0; i < parts.Length; i++)
                                {
                                    val[i] = byte.Parse(parts[i], CultureInfo.CurrentCulture);
                                }

                                registryValue = val;
                                break;

                            case RegistryValueKind.DWord:
                                registryValue = uint.Parse(this.Data, CultureInfo.CurrentCulture);
                                break;

                            case RegistryValueKind.MultiString:
                                string[] parts1 = this.Data.Split(separator);
                                registryValue = parts1;
                                break;

                            case RegistryValueKind.QWord:
                                registryValue = ulong.Parse(this.Data, CultureInfo.CurrentCulture);
                                break;

                            default:
                                registryValue = this.Data;
                                break;
                        }

                        subKey.SetValue(this.Value, registryValue, valueKind);
                    }

                    changed = true;
                }

                subKey.Close();
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Registry Key: {0} not found in Hive: {1}, View: {2} on: {3}", this.Key, this.RegistryHive, this.RegistryView, this.MachineName));
            }

            if (changed)
            {
                // Broadcast config change
                if (NativeMethods.SendMessageTimeout(NativeMethods.HWND_BROADCAST, NativeMethods.WM_SETTINGCHANGE, 0, "Environment", NativeMethods.SMTO_ABORTIFHUNG, NativeMethods.SENDMESSAGE_TIMEOUT, 0) == 0)
                {
                    this.Log.LogTaskWarning("NativeMethods.SendMessageTimeout returned 0");
                }
            }

            this.registryKey.Close();
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            try
            {
                this.registryKey = RegistryKey.OpenRemoteBaseKey(this.hive, this.MachineName, this.view);
            }
            catch (System.ArgumentException)
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "The Registry Hive provided is not valid: {0}", this.RegistryHive));
                return;
            }

            switch (this.TaskAction)
            {
                case CreateKeyTaskAction:
                    this.CreateKey();
                    break;

                case DeleteKeyTaskAction:
                    this.DeleteKey();
                    break;

                case DeleteKeyTreeTaskAction:
                    this.DeleteKeyTree();
                    break;

                case GetTaskAction:
                    this.Get();
                    break;

                case SetTaskAction:
                    this.Set();
                    break;

                case CheckEmptyTaskAction:
                    this.CheckEmpty();
                    break;

                case DeleteValueTaskAction:
                    this.DeleteValue();
                    break;

                case CheckValueExistsTaskAction:
                    this.CheckValueExists();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Gets the data.
        /// </summary>
        [Output]
        public string Data { get; set; }

        /// <summary>
        /// Sets the <see cref="Type"/> of the data. RegistryValueKind Enumeration. Support for Binary, DWord, MultiString, QWord, ExpandString
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Indicates whether the Registry Key is empty or not
        /// </summary>
        [Output]
        public bool Empty { get; set; }

        /// <summary>
        /// Indicates whether the Registry value exists
        /// </summary>
        [Output]
        public bool Exists { get; set; }

        /// <summary>
        /// Sets the key.
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// Sets the Registry Hive. Supports ClassesRoot, CurrentUser, LocalMachine, Users, PerformanceData, CurrentConfig, DynData
        /// </summary>
        [Required]
        public string RegistryHive
        {
            get => this.hive.ToString();
            set => this.hive = (RegistryHive)Enum.Parse(typeof(RegistryHive), value);
        }

        /// <summary>
        /// Sets the Registry View. Supports Registry32, Registry64 and Default. Defaults to Default
        /// </summary>
        public string RegistryView
        {
            get => this.view.ToString();
            set => this.view = (RegistryView)Enum.Parse(typeof(RegistryView), value);
        }

        /// <summary>
        /// Sets the value. If Value is not provided, an attempt will be made to read the Default Value.
        /// </summary>
        public string Value { get; set; }

        #endregion Public Properties
    }
}
