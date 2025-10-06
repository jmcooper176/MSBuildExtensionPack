namespace MSBuild.ExtensionPack.Base.Logging
{
    using System;
    using System.Globalization;

    public static class Environment
    {
        #region Public Methods

        public static bool? TestEnvironmentValue(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            string? value = System.Environment.GetEnvironmentVariable(variable, target);

            return string.IsNullOrEmpty(value)
                ? null
                : int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out int nonZero)
                    ? nonZero >= 1 || nonZero < 0
                    : (bool.TryParse(value, out bool result) || (result = Convert.ToBoolean(value, CultureInfo.CurrentCulture))) && result;
        }

        #endregion Public Methods
    }
}
