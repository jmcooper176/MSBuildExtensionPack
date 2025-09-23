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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using MSBuild.ExtensionPack.Base.Interface;
using MSBuild.ExtensionPack.Base.Wmi;

namespace MSBuild.ExtensionPack.Base.Validator
{
    public class NullValidator : IPropertyValidator<object?>
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullValidator"/> class.
        /// </summary>
        public NullValidator()
            : this($"No validation of 'System.Object' value for Validator '{nameof(NullValidator)}'.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullValidator"/> class.
        /// </summary>
        /// <param name="errorMessageAccessor">Specifies a <see cref="Func{TResult}"/> that enables access to validation resources.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="errorMessageAccessor"/> is <see langref="null"/>.</exception>
        public NullValidator(Func<string> errorMessageAccessor)
        {
            ArgumentNullException.ThrowIfNull(errorMessageAccessor, nameof(errorMessageAccessor));

            ErrorMessageString = errorMessageAccessor.Invoke();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullValidator"/> class.
        /// </summary>
        /// <param name="errorMessage">Specifies the error message to associate with this validator.</param>
        public NullValidator(string errorMessage)
            : this(errorMessageAccessor: () => errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <inheritdoc/>
        public string? DetailedMessage
        {
            get
            {
                StringBuilder buffer = new(Initialize.OPTIMAL_INITIAL_STRINGBUILDER);
                buffer.AppendLine(ErrorMessageString);
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Value Instance Type:  {0}", ObjectType.Name).AppendLine();
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Instance Value:  {0}", ObjectInstance).AppendLine();
                return buffer.ToString();
            }
        }

        /// <inheritdoc/>
        public string? DisplayName { get; set; }

        /// <inheritdoc/>
        public string? ErrorMessage { get; set; }

        /// <inheritdoc/>
        public string? ErrorMessageResourceName { get; set; }

        /// <inheritdoc/>
        public Type? ErrorMessageResourceType { get; set; }

        /// <inheritdoc/>
        public string ErrorMessageString { get; private set; }

        /// <inheritdoc/>
        public bool FailureAsWarning { get; set; }

        /// <inheritdoc/>
        public ISet<object?>? Illegal => null;

        /// <inheritdoc/>
        public ISet<object?>? Indeterminate => null;

        /// <inheritdoc/>
        public IDictionary<object, object?> Items => new Dictionary<object, object?>();

        /// <inheritdoc/>
        public ISet<object?> Legal => new HashSet<object?>();

        /// <inheritdoc/>
        public string? MemberName { get; set; }

        /// <inheritdoc/>
        public object? ObjectInstance { get; private set; }

        /// <inheritdoc/>
        public Type ObjectType => ObjectInstance?.GetType() ?? typeof(object);

        /// <inheritdoc/>
        public bool ThrowOnFailure { get; set; }

        /// <inheritdoc/>
        public string? ValidatorFullName => this.GetType().FullName;

        /// <inheritdoc/>
        public string ValidatorName => this.GetType().Name;

        /// <inheritdoc/>
        public bool ValidatorResult { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <inheritdoc/>
        public string? FormatErrorMessage(string name, [CallerFilePath] string? path = null, [CallerMemberName] string? member = null, [CallerLineNumber] int lineNumber = 0)
        {
            return FormatErrorMessage(name, ObjectInstance, path, member, lineNumber);
        }

        /// <inheritdoc/>
        public string? FormatErrorMessage(string name, object? value, [CallerFilePath] string? path = null, [CallerMemberName] string? member = null, [CallerLineNumber] int lineNumber = 0)
        {
            DisplayName = member ?? string.Empty;
            ObjectInstance = value;
            MemberName = member ?? string.Empty;
            ErrorMessageString = $"{path}({lineNumber}) : Name {name} no validation for Member {MemberName} of Type {ObjectType.Name} with Value {value}.";
            return ErrorMessageString;
        }

        /// <inheritdoc/>
        public bool GetValidationResult(object? value)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool IsValid(object? value)
        {
            return IsValid(value, nameof(value));
        }

        /// <inheritdoc/>
        public bool IsValid(object? value, string name)
        {
            ObjectInstance = value;
            MemberName = name;
            this.ValidatorResult = GetValidationResult(ObjectInstance);

            return this.ValidatorResult;
        }

        /// <inheritdoc/>
        public void Validate(object? value)
        {
            Validate(value, nameof(value));
        }

        /// <inheritdoc/>
        public void Validate(object? value, string name)
        {
            return;
        }

        #endregion Public Methods
    }
}
