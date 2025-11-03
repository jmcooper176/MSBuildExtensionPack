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

// Ignore Spelling: cls iid RCW

namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    /// <summary>
    /// Implements a wrapper around the <c>MSI</c><c>COM</c><see cref="Type"/><c>WindowsInstaller.Client</c>.
    /// </summary>
    /// <seealso cref="IMsiCom"/>
    /// <seealso cref="IEquatable{ClientObject}"/>
    /// <seealso cref="IEqualityComparer{ClientObject}"/>
    [SupportedOSPlatform("windows")]
    public class ClientObject : IMsiCom, IEquatable<ClientObject>, IEqualityComparer<ClientObject>
    {
        private bool disposedValue;

        /// <summary>
        /// Finalizes an instance of the <see cref="ClientObject"/> class.
        /// </summary>
        ~ClientObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientObject"/> class.
        /// </summary>
        /// <param name="clsId"> Specifies the class identifier ("CLSID").</param>
        /// <param name="iid">   Specifies the Interface Identifier ("IID").</param>
        /// <param name="progId">Specifies the Programmatic Identifier ("ProgId").</param>
        /// <exception cref="InvalidOperationException">
        /// Either <paramref name="clsId"/> or <paramref name="progId"/> must be defined to create the COM object.
        /// </exception>
        protected ClientObject(Guid clsId, Guid iid, string? progId)
        {
            ProgId = progId ?? GetProgrammaticIdentifier() ?? string.Empty;
            ClsId = clsId != Guid.Empty ? clsId : GetClassIdentifier();

            if (ClsId == Guid.Empty && string.IsNullOrWhiteSpace(ProgId))
            {
                throw new InvalidOperationException($"Either {nameof(ClsId)} or {nameof(ProgId)} must be defined to create the COM object.");
            }

            IID = iid != Guid.Empty ? iid : GetInterfaceIdentifier();
            InterfaceTypes = [];
            DefaultComparer = EqualityComparer<ClientObject>.Create((l, r) => ReferenceEquals(l, r) || (l?.Equals(r) == true), i => GetHashCode(i));
        }

        /// <summary>
        /// Gets a value representing the default <see cref="IEqualityComparer{T}"/> comparer.
        /// </summary>
        /// <value>The default <see cref="IEqualityComparer{T}"/> comparer.</value>
        protected virtual EqualityComparer<ClientObject> DefaultComparer { get; }

        /// <summary>
        /// Releases unmanaged and (optionally) managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langref="true"/> to release both managed and unmanaged resources; otherwise, <see langref="false"/> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    InterfaceTypes.ToList().Clear();
                    ReleaseAll();
                }

                ComType = null;
                Instance = null;
                RCW = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientObject"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Unable to generate 'ComType' for ProgID or Unable to generate 'ComType' for CLSID
        /// </exception>
        /// <exception cref="ArgumentException">Unable to activate 'ComType' to create the COM Object</exception>
        public ClientObject()
            : this(Guid.Empty, Guid.Empty, null)
        {
            ComType = !string.IsNullOrWhiteSpace(ProgId)
                ? Type.GetTypeFromProgID(ProgId, throwOnError: true) ?? throw new ArgumentNullException($"Unable to generate 'ComType' for ProgID '{ProgId}'")
                : Marshal.GetTypeFromCLSID(ClsId) ?? throw new ArgumentNullException($"Unable to generate 'ComType' for CLSID '{ClsId}'");

            Instance = Activator.CreateInstance(ComType) ?? throw new ArgumentException($"Unable to activate 'ComType' to create the COM Object");
            InterfaceTypes = [.. ComType.GetInterfaces()];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientObject"/> class.
        /// </summary>
        /// <param name="instance">Specifies the <c>COM</c> object instance.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="instance"/> cannot be null.</exception>
        public ClientObject([AllowNull] dynamic instance)
            : this()
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            Instance = instance;
        }

        /// <inheritdoc/>
        public Guid ClsId { get; }

        /// <inheritdoc/>
        public Type? ComType { get; private set; }

        /// <inheritdoc/>
        public Guid IID { get; }

        /// <inheritdoc/>
        public dynamic? Instance { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<Type> InterfaceTypes { get; }

        /// <inheritdoc/>
        public string ProgId { get; }

        /// <inheritdoc/>
        public virtual object? RCW { get; private set; }

        /// <summary>
        /// Implements the operator != for <see cref="ClientObject"/>.
        /// </summary>
        /// <param name="left"> Specifies the left-hand <see cref="ClientObject"/> value.</param>
        /// <param name="right">Specifies the right-hand <see cref="ClientObject"/> value.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="left"/> does not equal <paramref name="right"/>; otherwise <see langref="false"/>.
        /// </returns>
        public static bool operator !=(ClientObject? left, ClientObject? right) => left?.Equals(right) != true;

        /// <summary>
        /// Implements the operator == for <see cref="ClientObject"/>.
        /// </summary>
        /// <param name="left"> Specifies the left-hand <see cref="ClientObject"/> value.</param>
        /// <param name="right">Specifies the right-hand <see cref="ClientObject"/> value.</param>
        /// <returns><see langref="true"/> if <paramref name="left"/> equals <paramref name="right"/>; otherwise <see langref="false"/>.</returns>
        public static bool operator ==(ClientObject? left, ClientObject? right) => left?.Equals(right) == true;

        /// <inheritdoc/>
        public object Clone() => new ClientObject(this.Instance);

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is not null
            && obj is ClientObject other
            && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(ClientObject? other) => other is not null
            && this.ClsId == other.ClsId
            && this.ComType == other.ComType
            && this.IID == other.IID
            && this.InterfaceTypes.SequenceEqual(other.InterfaceTypes)
            && this.ProgId.Equals(other.ProgId, StringComparison.Ordinal);

        /// <inheritdoc/>
        public bool Equals(ClientObject? x, ClientObject? y) => DefaultComparer.Equals(x, y);

        /// <inheritdoc/>
        public virtual Guid GetClassIdentifier() => ComType is not null ? Marshal.GenerateGuidForType(ComType) : Guid.Empty;

        /// <inheritdoc/>
        public object? GetComObjectData(object key) => Instance is not null ? Marshal.GetComObjectData(Instance, key) : null;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(
            this.ClsId,
            this.ComType,
            this.IID,
            this.Instance,
            this.InterfaceTypes,
            this.ProgId);

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] ClientObject obj) => obj.GetHashCode();

        /// <inheritdoc/>
        public virtual Guid GetInterfaceIdentifier() => new("000C1098-0000-0000-C000-000000000046");

        /// <inheritdoc/>
        public virtual string? GetProgrammaticIdentifier() => (ComType is not null ? Marshal.GenerateProgIdForType(ComType) : null) ?? "WindowsInstaller.Client";

        /// <inheritdoc/>
        public int QueryInterface(out nint returnedInterface)
        {
            returnedInterface = IntPtr.Zero;

            return RCW is not null ? Marshal.QueryInterface(Marshal.GetIUnknownForObject(Instance), IID, out returnedInterface) : -1;
        }

        /// <inheritdoc/>
        public int Release() => Instance is not null ? Marshal.ReleaseComObject(Instance) : 0;

        /// <inheritdoc/>
        public bool ReleaseAll() => Instance is not null && Marshal.FinalReleaseComObject(Instance) == 0;

        /// <inheritdoc/>
        public bool SetComObjectData(object key, object? value) => Instance is not null && Marshal.SetComObjectData(Instance, key, value);

        /// <inheritdoc/>
        public IMsiCom? ToComObject(dynamic instance) => new ClientObject(instance);

        /// <inheritdoc/>
        public dynamic? ToInstance() => Instance;

        /// <inheritdoc/>
        public override string ToString() => $"ComType='{ComType?.FullName ?? "<< Null >>"}' | IID='{IID:B}' | ProgId='{ProgId}#{ClsId:D}'";
    }
}
