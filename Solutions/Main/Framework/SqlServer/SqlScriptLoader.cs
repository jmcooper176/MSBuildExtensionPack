//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlScriptLoader.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.SqlServer
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// SqlScriptLoader
    /// </summary>
    public sealed class SqlScriptLoader
    {
        private readonly StreamReader reader;
        private readonly StringBuilder contents;
        private readonly bool strip;
        private char currentChar;
        private char nextChar;
        private bool inComment;
        private int commentDepth;
        
        /// <summary>
        /// Initializes a new instance of the SqlScriptLoader class
        /// </summary>
        /// <param name="reader">StreamReader</param>
        /// <param name="stripMultiLineComments">bool</param>
        public SqlScriptLoader(StreamReader reader, bool stripMultiLineComments)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            strip = stripMultiLineComments;
            this.reader = reader;
            contents = new StringBuilder();
        }

        /// <summary>
        /// ReadToEnd
        /// </summary>
        /// <returns>string</returns>
        public string ReadToEnd()
        {
            if (reader.EndOfStream)
            {
                return contents.ToString();
            }

            if (strip)
            {
                commentDepth = 0;
                while (!reader.EndOfStream)
                {
                    if (!Read())
                    {
                        break;
                    }

                    if (inComment && currentChar == '*' && Peek() && nextChar == '/')
                    {
                        commentDepth--;
                        Read();

                        if (commentDepth == 0)
                        {
                            inComment = false;
                            continue;
                        }
                    }

                    if (currentChar == '/' && Peek() && nextChar == '*')
                    {
                        inComment = true;
                        commentDepth++;
                        Read();
                        continue;
                    }

                    if (!inComment)
                    {
                        contents.Append(currentChar);
                    }
                }
            }
            else
            {
                contents.Append(reader.ReadToEnd());
            }

            return contents.ToString();
        }

        private bool Read()
        {
            int nextByte = reader.Read();
            if (nextByte == -1)
            {
                return false;
            }

            currentChar = Convert.ToChar(nextByte);
            return true;
        }

        private bool Peek()
        {
            int nextByte = reader.Peek();
            if (nextByte == -1)
            {
                return false;
            }

            nextChar = Convert.ToChar(nextByte);
            return true;
        }
    }
}
