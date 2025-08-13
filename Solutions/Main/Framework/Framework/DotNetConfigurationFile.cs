// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Licensed under the Apache License, Version 2.0 (the “License”); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an “AS IS”
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language
// governing permissions and limitations under the License.
//
// SPDX-License-Identifier: Apache-2.0 Copyright (c) 2025, John Merryweather Cooper. All Rights Reserved. Ignore Spelling: cyclonedx Cli
namespace MSBuild.ExtensionPack.Framework
{
    public enum DotNetConfigurationFile
    {
        /// <summary>
        /// Update the machine.config.
        /// </summary>
        MachineConfig,

        /// <summary>
        /// Update the web.config in the framework config directory
        /// </summary>
        WebConfig
    }
}
