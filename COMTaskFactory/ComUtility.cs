namespace MSBuild.ExtensionPack.COMTaskFactory
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    public static class ComUtility
    {
        /// <summary>
        /// Creates an array instance of element <paramref name="theType"/> with a zero lower bound.
        /// </summary>
        /// <param name="theType">Specifies the element <see cref="Type"/>.</param>
        /// <returns>An <see cref="Array"/> of elements of <paramref name="theType"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// Throws when the code for an <see cref="Array"/> of the specified type <paramref name="theType"/> is not available.
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// Throws because <paramref name="theType"/> is either a <see cref="TypedReference"/><see cref="struct"/> or <see
        /// cref="Type.IsByRef"/> is <see langref="true"/>.
        /// </exception>
        public static Type CreateArray([DisallowNull] this Type theType)
        {
            if (theType.IsByRef)
            {
                throw new TypeLoadException($"Cannot create an array of type '{theType.FullName}'.");
            }

            try
            {
                return theType.MakeArrayType();
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating array type from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates an array instance of element <paramref name="theType"/> with a zero lower bound and a rank of 1 or greater.
        /// </summary>
        /// <param name="theType">Specifies the element <see cref="Type"/>.</param>
        /// <param name="rank">   Specifies the rank of the array to create. Must be greater than zero.</param>
        /// <returns>An <see cref="Array"/> of elements of <paramref name="theType"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="rank"/> is less than or equal to zero.</exception>
        /// <exception cref="NotSupportedException">
        /// Throws when the code for an <see cref="Array"/> of the specified type <paramref name="theType"/> is not available.
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// Throws because <paramref name="theType"/> is either a <see cref="TypedReference"/><see cref="struct"/> or <see
        /// cref="Type.IsByRef"/> is <see langref="true"/>.
        /// </exception>
        public static Type CreateArray([DisallowNull] this Type theType, int rank)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rank, nameof(rank));

            try
            {
                return theType.MakeArrayType(rank);
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating array type from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates the COM instance from the <c>COM</c> type derived from <paramref name="progId"/>.
        /// </summary>
        /// <param name="progId">Specifies the <c>COM</c> type <c>ProgId</c>.</param>
        /// <returns>
        /// An <see cref="object"/> representing the new <c>COM</c> instance with <paramref name="progId"/>; otherwise, <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when the <c>COM</c> type derived from <paramref name="progId"/> is <see langref="null"/>; OR when <paramref
        /// name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throws when the derived <c>COM</c> type is not a runtime type; or the derived <c>COM</c> type is an open generic type.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Throws when the derived <c>COM</c> type is a <see cref="TypeBuilder"/>; creation of a <see cref="TypedReference"/>, <see
        /// cref="ArgIterator"/>, <see cref="Void"/>; OR <see cref="RuntimeArgumentHandle"/> type or an <see cref="Array"/> of such
        /// a <see cref="Type"/>.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// Throws when the constructor of the derived <c>COM</c> type throws an exception.
        /// </exception>
        /// <exception cref="MethodAccessException">
        /// Throws when the caller does not have permission to call the constructor of the derived <c>COM</c> type.
        /// </exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws when there is a failure to acquire the <c>COM</c> type from <paramref name="progId"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws when the derived <c>COM</c> type does not have a public default constructor.
        /// </exception>
        /// <exception cref="COMException">Throws when the <c>COM</c> type is not registered or cannot be activated.</exception>
        /// <exception cref="TypeLoadException">Throws when the derived <c>COM</c> type is not a valid <c>COM</c> type.</exception>
        [SupportedOSPlatform("windows")]
        public static object? CreateComInstance(string progId)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(progId, nameof(progId));

            try
            {
                var comType = GetTypeFromProgId(progId);

                return OperatingSystem.IsWindows() && comType is not null ? comType.CreateComInstance() : throw new ArgumentException($"COM class with ProgID '{progId}' not found.");
            }
            catch (Exception ex) when (
                ex is ArgumentNullException
                || ex is ArgumentException
                || ex is NotSupportedException
                || ex is TargetInvocationException
                || ex is MethodAccessException
                || ex is MemberAccessException
                || ex is InvalidComObjectException
                || ex is MissingMethodException
                || ex is COMException
                || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating COM instance from ProgID '{progId}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates the COM instance from <paramref name="comType"/>; optionally with <paramref name="arguments"/> to the constructor.
        /// </summary>
        /// <param name="comType">  Specifies the <c>COM</c> to activate.</param>
        /// <param name="arguments">
        /// An <see cref="Array"/> of zero or more arguments that match in number, order, and type the parameters of the constructor
        /// to invoke.
        /// </param>
        /// <returns>
        /// An <see cref="object"/> representing the new <c>COM</c> instance with <paramref name="progId"/>; otherwise, <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when the <c>COM</c> type derived from <paramref name="progId"/> is <see langref="null"/>; OR when <paramref
        /// name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throws when the derived <c>COM</c> type is not a runtime type; or the derived <c>COM</c> type is an open generic type.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Throws when the derived <c>COM</c> type is a <see cref="TypeBuilder"/>; creation of a <see cref="TypedReference"/>, <see
        /// cref="ArgIterator"/>, <see cref="Void"/>; OR <see cref="RuntimeArgumentHandle"/> type or an <see cref="Array"/> of such
        /// a <see cref="Type"/>.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// Throws when the constructor of the derived <c>COM</c> type throws an exception.
        /// </exception>
        /// <exception cref="MethodAccessException">
        /// Throws when the caller does not have permission to call the constructor of the derived <c>COM</c> type.
        /// </exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws when the <paramref name="comType"/> was not obtained through <see cref="Type.GetTypeFromCLSID(Guid)"/> or <see cref="Type.GetTypeFromProgID(string)"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws when the derived <c>COM</c> type does not have a public default constructor.
        /// </exception>
        /// <exception cref="COMException">Throws when the <c>COM</c> type is not registered or cannot be activated.</exception>
        /// <exception cref="TypeLoadException">Throws when the derived <c>COM</c> type is not a valid <c>COM</c> type.</exception>
        [SupportedOSPlatform("windows")]
        public static object? CreateComInstance([DisallowNull] this Type comType, params object?[]? arguments)
        {
            if (!comType.IsCOMObject)
            {
                throw new ArgumentException($"The type '{comType.FullName}' is not a COM type.", nameof(comType));
            }

            try
            {
                return Activator.CreateInstance(comType, arguments);
            }
            catch (Exception ex) when (
                ex is ArgumentNullException
                || ex is NotSupportedException
                || ex is TargetInvocationException
                || ex is MethodAccessException
                || ex is MemberAccessException
                || ex is InvalidComObjectException
                || ex is MissingMethodException
                || ex is COMException
                || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating COM instance from Type '{comType.FullName}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates the COM instance for <typeparamref name="TCom"/> from the default constructor.
        /// </summary>
        /// <typeparam name="TCom">The type of the COM.</typeparam>
        /// <returns>An <typeparamref name="TCom"/> representing the new <c>COM</c> instance.</returns>
        /// <exception cref="ArgumentException">The type '{typeof(TCom).FullName}' is not a COM type.</exception>
        /// <exception cref="MissingMethodException">
        /// Throws when the derived <c>COM</c> type does not have a public default constructor.
        /// </exception>
        public static TCom CreateComInstance<TCom>() where TCom : class
        {
            if (!typeof(TCom).IsCOMObject)
            {
                throw new ArgumentException($"The type '{typeof(TCom).FullName}' is not a COM type.", nameof(TCom));
            }

            try
            {
                return Activator.CreateInstance<TCom>();
            }
            catch (MissingMethodException ex)
            {
                Console.Error.WriteLine($"Error creating COM instance of type '{typeof(TCom).FullName}': {ex.Message}");
                throw;
            }
        }

        public static object? CreateComInstance(Guid clsId)
        {
            try
            {
                var comType = GetTypeFromClsId(clsId);

                return comType is null ? throw new ArgumentException($"COM class with ClsID '{clsId}' not found.") : CreateComInstance(comType);
            }
            catch (Exception ex) when (
                ex is ArgumentNullException
                || ex is ArgumentException
                || ex is NotSupportedException
                || ex is TargetInvocationException
                || ex is MethodAccessException
                || ex is MemberAccessException
                || ex is InvalidComObjectException
                || ex is MissingMethodException
                || ex is COMException
                || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating COM instance from ClsID '{clsId}': {ex.Message}");
                throw;
            }
        }

        public static Type CreateGeneric([DisallowNull] this Type theType, params Type[] typeArguments)
        {
            if (!theType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"The type '{theType.FullName}' is not a generic type definition.", nameof(theType));
            }

            try
            {
                return theType.MakeGenericType(typeArguments);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException || ex is ArgumentException || ex is NotSupportedException)
            {
                Console.Error.WriteLine($"Error creating generic type from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        public static Type CreatePointer([DisallowNull] this Type theType)
        {
            try
            {
                return theType.MakePointerType();
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating pointer type from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        public static Type CreateReference([DisallowNull] this Type theType)
        {
            try
            {
                return theType.MakeByRefType();
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is TypeLoadException)
            {
                Console.Error.WriteLine($"Error creating reference type from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        public static Type[] FindTypeInterfaces([DisallowNull] this Type theType, TypeFilter filter, object? filterCriteria)
        {
            try
            {
                return theType!.FindInterfaces(filter, filterCriteria);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is TargetInvocationException)
            {
                Console.Error.WriteLine($"Error finding interfaces on type '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        public static Type[] FindTypeInterfaces<TCriteria>([DisallowNull] this Type theType, Func<Type, TCriteria?, bool> filterPredicate, TCriteria? filterCriteria)
        {
            return FindTypeInterfaces(
                theType: theType,
                filter: (t, c) => filterPredicate.Invoke(t, (TCriteria?)c),
                filterCriteria: filterCriteria);
        }

        public static MemberInfo[] FindTypeMembers([DisallowNull] this Type theType, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter? filter, object? filterCriteria)
        {
            try
            {
                return theType is not null ? theType.FindMembers(memberType, bindingAttr, filter, filterCriteria) : Array.Empty<MemberInfo>();
            }
            catch (ArgumentNullException ex)
            {
                Console.Error.WriteLine($"Error finding members on type '{theType?.FullName}': {ex.Message}");
                throw;
            }
        }

        public static MemberInfo[] FindTypeMembers<TCriteria>([DisallowNull] this Type theType, MemberTypes memberType, BindingFlags bindingAttr, Func<MemberInfo, TCriteria?, bool> filterPredicate, TCriteria? filterCriteria)
        {
            return theType is not null ? FindTypeMembers(
                theType: theType,
                memberType: memberType,
                bindingAttr: bindingAttr,
                filter: (m, c) => filterPredicate.Invoke(m, (TCriteria?)c),
                filterCriteria: filterCriteria) : Array.Empty<MemberInfo>();
        }

        public static int GetTypeArrayRank([DisallowNull] this Type theType)
        {
            try
            {
                return theType?.IsArray == true ? theType.GetArrayRank() : 0;
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentException)
            {
                Console.Error.WriteLine($"Error getting array rank from '{theType.FullName}': {ex.Message}");
                throw;
            }
        }

        public static Assembly? GetTypeAssembly([DisallowNull] this Type theType)
        {
            return theType?.Assembly;
        }

        public static string? GetTypeAssemblyQualifiedName([DisallowNull] this Type theType)
        {
            return theType is not null ? theType.AssemblyQualifiedName : string.Empty;
        }

        public static TypeAttributes? GetTypeAttributes([DisallowNull] this Type theType)
        {
            return theType?.Attributes;
        }

        public static Type? GetTypeBaseType([DisallowNull] this Type theType)
        {
            return theType?.BaseType;
        }

        public static ConstructorInfo? GetTypeConstructor([DisallowNull] this Type theType, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
        {
            return theType?.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
        }

        public static ConstructorInfo[] GetTypeConstructors([DisallowNull] this Type theType)
        {
            return theType?.IsClass == true ? theType.GetConstructors() : Array.Empty<ConstructorInfo>();
        }

        public static IEnumerable<CustomAttributeData> GetTypeCustomAttributes([DisallowNull] this Type theType)
        {
            return theType?.CustomAttributes ?? Enumerable.Empty<CustomAttributeData>();
        }

        public static MethodBase? GetTypeDeclaringMethod([DisallowNull] this Type theType)
        {
            return theType?.DeclaringMethod;
        }

        public static Type? GetTypeDeclaringType([DisallowNull] this Type theType)
        {
            return theType?.DeclaringType;
        }

        public static MemberInfo[] GetTypeDefaultMembers([DisallowNull] this Type theType)
        {
            return theType is not null ? theType.GetDefaultMembers() : Array.Empty<MemberInfo>();
        }

        public static Type? GetTypeElementType([DisallowNull] this Type theType)
        {
            return theType.HasElementType ? theType?.GetElementType() : default;
        }

        public static string? GetTypeEnumName([DisallowNull] this Type theType, object value)
        {
            return value is not null && theType.IsEnum && theType.IsEnumDefined(value) ? theType.GetEnumName(value) : string.Empty;
        }

        public static string[] GetTypeEnumNames([DisallowNull] this Type theType)
        {
            return theType?.IsEnum == true ? theType.GetEnumNames() : Array.Empty<string>();
        }

        public static Type? GetTypeEnumUnderlyingType([DisallowNull] this Type theType)
        {
            return theType?.IsEnum == true ? theType.GetEnumUnderlyingType() : default;
        }

        public static Array GetTypeEnumValues([DisallowNull] this Type theType)
        {
            return theType.IsEnum ? theType.GetEnumValues() : Array.Empty<Enum>();
        }

        public static Array GetTypeEnumValuesAsUnderlyingType([DisallowNull] this Type theType)
        {
            return theType.IsEnum ? theType.GetEnumValuesAsUnderlyingType() : Array.Empty<int>();
        }

        public static EventInfo? GetTypeEvent([DisallowNull] this Type theType, string name)
        {
            return theType.GetEvent(name);
        }

        public static EventInfo[] GetTypeEvents([DisallowNull] this Type theType)
        {
            return theType.GetEvents();
        }

        public static EventInfo[] GetTypeEvents([DisallowNull] this Type theType, BindingFlags bindingAttr)
        {
            return theType.GetEvents(bindingAttr);
        }

        public static FieldInfo? GetTypeField([DisallowNull] this Type theType, string name)
        {
            return theType.GetField(name);
        }

        public static FieldInfo? GetTypeField([DisallowNull] this Type theType, string name, BindingFlags bindingAttr)
        {
            return theType.GetField(name, bindingAttr);
        }

        public static FieldInfo[] GetTypeFields([DisallowNull] this Type theType)
        {
            return theType.GetFields();
        }

        public static FieldInfo[] GetTypeFields([DisallowNull] this Type theType, BindingFlags bindingAttr)
        {
            return theType.GetFields(bindingAttr);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with the specified <paramref name="clsId"/><c>CLSID</c> identifier.
        /// </summary>
        /// <param name="clsId">Specifies the <c>CLSID</c> of the <see cref="Type"/> to get.</param>
        /// <returns>A <see cref="Type"/> representing the <c>COM</c> type associated with <paramref name="clsId"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if the resulting <see cref="Type"/> is not a valid <c>COM</c> instance.
        /// </exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromClsId(Guid clsId)
        {
            var comType = OperatingSystem.IsWindows() ? Marshal.GetTypeFromCLSID(clsId) : default;
            return comType?.IsCOMObject == true ? comType : throw new InvalidOperationException($"Parameter {nameof(clsId)} with CLSID value '{clsId}' is not a valid COM type GUID.");
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with the specified <paramref name="clsId"/><c>CLSID</c> identifier.
        /// </summary>
        /// <param name="clsId">       Specifies the <c>CLSID</c> of the <see cref="Type"/> to get.</param>
        /// <param name="throwOnError">
        /// If <see langref="true"/>, throw an <see cref="Exception"/> for any error that occurs; otherwise, suppress such <see cref="Exception"/>.
        /// </param>
        /// <returns>A <see cref="Type"/> representing the <c>COM</c> type associated with <paramref name="clsId"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if the resulting <see cref="Type"/> is not a valid <c>COM</c> instance.
        /// </exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromClsId(Guid clsId, bool throwOnError)
        {
            var comType = OperatingSystem.IsWindows() ? Type.GetTypeFromCLSID(clsId, throwOnError) : default;
            return comType?.IsCOMObject == true ? comType : throw new InvalidOperationException($"Parameter {nameof(clsId)} with CLSID value '{clsId}' is not a valid COM type GUID.");
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with <paramref name="progId"/>.
        /// </summary>
        /// <param name="progId">Specifies the <c>ProgID</c> of the type to get.</param>
        /// <returns>
        /// A <see cref="Type"/> associated with <paramref name="progId"/>, if the <paramref name="progId"/> is a valid entry in the
        /// registry nad a <see cref="Type"/> is associated with it; otherwise <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when <paramref name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromProgId(string progId)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(progId, nameof(progId));

            return OperatingSystem.IsWindows() ? Type.GetTypeFromProgID(progId) : default;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with <paramref name="progId"/>.
        /// </summary>
        /// <param name="progId">Specifies the <c>ProgID</c> of the type to get.</param>
        /// <param name="server">
        /// Specifies the name of the remote server on which to get and activate the <c>COM</c> type. If <see langref="null"/>, the
        /// local server will automatically be used.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> associated with <paramref name="progId"/>, if the <paramref name="progId"/> is a valid entry in the
        /// registry nad a <see cref="Type"/> is associated with it; otherwise <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when <paramref name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromProgId(string progId, string? server)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(progId, nameof(progId));

            return OperatingSystem.IsWindows() ? Type.GetTypeFromProgID(progId, server) : default;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with <paramref name="progId"/>.
        /// </summary>
        /// <param name="progId">      Specifies the <c>ProgID</c> of the type to get.</param>
        /// <param name="throwOnError">
        /// If <see langref="true"/>, an <see cref="Exception"/> will be thrown for any error; otherwise, <see cref="Exception"/>
        /// will be ignored.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> associated with <paramref name="progId"/>, if the <paramref name="progId"/> is a valid entry in the
        /// registry nad a <see cref="Type"/> is associated with it; otherwise <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when <paramref name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <exception cref="COMException">Throws when the <c>COM</c> type is not registered or cannot be activated.</exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromProgId(string progId, bool throwOnError)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(progId, nameof(progId));

            try
            {
                return OperatingSystem.IsWindows() ? Type.GetTypeFromProgID(progId, throwOnError) : default;
            }
            catch (COMException ex)
            {
                Console.Error.WriteLine($"Error getting COM type from ProgID '{progId}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> associated with <paramref name="progId"/>.
        /// </summary>
        /// <param name="progId">      Specifies the <c>ProgID</c> of the type to get.</param>
        /// <param name="server">      
        /// Specifies the name of the remote server on which to get and activate the <c>COM</c> type. If <see langref="null"/>, the
        /// local server will automatically be used.
        /// </param>
        /// <param name="throwOnError">
        /// If <see langref="true"/>, an <see cref="Exception"/> will be thrown for any error; otherwise, <see cref="Exception"/>
        /// will be ignored.
        /// </param>
        /// <returns>
        /// A <see cref="Type"/> associated with <paramref name="progId"/>, if the <paramref name="progId"/> is a valid entry in the
        /// registry nad a <see cref="Type"/> is associated with it; otherwise <see langref="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when <paramref name="progId"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <exception cref="COMException">Throws when the <c>COM</c> type is not registered or cannot be activated.</exception>
        [SupportedOSPlatform("windows")]
        public static Type? GetTypeFromProgId(string progId, string? server, bool throwOnError)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(progId, nameof(progId));

            try
            {
                return OperatingSystem.IsWindows() ? Type.GetTypeFromProgID(progId, server, throwOnError) : default;
            }
            catch (COMException ex)
            {
                Console.Error.WriteLine($"Error getting COM type from ProgID '{progId}': {ex.Message}");
                throw;
            }
        }

        public static Type? GetTypeFromTypeName(string typeName, bool throwOnError = true)
        {
            return Type.GetType(typeName, throwOnError);
        }

        [SupportedOSPlatform("windows")]
        public static string? GetTypeFullName(string progId)
        {
            return OperatingSystem.IsWindows() ? GetTypeFullName(GetTypeFromProgId(progId)) : string.Empty;
        }

        [SupportedOSPlatform("windows")]
        public static string? GetTypeFullName(Guid clsId)
        {
            return OperatingSystem.IsWindows() ? GetTypeFullName(GetTypeFromClsId(clsId)) : string.Empty;
        }

        public static string? GetTypeFullName([DisallowNull] this Type theType)
        {
            return theType is not null ? theType.FullName : $"COM class with Type Full Name '{theType!.FullName}' not found.";
        }

        public static GenericParameterAttributes GetTypeGenericParameterAttributes([DisallowNull] this Type theType)
        {
            return theType?.GenericParameterAttributes ?? default;
        }

        public static int GetTypeGenericParameterPosition([DisallowNull] this Type theType)
        {
            return theType?.GenericParameterPosition ?? 0;
        }

        public static Type[] GetTypeGenericTypeArguments([DisallowNull] this Type theType)
        {
            return theType is not null ? theType.GenericTypeArguments : Array.Empty<Type>();
        }

        public static Guid? GetTypeGuid([DisallowNull] this Type theType)
        {
            return theType?.GUID;
        }

        public static RuntimeTypeHandle GetTypeHandle([DisallowNull] this Type theType)
        {
            return theType.TypeHandle;
        }

        public static ConstructorInfo? GetTypeInitializer([DisallowNull] this Type theType)
        {
            return theType?.TypeInitializer;
        }

        public static Type? GetTypeInterface([DisallowNull] this Type theType, string name)
        {
            return theType.GetInterface(name);
        }

        public static InterfaceMapping GetTypeInterfaceMap([DisallowNull] this Type theType, Type interfaceType)
        {
            return theType.GetInterfaceMap(interfaceType);
        }

        public static Type[] GetTypeInterfaces([DisallowNull] this Type theType)
        {
            return theType.GetInterfaces();
        }

        public static MemberTypes GetTypeMemberType([DisallowNull] this Type theType)
        {
            return theType.MemberType;
        }

        public static MemberInfo GetTypeMemberWithSameMetadataDefinitionAs([DisallowNull] this Type theType, MemberInfo member)
        {
            return theType.GetMemberWithSameMetadataDefinitionAs(member);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name)
        {
            return theType.GetMethod(name);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name, BindingFlags bindingAttr)
        {
            return theType.GetMethod(name, bindingAttr);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name, BindingFlags bindingAttr, Type[] types)
        {
            return theType.GetMethod(name, bindingAttr, types);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name, int genericParameterCount, BindingFlags bindingAttr, Type[] types)
        {
            return theType.GetMethod(name, genericParameterCount, bindingAttr, types);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers)
        {
            return theType.GetMethod(name, bindingAttr, binder, types, modifiers);
        }

        public static MethodInfo? GetTypeMethod([DisallowNull] this Type theType, string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callingConventions, Type[] types, ParameterModifier[]? modifiers)
        {
            return theType.GetMethod(name, bindingAttr, binder, callingConventions, types, modifiers);
        }

        public static MethodInfo[] GetTypeMethods([DisallowNull] this Type theType)
        {
            return theType.GetMethods();
        }

        public static MethodInfo[] GetTypeMethods([DisallowNull] this Type theType, BindingFlags bindingAttr)
        {
            return theType.GetMethods(bindingAttr);
        }

        public static Module GetTypeModule([DisallowNull] this Type theType)
        {
            return theType.Module;
        }

        [SupportedOSPlatform("windows")]
        public static string? GetTypeName(string progId)
        {
            return OperatingSystem.IsWindows() ? GetTypeName(GetTypeFromProgId(progId)) : string.Empty;
        }

        [SupportedOSPlatform("windows")]
        public static string? GetTypeName(Guid clsId)
        {
            return OperatingSystem.IsWindows() ? GetTypeName(GetTypeFromClsId(clsId)) : string.Empty;
        }

        public static string? GetTypeName([DisallowNull] this Type theType)
        {
            return theType is not null ? theType.Name : $"COM class with Type Full Name '{theType!.FullName}' not found.";
        }

        public static string GetTypeNamespace([DisallowNull] this Type theType)
        {
            return theType.Namespace ?? string.Empty;
        }

        public static Type? GetTypeNestedType([DisallowNull] this Type theType, string name)
        {
            return theType.GetNestedType(name);
        }

        public static Type? GetTypeNestedType([DisallowNull] this Type theType, string name, BindingFlags bindingAttr)
        {
            return theType.GetNestedType(name, bindingAttr);
        }

        public static Type[] GetTypeNestedTypes([DisallowNull] this Type theType)
        {
            return theType.GetNestedTypes();
        }

        public static Type[] GetTypeNestedTypes([DisallowNull] this Type theType, BindingFlags bindingAttr)
        {
            return theType.GetNestedTypes(bindingAttr);
        }

        public static Type[] GetTypeOptionalCustomModifiers([DisallowNull] this Type theType)
        {
            return theType.GetOptionalCustomModifiers();
        }

        public static PropertyInfo? GetTypeProperty([DisallowNull] this Type theType, string name)
        {
            return theType?.GetProperty(name);
        }

        public static object? GetTypeProperty([DisallowNull] this Type theType, string name, object? instance)
        {
            return theType?.GetProperty(name)?.GetValue(instance);
        }

        public static object? GetTypeProperty([DisallowNull] this Type theType, string name, object? instance, object?[]? index)
        {
            return theType?.GetProperty(name)?.GetValue(instance, index);
        }

        public static object? GetTypeProperty([DisallowNull] this Type theType, string name, object? instance, BindingFlags bindingAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        {
            return theType?.GetProperty(name)?.GetValue(instance, bindingAttr, binder, index, culture ?? CultureInfo.CurrentCulture);
        }

        public static Type GetTypeReflectedType([DisallowNull] this Type theType)
        {
            return theType.ReflectedType ?? typeof(object);
        }

        public static Type[] GetTypeRequiredCustomModifiers([DisallowNull] this Type theType)
        {
            return theType.GetRequiredCustomModifiers();
        }

        public static EventInfo? GetTypeRuntimeEvent([DisallowNull] this Type theType, string name)
        {
            return theType.GetRuntimeEvent(name);
        }

        public static IEnumerable<EventInfo> GetTypeRuntimeEvents([DisallowNull] this Type theType)
        {
            return theType.GetRuntimeEvents();
        }

        public static FieldInfo? GetTypeRuntimeField([DisallowNull] this Type theType, string name)
        {
            return theType.GetRuntimeField(name);
        }

        public static IEnumerable<FieldInfo> GetTypeRuntimeFields([DisallowNull] this Type theType)
        {
            return theType.GetRuntimeFields();
        }

        public static MethodInfo? GetTypeRuntimeMethod([DisallowNull] this Type theType, string name, Type[] parameters)
        {
            return theType.GetRuntimeMethod(name, parameters);
        }

        public static IEnumerable<MethodInfo> GetTypeRuntimeMethods([DisallowNull] this Type theType)
        {
            return theType.GetRuntimeMethods();
        }

        public static IEnumerable<PropertyInfo> GetTypeRuntimeProperties([DisallowNull] this Type theType)
        {
            return theType.GetRuntimeProperties();
        }

        public static PropertyInfo? GetTypeRuntimeProperty([DisallowNull] this Type theType, string name)
        {
            return theType.GetRuntimeProperty(name);
        }

        public static Type GetTypeUnderlyingSystemType([DisallowNull] this Type theType)
        {
            return theType?.UnderlyingSystemType ?? typeof(object);
        }

        public static object? InvokeCom(
            Type comType,
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            object? comObject,
            object?[]? arguments,
            CultureInfo? culture)
        {
            return comType.InvokeMember(name, bindingAttr, binder, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static object? InvokeCom<TCom>(
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            TCom? comObject,
            object?[]? arguments,
            CultureInfo? culture) where TCom : class
        {
            return InvokeCom(typeof(TCom), name, bindingAttr, binder, comObject, arguments, culture);
        }

        public static object? InvokeCom(
            Type comType,
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            object? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters)
        {
            return comType.InvokeMember(name, bindingAttr, binder, comObject, arguments, modifiers, culture ?? CultureInfo.CurrentCulture, namedParameters);
        }

        public static object? InvokeCom<TCom>(
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            TCom? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters) where TCom : class
        {
            return InvokeCom(typeof(TCom), name, bindingAttr, binder, comObject, arguments, modifiers, culture, namedParameters);
        }

        public static object? InvokeCom(
            Type comType,
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            object? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture)
        {
            return comType.InvokeMember(name, bindingAttr, binder, comObject, arguments, [.. parameterModifiers.Values], culture ?? CultureInfo.CurrentCulture, [.. parameterModifiers.Keys]);
        }

        public static object? InvokeCom<TCom>(
            string name,
            BindingFlags bindingAttr,
            Binder? binder,
            TCom? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture) where TCom : class
        {
            return InvokeCom(typeof(TCom), name, bindingAttr, binder, comObject, arguments, parameterModifiers, culture);
        }

        public static object? InvokeComFieldGet(
            Type comType,
            string name,
            object? comObject,
            CultureInfo? culture)
        {
            return comType.InvokeMember(name, BindingFlags.GetField, null, comObject, null, culture ?? CultureInfo.CurrentCulture);
        }

        public static object? InvokeComFieldGet<TCom>(
            string name,
            TCom? comObject,
            CultureInfo? culture) where TCom : class
        {
            return InvokeComFieldGet(typeof(TCom), name, comObject, culture);
        }

        public static void InvokeComFieldSet(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            CultureInfo? culture)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetField | BindingFlags.IgnoreReturn, null, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static void InvokeComFieldSet(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetField | BindingFlags.IgnoreReturn, null, comObject, arguments, modifiers, culture ?? CultureInfo.CurrentCulture, namedParameters);
        }

        public static void InvokeComFieldSet<TCom>(
            string name,
            TCom? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters) where TCom : class
        {
            InvokeComFieldSet(typeof(TCom), name, comObject, arguments, modifiers, culture, namedParameters);
        }

        public static void InvokeComFieldSet(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetField | BindingFlags.IgnoreReturn, null, comObject, arguments, [.. parameterModifiers.Values], culture ?? CultureInfo.CurrentCulture, [.. parameterModifiers.Keys]);
        }

        public static void InvokeComFieldSet<TCom>(
            string name,
            TCom? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture) where TCom : class
        {
            InvokeComFieldSet(typeof(TCom), name, comObject, arguments, parameterModifiers, culture);
        }

        public static object? InvokeComMethod(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            CultureInfo? culture)
        {
            return comType.InvokeMember(name, BindingFlags.InvokeMethod, null, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static object? InvokeComMethod(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters)
        {
            return comType.InvokeMember(name, BindingFlags.InvokeMethod, null, comObject, arguments, modifiers, culture ?? CultureInfo.CurrentCulture, namedParameters);
        }

        public static object? InvokeComMethod(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture)
        {
            return comType.InvokeMember(name, BindingFlags.InvokeMethod, null, comObject, arguments, [.. parameterModifiers.Values], culture ?? CultureInfo.CurrentCulture, [.. parameterModifiers.Keys]);
        }

        public static object? InvokeComPropertyGet(Type comType, string name, object? comObject, CultureInfo? culture)
        {
            return comType.InvokeMember(name, BindingFlags.GetProperty, null, comObject, null, culture ?? CultureInfo.CurrentCulture);
        }

        public static object? InvokeComPropertyGet(Type comType, string name, object? comObject, object?[]? arguments, CultureInfo? culture)
        {
            return comType.InvokeMember(name, BindingFlags.GetProperty, null, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static void InvokeComPropertySet(Type comType, string name, object? comObject, object?[]? arguments, CultureInfo? culture)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetProperty | BindingFlags.IgnoreReturn, null, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static void InvokeComPropertySet(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetProperty | BindingFlags.IgnoreReturn, null, comObject, arguments, modifiers, culture ?? CultureInfo.CurrentCulture, namedParameters);
        }

        public static void InvokeComPropertySet<TCom>(
            string name,
            TCom? comObject,
            object?[]? arguments,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParameters) where TCom : class
        {
            InvokeComPropertySet(typeof(TCom), name, comObject, arguments, modifiers, culture, namedParameters);
        }

        public static void InvokeComPropertySet(
            Type comType,
            string name,
            object? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture)
        {
            _ = comType.InvokeMember(name, BindingFlags.SetField | BindingFlags.IgnoreReturn, null, comObject, arguments, [.. parameterModifiers.Values], culture ?? CultureInfo.CurrentCulture, [.. parameterModifiers.Keys]);
        }

        public static void InvokeComPropertySet<TCom>(
            string name,
            TCom? comObject,
            object?[]? arguments,
            IDictionary<string, ParameterModifier> parameterModifiers,
            CultureInfo? culture) where TCom : class
        {
            InvokeComPropertySet(typeof(TCom), name, comObject, arguments, parameterModifiers, culture);
        }

        public static void InvokeComVoidMethod(Type comType, string name, object? comObject, object?[]? arguments, CultureInfo? culture)
        {
            _ = comType.InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn, null, comObject, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static int Release(object? comObject)
        {
            return comObject is null
                ? -1
                : !Marshal.IsComObject(comObject)
                    ? throw new ArgumentException($"Parameter {nameof(comObject)} is not a valid 'COM' instance.", nameof(comObject))
                    : Marshal.ReleaseComObject(comObject);
        }

        public static bool TestComTypeEquivalentTo([DisallowNull] this Type comType, Type? other)
        {
            return comType.IsEquivalentTo(other);
        }

        public static bool TestTypeAbstract([DisallowNull] this Type theType)
        {
            return theType?.IsAbstract == true;
        }

        public static bool TestTypeAnsi([DisallowNull] Type theType)
        {
            return theType?.IsAnsiClass == true;
        }

        public static bool TestTypeArray([DisallowNull] this Type theType)
        {
            return theType?.IsArray == true;
        }

        public static bool TestTypeAssemblyCollectible([DisallowNull] this Type theType)
        {
            return theType?.IsCollectible == true;
        }

        public static bool TestTypeAssignableFrom([DisallowNull] this Type theType, Type? sourceType)
        {
            return theType.IsAssignableFrom(sourceType);
        }

        public static bool TestTypeAssignableTo([DisallowNull] this Type theType, Type? targetType)
        {
            return theType.IsAssignableTo(targetType);
        }

        public static bool TestTypeAutoClass([DisallowNull] this Type theType)
        {
            return theType?.IsAutoClass == true;
        }

        public static bool TestTypeAutoLayout([DisallowNull] this Type theType)
        {
            return theType?.IsAutoLayout == true;
        }

        public static bool TestTypeByReference([DisallowNull] this Type theType)
        {
            return theType?.IsByRef == true;
        }

        public static bool TestTypeByReferenceStruct([DisallowNull] this Type theType)
        {
            return theType?.IsByRefLike == true;
        }

        public static bool TestTypeClass([DisallowNull] this Type theType)
        {
            return theType?.IsClass == true;
        }

        public static bool TestTypeComObject([DisallowNull] this Type comType)
        {
            return comType?.IsCOMObject == true;
        }

        public static bool TestTypeComObject<TCom>() where TCom : class
        {
            return typeof(TCom)?.IsCOMObject == true;
        }

        public static bool TestTypeConstructedGenericType([DisallowNull] this Type theType)
        {
            return theType?.IsConstructedGenericType == true;
        }

        public static bool TestTypeContainsGenericParameters([DisallowNull] this Type theType)
        {
            return theType?.ContainsGenericParameters == true;
        }

        public static bool TestTypeContextHostable([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsContextful;
        }

        public static bool TestTypeDefinition([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsTypeDefinition;
        }

        public static bool TestTypeElementType([DisallowNull] this Type theType)
        {
            return theType.HasElementType;
        }

        public static bool TestTypeEnumDefined([DisallowNull] this Type theType, object value)
        {
            return theType.IsEnum && theType.IsEnumDefined(value);
        }

        public static bool TestTypeExplicitLayout([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsExplicitLayout;
        }

        public static bool TestTypeFunctionPointer([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsFunctionPointer;
        }

        public static bool TestTypeGenericMethodParameter([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsGenericMethodParameter;
        }

        public static bool TestTypeGenericParameter([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsGenericParameter;
        }

        public static bool TestTypeGenericType([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsGenericType;
        }

        public static bool TestTypeGenericTypeDefinition([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsGenericTypeDefinition;
        }

        public static bool TestTypeGenericTypeParameter([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsGenericTypeParameter;
        }

        public static bool TestTypeHasElementType([DisallowNull] this Type theType)
        {
            return theType is not null && theType.HasElementType;
        }

        public static bool TestTypeImported([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsImport;
        }

        public static bool TestTypeInstanceOfType([DisallowNull] this Type theType, object? other)
        {
            return theType.IsInstanceOfType(other);
        }

        public static bool TestTypeInterface([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsInterface;
        }

        public static bool TestTypeIsEnum([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsEnum;
        }

        public static bool TestTypeLayoutSequential([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsLayoutSequential;
        }

        public static bool TestTypeMarshalByReference([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsMarshalByRef;
        }

        public static bool TestTypeNested([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNested;
        }

        public static bool TestTypeNestedAssembly([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNestedAssembly;
        }

        public static bool TestTypeNestedFamilyOrAssembly([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNestedFamORAssem;
        }

        public static bool TestTypeNestedPrivate([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNestedPrivate;
        }

        public static bool TestTypeNestedPublic([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNestedPublic;
        }

        public static bool TestTypeNotPublic([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsNotPublic;
        }

        public static bool TestTypePointer([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsPointer;
        }

        public static bool TestTypePrimitive([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsPrimitive;
        }

        public static bool TestTypePublic([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsPublic;
        }

        public static bool TestTypeSealedClass([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsClass && theType.IsSealed;
        }

        public static bool TestTypeSecurityCritical([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSecurityCritical;
        }

        public static bool TestTypeSecuritySafeCritical([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSecuritySafeCritical;
        }

        public static bool TestTypeSecurityTransparent([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSecurityTransparent;
        }

        [Obsolete("Binary Serialization is a Serious Security Risk", DiagnosticId = "SerializableType")]
        public static bool TestTypeSerializable([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSerializable;
        }

        public static bool TestTypeSingleDimensionAndZeroLowerBound([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSZArray;
        }

        public static bool TestTypeSpecialName([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsSpecialName;
        }

        public static bool TestTypeUnicodeClass([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsUnicodeClass;
        }

        public static bool TestTypeUnmanagedFunctionPointer([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsUnmanagedFunctionPointer;
        }

        public static bool TestTypeValueType([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsValueType;
        }

        public static bool TestTypeVariableBoundArray([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsVariableBoundArray;
        }

        public static bool TestTypeVisible([DisallowNull] this Type theType)
        {
            return theType is not null && theType.IsVisible;
        }

        public static object? TypeInvokeMember([DisallowNull] this Type theType, string name, BindingFlags bindingAttr, Binder? binder, object? target, object?[]? arguments, CultureInfo? culture)
        {
            return theType.InvokeMember(name, bindingAttr, binder, target, arguments, culture ?? CultureInfo.CurrentCulture);
        }

        public static object? TypeInvokeMember([DisallowNull] this Type theType, string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters)
        {
            return theType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture ?? CultureInfo.CurrentCulture, namedParameters);
        }
    }
}
