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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using MSBuild.ExtensionPack.Base.Enumeration;
using MSBuild.ExtensionPack.Base.Interface;
using MSBuild.ExtensionPack.Base.Wmi;

namespace MSBuild.ExtensionPack.Base.Validator
{
    /// <summary>
    /// Implements <see cref="IPropertyValidator{TValue}"/> for values of <see cref="Type"/><see cref="int"/>.
    /// </summary>
    public class IntValidator : IPropertyValidator<int>
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntValidator"/> class.
        /// </summary>
        public IntValidator()
            : this($"No validation of 'int' value for Validator '{nameof(IntValidator)}'")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntValidator"/> class.
        /// </summary>
        /// <param name="errorMessageAccessor">Specifies a <see cref="Func{TResult}"/> that enables access to validation resources.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="errorMessageAccessor"/> is <see langref="null"/>.</exception>
        public IntValidator(Func<string> errorMessageAccessor)
        {
            ArgumentNullException.ThrowIfNull(errorMessageAccessor, nameof(errorMessageAccessor));

            ErrorMessageString = errorMessageAccessor.Invoke();
            Indeterminate = new HashSet<int>();
            Items = new Dictionary<object, object?>();

            switch (Range)
            {
                case IntValidatorRange.All:
                    Minimum = int.MinValue;
                    Maximum = int.MaxValue;
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Illegal = new HashSet<int>();
                    break;

                case IntValidatorRange.Positive:
                    Minimum = 1;
                    Maximum = int.MaxValue;
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Illegal = new HashSet<int>(Enumerable.Range(int.MinValue, 0));
                    break;

                case IntValidatorRange.NonNegative:
                    Minimum = 0;
                    Maximum = int.MaxValue;
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Illegal = new HashSet<int>(Enumerable.Range(int.MinValue, -1));
                    break;

                case IntValidatorRange.Negative:
                    Minimum = int.MinValue;
                    Maximum = -1;
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Illegal = new HashSet<int>(Enumerable.Range(0, int.MaxValue));
                    break;

                case IntValidatorRange.NonZero:
                    Minimum = int.MinValue;
                    Maximum = int.MaxValue;
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Legal.Remove(0);
                    Illegal = new HashSet<int>([0]);
                    break;

                case IntValidatorRange.Zero:
                    Minimum = 0;
                    Maximum = 0;
                    Legal = new HashSet<int>([0]);
                    Illegal = new HashSet<int>(Enumerable.Range(int.MinValue, int.MaxValue));
                    Illegal.Remove(0);
                    break;

                case IntValidatorRange.Inclusive:
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum));
                    Illegal = new HashSet<int>();
                    Illegal.UnionWith(Enumerable.Range(int.MinValue, Minimum - 1));
                    Illegal.UnionWith(Enumerable.Range(Maximum + 1, int.MaxValue));
                    break;

                case IntValidatorRange.Exclusive:
                    Legal = new HashSet<int>(Enumerable.Range(Minimum + 1, Maximum - 1));
                    Illegal = new HashSet<int>();
                    Illegal.UnionWith(Enumerable.Range(int.MinValue, Minimum));
                    Illegal.UnionWith(Enumerable.Range(Maximum, int.MaxValue));
                    break;

                case IntValidatorRange.HalfInclusive:
                    Legal = new HashSet<int>(Enumerable.Range(Minimum, Maximum - 1));
                    Illegal = new HashSet<int>();
                    Illegal.UnionWith(Enumerable.Range(int.MinValue, Minimum - 1));
                    Illegal.UnionWith(Enumerable.Range(Maximum, int.MaxValue));
                    break;

                case IntValidatorRange.HalfExclusive:
                    Legal = new HashSet<int>(Enumerable.Range(Minimum + 1, Maximum));
                    Illegal = new HashSet<int>();
                    Illegal.UnionWith(Enumerable.Range(int.MinValue, Minimum));
                    Illegal.UnionWith(Enumerable.Range(Maximum + 1, int.MaxValue));
                    Indeterminate = new HashSet<int>();
                    break;

