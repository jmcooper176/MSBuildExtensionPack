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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

using MSBuild.ExtensionPack.Base.SystemAttribute;

namespace MSBuild.ExtensionPack.Base.Enumeration
{
    /// <summary>
    /// Implied ranges for <see cref="ArgumentOutOfRangeException"/> for single values.
    /// </summary>
    /// <remarks>FF_FF_FF_FF_FF_FF_FF_FF</remarks>
    [Flags]
    public enum ImpliedRange : ulong
    {
        /// <summary>
        /// Unknown <see cref="ImpliedRange"/> value.
        /// </summary>
        /// <remarks>An Error.</remarks>
        [Display(Name = "Unknown", ShortName = "Unknown")]
        [Description("Unknown Value")]
        Unknown = 0L,

        /// <summary>
        /// Any <see langref="null"/> value of a reference <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Equal to Null", ShortName = "IsNull")]
        [Description("Any null value of a reference type throws an 'ArgumentOutOfRangeException'")]
        IsNull = 0x00_00_00_00_00_00_00_01,

        /// <summary>
        /// Negates the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        [Display(Name = "Negates the Implied Range", ShortName = "IsNot")]
        [Description("Any implied range is negated.")]
        IsNot = 0x00_00_00_00_00_00_00_02,

        /// <summary>
        /// Any not <see langref="null"/> value of a reference <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Not Equal to Null", ShortName = "IsNotNull")]
        [Description("Any not null value of a reference type throws an 'ArgumentOutOfRangeException'")]
        IsNotNull = IsNot | IsNull,

        /// <summary>
        /// Any empty value of a containing <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Empty Container", ShortName = "IsEmpty")]
        [Description("Any empty containing type throws an 'ArgumentOutOfRangeException'")]
        IsEmpty = 0x00_00_00_00_00_00_00_04,

        /// <summary>
        /// Any empty or <see langref="null"/> containing <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Empty or Null Container", ShortName = "IsEmptyOrNull")]
        [Description("Any empty or null containing type throws an 'ArgumentOutOfRangeException'")]
        IsNullOrEmpty = IsNull | IsEmpty,

        /// <summary>
        /// Any non-empty containing <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Non-Empty Container", ShortName = "IsNotEmpty")]
        [Description("Any non-empty containing type throws an 'ArgumentOutOfRangeException'")]
        IsNotEmpty = IsNot | IsEmpty,

        /// <summary>
        /// Any non-empty and not <see langref="null"/> containing <see cref="Type"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Non-Empty and Not Null Container", ShortName = "IsNotEmptyAndNotNull")]
        [Description("Any non-empty value and not null containing type throws an 'ArgumentOutOfRangeException'")]
        IsNotEmptyAndNotNull = IsNot | IsEmpty | IsNull,

        /// <summary>
        /// Requires equality for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsEqual = 0x00_00_00_00_00_00_00_08,

        /// <summary>
        /// Requires inequality for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsNotEqual = IsNot | IsEqual,

        /// <summary>
        /// Requires strictly greater than for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsGreaterThan = 0x00_00_00_00_00_00_01_00,

        /// <summary>
        /// Requires greater than or equality for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsGreaterThanOrEqual = IsGreaterThan | IsEqual,

        /// <summary>
        /// Requires strictly less than for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsLessThan = 0x00_00_00_00_00_00_02_00,

        /// <summary>
        /// Requires less than or equality for the <see cref="ImpliedRange"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsLessThanOrEqual = IsLessThan | IsEqual,

        /// <summary>
        /// For <see cref="Nullable{T}"/><see langref="struct"/> types, requires that the <see cref="ImpliedRange"/> have no value.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        NoValue = 0x00_00_00_00_00_00_04_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="Array"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsArray = 0x00_00_00_00_00_00_08_00,

        /// <summary>
        /// Any <see cref="Array"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyArray = IsNull | IsEmpty | IsArray,

        /// <summary>
        /// Any <see cref="Array"/> that is not <see langref="null"/> and not empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyArray = IsNot | IsNull | IsEmpty | IsArray,

