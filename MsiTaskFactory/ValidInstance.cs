namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Linq;

    public static class ValidInstance
    {
        #region Public Methods

        public static bool AllNotNull(params object?[] objects)
        {
            return objects.All(o => o is not null);
        }

        public static bool AllNull(params object?[] objects)
        {
            return objects.All(o => o is null);
        }

        public static bool AnyNotNull(params object?[] objects)
        {
            return objects.Any(o => o is not null);
        }

        public static bool AnyNull(params object?[] objects)
        {
            return objects.Any(o => o is null);
        }

        public static bool IsComInstance(this object? instance)
        {
            return instance is not null && instance.GetType().IsCOMObject;
        }

        public static bool IsInstanceOfCom(this object? instance, Type? type)
        {
            return AllNotNull(instance, type) && instance.IsComInstance() && type!.IsInstanceOfType(instance);
        }

        public static bool IsInstanceOfCom<TCom>(this object? instance) where TCom : class
        {
            return instance.IsInstanceOfCom(typeof(TCom));
        }

        public static bool IsValidDirectory(string? path)
        {
            const int MAX_COMPONENT = 64;
            const int MAX_PATH = 255;

            if (string.IsNullOrWhiteSpace(path) || path.Length > MAX_PATH)
            {
                return false;
            }

            var components = path?.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries) ?? [];

            return components.All(c => c.Length <= MAX_COMPONENT) && components.All(c => c.IndexOfAny(Path.GetInvalidPathChars()) < 0);
        }

        public static bool IsValidFileName(string? name)
        {
            const int MAX_PATH = 255;
            const int MAX_FILENAME = 250;
            const int MAX_EXTENSION = 64;

            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_PATH)
            {
                return false;
            }

            var fileName = Path.GetFileName(name) ?? string.Empty;
            var extension = Path.GetExtension(name) ?? string.Empty;

            if (fileName.Length > MAX_FILENAME || extension.Length > MAX_EXTENSION)
            {
                return false;
            }

            return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        public static bool IsValidMsiIdentifier(string? id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length > 72)
            {
                return false;
            }
            else if (!id.StartsWith('_') && !char.IsAsciiLetter(id[0]))
            {
                return false;
            }
            else
            {
                return id.Skip(1).All(c => char.IsAsciiLetterOrDigit(c) || c == '_' || c == '.');
            }
        }

        public static bool IsValidPath(string? path)
        {
            const int MAX_PATH = 255;

            if (string.IsNullOrWhiteSpace(path) || path.Length > MAX_PATH)
            {
                return false;
            }

            var directory = Path.GetDirectoryName(path) ?? string.Empty;
            var fileName = Path.GetFileName(path) ?? string.Empty;

            return IsValidDirectory(directory) && IsValidFileName(fileName);
        }

        public static bool IsValidPrivateMsiIdentifier(string? id)
        {
            return IsValidMsiIdentifier(id) && !IsValidPublicMsiIdentifier(id);
        }

        public static bool IsValidPublicMsiIdentifier(string? id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length > 72)
            {
                return false;
            }
            else if (!char.IsAsciiLetterUpper(id[0]))
            {
                return false;
            }
            else
            {
                return id.Skip(1).All(c => char.IsAsciiLetterUpper(c) || char.IsAsciiDigit(c) || c == '_' || c == '.');
            }
        }

        #endregion Public Methods
    }
}
