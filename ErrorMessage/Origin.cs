namespace MSBuild.ExtensionPack.ErrorMessage
{
    using System.Text;

    public class Origin
    {
        public Origin(string path)
            : this(path, 0, 0, 0, 0)
        {
        }

        public Origin(string path, int lineNumber)
            : this(path, lineNumber, 0, 0, 0)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber)
            : this(path, lineNumber, columnNumber, 0, 0)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber, int endColumnNumber)
            : this(path, lineNumber, columnNumber, 0, endColumnNumber)
        {
        }

        public Origin(string path, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber)
        {
            Path = path;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            EndLineNumber = endLineNumber;
            EndColumnNumber = endColumnNumber;
        }

        public int ColumnNumber { get; }

        public int EndColumnNumber { get; }

        public int EndLineNumber { get; }

        public int LineNumber { get; }

        public string Path { get; }

        public override string ToString()
        {
            StringBuilder builder = new(Path);
            builder.Append('(');

            if (LineNumber <= 0)
            {
                builder.Append(')');
                return builder.ToString();
            }
            else
            {
                builder.Append(LineNumber);
            }

            if (ColumnNumber > 0 && EndColumnNumber > 0)
            {
                builder.Append(", ").Append(ColumnNumber).Append('-').Append(EndColumnNumber).Append(')');
                return builder.ToString();
            }
            else if (ColumnNumber > 0)
            {
                builder.Append(", ").Append(ColumnNumber);
            }

            if (EndLineNumber > 0)
            {
                builder.Append(", ").Append(EndLineNumber);
            }

            if (EndColumnNumber > 0)
            {
                builder.Append(", ").Append(EndColumnNumber);
            }

            builder.Append(')');
            return builder.ToString();
        }
    }
}