        /// <summary>
        /// The underlying interface for the <see cref="ImpliedRange"/> is <see cref="ICollection{T}"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsCollection = 0x00_00_00_00_00_00_10_00,

        /// <summary>
        /// Any <see cref="ICollection{T}"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyCollection = IsNull | IsEmpty | IsCollection,

        /// <summary>
        /// Any <see cref="ICollection{T}"/> that is not <see langref="null"/> and not empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyCollection = IsNot | IsNull | IsEmpty | IsCollection,

        /// <summary>
        /// The underlying interface for the <see cref="ImpliedRange"/> is <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsDictionary = 0x00_00_00_00_00_00_20_00,

        /// <summary>
        /// Any <see cref="IDictionary{TKey, TValue}"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyDictionary = IsNull | IsEmpty | IsDictionary,

        /// <summary>
        /// Any <see cref="IDictionary{TKey, TValue}"/> that is not <see langref="null"/> and not empty will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyDictionary = IsNot | IsNull | IsEmpty | IsDictionary,

        /// <summary>
        /// The underlying interface for the <see cref="ImpliedRange"/> is <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsEnumerable = 0x00_00_00_00_00_00_40_00,

        /// <summary>
        /// Any <see cref="IEnumerable{T}"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyEnumerable = IsNull | IsEmpty | IsEnumerable,

        /// <summary>
        /// Any <see cref="IEnumerable{T}"/> that is not <see langref="null"/> and not empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyEnumerable = IsNot | IsNull | IsEmpty | IsEnumerable,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="string"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsString = 0x00_00_00_00_00_00_80_00,

        /// <summary>
        /// Any <see cref="string"/> that is not <see langref="null"/> and empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEmptyString = IsEmpty | IsString,

        /// <summary>
        /// Any <see cref="string"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyString = IsNull | IsEmpty | IsString,

        /// <summary>
        /// Any <see cref="string"/> that is not <see langref="null"/> and not empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyString = IsNot | IsNull | IsEmpty | IsString,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsNullable = 0x00_00_00_00_00_01_00_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="char"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsChar = 0x00_00_00_00_00_02_00_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="decimal"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsDecimal = 0x00_00_00_00_00_04_00_00,

