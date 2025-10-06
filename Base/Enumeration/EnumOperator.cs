namespace MSBuild.ExtensionPack.Base.Enumeration
{
    using System;
    using System.Globalization;
    using System.Numerics;

    public static class EnumOperator
    {
        #region Public Methods

        public static TScalar BitwiseAnd<TScalar>(this Enum left, TScalar right, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return ToScalar<TScalar>(left, provider) & right;
        }

        public static TScalar BitwiseOr<TScalar>(this Enum left, TScalar right, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return ToScalar<TScalar>(left, provider) | right;
        }

        public static TScalar BitwiseXor<TScalar>(this Enum left, TScalar right, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return ToScalar<TScalar>(left, provider) ^ right;
        }

        public static TEnum Decrement<TEnum>(this TEnum value, IFormatProvider? provider)
            where TEnum : struct, Enum
        {
            try
            {
                ulong ulongValue = ToScalar<ulong>(value, provider);
                return (TEnum)Enum.ToObject(typeof(TEnum), --ulongValue);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is InvalidOperationException)
            {
                throw new ArgumentException($"Could not perform 'Increment' operation on '{typeof(TEnum).Name}'.", ex);
            }
        }

        public static bool Equals<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return right is not null && left.Equals(right);
        }

        public static bool GreaterThan<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return right is not null && left.CompareTo(right) > 0;
        }

        public static bool GreaterThanOrEqualTo<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return right is not null && left.CompareTo(right) >= 0;
        }

        public static bool HasValue<TScalar>(this Enum parent, TScalar value, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return (ToScalar<TScalar>(parent, provider) & value) == value;
        }

        public static TEnum Increment<TEnum>(this TEnum value, IFormatProvider? provider)
            where TEnum : struct, Enum
        {
            try
            {
                ulong ulongValue = ToScalar<ulong>(value, provider);
                return (TEnum)Enum.ToObject(typeof(TEnum), ++ulongValue);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is InvalidOperationException)
            {
                throw new ArgumentException($"Could not perform 'Increment' operation on '{typeof(TEnum).Name}'.", ex);
            }
        }

        public static bool LessThan<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return !GreaterThanOrEqualTo(left, right);
        }

        public static bool LessThanOrEqualTo<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return !GreaterThan(left, right);
        }

        public static bool NotEquals<TEnum>(TEnum left, object? right) where TEnum : struct, Enum
        {
            return !Equals(left, right);
        }

        public static TScalar RemoveValue<TScalar>(this Enum parent, TScalar value, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return ToScalar<TScalar>(parent, provider) & ~value;
        }

        public static byte ToByte(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<byte>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static object Toggle<TScalar>(this Enum parent, TScalar value, IFormatProvider? provider)
            where TScalar : struct, IConvertible, INumber<TScalar>, IBitwiseOperators<TScalar, TScalar, TScalar>
        {
            return Enum.ToObject(parent.GetType(), ToScalar<TScalar>(parent, provider) ^ value);
        }

        public static short ToInt16(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<short>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static int ToInt32(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<int>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static long ToInt64(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<long>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static sbyte ToSByte(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<sbyte>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static object? ToScalar(this Enum value, IFormatProvider? provider)
        {
            return Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), provider ?? CultureInfo.CurrentCulture);
        }

        public static TUnderlyingType ToScalar<TUnderlyingType>(this Enum value, IFormatProvider? provider)
            where TUnderlyingType : struct, IConvertible
        {
            return Enum.GetUnderlyingType(value.GetType()) is not IConvertible
                ? throw new ArgumentException($"Type '{typeof(TUnderlyingType).Name}' is not a valid underlying type for an enumeration.", nameof(TUnderlyingType))
                : !Enum.GetUnderlyingType(value.GetType()).IsAssignableTo(typeof(TUnderlyingType))
                ? throw new ArgumentException($"Type '{typeof(TUnderlyingType).Name}' is not a assignable for an enumeration.", nameof(TUnderlyingType))
                : (TUnderlyingType)Convert.ChangeType(value, typeof(TUnderlyingType), provider ?? CultureInfo.CurrentCulture);
        }

        public static string ToString(this Enum value)
        {
            return Enum.GetName(value.GetType(), value) ?? string.Empty;
        }

        public static ushort ToUInt16(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<ushort>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static uint ToUInt32(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<uint>(value, provider ?? CultureInfo.CurrentCulture);
        }

        public static ulong ToUInt64(this Enum value, IFormatProvider? provider)
        {
            return ToScalar<ulong>(value, provider ?? CultureInfo.CurrentCulture);
        }

        #endregion Public Methods
    }
}
