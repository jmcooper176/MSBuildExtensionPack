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

// Ignore Spelling: Msi

namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    /// <summary>
    /// Interface for MSI COM objects.
    /// </summary>
    /// <seealso cref="System.ICloneable"/>
    /// <seealso cref="System.IDisposable"/>
    public interface IMsiCom : ICloneable, IDisposable
    {
        /// <summary> Gets a value indicating the <c>COM</c> object c>Class Identifier</c>. </summary> <value> The <see
        /// cref="Guid"/> representing the COM object <c>Class Identifier</c>. </value>
        Guid ClsId { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="Type"/> of the <c>MSI</c> COM object.
        /// </summary>
        /// <value>The type of the COM object.</value>
        Type? ComType { get; }

        /// <summary>
        /// Gets a value indicating the <c>COM</c> object <c>Interface Identifier</c>.
        /// </summary>
        /// <value>The <see cref="Guid"/> representing the COM object <c>Interface Identifier</c>.</value>
        Guid IID { get; }

        /// <summary>
        /// Gets a value indicating the <c>COM</c> object instance created by <see cref="Activator.CreateInstance(Type)"/>.
        /// </summary>
        /// <value>The <c>COM</c> object instance or <see langref="null"/> returned by <see cref="Activator.CreateInstance(Type)"/>.</value>
        dynamic? Instance { get; }

        /// <summary>
        /// Gets a value indicating the collection of <see cref="Type"/> interface types from <see cref="ComType"/>.
        /// </summary>
        /// <value>The <see cref="IEnumerable{T}"/> of <see cref="ComType"/> interface types.</value>
        IEnumerable<Type> InterfaceTypes { get; }

        /// <summary>
        /// Gets a value indicating the <c>COM</c> program identifier.
        /// </summary>
        /// <value>The program identifier ("ProgId") representing the <c>COM</c> object to be created.</value>
        string ProgId { get; }

        /// <summary>
        /// Gets the class identifier ("CLSID") for by this <c>COM</c> object's version independent programmatic identifier
        /// ("ProgId") in the registry.
        /// </summary>
        /// <returns>
        /// A <see cref="Guid"/> representing the registered <c>CLSID</c> for the <c>ProgID</c>; otherwise <see cref="Guid.Empty"/>
        /// if the <c>COM</c> type has not been registered.
        /// </returns>
        Guid GetClassIdentifier();

        /// <summary>
        /// Gets the COM object data by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Specifies the key into the internal hash table of this <c>COM</c> object to retrieve data from.</param>
        /// <returns>An <see cref="object"/> representing the data associated with <paramref name="key"/>; otherwise, <see langref="null"/>.</returns>
        object? GetComObjectData(object key);

        /// <summary>
        /// Gets the Interface Identifier ("IID") <see cref="Guid"/> for this <c>COM</c> type.
        /// </summary>
        /// <returns>A <see cref="Guid"/> representing the <c>IID</c> for this <see cref="Type"/>.</returns>
        Guid GetInterfaceIdentifier();

        /// <summary>
        /// Gets the programmatic identifier ("ProgId") for this <c>COM</c> type; otherwise, generates a <c>ProgId</c> following the
        /// rules of the Type Library Importer <c>TlbImp.exe</c> tool.
        /// </summary>
        /// <returns>
        /// A <c>ProgId</c><see cref="string"/> if present for this <see cref="Type"/>; otherwise, generates a <c>ProgId</c><see
        /// cref="string"/> following the rules of the Type Library Importer <c>TlbImp.exe</c> tool.
        /// </returns>
        string? GetProgrammaticIdentifier();

        /// <summary>
        /// Requests a pointer to an interface specified by <see cref="IID"/> and returns it in <paramref name="returnedInterface"/>.
        /// </summary>
        /// <param name="returnedInterface">Specifies the returned interface.</param>
        /// <returns>An <c>HRESULT</c> specifying the success or failure of the operation.</returns>
        int QueryInterface(out IntPtr returnedInterface);

        /// <summary>
        /// Decrements the reference count of the Runtime Callable Wrapper ("RCW") associated with this <c>COM</c> object.
        /// </summary>
        /// <returns>An <see cref="int"/> reflecting the current reference count.</returns>
        /// <remarks>
        /// If the reference count is zero, the <c>RCW</c> has typically been released since an <c>RCW</c> only keeps one copy of
        /// the <c>COM</c> object in memory.
        /// </remarks>
        int Release();

        /// <summary>
        /// Releases all references to this <c>COM</c> object.
        /// </summary>
        /// <returns><see langref="true"/> if the release of all references succeeds; otherwise, <see langref="false"/>.</returns>
        bool ReleaseAll();

        /// <summary>
        /// Sets the COM object data by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">  
        /// Specifies the key into the internal hash table of this <c>COM</c> object to set data to <paramref name="value"/>.
        /// </param>
        /// <param name="value">Specifies the value to update <paramref name="key"/> with.</param>
        /// <returns>
        /// <see langref="true"/> if setting <paramref name="key"/> to <paramref name="value"/> succeeded; otherwise, <see langref="false"/>.
        /// </returns>
        bool SetComObjectData(object key, object? value);

        /// <summary>
        /// Clones this instance and returns it as a <c>COM</c> object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IMsiCom"/> of <paramref name="instance"/>; otherwise, <see langref="null"/>.</returns>
        IMsiCom? ToComObject(object instance);

        /// <summary>
        /// Converts this <c>COM</c> object to its instance.
        /// </summary>
        /// <returns></returns>
        object? ToInstance();
    }
}
