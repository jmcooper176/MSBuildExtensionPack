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
using System.Runtime.CompilerServices;

namespace MSBuild.ExtensionPack.Base.Interface
{
    /// <summary>
    /// Interface for implementing validators for <see cref="BaseToolTask.ValidateParameters"/>.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value <see cref="Type"/> for validation.</typeparam>
    public interface IPropertyValidator<TValue>
    {
        /// <summary>
        /// Gets a value indicating the detailed error message to display to the console when <see cref="IsValid(TValue)"/>, <see
        /// cref="Validate(TValue)"/>, or <see cref="Validate(TValue, string)"/> fails.
        /// </summary>
        string? DetailedMessage { get; }

        /// <summary>
        /// Gets or sets a value indicating the member name to validate.
        /// </summary>
        string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the error message to display to the console when <see cref="IsValid(TValue)"/>, <see
        /// cref="Validate(TValue)"/>, or <see cref="Validate(TValue, string)"/> fails.
        /// </summary>
        string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the error message resource name to use to lookup the <see
        /// cref="ErrorMessageResourceType"/> property value if validation fails.
        /// </summary>
        string? ErrorMessageResourceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the resource <see cref="Type"/> to use for error-message lookup if validation fails.
        /// </summary>
        Type? ErrorMessageResourceType { get; set; }

        /// <summary>
        /// Gets the localized validation error message.
        /// </summary>
        string ErrorMessageString { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a validation failure is still treated as success with a warning message to console.
        /// </summary>
        bool FailureAsWarning { get; set; }

        /// <summary>
        /// Gets a value indicating the <see cref="ISet{T}"/> of values that will definitely cause a validation failure.
        /// </summary>
        ISet<TValue>? Illegal { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="ISet{T}"/> of values that may or may not cause a validation failure.
        /// </summary>
        ISet<TValue>? Indeterminate { get; }

        /// <summary>
        /// Gets the <see cref="IDictionary{TKey, TValue}"/> of key/value pairs that is associated with this validator.
        /// </summary>
        IDictionary<object, object?> Items { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="ISet{T}"/> of values that always cause a validation success.
        /// </summary>
        ISet<TValue> Legal { get; }

        /// <summary>
        /// Gets or sets a value indicating the member name to validate.
        /// </summary>
        string? MemberName { get; set; }

        /// <summary>
        /// Gets a value indicating the object to validate.
        /// </summary>
        TValue ObjectInstance { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="Type"/> of the object to validate.
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="ValidationException"/> will also be thrown on validation failure.
        /// </summary>
        bool ThrowOnFailure { get; set; }

        /// <summary>
        /// Gets a value indicating this validator's fully qualified name.
        /// </summary>
        string? ValidatorFullName { get; }

        /// <summary>
        /// Gets a value indicating this validator's short name.
        /// </summary>
        string ValidatorName { get; }

        /// <summary>
        /// Gets a value indicating this validator's result for <see cref="ObjectInstance"/> of <see cref="Type"/><see cref="ObjectType"/>.
        /// </summary>
        bool ValidatorResult { get; }

        /// <summary>
        /// Applies formatting to an error message, based on the source of the validation failure.
        /// </summary>
        /// <param name="name">      Specifies the name to include in the formatted message.</param>
        /// <param name="path">      Specifies the absolute file path to the source file containing the validation failure.</param>
        /// <param name="member">    Specifies the member in <paramref name="path"/> that is the source of the validation failure.</param>
        /// <param name="lineNumber">
        /// Specifies the line number in <paramref name="path"/> that is the source of the validation failure.
        /// </param>
        /// <returns>An instance of the formatted message.</returns>
        string? FormatErrorMessage(string name, [CallerFilePath] string? path = null, [CallerMemberName] string? member = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Applies formatting to an error message, based on the source of the validation failure.
        /// </summary>
        /// <param name="name">      Specifies the name to include in the formatted message.</param>
        /// <param name="value">     
        /// Specifies the value of <see cref="Type"/><typeparamref name="TValue"/> causing validation failure.
        /// </param>
        /// <param name="path">      Specifies the absolute file path to the source file containing the validation failure.</param>
        /// <param name="member">    Specifies the member in <paramref name="path"/> that is the source of the validation failure.</param>
        /// <param name="lineNumber">
        /// Specifies the line number in <paramref name="path"/> that is the source of the validation failure.
        /// </param>
        /// <returns>An instance of the formatted message.</returns>
        string? FormatErrorMessage(string name, TValue value, [CallerFilePath] string? path = null, [CallerMemberName] string? member = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Checks whether the specified value <paramref name="value"/> of <typeparamref name="TValue"/> is valid with respect to
        /// this validator.
        /// </summary>
        /// <param name="value">Specifies the value of <typeparamref name="TValue"/> to validate.</param>
        /// <returns><see langref="true"/> if <paramref name="value"/> is valid; otherwise, <see langref="false"/>.</returns>
        bool GetValidationResult(TValue value);

        /// <summary>
        /// Determines whether the specified value <paramref name="value"/> of <typeparamref name="TValue"/> is valid.
        /// </summary>
        /// <param name="value">Specifies the value of <typeparamref name="TValue"/> to validate.</param>
        /// <returns><see langref="true"/> if <paramref name="value"/> is valid; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="ValidationException">
        /// Throws if <see cref="ThrowOnFailure"/> is <see langref="true"/> and validation fails.
        /// </exception>
        /// <remarks><see cref="ValidatorResult"/> is always updated with the return value of <see cref="IsValid(TValue)"/>.</remarks>
        bool IsValid(TValue value);

        /// <summary>
        /// Determines whether the specified value <paramref name="value"/> of <typeparamref name="TValue"/> is valid.
        /// </summary>
        /// <param name="value">Specifies the value of <typeparamref name="TValue"/> to validate.</param>
        /// <param name="name"> Specifies the name to include in the error message.</param>
        /// <returns><see langref="true"/> if <paramref name="value"/> is valid; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="ValidationException">
        /// Throws if <see cref="ThrowOnFailure"/> is <see langref="true"/> and validation fails.
        /// </exception>
        /// <remarks><see cref="ValidatorResult"/> is always updated with the return value of <see cref="IsValid(TValue)"/>.</remarks>
        bool IsValid(TValue value, string name);

        /// <summary>
        /// Validates the specified instance <paramref name="value"/> of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="value">Specifies the instance of <typeparamref name="TValue"/> to validate.</param>
        /// <exception cref="ValidationException">Thrown if validation fails.</exception>
        /// <remarks>Result of <see cref="IsValid(TValue)"/> is always stored in <see cref="ValidationResult"/>.</remarks>
        void Validate(TValue value);

        /// <summary>
        /// Validates the specified instance <paramref name="value"/> of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="value">Specifies the instance of <typeparamref name="TValue"/> to validate.</param>
        /// <param name="name"> Specifies the name to include in the error message.</param>
        /// <exception cref="ValidationException">Thrown if validation fails.</exception>
        /// <remarks>Result of <see cref="IsValid(TValue)"/> is always stored in <see cref="ValidationResult"/>.</remarks>
        void Validate(TValue value, string name);
    }
}
