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
namespace MSBuild.ExtensionPack.VisualStudio
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class VBPProject
    {
        #region Private Fields

        private readonly List<string> lines = new List<string>();
        private string projectFile;

        #endregion Private Fields

        #region Public Constructors

        public VBPProject()
        {
        }

        public VBPProject(string projectFileExt)
        {
            ProjectFile = projectFileExt;
        }

        #endregion Public Constructors

        #region Public Properties

        public FileInfo ArtifactFile
        {
            get
            {
                string artifactFileName = null;
                if (!GetProjectProperty("ExeName32", ref artifactFileName))
                {
                    throw new ApplicationException("'ExeName32' Property not found");
                }

                artifactFileName = artifactFileName.Replace("\"", string.Empty);

                FileInfo projectFileInfo = new FileInfo(ProjectFile);

                string artifactPath = projectFileInfo.Directory.FullName;
                string path32 = null;
                if (GetProjectProperty("Path32", ref path32))
                {
                    path32 = path32.Replace("\"", string.Empty);
                    artifactPath = Path.Combine(artifactPath, path32);
                }

                artifactFileName = Path.Combine(artifactPath, artifactFileName);
                return new FileInfo(artifactFileName);
            }
        }

        public string ProjectFile
        {
            get => projectFile;

            set
            {
                if (!File.Exists(value))
                {
                    throw new Exception("Project file name does not exist");
                }

                projectFile = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public List<FileInfo> GetFiles()
        {
            List<FileInfo> retVal = new List<FileInfo>();
            FileInfo projectFileInfo = new FileInfo(projectFile);
            foreach (var line in lines)
            {
                var splittedLine = line.Split('=');
                switch (splittedLine[0])
                {
                    case "Form":
                    case "Module":
                    case "Class":
                    case "UserControl":
                        string fileName = splittedLine[1];
                        if (fileName.Contains(";"))
                        {
                            fileName = fileName.Substring(fileName.IndexOf(";", StringComparison.CurrentCulture) + 1);
                            fileName = fileName.Trim();
                        }

                        fileName = Path.Combine(projectFileInfo.Directory.FullName, fileName);
                        retVal.Add(new FileInfo(fileName));
                        break;
                }
            }

            return retVal;
        }

        public bool GetProjectProperty(string name, ref string value)
        {
            foreach (string line in lines)
            {
                string buffer = line.ToUpper(CultureInfo.InvariantCulture);

                if (buffer.StartsWith(name.ToUpper(CultureInfo.InvariantCulture) + "=", StringComparison.OrdinalIgnoreCase))
                {
                    value = line.Substring(1 + (name + "=").Length);
                    return true;
                }
            }

            return false;
        }

        public bool Load()
        {
            if (string.IsNullOrEmpty(ProjectFile))
            {
                return false;
            }

            StreamReader lineStream = null;
            try
            {
                lineStream = new StreamReader(projectFile, Encoding.Default);
                while (!lineStream.EndOfStream)
                {
                    lines.Add(lineStream.ReadLine());
                }
            }
            catch
            {
                // intended
            }
            finally
            {
                lineStream?.Close();
            }

            return true;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(projectFile) | lines.Count == 0)
            {
                return false;
            }

            StreamWriter lineStream = null;
            bool readOnly = false;
            try
            {
                if ((File.GetAttributes(projectFile) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    readOnly = true;
                    File.SetAttributes(projectFile, FileAttributes.Normal);
                }

                lineStream = new StreamWriter(projectFile, false, Encoding.Default);
                foreach (string line in lines)
                {
                    lineStream.WriteLine(line);
                }
            }
            catch
            {
                // intended
            }
            finally
            {
                lineStream?.Close();

                if (readOnly)
                {
                    File.SetAttributes(projectFile, FileAttributes.ReadOnly);
                }
            }

            return true;
        }

        public bool SetProjectProperty(string name, string value, bool addProp)
        {
            if (string.IsNullOrEmpty(name) | string.IsNullOrEmpty(value))
            {
                return false;
            }

            int index;

            for (index = 0; index <= lines.Count - 1; index++)
            {
                string buffer = lines[index].ToUpper(CultureInfo.InvariantCulture);

                if (buffer.StartsWith(name.ToUpper(CultureInfo.InvariantCulture) + "=", StringComparison.OrdinalIgnoreCase))
                {
                    lines[index] = lines[index].Substring(0, (name + "=").Length) + value;
                    return true;
                }
            }

            if (addProp)
            {
                lines.Add(name + "=" + value);
                return true;
            }

            return false;
        }

        #endregion Public Methods
    }
}