        /// <summary>
        /// Equality to any singleton <see cref="decimal"/> value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Equal to Singleton Decimal", ShortName = "SingletonDecimal")]
        [Description("Any value equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsEqualToDecimal = IsEqual | IsDecimal,

        /// <summary>
        /// Any <see cref="decimal"/> greater than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Singleton Decimal", ShortName = "SingletonGreaterThanDecimal")]
        [Description("Any value greater than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanDecimal = IsGreaterThan | IsDecimal,

        /// <summary>
        /// Any <see cref="decimal"/> greater than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Or Equal to Singleton Decimal", ShortName = "SingletonGreaterThanOrEqualDecimal")]
        [Description("Any value greater than or equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanOrEqualDecimal = IsGreaterThanOrEqual | IsDecimal,

        /// <summary>
        /// Any <see cref="decimal"/> less than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Less Than Singleton Decimal", ShortName = "SingletonLessThanDecimal")]
        [Description("Any value greater than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsLessThanDecimal = IsLessThan | IsDecimal,

        /// <summary>
        /// Any <see cref="decimal"/> less than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsLessThanOrEqualDecimal = IsLessThanOrEqual | IsDecimal,

        /// <summary>
        /// Any <see cref="Nullable{T}"/> where the underlying <see cref="Type"/> is a <see langref="struct"/> that has no value
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasNoValueNullableDecimal = NoValue | IsNullable | IsDecimal,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="double"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsDouble = 0x00_00_00_00_00_08_00_00,

        /// <summary>
        /// Any <see cref="Nullable{T}"/> where the underlying <see cref="Type"/> is a <see langref="struct"/> that has no value
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasNoValueNullableDouble = NoValue | IsNullable | IsDouble,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="float"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsFloat = 0x00_00_00_00_00_10_00_00,

        /// <summary>
        /// Any <see cref="Nullable{T}"/> where the underlying <see cref="Type"/> is a <see langref="struct"/> that has no value
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasNoValueNullableFloat = NoValue | IsNullable | IsFloat,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="int"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsInteger = 0x00_00_00_00_00_20_00_00,

        /// <summary>
        /// Equality to any single integer value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Equal to Singleton Integer", ShortName = "SingletonInteger")]
        [Description("Any value equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsEqualToInteger = IsEqual | IsInteger,

        /// <summary>
        /// Inequality to any single integer value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotEqualToInteger = IsNot | IsEqual | IsInteger,

        /// <summary>
        /// Any member of the set of integers greater than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Singleton Integer", ShortName = "SingletonGreaterThanInteger")]
        [Description("Any value greater than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanInteger = IsGreaterThan | IsInteger,

        /// <summary>
        /// Any member of the set of integers greater than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Or Equal to Singleton Integer", ShortName = "SingletonGreaterThanOrEqualInteger")]
        [Description("Any value greater than or equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanOrEqualInteger = IsGreaterThanOrEqual | IsInteger,

        /// <summary>
        /// Any member of the set of integers less than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Less Than Singleton Integer", ShortName = "SingletonLessThanInteger")]
        [Description("Any value less than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsLessThanInteger = IsLessThan | IsInteger,

        /// <summary>
        /// Any member of the set of integers less than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Less Than Singleton Integer", ShortName = "SingletonLessThanInteger")]
        [Description("Any value less than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsLessThanOrEqualInteger = IsLessThanOrEqual | IsInteger,

        /// <summary>
        /// Any <see cref="Nullable{T}"/> where the underlying <see cref="Type"/> is a <see langref="struct"/> that has no value
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasNoValueNullableInteger = NoValue | IsNullable | IsInteger,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="long"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsLongInteger = 0x00_00_00_00_00_40_00_00,

        /// <summary>
        /// Equality to any singleton <see cref="long"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Equal to Singleton Long Integer", ShortName = "SingletonLongInteger")]
        [Description("Any value equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsEqualToLongInteger = IsEqual | IsLongInteger,

        /// <summary>
        /// Any member of the set <see cref="long"/> integers not equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotEqualToLongInteger = IsNot | IsEqual | IsLongInteger,

        /// <summary>
        /// Any member of the set of <see cref="long"/> integers greater than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Singleton Long Integer", ShortName = "SingletonGreaterThanLongInteger")]
        [Description("Any value greater than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanLongInteger = IsGreaterThan | IsLongInteger,

        /// <summary>
        /// Any member of the set of <see cref="long"/> integers greater than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Greater Than Or Equal to Singleton Long Integer", ShortName = "SingletonGreaterThanOrEqualLongInteger")]
        [Description("Any value greater than or equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsGreaterThanOrEqualLongInteger = IsGreaterThanOrEqual | IsLongInteger,

        /// <summary>
        /// Any member of the set of <see cref="long"/> integers less than value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Less Than Singleton Long Integer", ShortName = "SingletonLessThanLongInteger")]
        [Description("Any value greater than the singleton throws an 'ArgumentOutOfRangeException'")]
        IsLessThanLongInteger = IsLessThan | IsLongInteger,

        /// <summary>
        /// Any member of the set of <see cref="long"/> integers less than or equal to value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Display(Name = "Less Than Or Equal to Singleton Long Integer", ShortName = "SingletonLessThanOrEqualLongInteger")]
        [Description("Any value greater than or equal to the singleton throws an 'ArgumentOutOfRangeException'")]
        IsLessThanOrEqualLongInteger = IsLessThanOrEqual | IsLongInteger,

        /// <summary>
        /// Any <see cref="Nullable{T}"/> where the underlying <see cref="Type"/> is a <see langref="struct"/> that has no value
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasNoValueNullableLongInteger = NoValue | IsNullable | IsLongInteger,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="TimeSpan"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsTimeSpan = 0x00_00_00_00_00_80_00_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="StringBuilder"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsStringBuilder = 0x00_00_00_00_01_00_00_00,

        /// <summary>
        /// Any <see cref="StringBuilder"/> that is not <see langref="null"/> and empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEmptyStringBuilder = IsEmpty | IsStringBuilder,

        /// <summary>
        /// Any <see cref="StringBuilder"/> that is <see langref="null"/> or empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrEmptyStringBuilder = IsNull | IsEmpty | IsStringBuilder,

        /// <summary>
        /// Any <see cref="StringBuilder"/> that is not <see langref="null"/> and not empty throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNullAndNotEmptyStringBuilder = IsNot | IsNull | IsEmpty | IsStringBuilder,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is derived from <see cref="Enum"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsEnum = 0x00_00_00_00_02_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> that is equal to name or value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToEnum = IsEnum | IsEqual,

        /// <summary>
        /// Any <see cref="Enum"/> that is not equal to name or value throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotEqualToEnum = IsNot | IsEnum | IsEqual,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="char"/> restricted to <see cref="ASCIIEncoding"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsAscii = 0x00_00_00_00_04_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is <see cref="char.IsAscii(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiChar = IsAscii | IsChar,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="char"/> restricted to <see cref="UTF8Encoding"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsUtf8 = 0x00_00_00_00_08_00_00_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="char"/> restricted to
        /// <c>16-bit</c><see cref="UnicodeEncoding"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsUnicode = 0x00_00_00_00_10_00_00_00,

        /// <summary>
        /// The underlying <see cref="Type"/> for the <see cref="ImpliedRange"/> is <see cref="char"/> restricted to <see cref="UTF32Encoding"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsUtf32 = 0x00_00_00_00_20_00_00_00,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is lower case will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsLower = 0x00_00_00_00_40_00_00_00,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is upper case will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsUpper = 0x00_00_00_00_80_00_00_00,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a decimal digit will throw an
        /// <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsDigit = 0x00_00_00_01_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is <see cref="char.IsAsciiDigit(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiDigitChar = IsAsciiChar | IsDigit,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsDigit(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsDigitChar = IsUnicode | IsChar | IsDigit,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a hexadecimal digit will
        /// throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsHexDigit = 0x00_00_00_02_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiHexDigit(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiHexDigitChar = IsAsciiChar | IsHexDigit,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a letter will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsLetter = 0x00_00_00_04_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiLetter(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiLetterChar = IsAsciiChar | IsLetter,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsLetter(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsLetterChar = IsUnicode | IsChar | IsLetter,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiLetterLower(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiLetterLowerChar = IsAsciiLetterChar | IsLower,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsLower(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsLowerChar = IsLetterChar | IsLower,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiLetterUpper(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiLetterUpperChar = IsAsciiLetterChar | IsUpper,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsUpper(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsUpperChar = IsLetterChar | IsUpper,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiLetterOrDigit(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiLetterOrDigitChar = IsAsciiLetterChar | IsAsciiDigitChar,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsLetterOrDigit(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsLetterOrDigitChar = IsUnicode | IsChar | IsLetter | IsDigit,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiHexDigitLower(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiHexDigitLowerChar = IsAsciiLetterLowerChar | IsHexDigit,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsAsciiHexDigitUpper(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAsciiHexDigitUpperChar = IsAsciiLetterUpperChar | IsHexDigit,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is any sort of number will throw
        /// an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsNumber = 0x00_00_00_08_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsNumber(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNumberChar = IsUnicode | IsChar | IsNumber,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a control character will
        /// throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsControl = 0x00_00_00_10_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsControl(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsControlChar = IsUnicode | IsChar | IsControl,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a punctuation character will
        /// throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsPunctuation = 0x00_00_00_20_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsPunctuation(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPunctuationChar = IsUnicode | IsChar | IsPunctuation,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a separator character will
        /// throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsSeparator = 0x00_00_00_40_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsSeparator(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsSeparatorChar = IsUnicode | IsChar | IsSeparator,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a symbol character will throw
        /// an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>It is an error to pass this <see cref="ImpliedRange"/> value by itself.</remarks>
        IsSymbol = 0x00_00_00_80_00_00_00_00,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsSymbol(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsSymbolChar = IsUnicode | IsChar | IsSymbol,

        /// <summary>
        /// Any <see cref="char"/> that can be displayed on the console throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsGraphicalChar = IsUnicode | IsChar | IsDigit | IsHexDigit | IsLetter | IsNumber | IsPunctuation | IsSymbol,

        /// <summary>
        /// Any <see cref="char"/> this is a <see cref="IsGraphicalChar"/> or <see cref="IsControlChar"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPrintableChar = IsGraphicalChar | IsControlChar,

        /// <summary>
        /// Any <see cref="UnicodeEncoding"/> or <see cref="ASCIIEncoding"/><see cref="char"/> that is a white space character will
        /// throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsWhiteSpace = 0x00_00_01_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="string"/> that is <see langref="null"/>, empty, or all whitespace throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNullOrWhiteSpaceString = IsNull | IsEmpty | IsWhiteSpace | IsString,

        /// <summary>
        /// Any <see cref="char"/> that is not <see cref="char.IsWhiteSpace(char)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsWhiteSpaceChar = IsUnicode | IsChar | IsWhiteSpace,

        /// <summary>
        /// Any <see cref="float"/>, <see cref="decimal"/>, or <see cref="double"/> that is above the <see
        /// cref="Math.Ceiling(double)"/> will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAboveCeiling = 0x00_00_02_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.Ceiling(double)"/> of a value is greater than throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsAboveCeilingDouble = IsAboveCeiling | IsDouble,

        /// <summary>
        /// Any <see cref="float"/>, <see cref="decimal"/>, or <see cref="double"/> that is below the <see
        /// cref="Math.Floor(double)"/> will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsBelowFloor = 0x00_00_04_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.Floor(double)"/> of a value is less than throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsBelowFloorDouble = IsBelowFloor | IsDouble,

        /// <summary>
        /// Any <see cref="float"/>, <see cref="decimal"/>, or <see cref="double"/> that is an odd integer value will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsOddInteger = 0x00_00_08_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="decimal"/> for which <see cref="decimal.IsOddInteger(decimal)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsOddIntegerDecimal = IsOddInteger | IsDecimal,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsOddInteger(double)"/> true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsOddIntegerDouble = IsOddInteger | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsOddInteger(float)"/> true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsOddIntegerFloat = IsOddInteger | IsFloat,

        /// <summary>
        /// Any <see cref="float"/>, <see cref="decimal"/>, or <see cref="double"/> that is an even integer value will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEvenInteger = 0x00_00_10_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="decimal"/> for which <see cref="decimal.IsEvenInteger(decimal)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEvenIntegerDecimal = IsEvenInteger | IsDecimal,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsEvenInteger(double)"/> true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEvenIntegerDouble = IsEvenInteger | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsEvenInteger(float)"/> true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEvenIntegerFloat = IsEvenInteger | IsFloat,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsInteger(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>Implies <see cref="IsOddIntegerDouble"/> xor <see cref="IsEvenIntegerDouble"/>.</remarks>
        IsIntegerDouble = IsOddInteger | IsEvenInteger | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsInteger(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>Implies <see cref="IsOddIntegerFloat"/> xor <see cref="IsEvenIntegerFloat"/>.</remarks>
        IsIntegerFloat = IsOddInteger | IsEvenInteger | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is Not-a-Number will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNan = 0x00_00_20_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsNaN(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNanDouble = IsEqual | IsNan | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsNaN(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNanFloat = IsEqual | IsNan | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is equal to an <c>IEEE</c> positive infinity will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPositiveInfinity = 0x00_00_40_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsPositiveInfinity(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToPositiveInfinityDouble = IsEqual | IsPositiveInfinity | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsPositiveInfinity(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToPositiveInfinityFloat = IsEqual | IsPositiveInfinity | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is equal to an <c>IEEE</c> negative infinity will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeInfinity = 0x00_00_80_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsNegativeInfinity(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNegativeInfinityDouble = IsEqual | IsNegativeInfinity | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsNegativeInfinity(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNegativeInfinityFloat = IsEqual | IsNegativeInfinity | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is equal to an <c>IEEE</c> negative or positive infinity will throw
        /// an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsInfinity = 0x00_01_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsInfinity(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>Implies <see cref="IsEqualToNegativeInfinityDouble"/> xor <see cref="IsEqualToPositiveInfinityDouble"/>.</remarks>
        IsEqualToInfinityDouble = IsEqual | IsInfinity | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsInfinity(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <remarks>Implies <see cref="IsEqualToNegativeInfinityFloat"/> xor <see cref="IsEqualToPositiveInfinityFloat"/>.</remarks>
        IsEqualToInfinityFloat = IsEqual | IsInfinity | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is negative will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegative = 0x00_02_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="decimal"/> for which <see cref="decimal.IsNegative(decimal)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeIntegerDecimal = IsNegative | IsDecimal,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsNegative(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeDouble = IsNegative | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsNegative(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeFloat = IsNegative | IsFloat,

        /// <summary>
        /// Any member of the set of negative <see cref="long"/> integers throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeLongInteger = IsNegative | IsLongInteger,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is positive will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPositive = 0x00_04_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="decimal"/> for which <see cref="decimal.IsPositive(decimal)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPositiveIntegerDecimal = IsPositive | IsDecimal,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsPositive(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPositiveDouble = IsPositive | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsPositive(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPositiveFloat = IsPositive | IsFloat,

        /// <summary>
        /// Any positive <see cref="long"/> integer throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNaturalLongInteger = IsPositive | IsLongInteger,

        /// <summary>
        /// Any positive <see cref="long"/> integer throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNaturalLongInteger = IsNot | IsPositive | IsLongInteger,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is normalized will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNormal = 0x00_08_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsNormal(double)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNormalDouble = IsNot | IsNormal | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsNormal(float)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNormalFloat = IsNot | IsNormal | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is sub-normal will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsSubNormal = 0x00_10_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsSubnormal(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsSubNormalDouble = IsSubNormal | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsSubnormal(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsSubNormalFloat = IsSubNormal | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is a binary bit (a power of 2) will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPowerOfTwo = 0x00_20_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsPow2(double)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPowerOfTwoDouble = IsPowerOfTwo | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsPow2(float)"/> is true throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsPowerOfTwoFloat = IsPowerOfTwo | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is a real number will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsRealNumber = 0x00_40_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="double"/> for which <see cref="double.IsRealNumber(double)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotRealNumberDouble = IsNot | IsRealNumber | IsDouble,

        /// <summary>
        /// Any <see cref="float"/> for which <see cref="float.IsRealNumber(float)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotRealNumberFloat = IsNot | IsRealNumber | IsFloat,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is equal to negative zero will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeZero = 0x00_80_00_00_00_00_00_00,

        /// <summary>
        /// Any singleton <see cref="double"/> equal to <see cref="double.NegativeZero"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNegativeZeroDouble = IsEqual | IsNegativeZero | IsDouble,

        /// <summary>
        /// Any singleton <see cref="float"/> equal to <see cref="float.NegativeZero"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsEqualToNegativeZeroFloat = IsEqual | IsNegativeZero | IsFloat,

        /// <summary>
        /// Any <see cref="Type"/> not a <see cref="float"/> or <see cref="double"/> that is equal to zero will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsZero = 0x01_00_00_00_00_00_00_00,

        /// <summary>
        /// Any member of the set of negative <see cref="long"/> integers throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNegativeLongIntegerOrZero = IsZero | IsNegative | IsLongInteger,

        /// <summary>
        /// Zero or any <see cref="long"/> natural number throws.
        /// </summary>
        IsNotWholeLongInteger = IsNot | IsZero | IsPositive | IsLongInteger,

        /// <summary>
        /// Any <see cref="float"/> or <see cref="double"/> that is within +/- <see cref="double.Epsilon"/> will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNearZero = 0x02_00_00_00_00_00_00_00,

        /// <summary>
        /// Any singleton <see cref="double"/> within <see cref="double.Epsilon"/> of zero throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNearZeroDouble = IsNearZero | IsDouble,

        /// <summary>
        /// Any singleton <see cref="float"/> within <see cref="float.Epsilon"/> of zero throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNearZeroFloat = IsNearZero | IsFloat,

        /// <summary>
        /// Any <see cref="decimal"/> that is in canonical form will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsCanonical = 0x04_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="decimal"/> for which <see cref="decimal.IsCanonical(decimal)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotCanonicalDecimal = IsNot | IsCanonical | IsDecimal,

        /// <summary>
        /// Any <see cref="Enum"/> that is contained in <see cref="Enum.GetNames{TEnum}()"/> throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsName = 0x08_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> where <see cref="string"/> value is not contained in <see cref="Enum.GetNames(Type)"/> throws an
        /// <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotNameOfEnum = IsNot | IsEnum | IsName,

        /// <summary>
        /// Any <see cref="Enum"/> that is contained in <see cref="Enum.GetValues{TEnum}()"/> throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsValue = 0x10_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> where the <see cref="object"/> value is not contained in <see cref="Enum.GetValues(Type)"/>
        /// throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotValueOfEnum = IsNot | IsEnum | IsValue,

        /// <summary>
        /// Any <see cref="Enum"/> that is contained in <see cref="Enum.GetValuesAsUnderlyingType{TEnum}()"/> throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsValueOfUnderlyingType = 0x20_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> where the value of <see cref="Enum.GetUnderlyingType(Type)"/> is not contained in <see
        /// cref="Enum.GetValuesAsUnderlyingType(Type)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotValueOfUnderlyingTypeOfEnum = IsNot | IsEnum | IsValueOfUnderlyingType,

        /// <summary>
        /// Any <see cref="FlagsAttribute"/><see cref="Enum"/> that is contains a <see cref="Enum"/> throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        HasFlag = 0x40_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> marked as <see cref="FlagsAttribute"/> does not have one or more bit fields set because <see
        /// cref="Enum.HasFlag(Enum)"/> is false throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        FlagNotFoundEnum = IsNot | IsEnum | HasFlag,

        /// <summary>
        /// Any <see cref="Enum"/> name or value this is <see cref="Enum.IsDefined{TEnum}(TEnum)"/> throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsDefined = 0x80_00_00_00_00_00_00_00,

        /// <summary>
        /// Any <see cref="Enum"/> where the value of any integral <see cref="Type"/> or <see cref="string"/> is not <see
        /// cref="Enum.IsDefined(Type, object)"/> throws <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        IsNotDefinedEnum = IsNot | IsEnum | IsDefined,
    }

    /// <summary>
    /// Extension methods to recover custom <see cref="Attribute"/> s using <see cref="CustomAttribute"/> from <see
    /// cref="ImpliedRange"/> fields.
    /// </summary>
    public static class ImpliedRangeExtension
    {
        #region Public Methods

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateField"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateField"/>.</returns>
        public static bool? GetAutoGenerateField(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateField;
        }

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateFilter"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateFilter"/>.</returns>
        public static bool? GetAutoGenerateFilter(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateFilter;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string? GetDescription(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDescriptionAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetDescription2(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DescriptionAttribute"/> or <see langref="null"/> if no <see cref="DescriptionAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DescriptionAttribute? GetDescriptionAttribute(this ImpliedRange value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DescriptionAttribute, ImpliedRange>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the <see cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DisplayAttribute"/> or <see langref="null"/> if no <see cref="DisplayAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DisplayAttribute? GetDisplayAttribute(this ImpliedRange value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DisplayAttribute, ImpliedRange>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the group name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the group name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetGroupName(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.GroupName;
        }

        /// <summary>
        /// Extension method to recover the name string from the <see cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetName(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>An <see cref="int"/> or <see langref="null"/> representing the order property of the <see cref="DisplayAttribute"/>.</returns>
        public static int? GetOrder(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Order;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the prompt for the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetPrompt(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Prompt;
        }

        /// <summary>
        /// Extension method to recover the resource <see cref="Type"/> from the <see cref="DisplayAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="Type"/> or <see langref="null"/> representing the resource <see cref="Type"/> of the <see cref="DisplayAttribute"/>.</returns>
        public static Type? GetResourceType(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ResourceType;
        }

        /// <summary>
        /// Extension method to recover the short name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="ImpliedRange"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="ImpliedRange"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the short name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? ShortName(this ImpliedRange value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ShortName;
        }

        #endregion Public Methods
    }
}
