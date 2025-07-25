// This file is part of CycloneDX CLI Tool
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
// SPDX-License-Identifier: Apache-2.0 Copyright (c) OWASP Foundation. All Rights Reserved. Ignore Spelling: cyclonedx Cli
namespace MSBuild.ExtensionPack.SqlServer
{
    using Microsoft.Data.SqlClient;

    using System;
    using System.IO;

    internal class ExecuteEventArgs : EventArgs
    {
        public ExecuteEventArgs(FileInfo scriptFileInfo)
        {
            this.ScriptFileInfo = scriptFileInfo;
            Succeeded = true;
        }

        public ExecuteEventArgs(SqlErrorCollection sqlInfo)
        {
            Succeeded = true;
            this.SqlInfo = sqlInfo;
        }

        public ExecuteEventArgs(FileInfo scriptFileInfo, Exception reasonForFailure)
        {
            this.ScriptFileInfo = scriptFileInfo;
            ExecutionException = reasonForFailure;
        }

        public Exception ExecutionException { get; }
        public FileInfo ScriptFileInfo { get; }
        public SqlErrorCollection SqlInfo { get; }
        public bool Succeeded { get; }
    }
}