                default:
                    Legal = new HashSet<int>();
                    Illegal = new HashSet<int>(Enumerable.Range(int.MinValue, int.MaxValue));
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntValidator"/> class.
        /// </summary>
        /// <param name="errorMessage">Specifies the error message to associate with this validator.</param>
        public IntValidator(string errorMessage)
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
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Value Instance Type:  {0}", ObjectType).AppendLine();
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Pre-defined Range:  {0}", Range).AppendLine();
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Property 'Minimum' Value:  {0}", Minimum).AppendLine();
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Property 'Maximum' Value:  {0}", Maximum).AppendLine();
                buffer.AppendFormat(CultureInfo.InvariantCulture, "Instance Value:  {0}", ObjectInstance).AppendLine();
                return buffer.ToString();
            }
        }

        /// <inheritdoc/>
        public string? DisplayName
        {
            get
            {
                return MemberName ?? "Null Member Name";
            }

            set
            {
                MemberName = value;
            }
        }

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
        public ISet<int>? Illegal { get; }

        /// <inheritdoc/>
        public ISet<int>? Indeterminate { get; }

        /// <inheritdoc/>
        public IDictionary<object, object?> Items { get; }

        /// <inheritdoc/>
        public ISet<int> Legal { get; }

        /// <summary>
        /// Gets or sets a value indicating the maximum <see cref="int"/> value that is in range for inclusive and out of range for exclusive.
        /// </summary>
        public int Maximum { get; set; }

        /// <inheritdoc/>
        public string? MemberName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the minimum <see cref="int"/> value that is in range for inclusive and out of range for exclusive.
        /// </summary>
        public int Minimum { get; set; }

        /// <inheritdoc/>
        public int ObjectInstance { get; private set; }

        /// <inheritdoc/>
        public Type ObjectType => ObjectInstance.GetType();

        /// <summary>
        /// Gets or sets a value indicating the pre-defined <see cref="IntValidatorRange"/> for this validator.
        /// </summary>
        public IntValidatorRange Range { get; set; }

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
        public string? FormatErrorMessage(string name, int value, [CallerFilePath] string? path = null, [CallerMemberName] string? member = null, [CallerLineNumber] int lineNumber = 0)
        {
            DisplayName = member ?? string.Empty;
            ObjectInstance = value;
            MemberName = member ?? string.Empty;
            ErrorMessageString = $"{path}({lineNumber}) : Name {name} failed validation for Member {MemberName} of Type {ObjectType.Name} with Value {ObjectInstance}.";
            return ErrorMessageString;
        }

        /// <inheritdoc/>
        public bool GetValidationResult(int value)
        {
            return Range switch
            {
                IntValidatorRange.All => true,
                IntValidatorRange.None => false,
                IntValidatorRange.NonZero => Illegal?.Contains(value) != true,
                IntValidatorRange.Zero => value == 0,
                IntValidatorRange.Inclusive or IntValidatorRange.Exclusive or IntValidatorRange.HalfInclusive or IntValidatorRange.HalfExclusive => Legal.Contains(value),
                _ => Legal.Contains(value) && Indeterminate?.Contains(value) != true && Illegal?.Contains(value) != true,
            };
        }

        /// <inheritdoc/>
        public bool IsValid(int value)
        {
            return IsValid(value, nameof(value));
        }

        /// <inheritdoc/>
        public bool IsValid(int value, string name)
        {
            ObjectInstance = value;
            MemberName = name;
            ValidatorResult = GetValidationResult(ObjectInstance);

            if (ValidatorResult && (!ThrowOnFailure ^ FailureAsWarning))
            {
                return true;
            }
            else
            {
                throw new ValidationException(FormatErrorMessage(name, value), new ArgumentOutOfRangeException(name, value, ErrorMessageString));
            }
        }

        /// <inheritdoc/>
        public void Validate(int value)
        {
            Validate(value, nameof(value));
        }

        /// <inheritdoc/>
        public void Validate(int value, string name)
        {
            if (!IsValid(value) && (ThrowOnFailure ^ !FailureAsWarning))
            {
                throw new ValidationException(FormatErrorMessage(name, value), new ArgumentOutOfRangeException(name, value, ErrorMessageString));
            }
            else if (!IsValid(value))
            {
                Console.Error.WriteLine(FormatErrorMessage(name, value));
            }
        }

        #endregion Public Methods
    }
}
