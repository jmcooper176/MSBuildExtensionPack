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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Implements a Ada-like <see cref="int"/> sub-range object.
    /// </summary>
    /// <seealso cref="IComparable"/>
    /// <seealso cref="IComparable{T}"/>
    /// <seealso cref="IConvertible"/>
    /// <seealso cref="IEquatable{T}"/>
    /// <seealso cref="IEqualityComparer{T}"/>
    /// <seealso cref="IFormattable"/>
    /// <seealso cref="IParsable{T}"/>
    /// <seealso cref="ISpanFormattable"/>
    /// <seealso cref="ISpanParsable{T}"/>
    /// <seealso cref="IUtf8SpanFormattable"/>
    /// <seealso cref="IUtf8SpanParsable{T}"/>
    /// <seealso cref="IAdditionOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IAdditiveIdentity{TSelf, TResult}"/>
    /// <seealso cref="IBinaryInteger{T}"/>
    /// <seealso cref="IBinaryNumber{T}"/>
    /// <seealso cref="IBitwiseOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IComparisonOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IEqualityOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IDecrementOperators{T}"/>
    /// <seealso cref="IDivisionOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IIncrementOperators{T}"/>
    /// <seealso cref="IModulusOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IMultiplicativeIdentity{TSelf, TResult}"/>
    /// <seealso cref="IMultiplyOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="INumber{T}"/>
    /// <seealso cref="INumberBase{T}"/>
    /// <seealso cref="ISubtractionOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IUnaryNegationOperators{TSelf, TResult}"/>
    /// <seealso cref="IUnaryPlusOperators{TSelf, TResult}"/>
    /// <seealso cref="IShiftOperators{TSelf, TOther, TResult}"/>
    /// <seealso cref="IMinMaxValue{T}"/>
    /// <seealso cref="ISignedNumber{T}"/>
    public class SubRange :
        IComparable,
        IComparable<SubRange>,
        IConvertible,
        IEquatable<SubRange>,
        IEqualityComparer<SubRange>,
        IFormattable,
        IParsable<SubRange>,
        ISpanFormattable,
        ISpanParsable<SubRange>,
        IUtf8SpanFormattable,
        IUtf8SpanParsable<SubRange>,
        IAdditionOperators<SubRange, SubRange, SubRange>,
        IAdditiveIdentity<SubRange, SubRange>,
        IBinaryInteger<SubRange>,
        IBinaryNumber<SubRange>,
        IBitwiseOperators<SubRange, SubRange, SubRange>,
        IComparisonOperators<SubRange, SubRange, bool>,
        IEqualityOperators<SubRange, SubRange, bool>,
        IDecrementOperators<SubRange>,
        IDivisionOperators<SubRange, SubRange, SubRange>,
        IIncrementOperators<SubRange>,
        IModulusOperators<SubRange, SubRange, SubRange>,
        IMultiplicativeIdentity<SubRange, SubRange>,
        IMultiplyOperators<SubRange, SubRange, SubRange>,
        INumber<SubRange>,
        INumberBase<SubRange>,
        ISubtractionOperators<SubRange, SubRange, SubRange>,
        IUnaryNegationOperators<SubRange, SubRange>,
        IUnaryPlusOperators<SubRange, SubRange>,
        IShiftOperators<SubRange, SubRange, SubRange>,
        IMinMaxValue<SubRange>,
        ISignedNumber<SubRange>
    {
        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubRange"/> class.
        /// </summary>
        /// <param name="value">            Specifies the value to assign to <see cref="SubRange.Value"/>.</param>
        /// <param name="minValue">         
        /// Specifies the minimum value inclusive to assign to <see cref="SubRange.Range"/> and <see cref="SubRange.MinValue"/>.
        /// </param>
        /// <param name="maxExclusiveValue">
        /// Specifies the minimum value exclusive to assign to <see cref="SubRange.Range"/> and <see cref="SubRange.MaxValue"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws when:
        /// <list type="bullet">
        /// <item>
        /// <term>Negative</term>
        /// <description><paramref name="minValue"/> is negative.</description>
        /// </item>
        /// <item>
        /// <term>LessThanOrEqual</term>
        /// <description><paramref name="maxExclusiveValue"/> is less than or equal to <paramref name="minValue"/>.</description>
        /// </item>
        /// </list>
        /// </exception>
        protected SubRange(int value, int minValue, int maxExclusiveValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(minValue, nameof(minValue));
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusiveValue, minValue, nameof(maxExclusiveValue));

            Value = value;
            Range = new(minValue, maxExclusiveValue);
        }

        #endregion Protected Constructors

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubRange"/> class.
        /// </summary>
        /// <param name="value">Specifies the value to assign to <see cref="SubRange.Value"/>.</param>
        public SubRange(int value)
            : this(value, MinValue, MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubRange"/> class.
        /// </summary>
        /// <param name="value">        Specifies the value to assign to <see cref="SubRange.Value"/>.</param>
        /// <param name="halfInclusive">
        /// Specifies the legal values for <paramref name="value"/> over the <see cref="Range"/><paramref name="halfInclusive"/>.
        /// </param>
        public SubRange(int value, Range halfInclusive)
            : this(value, halfInclusive.Start.Value, halfInclusive.End.Value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubRange"/> class.
        /// </summary>
        /// <param name="value">            Specifies the value to assign to <see cref="SubRange.Value"/>.</param>
        /// <param name="minValue">         
        /// Specifies the minimum <see cref="Index"/> value inclusive to assign to <see cref="SubRange.Range"/> and <see cref="SubRange.MinValue"/>.
        /// </param>
        /// <param name="maxExclusiveValue">
        /// Specifies the maximum <see cref="Index"/> value exclusive to assign to <see cref="SubRange.Range"/> and <see cref="SubRange.MaxValue"/>.
        /// </param>
        public SubRange(int value, Index minValue, Index maxExclusiveValue)
            : this(value, minValue.Value, maxExclusiveValue.Value)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <inheritdoc/>
        public static SubRange AdditiveIdentity => Zero;

        /// <inheritdoc/>
        public static SubRange MaxValue => (SubRange)Range.End.Value;

        /// <inheritdoc/>
        public static SubRange MinValue => (SubRange)Range.Start.Value;

        /// <inheritdoc/>
        public static SubRange MultiplicativeIdentity => One;

        /// <inheritdoc/>
        public static SubRange NegativeOne => (SubRange)(-1);

        /// <inheritdoc/>
        public static SubRange One => (SubRange)1;

        /// <inheritdoc/>
        public static int Radix => 10;

        public static Range Range { get; private set; }

        /// <inheritdoc/>
        public static SubRange Zero => (SubRange)0;

        public int Value { get; }

        #endregion Public Properties

        #region Public Methods

        /// <inheritdoc/>
        public static SubRange Abs(SubRange value) => value == Zero ? Zero : value < Zero ? value * NegativeOne : value;

        /// <summary>
        /// Explicit conversion from an <see cref="int"/> to <see cref="SubRange"/>.
        /// </summary>
        /// <param name="i">Specifies the value to wrap in a new <see cref="SubRange"/>.</param>
        public static explicit operator SubRange(int i) => new(i);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="bool"/>.</param>
        public static implicit operator bool(SubRange s) => ToType<bool>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="byte"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="byte"/>.</param>
        public static implicit operator byte(SubRange s) => ToType<byte>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="char"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="char"/>.</param>
        public static implicit operator char(SubRange s) => ToType<char>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="int"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="int"/>.</param>
        public static implicit operator int(SubRange s) => ToType<int>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="long"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="long"/>.</param>
        public static implicit operator long(SubRange s) => ToType<long>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="sbyte"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="sbyte"/>.</param>
        public static implicit operator sbyte(SubRange s) => ToType<sbyte>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="short"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="short"/>.</param>
        public static implicit operator short(SubRange s) => ToType<short>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="s">
        /// Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="string"/> or <see cref="string.Empty"/> if
        /// <see cref="ToType{TConversionType}(SubRange, IFormatProvider?)"/> returns <see langref="null"/>.
        /// </param>
        public static implicit operator string(SubRange s) => ToType<string>(s, CultureInfo.CurrentCulture) ?? string.Empty;

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="uint"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="uint"/>.</param>
        public static implicit operator uint(SubRange s) => ToType<uint>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="ulong"/>.</param>
        public static implicit operator ulong(SubRange s) => ToType<ulong>(s, null);

        /// <summary>
        /// Implicit conversion from a <see cref="SubRange"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <param name="s">Specifies the <see cref="SubRange"/> to unwrap and convert to a <see cref="ushort"/>.</param>
        public static implicit operator ushort(SubRange s) => ToType<ushort>(s, null);

        /// <inheritdoc/>
        public static bool IsCanonical(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsComplexNumber(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsEvenInteger(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsFinite(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsImaginaryNumber(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsInfinity(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsInteger(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsNaN(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsNegative(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsNegativeInfinity(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsNormal(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsOddInteger(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsPositive(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsPositiveInfinity(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsPow2(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsRealNumber(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsSubnormal(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool IsZero(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange Log2(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange MaxMagnitude(SubRange x, SubRange y) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange MaxMagnitudeNumber(SubRange x, SubRange y) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange MinMagnitude(SubRange x, SubRange y) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange MinMagnitudeNumber(SubRange x, SubRange y) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator -(SubRange left, SubRange right) => (SubRange)unchecked((int)left - (int)right);

        /// <inheritdoc/>
        public static SubRange operator -(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator --(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool operator !=(SubRange? left, SubRange? right) => left?.Equals(right) != true;

        /// <inheritdoc/>
        public static SubRange operator %(SubRange left, SubRange right) => (SubRange)unchecked((int)left % (int)right);

        /// <inheritdoc/>
        public static SubRange operator &(SubRange left, SubRange right) => (SubRange)unchecked((int)left & (int)right);

        /// <inheritdoc/>
        public static SubRange operator *(SubRange left, SubRange right) => (SubRange)unchecked((int)left * (int)right);

        /// <inheritdoc/>
        public static SubRange operator /(SubRange left, SubRange right) => (SubRange)unchecked((int)left / (int)right);

        /// <inheritdoc/>
        public static SubRange operator ^(SubRange left, SubRange right) => (SubRange)unchecked((int)left ^ (int)right);

        /// <inheritdoc/>
        public static SubRange operator ~(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator +(SubRange left, SubRange right) => (SubRange)unchecked((int)left + (int)right);

        /// <inheritdoc/>
        public static SubRange operator +(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator ++(SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool operator <(SubRange left, SubRange right) => (int)left < (int)right;

        /// <inheritdoc/>
        public static SubRange operator <<(SubRange value, int shiftAmount) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator <<(SubRange value, SubRange shiftAmount) => (SubRange)unchecked((int)value << shiftAmount);

        /// <inheritdoc/>
        public static bool operator <=(SubRange left, SubRange right) => (left < right) || (left == right);

        /// <inheritdoc/>
        public static bool operator ==(SubRange? left, SubRange? right) => left?.Equals(right) == true;

        /// <inheritdoc/>
        public static bool operator >(SubRange left, SubRange right) => !((int)left <= (int)right);

        /// <inheritdoc/>
        public static bool operator >=(SubRange left, SubRange right) => !((int)left < (int)right);

        /// <inheritdoc/>
        public static SubRange operator >>(SubRange value, int shiftAmount) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator >>(SubRange value, SubRange shiftAmount) => (SubRange)unchecked((int)value >> shiftAmount);

        /// <inheritdoc/>
        public static SubRange operator >>>(SubRange value, int shiftAmount) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator >>>(SubRange value, SubRange shiftAmount) => (SubRange)unchecked((int)value >>> shiftAmount);

        /// <inheritdoc/>
        public static SubRange Parse(string s, IFormatProvider? provider) => (SubRange)int.Parse(s, provider);

        /// <inheritdoc/>
        public static SubRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange PopCount(SubRange value) => throw new NotImplementedException();

        /// <summary>
        /// Generic <see cref="SubRange.ToType(Type, IFormatProvider?)"/> method to convert <paramref name="value"/> to
        /// <typeparamref name="TConversionType"/>.
        /// </summary>
        /// <typeparam name="TConversionType">Specifies the conversion <see cref="Type"/> to be applied to <paramref name="value"/>.</typeparam>
        /// <param name="value">   Specifies the value to convert.</param>
        /// <param name="provider">
        /// Specifies the <see cref="IFormatProvider"/> for formatting. Currently, this value is only used for <see cref="string"/> conversions.
        /// </param>
        /// <returns>Returns an <see cref="object"/> cast to <typeparamref name="TConversionType"/>; otherwise, <see langref="null"/>.</returns>
        /// <exception cref="InvalidCastException">
        /// Throws if:
        /// <list type="bullet">
        /// <item>
        /// <term><see langref="null"/> and <see cref="ValueType"/></term>
        /// <description><paramref name="value"/> is <see langref="null"/> and a <see cref="ValueType"/>.</description>
        /// </item>
        /// <item>
        /// <term>Not <see cref="IConvertible"/></term>
        /// <description><paramref name="value"/> is not derived from <see cref="IConvertible"/>.</description>
        /// </item>
        /// <item>
        /// <term>NotSupported</term>
        /// <description><typeparamref name="TConversionType"/> conversion is not supported.</description>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref="FormatException"><paramref name="value"/> is not in a format recognized by <typeparamref name="TConversionType"/>.</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> represents a number that is out of the range of <typeparamref name="TConversionType"/>.
        /// </exception>
        public static TConversionType? ToType<TConversionType>(object? value, IFormatProvider? provider)
        {
            if (value is null && typeof(TConversionType).IsValueType)
            {
                throw new InvalidCastException();
            }
            else if (value is not IConvertible)
            {
                throw new InvalidCastException();
            }

            try
            {
                return (TConversionType?)Convert.ChangeType(value, typeof(TConversionType), provider);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Convenience generic <see cref="SubRange.ToType(Type, IFormatProvider?)"/> method to convert <paramref name="value"/> to
        /// <typeparamref name="TConversionType"/>.
        /// </summary>
        /// <typeparam name="TConversionType">Specifies the conversion <see cref="Type"/> to be applied to <paramref name="value"/>.</typeparam>
        /// <param name="value">   Specifies the <see cref="SubRange"/> instance to convert.</param>
        /// <param name="provider">
        /// Specifies the <see cref="IFormatProvider"/> for formatting. Currently, this value is only used for <see cref="string"/> conversions.
        /// </param>
        /// <returns>Returns an <see cref="object"/> cast to <typeparamref name="TConversionType"/>; otherwise, <see langref="null"/>.</returns>
        public static TConversionType? ToType<TConversionType>(SubRange value, IFormatProvider? provider)
        {
            return ToType<TConversionType>(value.Value, provider);
        }

        /// <inheritdoc/>
        public static SubRange TrailingZeroCount(SubRange value) => (SubRange)int.TrailingZeroCount(value);

        /// <inheritdoc/>
        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out SubRange result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out SubRange result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out SubRange result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryConvertToChecked<TOther>(SubRange value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryConvertToSaturating<TOther>(SubRange value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryConvertToTruncating<TOther>(SubRange value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SubRange result)
        {
            if (int.TryParse(s, provider, out int value))
            {
                result = new(value);
                return true;
            }
            else
            {
                result = Zero;
                return false;
            }
        }

        /// <inheritdoc/>
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SubRange result) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SubRange result) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SubRange result) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out SubRange value) => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            return obj is not null && Type.GetTypeCode(obj.GetType()) == GetTypeCode() ? CompareTo((SubRange?)obj) : -1;
        }

        /// <inheritdoc/>
        public int CompareTo(SubRange? other)
        {
            if (other is null)
            {
                return -1;
            }
            else if (this.Value > other.Value)
            {
                return 1;
            }
            else if (this.Value < other.Value)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <inheritdoc/>
        public bool Equals(SubRange? other)
        {
            return Equals(this, other);
        }

        /// <inheritdoc/>
        public bool Equals(SubRange? x, SubRange? y)
        {
            return ReferenceEquals(x, y) ? true : x is null ^ y is null ? false : x!.Value == y!.Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is not null && Type.GetTypeCode(obj.GetType()) == GetTypeCode() && Equals((SubRange)obj);

        /// <inheritdoc/>
        public int GetByteCount() => ((IBinaryInteger<int>)this.Value).GetByteCount();

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] SubRange obj) => HashCode.Combine(this.Value, MinValue, MaxValue);

        /// <inheritdoc/>
        public override int GetHashCode() => GetHashCode(this);

        /// <inheritdoc/>
        public int GetShortestBitLength() => ((IBinaryInteger<int>)this.Value).GetShortestBitLength();

        /// <inheritdoc/>
        public TypeCode GetTypeCode() => Type.GetTypeCode(typeof(SubRange));

        /// <inheritdoc/>
        public bool ToBoolean(IFormatProvider? provider) => Convert.ToBoolean(Value, provider);

        public byte ToByte(IFormatProvider? provider) => Convert.ToByte(Value, provider);

        /// <inheritdoc/>
        public char ToChar(IFormatProvider? provider) => Convert.ToChar(Value, provider);

        /// <inheritdoc/>
        public DateTime ToDateTime(IFormatProvider? provider) => Convert.ToDateTime(Value, provider);

        /// <inheritdoc/>
        public decimal ToDecimal(IFormatProvider? provider) => Convert.ToDecimal(Value, provider);

        /// <inheritdoc/>
        public double ToDouble(IFormatProvider? provider) => Convert.ToDouble(Value, provider);

        /// <inheritdoc/>
        public short ToInt16(IFormatProvider? provider) => Convert.ToInt16(Value, provider);

        /// <inheritdoc/>
        public int ToInt32(IFormatProvider? provider) => Convert.ToInt32(Value, provider);

        /// <inheritdoc/>
        public long ToInt64(IFormatProvider? provider) => Convert.ToInt64(Value, provider);

        /// <inheritdoc/>
        public sbyte ToSByte(IFormatProvider? provider) => Convert.ToSByte(Value, provider);

        /// <inheritdoc/>
        public float ToSingle(IFormatProvider? provider) => Convert.ToSingle(Value, provider);

        /// <inheritdoc/>
        public override string ToString() => Convert.ToString(Value, null);

        /// <inheritdoc/>
        public string ToString(IFormatProvider? provider) => Convert.ToString(Value, provider);

        /// <inheritdoc/>
        public string ToString([DisallowNull] string? format, IFormatProvider? formatProvider)
        {
            return string.Format(formatProvider, format, ToString(formatProvider) ?? string.Empty);
        }

        /// <summary>
        /// Generic <see cref="SubRange.ToType(Type, IFormatProvider?)"/> method to convert the current instance of <see
        /// cref="SubRange"/> to <typeparamref name="TConversionType"/>.
        /// </summary>
        /// <typeparam name="TConversionType">Specifies the conversion type for the current instance of <see cref="SubRange"/>.</typeparam>
        /// <param name="provider">
        /// Specifies the <see cref="IFormatProvider"/> for formatting the current instance. Currently, this value is only used for
        /// <see cref="string"/> conversions.
        /// </param>
        /// <returns>
        /// Returns an <see cref="object"/> containing the current instance of <see cref="SubRange.Value"/> to <typeparamref
        /// name="TConversionType"/>; otherwise, <see langref="null"/>.
        /// </returns>
        public TConversionType? ToType<TConversionType>(IFormatProvider? provider)
        {
            return (TConversionType?)ToType(Type.GetTypeCode(typeof(TConversionType)), provider);
        }

        /// <summary>
        /// <see cref="SubRange.ToType(Type, IFormatProvider?)"/> method to convert the current instance of <see cref="SubRange"/>
        /// to <paramref name="typeCode"/>.
        /// </summary>
        /// <param name="typeCode">Specifies the <see cref="TypeCode"/> to convert this <see cref="SubRange"/> instance with.</param>
        /// <param name="provider">
        /// Specifies the <see cref="IFormatProvider"/> for formatting the current instance. Currently, this value is only used for
        /// <see cref="string"/> conversions.
        /// </param>
        /// <returns>
        /// Returns an <see cref="object"/> containing the current instance of <see cref="SubRange.Value"/> to <paramref
        /// name="typeCode"/>; otherwise, <see langref="null"/>.
        /// </returns>
        /// <exception cref="InvalidCastException">Throws if <paramref name="typeCode"/> conversion is not supported.</exception>
        public object ToType(TypeCode typeCode, IFormatProvider? provider)
        {
            return typeCode switch
            {
                TypeCode.Boolean => this.ToBoolean(provider),
                TypeCode.Byte => this.ToByte(provider),
                TypeCode.Char => this.ToChar(provider),
                TypeCode.DateTime => this.ToDateTime(provider),
                TypeCode.Decimal => this.ToDecimal(provider),
                TypeCode.Double => this.ToDouble(provider),
                TypeCode.Int16 => this.ToInt16(provider),
                TypeCode.Int32 => this.ToInt32(provider),
                TypeCode.Int64 => this.ToInt64(provider),
                TypeCode.Object => Value.GetTypeCode().Equals(typeCode)
                                        ? (object)Value
                                        : throw new InvalidCastException($"Conversion to {typeCode} is not supported."),
                TypeCode.SByte => this.ToSByte(provider),
                TypeCode.Single => this.ToSingle(provider),
                TypeCode.String => this.ToString(provider),
                TypeCode.UInt16 => this.ToUInt16(provider),
                TypeCode.UInt32 => this.ToUInt32(provider),
                TypeCode.UInt64 => this.ToUInt64(provider),
                _ => throw new InvalidCastException($"Conversion to {typeCode} is not supported."),
            };
        }

        /// <inheritdoc/>
        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            return ToType(Type.GetTypeCode(conversionType), provider);
        }

        /// <inheritdoc/>
        public ushort ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(Value, provider);

        /// <inheritdoc/>
        public uint ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(Value, provider);

        /// <inheritdoc/>
        public ulong ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(Value, provider);

        /// <inheritdoc/>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) => throw new NotImplementedException();

        /// <inheritdoc/>
        public static SubRange operator |(SubRange left, SubRange right) => (SubRange)((int)left | (int)right);

        #endregion Public Methods
    }
}
