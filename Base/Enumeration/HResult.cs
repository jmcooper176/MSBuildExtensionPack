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
namespace MSBuild.ExtensionPack.Base.Enumeration
{
    using System.Security.Principal;

    using Microsoft.VisualStudio;

    /// <summary>
    /// Enumeration of <c>HRESULT</c> values.
    /// </summary>
    public enum HResult : int
    {
        /// <summary>
        /// Error <c>HRESULT</c> for first <c>CACHE_S</c> value.
        /// </summary>
        CACHE_S_FIRST = CACHE_S_FORMATETC_NOTSUPPORTED,

        /// <summary>
        /// Error <c>HRESULT</c> for FORMATETC not supported
        /// </summary>
        CACHE_S_FORMATETC_NOTSUPPORTED = 0x00040170,

        /// <summary>
        /// Error <c>HRESULT</c> for same cache
        /// </summary>
        CACHE_S_SAMECACHE = 0x00040171,

        /// <summary>
        /// Error <c>HRESULT</c> for some cache(s) not updated
        /// </summary>
        CACHE_S_SOMECACHES_NOTUPDATED = 0x00040172,

        /// <summary>
        /// Error <c>HRESULT</c> for no information available.
        /// </summary>
        CACHE_S_LAST = 0x0004017F,

        /// <summary>
        /// Error <c>HRESULT</c> for CATID does not exist
        /// </summary>
        CAT_E_FIRST = CAT_E_CATIDNOEXIST,

        /// <summary>
        /// Error <c>HRESULT</c> for CATID does not exist
        /// </summary>
        CAT_E_CATIDNOEXIST = unchecked((int)(0x80040160)),

        /// <summary>
        /// Error <c>HRESULT</c> for description not found
        /// </summary>
        CAT_E_LAST = CAT_E_NODESCRIPTION,

        /// <summary>
        /// Error <c>HRESULT</c> for description not found
        /// </summary>
        CAT_E_NODESCRIPTION = unchecked((int)(0x80040161)),

        /// <summary>
        /// Error <c>HRESULT</c> for a required certificate is not within its validity period when verifying against the current
        /// system clock or the timestamp in the signed file.
        /// </summary>
        CERT_E_EXPIRED = unchecked((int)(0x800B0101)),

        /// <summary>
        /// Error <c>HRESULT</c> for the validity periods of the certification chain do not nest correctly.
        /// </summary>
        CERT_E_VALIDITYPERIODNESTING = unchecked((int)(0x800B0102)),

        /// <summary>
        /// Error <c>HRESULT</c> for a certificate that can only be used as an end-entity is being used as a CA or visa versa.
        /// </summary>
        CERT_E_ROLE = unchecked((int)(0x800B0103)),

        /// <summary>
        /// Error <c>HRESULT</c> for a path length constraint in the certification chain has been violated.
        /// </summary>
        CERT_E_PATHLENCONST = unchecked((int)(0x800B0104)),

        /// <summary>
        /// Error <c>HRESULT</c> for a certificate contains an unknown extension that is marked 'critical'.
        /// </summary>
        CERT_E_CRITICAL = unchecked((int)(0x800B0105)),

        /// <summary>
        /// Error <c>HRESULT</c> for a certificate being used for a purpose other than the ones specified by its CA.
        /// </summary>
        CERT_E_PURPOSE = unchecked((int)(0x800B0106)),

        /// <summary>
        /// A parent of a given certificate in fact did not issue that child certificate.
        /// </summary>
        CERT_E_ISSUERCHAINING = unchecked((int)(0x800B0107)),

        /// <summary>
        /// Error <c>HRESULT</c> for a certificate is missing or has an empty value for an important field, such as a subject or
        /// issuer name.
        /// </summary>
        CERT_E_MALFORMED = unchecked((int)(0x800B0108)),

        /// <summary>
        /// Error <c>HRESULT</c> for a certificate chain processed, but terminated in a root certificate which is not trusted by the
        /// trust provider.
        /// </summary>
        CERT_E_UNTRUSTEDROOT = unchecked((int)(0x800B0109)),

        /// <summary>
        /// Error <c>HRESULT</c> for an internal certificate chaining error has occurred.
        /// </summary>
        CERT_E_CHAINING = unchecked((int)(0x800B010A)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for thread local storage failure
        /// </summary>
        CO_E_INIT_TLS = unchecked((int)(0x80004006)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for get shared memory allocator failure
        /// </summary>
        CO_E_INIT_SHARED_ALLOCATOR = unchecked((int)(0x80004007)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for get memory allocator failure
        /// </summary>
        CO_E_INIT_MEMORY_ALLOCATOR = unchecked((int)(0x80004008)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to initialize class cache
        /// </summary>
        CO_E_INIT_CLASS_CACHE = unchecked((int)(0x80004009)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to initialize RPC services
        /// </summary>
        CO_E_INIT_RPC_CHANNEL = unchecked((int)(0x8000400A)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for cannot set thread local storage channel control
        /// </summary>
        CO_E_INIT_TLS_SET_CHANNEL_CONTROL = unchecked((int)(0x8000400B)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for could not allocate thread local storage channel control
        /// </summary>
        CO_E_INIT_TLS_CHANNEL_CONTROL = unchecked((int)(0x8000400C)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the user supplied memory allocator is unacceptable
        /// </summary>
        CO_E_INIT_UNACCEPTED_USER_ALLOCATOR = unchecked((int)(0x8000400D)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the OLE service mutex already exists
        /// </summary>
        CO_E_INIT_SCM_MUTEX_EXISTS = unchecked((int)(0x8000400E)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the OLE service file mapping already exists
        /// </summary>
        CO_E_INIT_SCM_FILE_MAPPING_EXISTS = unchecked((int)(0x8000400F)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to map view of file for OLE service
        /// </summary>
        CO_E_INIT_SCM_MAP_VIEW_OF_FILE = unchecked((int)(0x80004010)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for failure attempting to launch OLE service
        /// </summary>
        CO_E_INIT_SCM_EXEC_FAILURE = unchecked((int)(0x80004011)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for there was an attempt to call CoInitialize a second time while single threaded
        /// </summary>
        CO_E_INIT_ONLY_SINGLE_THREADED = unchecked((int)(0x80004012)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for a remote activation was necessary but was not allowed
        /// </summary>
        CO_E_CANT_REMOTE = unchecked((int)(0x80004013)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for a remote activation was necessary but the server name provided was invalid
        /// </summary>
        CO_E_BAD_SERVER_NAME = unchecked((int)(0x80004014)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the class is configured to run as a security id different from the caller
        /// </summary>
        CO_E_WRONG_SERVER_IDENTITY = unchecked((int)(0x80004015)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for use of <c>OLE1</c> services requiring DDE windows is disabled
        /// </summary>
        CO_E_OLE1DDE_DISABLED = unchecked((int)(0x80004016)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for a RunAs specification must be {domain name}\{user name} or simply {user name}
        /// </summary>
        CO_E_RUNAS_SYNTAX = unchecked((int)(0x80004017)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server process could not be started. The pathname may be incorrect.
        /// </summary>
        CO_E_CREATEPROCESS_FAILURE = unchecked((int)(0x80004018)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server process could not be started as the configured identity. The pathname may
        /// be incorrect or unavailable.
        /// </summary>
        CO_E_RUNAS_CREATEPROCESS_FAILURE = unchecked((int)(0x80004019)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server process could not be started because the configured identity is
        /// incorrect. Check the username and password.
        /// </summary>
        CO_E_RUNAS_LOGON_FAILURE = unchecked((int)(0x8000401A)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the client is not allowed to launch this server.
        /// </summary>
        CO_E_LAUNCH_PERMSSION_DENIED = unchecked((int)(0x8000401B)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the service providing this server could not be started.
        /// </summary>
        CO_E_START_SERVICE_FAILURE = unchecked((int)(0x8000401C)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for this computer was unable to communicate with the computer providing the server.
        /// </summary>
        CO_E_REMOTE_COMMUNICATION_FAILURE = unchecked((int)(0x8000401D)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server did not respond after being launched.
        /// </summary>
        CO_E_SERVER_START_TIMEOUT = unchecked((int)(0x8000401E)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the registration information for this server is inconsistent or incomplete.
        /// </summary>
        CO_E_CLSREG_INCONSISTENT = unchecked((int)(0x8000401F)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the registration information for this interface is inconsistent or incomplete.
        /// </summary>
        CO_E_IIDREG_INCONSISTENT = unchecked((int)(0x80004020)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the operation attempted is not supported.
        /// </summary>
        CO_E_NOT_SUPPORTED = unchecked((int)(0x80004021)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for a dll must be loaded.
        /// </summary>
        CO_E_RELOAD_DLL = unchecked((int)(0x80004022)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for a Microsoft Software Installer error was encountered.
        /// </summary>
        CO_E_MSI_ERROR = unchecked((int)(0x80004023)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the specified activation could not occur in the client context as specified.
        /// </summary>
        CO_E_ATTEMPT_TO_CREATE_OUTSIDE_CLIENT_CONTEXT = unchecked((int)(0x80004024)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for activations on the server are paused.
        /// </summary>
        CO_E_SERVER_PAUSED = unchecked((int)(0x80004025)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for activations on the server are not paused.
        /// </summary>
        CO_E_SERVER_NOT_PAUSED = unchecked((int)(0x80004026)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the component or application containing the component has been disabled.
        /// </summary>
        CO_E_CLASS_DISABLED = unchecked((int)(0x80004027)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the common language runtime is not available
        /// </summary>
        CO_E_CLRNOTAVAILABLE = unchecked((int)(0x80004028)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the thread-pool rejected the submitted asynchronous work.
        /// </summary>
        CO_E_ASYNC_WORK_REJECTED = unchecked((int)(0x80004029)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server started, but did not finish initializing in a timely fashion.
        /// </summary>
        CO_E_SERVER_INIT_TIMEOUT = unchecked((int)(0x8000402A)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to complete the call since there is no <c>COM</c>+ security context inside IObjectControl.Activate.
        /// </summary>
        CO_E_NO_SECCTX_IN_ACTIVATE = unchecked((int)(0x8000402B)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the provided tracker configuration is invalid
        /// </summary>
        CO_E_TRACKER_CONFIG = unchecked((int)(0x80004030)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the provided thread pool configuration is invalid
        /// </summary>
        CO_E_THREADPOOL_CONFIG = unchecked((int)(0x80004031)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the provided side-by-side configuration is invalid
        /// </summary>
        CO_E_SXS_CONFIG = unchecked((int)(0x80004032)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the server principal name (SPN) obtained during security negotiation is malformed.
        /// </summary>
        CO_E_MALFORMED_SPN = unchecked((int)(0x80004033)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to impersonate D <c>COM</c> client
        /// </summary>
        CO_E_FAILEDTOIMPERSONATE = VSConstants.CO_E_FAILEDTOIMPERSONATE,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to obtain server's security context
        /// </summary>
        CO_E_FAILEDTOGETSECCTX = VSConstants.CO_E_FAILEDTOGETSECCTX,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to open the access token of the current thread
        /// </summary>
        CO_E_FAILEDTOOPENTHREADTOKEN = VSConstants.CO_E_FAILEDTOOPENTHREADTOKEN,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to obtain user info from an access token
        /// </summary>
        CO_E_FAILEDTOGETTOKENINFO = VSConstants.CO_E_FAILEDTOGETTOKENINFO,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the client who called IAccessControl::IsAccessPermitted was not the trustee provided
        /// to the method
        /// </summary>
        CO_E_TRUSTEEDOESNTMATCHCLIENT = VSConstants.CO_E_TRUSTEEDOESNTMATCHCLIENT,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to obtain the client's security blanket
        /// </summary>
        CO_E_FAILEDTOQUERYCLIENTBLANKET = VSConstants.CO_E_FAILEDTOQUERYCLIENTBLANKET,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to set a discretionary ACL into a security descriptor
        /// </summary>
        CO_E_FAILEDTOSETDACL = VSConstants.CO_E_FAILEDTOSETDACL,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the system function, AccessCheck, returned false
        /// </summary>
        CO_E_ACCESSCHECKFAILED = VSConstants.CO_E_ACCESSCHECKFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for either NetAccessDel or NetAccessAdd returned an error code.
        /// </summary>
        CO_E_NETACCESSAPIFAILED = VSConstants.CO_E_NETACCESSAPIFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for one of the trustee strings provided by the user did not conform to the <see
        /// cref="NTAccount"/> syntax and it was not the * string
        /// </summary>
        CO_E_WRONGTRUSTEENAMESYNTAX = unchecked((int)(0x8001012C)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for one of the security identifiers provided by the user was invalid
        /// </summary>
        CO_E_INVALIDSID = VSConstants.CO_E_INVALIDSID,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to convert a wide character trustee string to a multi-byte trustee string
        /// </summary>
        CO_E_CONVERSIONFAILED = VSConstants.CO_E_CONVERSIONFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to find a security identifier that corresponds to a trustee string provided
        /// by the user
        /// </summary>
        CO_E_NOMATCHINGSIDFOUND = VSConstants.CO_E_NOMATCHINGSIDFOUND,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the system function, LookupAccountSID, failed
        /// </summary>
        CO_E_LOOKUPACCSIDFAILED = VSConstants.CO_E_LOOKUPACCSIDFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to find a trustee name that corresponds to a security identifier provided by
        /// the user
        /// </summary>
        CO_E_NOMATCHINGNAMEFOUND = VSConstants.CO_E_NOMATCHINGNAMEFOUND,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the system function, LookupAccountName, failed
        /// </summary>
        CO_E_LOOKUPACCNAMEFAILED = VSConstants.CO_E_LOOKUPACCNAMEFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to set or reset a serialization handle
        /// </summary>
        CO_E_SETSERLHNDLFAILED = VSConstants.CO_E_SETSERLHNDLFAILED,

        /// <summary>
        /// <c>COM</c> Error unable to obtain the Windows directory
        /// </summary>
        CO_E_FAILEDTOGETWINDIR = VSConstants.CO_E_FAILEDTOGETWINDIR,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for path too long
        /// </summary>
        CO_E_PATHTOOLONG = VSConstants.CO_E_PATHTOOLONG,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to generate a uuid or <see cref="Guid"/>.
        /// </summary>
        CO_E_FAILEDTOGENUUID = VSConstants.CO_E_FAILEDTOGENUUID,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to create file
        /// </summary>
        CO_E_FAILEDTOCREATEFILE = VSConstants.CO_E_FAILEDTOCREATEFILE,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to close a serialization handle or a file handle.
        /// </summary>
        CO_E_FAILEDTOCLOSEHANDLE = VSConstants.CO_E_FAILEDTOCLOSEHANDLE,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the number of ACEs in an ACL exceeds the system limit.
        /// </summary>
        CO_E_EXCEEDSYSACLLIMIT = VSConstants.CO_E_EXCEEDSYSACLLIMIT,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for not all the DENY_ACCESS ACEs are arranged in front of the GRANT_ACCESS ACEs in the stream.
        /// </summary>
        CO_E_ACESINWRONGORDER = VSConstants.CO_E_ACESINWRONGORDER,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the version of ACL format in the stream is not supported by this implementation of IAccessControl
        /// </summary>
        CO_E_INCOMPATIBLESTREAMVERSION = VSConstants.CO_E_INCOMPATIBLESTREAMVERSION,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to open the access token of the server process
        /// </summary>
        CO_E_FAILEDTOOPENPROCESSTOKEN = VSConstants.CO_E_FAILEDTOOPENPROCESSTOKEN,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for unable to decode the ACL in the stream provided by the user
        /// </summary>
        CO_E_DECODEFAILED = VSConstants.CO_E_DECODEFAILED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for the <c>COM</c> IAccessControl object is not initialized
        /// </summary>
        CO_E_ACNOTINITIALIZED = VSConstants.CO_E_ACNOTINITIALIZED,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for call Cancellation is disabled
        /// </summary>
        CO_E_CANCEL_DISABLED = VSConstants.CO_E_CANCEL_DISABLED,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for unknown interface.
        /// </summary>
        DISP_E_UNKNOWNINTERFACE = VSConstants.DISP_E_UNKNOWNINTERFACE,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for member not found.
        /// </summary>
        DISP_E_MEMBERNOTFOUND = VSConstants.DISP_E_MEMBERNOTFOUND,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for parameter not found.
        /// </summary>
        DISP_E_PARAMNOTFOUND = VSConstants.DISP_E_PARAMNOTFOUND,

        /// <summary>
        /// Type mismatch.
        /// </summary>
        DISP_E_TYPEMISMATCH = VSConstants.DISP_E_TYPEMISMATCH,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for unknown name.
        /// </summary>
        DISP_E_UNKNOWNNAME = VSConstants.DISP_E_UNKNOWNNAME,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for no named arguments.
        /// </summary>
        DISP_E_NONAMEDARGS = VSConstants.DISP_E_NONAMEDARGS,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for bad variable type.
        /// </summary>
        DISP_E_BADVARTYPE = VSConstants.DISP_E_BADVARTYPE,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for <see cref="Exception"/> occurred.
        /// </summary>
        DISP_E_EXCEPTION = VSConstants.DISP_E_EXCEPTION,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for out of present range.
        /// </summary>
        DISP_E_OVERFLOW = VSConstants.DISP_E_OVERFLOW,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for invalid index.
        /// </summary>
        DISP_E_BADINDEX = VSConstants.DISP_E_BADINDEX,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for unknown language.
        /// </summary>
        DISP_E_UNKNOWNLCID = VSConstants.DISP_E_UNKNOWNLCID,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for memory is locked.
        /// </summary>
        DISP_E_ARRAYISLOCKED = VSConstants.DISP_E_ARRAYISLOCKED,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for invalid number of parameters.
        /// </summary>
        DISP_E_BADPARAMCOUNT = VSConstants.DISP_E_BADPARAMCOUNT,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for parameter not optional.
        /// </summary>
        DISP_E_PARAMNOTOPTIONAL = VSConstants.DISP_E_PARAMNOTOPTIONAL,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for invalid callee.
        /// </summary>
        DISP_E_BADCALLEE = VSConstants.DISP_E_BADCALLEE,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for does not support a collection.
        /// </summary>
        DISP_E_NOTACOLLECTION = VSConstants.DISP_E_NOTACOLLECTION,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for division by zero.
        /// </summary>
        DISP_E_DIVBYZERO = VSConstants.DISP_E_DIVBYZERO,

        /// <summary>
        /// <c>IDispatch</c><c>COM</c> Error <c>HRESULT</c> for buffer too small
        /// </summary>
        DISP_E_BUFFERTOOSMALL = VSConstants.DISP_E_BUFFERTOOSMALL,

        /// <summary>
        /// Trying to revoke a drop target that has not been registered
        /// </summary>
        DRAGDROP_E_FIRST = DRAGDROP_E_NOTREGISTERED,

        /// <summary>
        /// Trying to revoke a drop target that has not been registered
        /// </summary>
        DRAGDROP_E_NOTREGISTERED = unchecked((int)(0x80040100)),

        /// <summary>
        /// This window has already been registered as a drop target
        /// </summary>
        DRAGDROP_E_ALREADYREGISTERED = unchecked((int)(0x80040101)),

        /// <summary>
        /// Invalid window handle
        /// </summary>
        DRAGDROP_E_INVALIDHWND = unchecked((int)(0x80040102)),

        /// <summary>
        /// No information available.
        /// </summary>
        DRAGDROP_E_LAST = unchecked((int)(0x8004010F)),

        /// <summary>
        /// Invalid FORMATETC structure
        /// </summary>
        DV_E_FORMATETC = unchecked((int)(0x80040064)),

        /// <summary>
        /// Invalid DVTARGETDEVICE structure
        /// </summary>
        DV_E_DVTARGETDEVICE = unchecked((int)(0x80040065)),

        /// <summary>
        /// Invalid STDGMEDIUM structure
        /// </summary>
        DV_E_STGMEDIUM = unchecked((int)(0x80040066)),

        /// <summary>
        /// Invalid STATDATA structure
        /// </summary>
        DV_E_STATDATA = unchecked((int)(0x80040067)),

        /// <summary>
        /// Invalid lindex
        /// </summary>
        DV_E_LINDEX = unchecked((int)(0x80040068)),

        /// <summary>
        /// Invalid tymed
        /// </summary>
        DV_E_TYMED = unchecked((int)(0x80040069)),

        /// <summary>
        /// Invalid clipboard format
        /// </summary>
        DV_E_CLIPFORMAT = unchecked((int)(0x8004006A)),

        /// <summary>
        /// Invalid aspect(s)
        /// </summary>
        DV_E_DVASPECT = unchecked((int)(0x8004006B)),

        /// <summary>
        /// tdSize parameter of the DVTARGETDEVICE structure is invalid
        /// </summary>
        DV_E_DVTARGETDEVICE_SIZE = unchecked((int)(0x8004006C)),

        /// <summary>
        /// Object doesn't support IViewObject interface
        /// </summary>
        DV_E_NOIVIEWOBJECT = unchecked((int)(0x8004006D)),

        /// <summary>
        /// Error <c>HRESULT</c> for the data necessary to complete this operation is not yet available.
        /// </summary>
        E_PENDING = VSConstants.E_PENDING,

        /// <summary>
        /// Error <c>HRESULT</c> for a call to a not implemented method.
        /// </summary>
        E_NOTIMPL = VSConstants.E_NOTIMPL,

        /// <summary>
        /// Error <c>HRESULT</c> for the request of a not-implemented interface.
        /// </summary>
        E_NOINTERFACE = VSConstants.E_NOINTERFACE,

        /// <summary>
        /// Error <c>HRESULT</c> for invalid pointer
        /// </summary>
        E_POINTER = VSConstants.E_POINTER,

        /// <summary>
        /// Error <c>HRESULT</c> for operation aborted
        /// </summary>
        E_ABORT = VSConstants.E_ABORT,

        /// <summary>
        /// Error <c>HRESULT</c> for unspecified error
        /// </summary>
        E_FAIL = VSConstants.E_FAIL,

        /// <summary>
        /// Error <c>HRESULT</c> for an unexpected condition.
        /// </summary>
        E_UNEXPECTED = VSConstants.E_UNEXPECTED,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for invalid <c>OLEVERB</c> structure
        /// </summary>
        OLE_E_FIRST = OLE_E_OLEVERB,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for invalid <c>OLEVERB</c> structure
        /// </summary>
        OLE_E_OLEVERB = VSConstants.OLE_E_OLEVERB,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for invalid advise flags
        /// </summary>
        OLE_E_ADVF = VSConstants.OLE_E_ADVF,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for cannot enumerate any more, because the associated data is missing
        /// </summary>
        OLE_E_ENUM_NOMORE = VSConstants.OLE_E_ENUM_NOMORE,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for this implementation doesn't take advises
        /// </summary>
        OLE_E_ADVISENOTSUPPORTED = VSConstants.OLE_E_ADVISENOTSUPPORTED,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for there is no connection for this connection ID
        /// </summary>
        OLE_E_NOCONNECTION = VSConstants.OLE_E_NOCONNECTION,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for need to run the object to perform this operation
        /// </summary>
        OLE_E_NOTRUNNING = VSConstants.OLE_E_NOTRUNNING,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for there is no cache to operate on
        /// </summary>
        OLE_E_NOCACHE = VSConstants.OLE_E_NOCACHE,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for uninitialized object
        /// </summary>
        OLE_E_BLANK = VSConstants.OLE_E_BLANK,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for linked object's source class has changed
        /// </summary>
        OLE_E_CLASSDIFF = VSConstants.OLE_E_CLASSDIFF,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for not able to get the moniker of the object
        /// </summary>
        OLE_E_CANT_GETMONIKER = VSConstants.OLE_E_CANT_GETMONIKER,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for not able to bind to the source
        /// </summary>
        OLE_E_CANT_BINDTOSOURCE = VSConstants.OLE_E_CANT_BINDTOSOURCE,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for object is static, operation not allowed
        /// </summary>
        OLE_E_STATIC = VSConstants.OLE_E_STATIC,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for user canceled out of save dialog
        /// </summary>
        OLE_E_PROMPTSAVECANCELLED = VSConstants.OLE_E_PROMPTSAVECANCELLED,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for invalid rectangle
        /// </summary>
        OLE_E_INVALIDRECT = VSConstants.OLE_E_INVALIDRECT,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for compobj.dll is too old for the initializing ole2.dll
        /// </summary>
        OLE_E_WRONGCOMPOBJ = VSConstants.OLE_E_WRONGCOMPOBJ,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for invalid window handle
        /// </summary>
        OLE_E_INVALIDHWND = VSConstants.OLE_E_INVALIDHWND,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for object is not in any of the inplace active states
        /// </summary>
        OLE_E_NOT_INPLACEACTIVE = VSConstants.OLE_E_NOT_INPLACEACTIVE,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for not able to convert object
        /// </summary>
        OLE_E_CANTCONVERT = VSConstants.OLE_E_CANTCONVERT,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for not able to perform the operation because object is not given storage yet
        /// </summary>
        OLE_E_NOSTORAGE = VSConstants.OLE_E_NOSTORAGE,

        /// <summary>
        /// <c>OLE</c> Error <c>HRESULT</c> for no information available.
        /// </summary>
        OLE_E_LAST = unchecked((int)(0x800400FF)),

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for call was rejected by callee.
        /// </summary>
        RPC_E_CALL_REJECTED = VSConstants.RPC_E_CALL_REJECTED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for call was canceled by the message filter.
        /// </summary>
        RPC_E_CALL_CANCELED = VSConstants.RPC_E_CALL_CANCELED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the caller is dispatching an inter-task <c>SendMessage</c> call and cannot call out
        /// via <c>PostMessage</c>.
        /// </summary>
        RPC_E_CANTPOST_INSENDCALL = VSConstants.RPC_E_CANTPOST_INSENDCALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the caller is dispatching an asynchronous call and cannot make an outgoing call on
        /// behalf of this call.
        /// </summary>
        RPC_E_CANTCALLOUT_INASYNCCALL = VSConstants.RPC_E_CANTCALLOUT_INASYNCCALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for it is illegal to call out while inside message filter.
        /// </summary>
        RPC_E_CANTCALLOUT_INEXTERNALCALL = VSConstants.RPC_E_CANTCALLOUT_INEXTERNALCALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the connection terminated or is in a bogus state and cannot be used any more. Other
        /// connections are still valid.
        /// </summary>
        RPC_E_CONNECTION_TERMINATED = VSConstants.RPC_E_CONNECTION_TERMINATED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the callee (server [not server application]) is not available and disappeared, all
        /// connections are invalid. The call may have executed.
        /// </summary>
        RPC_E_SERVER_DIED = VSConstants.RPC_E_SERVER_DIED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the caller (client) disappeared while the callee (server) was processing a call.
        /// </summary>
        RPC_E_CLIENT_DIED = VSConstants.RPC_E_CLIENT_DIED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the data packet with the marshaled parameter data is incorrect.
        /// </summary>
        RPC_E_INVALID_DATAPACKET = VSConstants.RPC_E_INVALID_DATAPACKET,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the call was not transmitted properly, the message queue was full and was not
        /// emptied after yielding.
        /// </summary>
        RPC_E_CANTTRANSMIT_CALL = VSConstants.RPC_E_CANTTRANSMIT_CALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the client (caller) cannot marshal the parameter data - low memory, etc.
        /// </summary>
        RPC_E_CLIENT_CANTMARSHAL_DATA = VSConstants.RPC_E_CLIENT_CANTMARSHAL_DATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the client (caller) cannot unmarshal the return data - low memory, etc.
        /// </summary>
        RPC_E_CLIENT_CANTUNMARSHAL_DATA = VSConstants.RPC_E_CLIENT_CANTUNMARSHAL_DATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the server (callee) cannot marshal the return data - low memory, etc.
        /// </summary>
        RPC_E_SERVER_CANTMARSHAL_DATA = VSConstants.RPC_E_SERVER_CANTMARSHAL_DATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the server (callee) cannot unmarshal the parameter data - low memory, etc.
        /// </summary>
        RPC_E_SERVER_CANTUNMARSHAL_DATA = VSConstants.RPC_E_SERVER_CANTUNMARSHAL_DATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for received data is invalid, could be server or client data.
        /// </summary>
        RPC_E_INVALID_DATA = VSConstants.RPC_E_INVALID_DATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for a particular parameter is invalid and cannot be (un)marshaled.
        /// </summary>
        RPC_E_INVALID_PARAMETER = VSConstants.RPC_E_INVALID_PARAMETER,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for there is no second outgoing call on same channel in DDE conversation.
        /// </summary>
        RPC_E_CANTCALLOUT_AGAIN = VSConstants.RPC_E_CANTCALLOUT_AGAIN,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the callee (server [not server application]) is not available and disappeared, all
        /// connections are invalid. The call did not execute.
        /// </summary>
        RPC_E_SERVER_DIED_DNE = VSConstants.RPC_E_SERVER_DIED_DNE,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for system call failed.
        /// </summary>
        RPC_E_SYS_CALL_FAILED = VSConstants.RPC_E_SYS_CALL_FAILED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for could not allocate some required resource (memory, events, ...)
        /// </summary>
        RPC_E_OUT_OF_RESOURCES = VSConstants.RPC_E_OUT_OF_RESOURCES,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for attempted to make calls on more than one thread in single threaded mode.
        /// </summary>
        RPC_E_ATTEMPTED_MULTITHREAD = VSConstants.RPC_E_ATTEMPTED_MULTITHREAD,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the requested interface is not registered on the server object.
        /// </summary>
        RPC_E_NOT_REGISTERED = VSConstants.RPC_E_NOT_REGISTERED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>RPC</c> could not call the server or could not return the results of calling the server.
        /// </summary>
        RPC_E_FAULT = VSConstants.RPC_E_FAULT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the server threw an <see cref="Exception"/>.
        /// </summary>
        RPC_E_SERVERFAULT = VSConstants.RPC_E_SERVERFAULT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for cannot change thread mode after it is set.
        /// </summary>
        RPC_E_CHANGED_MODE = VSConstants.RPC_E_CHANGED_MODE,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the method called does not exist on the server.
        /// </summary>
        RPC_E_INVALIDMETHOD = VSConstants.RPC_E_INVALIDMETHOD,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the object invoked has disconnected from its clients.
        /// </summary>
        RPC_E_DISCONNECTED = VSConstants.RPC_E_DISCONNECTED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the object invoked chose not to process the call now. Try again later.
        /// </summary>
        RPC_E_RETRY = VSConstants.RPC_E_RETRY,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the message filter indicated that the application is busy.
        /// </summary>
        RPC_E_SERVERCALL_RETRYLATER = VSConstants.RPC_E_SERVERCALL_RETRYLATER,

        /// <summary>
        /// The message filter rejected the call.
        /// </summary>
        RPC_E_SERVERCALL_REJECTED = VSConstants.RPC_E_SERVERCALL_REJECTED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for a call control interfaces was called with invalid data.
        /// </summary>
        RPC_E_INVALID_CALLDATA = VSConstants.RPC_E_INVALID_CALLDATA,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for an outgoing call cannot be made since the application is dispatching an
        /// input-synchronous call.
        /// </summary>
        RPC_E_CANTCALLOUT_ININPUTSYNCCALL = VSConstants.RPC_E_CANTCALLOUT_ININPUTSYNCCALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the application called an interface that was marshaled for a different thread.
        /// </summary>
        RPC_E_WRONG_THREAD = VSConstants.RPC_E_WRONG_THREAD,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>CoInitialize</c> has not been called on the current thread.
        /// </summary>
        RPC_E_THREAD_NOT_INIT = VSConstants.RPC_E_THREAD_NOT_INIT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the version of <c>OLE</c> on the client and server machines does not match.
        /// </summary>
        RPC_E_VERSION_MISMATCH = VSConstants.RPC_E_VERSION_MISMATCH,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>OLE</c> received a packet with an invalid header.
        /// </summary>
        RPC_E_INVALID_HEADER = VSConstants.RPC_E_INVALID_HEADER,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>OLE</c> received a packet with an invalid extension.
        /// </summary>
        RPC_E_INVALID_EXTENSION = VSConstants.RPC_E_INVALID_EXTENSION,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the requested object or interface does not exist.
        /// </summary>
        RPC_E_INVALID_IPID = VSConstants.RPC_E_INVALID_IPID,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for call context cannot be accessed after call completed.
        /// </summary>
        RPC_E_CALL_COMPLETE = VSConstants.RPC_E_CALL_COMPLETE,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for impersonate on unsecured calls is not supported.
        /// </summary>
        RPC_E_UNSECURE_CALL = VSConstants.RPC_E_UNSECURE_CALL,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for security must be initialized before any interfaces are marshaled or unmarshaled. It
        /// cannot be changed once initialized.
        /// </summary>
        RPC_E_TOO_LATE = VSConstants.RPC_E_TOO_LATE,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for no security packages are installed on this machine or the user is not logged on or
        /// there are no compatible security packages between the client and server.
        /// </summary>
        RPC_E_NO_GOOD_SECURITY_PACKAGES = VSConstants.RPC_E_NO_GOOD_SECURITY_PACKAGES,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for access is denied.
        /// </summary>
        RPC_E_ACCESS_DENIED = VSConstants.RPC_E_ACCESS_DENIED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for remote calls are not allowed for this process.
        /// </summary>
        RPC_E_REMOTE_DISABLED = VSConstants.RPC_E_REMOTE_DISABLED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the marshaled interface data packet (OBJREF) has an invalid or unknown format.
        /// </summary>
        RPC_E_INVALID_OBJREF = VSConstants.RPC_E_INVALID_OBJREF,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for no context is associated with this call. This happens for some custom marshaled
        /// calls and on the client side of the call.
        /// </summary>
        RPC_E_NO_CONTEXT = VSConstants.RPC_E_NO_CONTEXT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for this operation returned because the timeout period expired.
        /// </summary>
        RPC_E_TIMEOUT = VSConstants.RPC_E_TIMEOUT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for there are no synchronize objects to wait on.
        /// </summary>
        RPC_E_NO_SYNC = VSConstants.RPC_E_NO_SYNC,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for full subject issuer chain SSL principal name expected from the server.
        /// </summary>
        RPC_E_FULLSIC_REQUIRED = VSConstants.RPC_E_FULLSIC_REQUIRED,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for principal name is not a valid MSSTD name.
        /// </summary>
        RPC_E_INVALID_STD_NAME = VSConstants.RPC_E_INVALID_STD_NAME,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for the requested object does not exist.
        /// </summary>
        RPC_E_INVALID_OBJECT = VSConstants.RPC_E_INVALID_OBJECT,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>OLE</c> has sent a request and is waiting for a reply.
        /// </summary>
        RPC_S_CALLPENDING = VSConstants.RPC_S_CALLPENDING,

        /// <summary>
        /// <c>RPC</c> Error <c>HRESULT</c> for <c>OLE</c> is waiting before retrying a request.
        /// </summary>
        RPC_S_WAITONTIMER = VSConstants.RPC_S_WAITONTIMER,

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for unable to perform requested operation.
        /// </summary>
        STG_E_INVALIDFUNCTION = unchecked((int)(0x80030001)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for %1 could not be found.
        /// </summary>
        STG_E_FILENOTFOUND = unchecked((int)(0x80030002)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the path %1 could not be found.
        /// </summary>
        STG_E_PATHNOTFOUND = unchecked((int)(0x80030003)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for there are insufficient resources to open another file.
        /// </summary>
        STG_E_TOOMANYOPENFILES = unchecked((int)(0x80030004)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for access Denied.
        /// </summary>
        STG_E_ACCESSDENIED = unchecked((int)(0x80030005)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for attempted an operation on an invalid object.
        /// </summary>
        STG_E_INVALIDHANDLE = unchecked((int)(0x80030006)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for there is insufficient memory available to complete operation.
        /// </summary>
        STG_E_INSUFFICIENTMEMORY = unchecked((int)(0x80030008)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for invalid pointer error.
        /// </summary>
        STG_E_INVALIDPOINTER = unchecked((int)(0x80030009)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for there are no more entries to return.
        /// </summary>
        STG_E_NOMOREFILES = unchecked((int)(0x80030012)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for disk is write-protected.
        /// </summary>
        STG_E_DISKISWRITEPROTECTED = unchecked((int)(0x80030013)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for an error occurred during a seek operation.
        /// </summary>
        STG_E_SEEKERROR = unchecked((int)(0x80030019)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for a disk error occurred during a write operation.
        /// </summary>
        STG_E_WRITEFAULT = unchecked((int)(0x8003001D)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for a disk error occurred during a read operation.
        /// </summary>
        STG_E_READFAULT = unchecked((int)(0x8003001E)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for a share violation has occurred.
        /// </summary>
        STG_E_SHAREVIOLATION = unchecked((int)(0x80030020)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for a lock violation has occurred.
        /// </summary>
        STG_E_LOCKVIOLATION = unchecked((int)(0x80030021)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for %1 already exists.
        /// </summary>
        STG_E_FILEALREADYEXISTS = unchecked((int)(0x80030050)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for invalid parameter error.
        /// </summary>
        STG_E_INVALIDPARAMETER = unchecked((int)(0x80030057)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for there is insufficient disk space to complete operation.
        /// </summary>
        STG_E_MEDIUMFULL = unchecked((int)(0x80030070)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for illegal write of non-simple property to simple property set.
        /// </summary>
        STG_E_PROPSETMISMATCHED = unchecked((int)(0x800300F0)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for an API call exited abnormally.
        /// </summary>
        STG_E_ABNORMALAPIEXIT = unchecked((int)(0x800300FA)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the file %1 is not a valid compound file.
        /// </summary>
        STG_E_INVALIDHEADER = unchecked((int)(0x800300FB)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the name %1 is not valid.
        /// </summary>
        STG_E_INVALIDNAME = unchecked((int)(0x800300FC)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for an unexpected error occurred.
        /// </summary>
        STG_E_UNKNOWN = unchecked((int)(0x800300FD)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for that function is not implemented.
        /// </summary>
        STG_E_UNIMPLEMENTEDFUNCTION = unchecked((int)(0x800300FE)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for invalid flag error.
        /// </summary>
        STG_E_INVALIDFLAG = unchecked((int)(0x800300FF)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for attempted to use an object that is busy.
        /// </summary>
        STG_E_INUSE = unchecked((int)(0x80030100)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the storage has been changed since the last commit.
        /// </summary>
        STG_E_NOTCURRENT = unchecked((int)(0x80030101)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for attempted to use an object that has ceased to exist.
        /// </summary>
        STG_E_REVERTED = unchecked((int)(0x80030102)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for cannot save.
        /// </summary>
        STG_E_CANTSAVE = unchecked((int)(0x80030103)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the compound file %1 was produced with an incompatible version of storage.
        /// </summary>
        STG_E_OLDFORMAT = unchecked((int)(0x80030104)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the compound file %1 was produced with a newer version of storage.
        /// </summary>
        STG_E_OLDDLL = unchecked((int)(0x80030105)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for <c>share.exe</c> or equivalent is required for operation.
        /// </summary>
        STG_E_SHAREREQUIRED = unchecked((int)(0x80030106)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for illegal operation called on non-file based storage.
        /// </summary>
        STG_E_NOTFILEBASEDSTORAGE = unchecked((int)(0x80030107)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for illegal operation called on object with extant marshalings.
        /// </summary>
        STG_E_EXTANTMARSHALLINGS = unchecked((int)(0x80030108)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the docfile has been corrupted.
        /// </summary>
        STG_E_DOCFILECORRUPT = unchecked((int)(0x80030109)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for <c>OLE32.DLL</c> has been loaded at the wrong address.
        /// </summary>
        STG_E_BADBASEADDRESS = unchecked((int)(0x80030110)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the compound file is too large for the current implementation
        /// </summary>
        STG_E_DOCFILETOOLARGE = unchecked((int)(0x80030111)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the compound file was not created with the STGM_SIMPLE flag
        /// </summary>
        STG_E_NOTSIMPLEFORMAT = unchecked((int)(0x80030112)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the file download was aborted abnormally. The file is incomplete.
        /// </summary>
        STG_E_INCOMPLETE = unchecked((int)(0x80030201)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for the file download has been terminated.
        /// </summary>
        STG_E_TERMINATED = unchecked((int)(0x80030202)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for generic Copy Protection Error.
        /// </summary>
        STG_E_STATUS_COPY_PROTECTION_FAILURE = unchecked((int)(0x80030305)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - DVD CSS Authentication failed.
        /// </summary>
        STG_E_CSS_AUTHENTICATION_FAILURE = unchecked((int)(0x80030306)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - The given sector does not have a valid CSS key.
        /// </summary>
        STG_E_CSS_KEY_NOT_PRESENT = unchecked((int)(0x80030307)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - DVD session key not established.
        /// </summary>
        STG_E_CSS_KEY_NOT_ESTABLISHED = unchecked((int)(0x80030308)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - The read failed because the sector is encrypted.
        /// </summary>
        STG_E_CSS_SCRAMBLED_SECTOR = unchecked((int)(0x80030309)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - The current DVD's region does not correspond
        /// to the region setting of the drive.
        /// </summary>
        STG_E_CSS_REGION_MISMATCH = unchecked((int)(0x8003030A)),

        /// <summary>
        /// <c>IStorage</c><c>COM</c> Error <c>HRESULT</c> for Copy Protection Error - The drive's region setting may be permanent
        /// or the number of user resets has been exhausted.
        /// </summary>
        STG_E_RESETS_EXHAUSTED = unchecked((int)(0x8003030B)),

        /// <summary>
        /// Error <c>HRESULT</c> for buffer too small.
        /// </summary>
        TYPE_E_BUFFERTOOSMALL = unchecked((int)(0x80028016)),

        /// <summary>
        /// Error <c>HRESULT</c> for field name not defined in the record.
        /// </summary>
        TYPE_E_FIELDNOTFOUND = unchecked((int)(0x80028017)),

        /// <summary>
        /// Old format or invalid type library.
        /// </summary>
        TYPE_E_INVDATAREAD = unchecked((int)(0x80028018)),

        /// <summary>
        /// Error <c>HRESULT</c> for old format or invalid type library.
        /// </summary>
        TYPE_E_UNSUPFORMAT = unchecked((int)(0x80028019)),

        /// <summary>
        /// Error <c>HRESULT</c> for error accessing the OLE registry.
        /// </summary>
        TYPE_E_REGISTRYACCESS = unchecked((int)(0x8002801C)),

        /// <summary>
        /// Error <c>HRESULT</c> for library not registered.
        /// </summary>
        TYPE_E_LIBNOTREGISTERED = unchecked((int)(0x8002801D)),

        /// <summary>
        /// Error <c>HRESULT</c> for bound to unknown type.
        /// </summary>
        TYPE_E_UNDEFINEDTYPE = unchecked((int)(0x80028027)),

        /// <summary>
        /// Error <c>HRESULT</c> for qualified name disallowed.
        /// </summary>
        TYPE_E_QUALIFIEDNAMEDISALLOWED = unchecked((int)(0x80028028)),

        /// <summary>
        /// Error <c>HRESULT</c> for invalid forward reference, or reference to uncompiled type.
        /// </summary>
        TYPE_E_INVALIDSTATE = unchecked((int)(0x80028029)),

        /// <summary>
        /// Error <c>HRESULT</c> for type mismatch.
        /// </summary>
        TYPE_E_WRONGTYPEKIND = unchecked((int)(0x8002802A)),

        /// <summary>
        /// Error <c>HRESULT</c> for element not found.
        /// </summary>
        TYPE_E_ELEMENTNOTFOUND = unchecked((int)(0x8002802B)),

        /// <summary>
        /// Error <c>HRESULT</c> for ambiguous name.
        /// </summary>
        TYPE_E_AMBIGUOUSNAME = unchecked((int)(0x8002802C)),

        /// <summary>
        /// Error <c>HRESULT</c> for name already exists in the library.
        /// </summary>
        TYPE_E_NAMECONFLICT = unchecked((int)(0x8002802D)),

        /// <summary>
        /// Error <c>HRESULT</c> for unknown LCID.
        /// </summary>
        TYPE_E_UNKNOWNLCID = unchecked((int)(0x8002802E)),

        /// <summary>
        /// Error <c>HRESULT</c> for function not defined in specified DLL.
        /// </summary>
        TYPE_E_DLLFUNCTIONNOTFOUND = unchecked((int)(0x8002802F)),

        /// <summary>
        /// Error <c>HRESULT</c> for wrong module kind for the operation.
        /// </summary>
        TYPE_E_BADMODULEKIND = unchecked((int)(0x800288BD)),

        /// <summary>
        /// Error <c>HRESULT</c> for size may not exceed 64K.
        /// </summary>
        TYPE_E_SIZETOOBIG = unchecked((int)(0x800288C5)),

        /// <summary>
        /// Error <c>HRESULT</c> for duplicate ID in inheritance hierarchy.
        /// </summary>
        TYPE_E_DUPLICATEID = unchecked((int)(0x800288C6)),

        /// <summary>
        /// Error <c>HRESULT</c> for incorrect inheritance depth in standard OLE hmember.
        /// </summary>
        TYPE_E_INVALIDID = unchecked((int)(0x800288CF)),

        /// <summary>
        /// Error <c>HRESULT</c> for type mismatch.
        /// </summary>
        TYPE_E_TYPEMISMATCH = unchecked((int)(0x80028CA0)),

        /// <summary>
        /// Error <c>HRESULT</c> for invalid number of arguments.
        /// </summary>
        TYPE_E_OUTOFBOUNDS = unchecked((int)(0x80028CA1)),

        /// <summary>
        /// Error <c>HRESULT</c> for I/O Error.
        /// </summary>
        TYPE_E_IOERROR = unchecked((int)(0x80028CA2)),

        /// <summary>
        /// Error <c>HRESULT</c> for error creating unique tmp file.
        /// </summary>
        TYPE_E_CANTCREATETMPFILE = unchecked((int)(0x80028CA3)),

        /// <summary>
        /// Error <c>HRESULT</c> for error loading type library/DLL.
        /// </summary>
        TYPE_E_CANTLOADLIBRARY = unchecked((int)(0x80029C4A)),

        /// <summary>
        /// Error <c>HRESULT</c> for inconsistent property functions.
        /// </summary>
        TYPE_E_INCONSISTENTPROPFUNCS = unchecked((int)(0x80029C83)),

        /// <summary>
        /// Error <c>HRESULT</c> for circular dependency between types/modules.
        /// </summary>
        TYPE_E_CIRCULARTYPE = unchecked((int)(0x80029C84)),

        /// <summary>
        /// Error <c>HRESULT</c> for class does not support aggregation (or class object is remote)
        /// </summary>
        CLASSFACTORY_E_FIRST = CLASS_E_NOAGGREGATION,

        /// <summary>
        /// <c>DllGetClassObject</c> Error <c>HRESULT</c> for class does not support aggregation (or class object is remote)
        /// </summary>
        CLASS_E_NOAGGREGATION = unchecked((int)(0x80040110)),

        /// <summary>
        /// <c>DllGetClassObject</c> Error <c>HRESULT</c> for ClassFactory cannot supply requested class
        /// </summary>
        CLASS_E_CLASSNOTAVAILABLE = unchecked((int)(0x80040111)),

        /// <summary>
        /// <c>DllGetClassObject</c> Error <c>HRESULT</c> for class is not licensed for use
        /// </summary>
        CLASS_E_NOTLICENSED = unchecked((int)(0x80040112)),

        /// <summary>
        /// Error <c>HRESULT</c> no information available.
        /// </summary>
        CLASSFACTORY_E_LAST = unchecked((int)(0x8004011F)),

        /// <summary>
        /// Error <c>HRESULT</c> for no information available.
        /// </summary>
        MARSHAL_E_FIRST = unchecked((int)(0x80040120)),

        /// <summary>
        /// Error <c>HRESULT</c> for no information available.
        /// </summary>
        MARSHAL_E_LAST = unchecked((int)(0x8004012F)),

        /// <summary>
        /// Database Error <c>HRESULT</c> for no information available.
        /// </summary>
        DATA_E_FIRST = unchecked((int)(0x80040130)),

        /// <summary>
        /// Database Error <c>HRESULT</c> for no information available.
        /// </summary>
        DATA_E_LAST = unchecked((int)(0x8004013F)),

        /// <summary>
        /// <c>USER32</c> Error <c>HRESULT</c> for error drawing view
        /// </summary>
        VIEW_E_FIRST = VIEW_E_DRAW,

        /// <summary>
        /// <c>USER32</c> Error <c>HRESULT</c> for error drawing view
        /// </summary>
        VIEW_E_DRAW = unchecked((int)(0x80040140)),

        /// <summary>
        /// <c>USER32</c> Error <c>HRESULT</c> for no information available.
        /// </summary>
        VIEW_E_LAST = unchecked((int)(0x8004014F)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for could not read key from registry
        /// </summary>
        REGDB_E_FIRST = REGDB_E_READREGDB,

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for could not read key from registry
        /// </summary>
        REGDB_E_READREGDB = unchecked((int)(0x80040150)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for could not write key to registry
        /// </summary>
        REGDB_E_WRITEREGDB = unchecked((int)(0x80040151)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for could not find the key in the registry
        /// </summary>
        REGDB_E_KEYMISSING = unchecked((int)(0x80040152)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for invalid value for registry
        /// </summary>
        REGDB_E_INVALIDVALUE = unchecked((int)(0x80040153)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for class not registered
        /// </summary>
        REGDB_E_CLASSNOTREG = unchecked((int)(0x80040154)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for interface not registered
        /// </summary>
        REGDB_E_IIDNOTREG = unchecked((int)(0x80040155)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for threading model entry is not valid
        /// </summary>
        REGDB_E_BADTHREADINGMODEL = unchecked((int)(0x80040156)),

        /// <summary>
        /// Register Database Error <c>HRESULT</c> for no information available.
        /// </summary>
        REGDB_E_LAST = unchecked((int)(0x8004015F)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for no package in the software installation data in the Active Directory meets
        /// this criteria.
        /// </summary>
        CS_E_FIRST = CS_E_PACKAGE_NOTFOUND,

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for no package in the software installation data in the Active Directory meets
        /// this criteria.
        /// </summary>
        CS_E_PACKAGE_NOTFOUND = unchecked((int)(0x80040164)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for deleting this will break the referential integrity of the software
        /// installation data in the Active Directory.
        /// </summary>
        CS_E_NOT_DELETABLE = unchecked((int)(0x80040165)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the CLSID was not found in the software installation data in the Active Directory.
        /// </summary>
        CS_E_CLASS_NOTFOUND = unchecked((int)(0x80040166)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the software installation data in the Active Directory is corrupt.
        /// </summary>
        CS_E_INVALID_VERSION = unchecked((int)(0x80040167)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for there is no software installation data in the Active Directory.
        /// </summary>
        CS_E_NO_CLASSSTORE = unchecked((int)(0x80040168)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for there is no software installation data object in the Active Directory.
        /// </summary>
        CS_E_OBJECT_NOTFOUND = unchecked((int)(0x80040169)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the software installation data object in the Active Directory already exists.
        /// </summary>
        CS_E_OBJECT_ALREADY_EXISTS = unchecked((int)(0x8004016A)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the path to the software installation data in the Active Directory is not correct.
        /// </summary>
        CS_E_INVALID_PATH = unchecked((int)(0x8004016B)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for a network error interrupted the operation.
        /// </summary>
        CS_E_NETWORK_ERROR = unchecked((int)(0x8004016C)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the size of this object exceeds the maximum size set by the Administrator.
        /// </summary>
        CS_E_ADMIN_LIMIT_EXCEEDED = unchecked((int)(0x8004016D)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for the schema for the software installation data in the Active Directory does not
        /// match the required schema.
        /// </summary>
        CS_E_SCHEMA_MISMATCH = unchecked((int)(0x8004016E)),

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for an error occurred in the software installation data in the Active Directory.
        /// </summary>
        CS_E_LAST = CS_E_INTERNAL_ERROR,

        /// <summary>
        /// Active Directory Error <c>HRESULT</c> for an error occurred in the software installation data in the Active Directory.
        /// </summary>
        CS_E_INTERNAL_ERROR = unchecked((int)(0x8004016F)),

        /// <summary>
        /// Cache Error <c>HRESULT</c> for cache not updated
        /// </summary>
        CACHE_E_FIRST = CACHE_E_NOCACHE_UPDATED,

        /// <summary>
        /// Cache Error <c>HRESULT</c> for cache not updated
        /// </summary>
        CACHE_E_NOCACHE_UPDATED = unchecked((int)(0x80040170)),

        /// <summary>
        /// Cache Error <c>HRESULT</c> for no information available.
        /// </summary>
        CACHE_E_LAST = unchecked((int)(0x8004017F)),

        /// <summary>
        /// <c>OLE</c> Object Error <c>HRESULT</c> for no verbs for OLE object
        /// </summary>
        OLEOBJ_E_FIRST = OLEOBJ_E_NOVERBS,

        /// <summary>
        /// <c>OLE</c> Object Error <c>HRESULT</c> for no verbs for OLE object
        /// </summary>
        OLEOBJ_E_NOVERBS = unchecked((int)(0x80040180)),

        /// <summary>
        /// <c>OLE</c> Object Error <c>HRESULT</c> for invalid verb for OLE object
        /// </summary>
        OLEOBJ_E_INVALIDVERB = unchecked((int)(0x80040181)),

        /// <summary>
        /// <c>OLE</c> Object Error <c>HRESULT</c> for no information available.
        /// </summary>
        OLEOBJ_E_LAST = unchecked((int)(0x8004018F)),

        /// <summary>
        /// No information available.
        /// </summary>
        CLIENTSITE_E_FIRST = unchecked((int)(0x80040190)),

        /// <summary>
        /// No information available.
        /// </summary>
        CLIENTSITE_E_LAST = unchecked((int)(0x8004019F)),

        /// <summary>
        /// Undo is not available
        /// </summary>
        INPLACE_E_NOTUNDOABLE = unchecked((int)(0x800401A0)),

        /// <summary>
        /// Undo is not available
        /// </summary>
        INPLACE_E_FIRST = INPLACE_E_NOTUNDOABLE,

        /// <summary>
        /// Space for tools is not available
        /// </summary>
        INPLACE_E_NOTOOLSPACE = unchecked((int)(0x800401A1)),

        /// <summary>
        /// No information available.
        /// </summary>
        INPLACE_E_LAST = unchecked((int)(0x800401AF)),

        /// <summary>
        /// No information available.
        /// </summary>
        ENUM_E_FIRST = unchecked((int)(0x800401B0)),

        /// <summary>
        /// No information available.
        /// </summary>
        ENUM_E_LAST = unchecked((int)(0x800401BF)),

        /// <summary>
        /// OLESTREAM Get method failed
        /// </summary>
        CONVERT10_E_FIRST = CONVERT10_E_OLESTREAM_GET,

        /// <summary>
        /// OLESTREAM Get method failed
        /// </summary>
        CONVERT10_E_OLESTREAM_GET = unchecked((int)(0x800401C0)),

        /// <summary>
        /// OLESTREAM Put method failed
        /// </summary>
        CONVERT10_E_OLESTREAM_PUT = unchecked((int)(0x800401C1)),

        /// <summary>
        /// Contents of the OLESTREAM not in correct format
        /// </summary>
        CONVERT10_E_OLESTREAM_FMT = unchecked((int)(0x800401C2)),

        /// <summary>
        /// There was an error in a Windows GDI call while converting the bitmap to a DIB
        /// </summary>
        CONVERT10_E_OLESTREAM_BITMAP_TO_DIB = unchecked((int)(0x800401C3)),

        /// <summary>
        /// Contents of the IStorage not in correct format
        /// </summary>
        CONVERT10_E_STG_FMT = unchecked((int)(0x800401C4)),

        /// <summary>
        /// Contents of IStorage is missing one of the standard streams
        /// </summary>
        CONVERT10_E_STG_NO_STD_STREAM = unchecked((int)(0x800401C5)),

        /// <summary>
        /// There was an error in a Windows GDI call while converting the DIB to a bitmap.
        /// </summary>
        CONVERT10_E_STG_DIB_TO_BITMAP = unchecked((int)(0x800401C6)),

        /// <summary>
        /// No information available.
        /// </summary>
        CONVERT10_E_LAST = unchecked((int)(0x800401CF)),

        /// <summary>
        /// OpenClipboard Failed
        /// </summary>
        CLIPBRD_E_FIRST = CLIPBRD_E_CANT_OPEN,

        /// <summary>
        /// OpenClipboard Failed
        /// </summary>
        CLIPBRD_E_CANT_OPEN = unchecked((int)(0x800401D0)),

        /// <summary>
        /// EmptyClipboard Failed
        /// </summary>
        CLIPBRD_E_CANT_EMPTY = unchecked((int)(0x800401D1)),

        /// <summary>
        /// SetClipboard Failed
        /// </summary>
        CLIPBRD_E_CANT_SET = unchecked((int)(0x800401D2)),

        /// <summary>
        /// Data on clipboard is invalid
        /// </summary>
        CLIPBRD_E_BAD_DATA = unchecked((int)(0x800401D3)),

        /// <summary>
        /// CloseClipboard Failed
        /// </summary>
        CLIPBRD_E_CANT_CLOSE = unchecked((int)(0x800401D4)),

        /// <summary>
        /// No information available.
        /// </summary>
        CLIPBRD_E_LAST = unchecked((int)(0x800401DF)),

        /// <summary>
        /// Moniker needs to be connected manually
        /// </summary>
        MK_E_FIRST = MK_E_CONNECTMANUALLY,

        /// <summary>
        /// Moniker needs to be connected manually
        /// </summary>
        MK_E_CONNECTMANUALLY = unchecked((int)(0x800401E0)),

        /// <summary>
        /// Operation exceeded deadline
        /// </summary>
        MK_E_EXCEEDEDDEADLINE = unchecked((int)(0x800401E1)),

        /// <summary>
        /// Moniker needs to be generic
        /// </summary>
        MK_E_NEEDGENERIC = unchecked((int)(0x800401E2)),

        /// <summary>
        /// Operation unavailable
        /// </summary>
        MK_E_UNAVAILABLE = unchecked((int)(0x800401E3)),

        /// <summary>
        /// Invalid syntax
        /// </summary>
        MK_E_SYNTAX = unchecked((int)(0x800401E4)),

        /// <summary>
        /// No object for moniker
        /// </summary>
        MK_E_NOOBJECT = unchecked((int)(0x800401E5)),

        /// <summary>
        /// Bad extension for file
        /// </summary>
        MK_E_INVALIDEXTENSION = unchecked((int)(0x800401E6)),

        /// <summary>
        /// Intermediate operation failed
        /// </summary>
        MK_E_INTERMEDIATEINTERFACENOTSUPPORTED = unchecked((int)(0x800401E7)),

        /// <summary>
        /// Moniker is not bindable
        /// </summary>
        MK_E_NOTBINDABLE = unchecked((int)(0x800401E8)),

        /// <summary>
        /// Moniker is not bound
        /// </summary>
        MK_E_NOTBOUND = unchecked((int)(0x800401E9)),

        /// <summary>
        /// Moniker cannot open file
        /// </summary>
        MK_E_CANTOPENFILE = unchecked((int)(0x800401EA)),

        /// <summary>
        /// User input required for operation to succeed
        /// </summary>
        MK_E_MUSTBOTHERUSER = unchecked((int)(0x800401EB)),

        /// <summary>
        /// Moniker class has no inverse
        /// </summary>
        MK_E_NOINVERSE = unchecked((int)(0x800401EC)),

        /// <summary>
        /// Moniker does not refer to storage
        /// </summary>
        MK_E_NOSTORAGE = unchecked((int)(0x800401ED)),

        /// <summary>
        /// No common prefix
        /// </summary>
        MK_E_NOPREFIX = unchecked((int)(0x800401EE)),

        /// <summary>
        /// Moniker could not be enumerated
        /// </summary>
        MK_E_LAST = MK_E_ENUMERATION_FAILED,

        /// <summary>
        /// Moniker could not be enumerated
        /// </summary>
        MK_E_ENUMERATION_FAILED = unchecked((int)(0x800401EF)),

        /// <summary>
        /// CoInitialize has not been called.
        /// </summary>
        CO_E_FIRST = CO_E_NOTINITIALIZED,

        /// <summary>
        /// CoInitialize has not been called.
        /// </summary>
        CO_E_NOTINITIALIZED = unchecked((int)(0x800401F0)),

        /// <summary>
        /// CoInitialize has already been called.
        /// </summary>
        CO_E_ALREADYINITIALIZED = unchecked((int)(0x800401F1)),

        /// <summary>
        /// Class of object cannot be determined
        /// </summary>
        CO_E_CANTDETERMINECLASS = unchecked((int)(0x800401F2)),

        /// <summary>
        /// Invalid class string
        /// </summary>
        CO_E_CLASSSTRING = unchecked((int)(0x800401F3)),

        /// <summary>
        /// Invalid interface string
        /// </summary>
        CO_E_IIDSTRING = unchecked((int)(0x800401F4)),

        /// <summary>
        /// Application not found
        /// </summary>
        CO_E_APPNOTFOUND = unchecked((int)(0x800401F5)),

        /// <summary>
        /// Application cannot be run more than once
        /// </summary>
        CO_E_APPSINGLEUSE = unchecked((int)(0x800401F6)),

        /// <summary>
        /// Some error in application program
        /// </summary>
        CO_E_ERRORINAPP = unchecked((int)(0x800401F7)),

        /// <summary>
        /// DLL for class not found
        /// </summary>
        CO_E_DLLNOTFOUND = unchecked((int)(0x800401F8)),

        /// <summary>
        /// Error in the DLL
        /// </summary>
        CO_E_ERRORINDLL = unchecked((int)(0x800401F9)),

        /// <summary>
        /// Wrong OS or OS version for application
        /// </summary>
        CO_E_WRONGOSFORAPP = unchecked((int)(0x800401FA)),

        /// <summary>
        /// Object is not registered
        /// </summary>
        CO_E_OBJNOTREG = unchecked((int)(0x800401FB)),

        /// <summary>
        /// Object is already registered
        /// </summary>
        CO_E_OBJISREG = unchecked((int)(0x800401FC)),

        /// <summary>
        /// Object is not connected to server
        /// </summary>
        CO_E_OBJNOTCONNECTED = unchecked((int)(0x800401FD)),

        /// <summary>
        /// Application was launched but it didn't register a class factory
        /// </summary>
        CO_E_APPDIDNTREG = unchecked((int)(0x800401FE)),

        /// <summary>
        /// Object has been released
        /// </summary>
        CO_E_LAST = CO_E_RELEASED,

        /// <summary>
        /// Object has been released
        /// </summary>
        CO_E_RELEASED = unchecked((int)(0x800401FF)),

        /// <summary>
        /// No information available.
        /// </summary>
        EVENT_E_FIRST = VS_E_BUSY,

        VS_E_BUSY = VSConstants.VS_E_BUSY,

        /// <summary>
        /// An event was unable to invoke any of the subscribers
        /// </summary>
        EVENT_E_ALL_SUBSCRIBERS_FAILED = VS_E_SPECIFYING_OUTPUT_UNSUPPORTED,

        VS_E_SPECIFYING_OUTPUT_UNSUPPORTED = VSConstants.VS_E_SPECIFYING_OUTPUT_UNSUPPORTED,

        /// <summary>
        /// No information available.
        /// </summary>
        CLASSFACTORY_S_FIRST = 0x00040110,

        /// <summary>
        /// No information available.
        /// </summary>
        CLASSFACTORY_S_LAST = 0x0004011F,

        /// <summary>
        /// A syntax error occurred trying to evaluate a query string
        /// </summary>
        EVENT_E_QUERYSYNTAX = unchecked((int)(0x80040203)),

        /// <summary>
        /// An invalid field name was used in a query string
        /// </summary>
        EVENT_E_QUERYFIELD = unchecked((int)(0x80040204)),

        /// <summary>
        /// An unexpected exception was raised
        /// </summary>
        EVENT_E_INTERNALEXCEPTION = unchecked((int)(0x80040205)),

        /// <summary>
        /// An unexpected internal error was detected
        /// </summary>
        EVENT_E_INTERNALERROR = unchecked((int)(0x80040206)),

        /// <summary>
        /// The owner SID on a per-user subscription doesn't exist
        /// </summary>
        EVENT_E_INVALID_PER_USER_SID = unchecked((int)(0x80040207)),

        /// <summary>
        /// A user-supplied component or subscriber raised an exception
        /// </summary>
        EVENT_E_USER_EXCEPTION = unchecked((int)(0x80040208)),

        /// <summary>
        /// An interface has too many methods to fire events from
        /// </summary>
        EVENT_E_TOO_MANY_METHODS = unchecked((int)(0x80040209)),

        /// <summary>
        /// A subscription cannot be stored unless its event class already exists
        /// </summary>
        EVENT_E_MISSING_EVENTCLASS = unchecked((int)(0x8004020A)),

        /// <summary>
        /// Not all the objects requested could be removed
        /// </summary>
        EVENT_E_NOT_ALL_REMOVED = unchecked((int)(0x8004020B)),

        /// <summary>
        /// <c>COM</c>+ is required for this operation, but is not installed
        /// </summary>
        EVENT_E_COMPLUS_NOT_INSTALLED = unchecked((int)(0x8004020C)),

        /// <summary>
        /// Cannot modify or delete an object that was not added using the <c>COM</c>+ Admin SDK
        /// </summary>
        EVENT_E_CANT_MODIFY_OR_DELETE_UNCONFIGURED_OBJECT = unchecked((int)(0x8004020D)),

        /// <summary>
        /// Cannot modify or delete an object that was added using the <c>COM</c>+ Admin SDK
        /// </summary>
        EVENT_E_CANT_MODIFY_OR_DELETE_CONFIGURED_OBJECT = unchecked((int)(0x8004020E)),

        /// <summary>
        /// The event class for this subscription is in an invalid partition
        /// </summary>
        EVENT_E_INVALID_EVENT_CLASS_PARTITION = unchecked((int)(0x8004020F)),

        /// <summary>
        /// The owner of the PerUser subscription is not logged on to the system specified
        /// </summary>
        EVENT_E_PER_USER_SID_NOT_LOGGED_ON = unchecked((int)(0x80040210)),

        /// <summary>
        /// No information available.
        /// </summary>
        EVENT_E_LAST = unchecked((int)(0x8004021F)),

        /// <summary>
        /// Trigger not found.
        /// </summary>
        SCHED_E_TRIGGER_NOT_FOUND = unchecked((int)(0x80041309)),

        /// <summary>
        /// One or more of the properties that are needed to run this task have not been set.
        /// </summary>
        SCHED_E_TASK_NOT_READY = unchecked((int)(0x8004130A)),

        /// <summary>
        /// There is no running instance of the task to terminate.
        /// </summary>
        SCHED_E_TASK_NOT_RUNNING = unchecked((int)(0x8004130B)),

        /// <summary>
        /// The Task Scheduler Service is not installed on this computer.
        /// </summary>
        SCHED_E_SERVICE_NOT_INSTALLED = unchecked((int)(0x8004130C)),

        /// <summary>
        /// The task object could not be opened.
        /// </summary>
        SCHED_E_CANNOT_OPEN_TASK = unchecked((int)(0x8004130D)),

        /// <summary>
        /// The object is either an invalid task object or is not a task object.
        /// </summary>
        SCHED_E_INVALID_TASK = unchecked((int)(0x8004130E)),

        /// <summary>
        /// No account information could be found in the Task Scheduler security database for the task indicated.
        /// </summary>
        SCHED_E_ACCOUNT_INFORMATION_NOT_SET = unchecked((int)(0x8004130F)),

        /// <summary>
        /// Unable to establish existence of the account specified.
        /// </summary>
        SCHED_E_ACCOUNT_NAME_NOT_FOUND = unchecked((int)(0x80041310)),

        /// <summary>
        /// Corruption was detected in the Task Scheduler security database, the database has been reset.
        /// </summary>
        SCHED_E_ACCOUNT_DBASE_CORRUPT = unchecked((int)(0x80041311)),

        /// <summary>
        /// Task Scheduler security services are available only on Windows NT.
        /// </summary>
        SCHED_E_NO_SECURITY_SERVICES = unchecked((int)(0x80041312)),

        /// <summary>
        /// The task object version is either unsupported or invalid.
        /// </summary>
        SCHED_E_UNKNOWN_OBJECT_VERSION = unchecked((int)(0x80041313)),

        /// <summary>
        /// The task has been configured with an unsupported combination of account settings and run time options.
        /// </summary>
        SCHED_E_UNSUPPORTED_ACCOUNT_OPTION = unchecked((int)(0x80041314)),

        /// <summary>
        /// The Task Scheduler Service is not running.
        /// </summary>
        SCHED_E_SERVICE_NOT_RUNNING = unchecked((int)(0x80041315)),

        VS_E_PROJECTALREADYEXISTS = VSConstants.VS_E_PROJECTALREADYEXISTS,

        VS_E_PACKAGENOTLOADED = VSConstants.VS_E_PACKAGENOTLOADED,

        VS_E_PROJECTNOTLOADED = VSConstants.VS_E_PROJECTNOTLOADED,

        VS_E_SOLUTIONNOTOPEN = VSConstants.VS_E_SOLUTIONNOTOPEN,

        VS_E_SOLUTIONALREADYOPEN = VSConstants.VS_E_SOLUTIONALREADYOPEN,

        VS_E_PROJECTMIGRATIONFAILED = VSConstants.VS_E_PROJECTMIGRATIONFAILED,

        VS_E_INCOMPATIBLEDOCDATA = VSConstants.VS_E_INCOMPATIBLEDOCDATA,

        VS_E_UNSUPPORTEDFORMAT = VSConstants.VS_E_UNSUPPORTEDFORMAT,

        VS_E_WIZARDBACKBUTTONPRESS = VSConstants.VS_E_WIZARDBACKBUTTONPRESS,

        VS_E_INCOMPATIBLEPROJECT = VSConstants.VS_E_INCOMPATIBLEPROJECT,

        VS_E_INCOMPATIBLECLASSICPROJECT = VSConstants.VS_E_INCOMPATIBLECLASSICPROJECT,

        VS_E_INCOMPATIBLEPROJECT_UNSUPPORTED_OS = VSConstants.VS_E_INCOMPATIBLEPROJECT_UNSUPPORTED_OS,

        VS_E_PROMPTREQUIRED = VSConstants.VS_E_PROMPTREQUIRED,

        VS_E_CIRCULARTASKDEPENDENCY = VSConstants.VS_E_CIRCULARTASKDEPENDENCY,

        VS_E_EDITORDISABLED = VSConstants.VS_E_EDITORDISABLED,

        UNDO_E_CLIENTABORT = VSConstants.UNDO_E_CLIENTABORT,

        /// <summary>
        /// Another single phase resource manager has already been enlisted in this transaction.
        /// </summary>
        XACT_E_FIRST = XACT_E_ALREADYOTHERSINGLEPHASE,

        /// <summary>
        /// Another single phase resource manager has already been enlisted in this transaction.
        /// </summary>
        XACT_E_ALREADYOTHERSINGLEPHASE = unchecked((int)(0x8004D000)),

        /// <summary>
        /// A retaining commit or abort is not supported
        /// </summary>
        XACT_E_CANTRETAIN = unchecked((int)(0x8004D001)),

        /// <summary>
        /// The transaction failed to commit for an unknown reason. The transaction was aborted.
        /// </summary>
        XACT_E_COMMITFAILED = unchecked((int)(0x8004D002)),

        /// <summary>
        /// Cannot call commit on this transaction object because the calling application did not initiate the transaction.
        /// </summary>
        XACT_E_COMMITPREVENTED = unchecked((int)(0x8004D003)),

        /// <summary>
        /// Instead of committing, the resource heuristically aborted.
        /// </summary>
        XACT_E_HEURISTICABORT = unchecked((int)(0x8004D004)),

        /// <summary>
        /// Instead of aborting, the resource heuristically committed.
        /// </summary>
        XACT_E_HEURISTICCOMMIT = unchecked((int)(0x8004D005)),

        /// <summary>
        /// Some of the states of the resource were committed while others were aborted, likely because of heuristic decisions.
        /// </summary>
        XACT_E_HEURISTICDAMAGE = unchecked((int)(0x8004D006)),

        /// <summary>
        /// Some of the states of the resource may have been committed while others may have been aborted, likely because of
        /// heuristic decisions.
        /// </summary>
        XACT_E_HEURISTICDANGER = unchecked((int)(0x8004D007)),

        /// <summary>
        /// The requested isolation level is not valid or supported.
        /// </summary>
        XACT_E_ISOLATIONLEVEL = unchecked((int)(0x8004D008)),

        /// <summary>
        /// The transaction manager doesn't support an asynchronous operation for this method.
        /// </summary>
        XACT_E_NOASYNC = unchecked((int)(0x8004D009)),

        /// <summary>
        /// Unable to enlist in the transaction.
        /// </summary>
        XACT_E_NOENLIST = unchecked((int)(0x8004D00A)),

        /// <summary>
        /// The requested semantics of retention of isolation across retaining commit and abort boundaries cannot be supported by
        /// this transaction implementation, or isoFlags was not equal to zero.
        /// </summary>
        XACT_E_NOISORETAIN = unchecked((int)(0x8004D00B)),

        /// <summary>
        /// There is no resource presently associated with this enlistment
        /// </summary>
        XACT_E_NORESOURCE = unchecked((int)(0x8004D00C)),

        /// <summary>
        /// The transaction failed to commit due to the failure of optimistic concurrency control in at least one of the resource managers.
        /// </summary>
        XACT_E_NOTCURRENT = unchecked((int)(0x8004D00D)),

        /// <summary>
        /// The transaction has already been implicitly or explicitly committed or aborted
        /// </summary>
        XACT_E_NOTRANSACTION = unchecked((int)(0x8004D00E)),

        /// <summary>
        /// An invalid combination of flags was specified
        /// </summary>
        XACT_E_NOTSUPPORTED = unchecked((int)(0x8004D00F)),

        /// <summary>
        /// The resource manager id is not associated with this transaction or the transaction manager.
        /// </summary>
        XACT_E_UNKNOWNRMGRID = unchecked((int)(0x8004D010)),

        /// <summary>
        /// This method was called in the wrong state
        /// </summary>
        XACT_E_WRONGSTATE = unchecked((int)(0x8004D011)),

        /// <summary>
        /// The indicated unit of work does not match the unit of work expected by the resource manager.
        /// </summary>
        XACT_E_WRONGUOW = unchecked((int)(0x8004D012)),

        /// <summary>
        /// An enlistment in a transaction already exists.
        /// </summary>
        XACT_E_XTIONEXISTS = unchecked((int)(0x8004D013)),

        /// <summary>
        /// An import object for the transaction could not be found.
        /// </summary>
        XACT_E_NOIMPORTOBJECT = unchecked((int)(0x8004D014)),

        /// <summary>
        /// The transaction cookie is invalid.
        /// </summary>
        XACT_E_INVALIDCOOKIE = unchecked((int)(0x8004D015)),

        /// <summary>
        /// The transaction status is in doubt. A communication failure occurred, or a transaction manager or resource manager has failed
        /// </summary>
        XACT_E_INDOUBT = unchecked((int)(0x8004D016)),

        /// <summary>
        /// A time-out was specified, but time-outs are not supported.
        /// </summary>
        XACT_E_NOTIMEOUT = unchecked((int)(0x8004D017)),

        /// <summary>
        /// The requested operation is already in progress for the transaction.
        /// </summary>
        XACT_E_ALREADYINPROGRESS = unchecked((int)(0x8004D018)),

        /// <summary>
        /// The transaction has already been aborted.
        /// </summary>
        XACT_E_ABORTED = unchecked((int)(0x8004D019)),

        /// <summary>
        /// The Transaction Manager returned a log full error.
        /// </summary>
        XACT_E_LOGFULL = unchecked((int)(0x8004D01A)),

        /// <summary>
        /// The Transaction Manager is not available.
        /// </summary>
        XACT_E_TMNOTAVAILABLE = unchecked((int)(0x8004D01B)),

        /// <summary>
        /// A connection with the transaction manager was lost.
        /// </summary>
        XACT_E_CONNECTION_DOWN = unchecked((int)(0x8004D01C)),

        /// <summary>
        /// A request to establish a connection with the transaction manager was denied.
        /// </summary>
        XACT_E_CONNECTION_DENIED = unchecked((int)(0x8004D01D)),

        /// <summary>
        /// Resource manager reenlistment to determine transaction status timed out.
        /// </summary>
        XACT_E_REENLISTTIMEOUT = unchecked((int)(0x8004D01E)),

        /// <summary>
        /// This transaction manager failed to establish a connection with another TIP transaction manager.
        /// </summary>
        XACT_E_TIP_CONNECT_FAILED = unchecked((int)(0x8004D01F)),

        /// <summary>
        /// This transaction manager encountered a protocol error with another TIP transaction manager.
        /// </summary>
        XACT_E_TIP_PROTOCOL_ERROR = unchecked((int)(0x8004D020)),

        /// <summary>
        /// This transaction manager could not propagate a transaction from another TIP transaction manager.
        /// </summary>
        XACT_E_TIP_PULL_FAILED = unchecked((int)(0x8004D021)),

        /// <summary>
        /// The Transaction Manager on the destination machine is not available.
        /// </summary>
        XACT_E_DEST_TMNOTAVAILABLE = unchecked((int)(0x8004D022)),

        /// <summary>
        /// The Transaction Manager has disabled its support for TIP.
        /// </summary>
        XACT_E_TIP_DISABLED = unchecked((int)(0x8004D023)),

        /// <summary>
        /// The transaction manager has disabled its support for remote/network transactions.
        /// </summary>
        XACT_E_NETWORK_TX_DISABLED = unchecked((int)(0x8004D024)),

        /// <summary>
        /// The partner transaction manager has disabled its support for remote/network transactions.
        /// </summary>
        XACT_E_PARTNER_NETWORK_TX_DISABLED = unchecked((int)(0x8004D025)),

        /// <summary>
        /// The transaction manager has disabled its support for XA transactions.
        /// </summary>
        XACT_E_XA_TX_DISABLED = unchecked((int)(0x8004D026)),

        /// <summary>
        /// MSDTC was unable to read its configuration information.
        /// </summary>
        XACT_E_UNABLE_TO_READ_DTC_CONFIG = unchecked((int)(0x8004D027)),

        /// <summary>
        /// MSDTC was unable to load the dtc proxy dll.
        /// </summary>
        XACT_E_UNABLE_TO_LOAD_DTC_PROXY = unchecked((int)(0x8004D028)),

        /// <summary>
        /// The local transaction has aborted.
        /// </summary>
        XACT_E_LAST = XACT_E_ABORTING,

        /// <summary>
        /// The local transaction has aborted.
        /// </summary>
        XACT_E_ABORTING = unchecked((int)(0x8004D029)),

        /// <summary>
        /// XACT_E_CLERKNOTFOUND
        /// </summary>
        XACT_E_CLERKNOTFOUND = unchecked((int)(0x8004D080)),

        /// <summary>
        /// XACT_E_CLERKEXISTS
        /// </summary>
        XACT_E_CLERKEXISTS = unchecked((int)(0x8004D081)),

        /// <summary>
        /// XACT_E_RECOVERYINPROGRESS
        /// </summary>
        XACT_E_RECOVERYINPROGRESS = unchecked((int)(0x8004D082)),

        /// <summary>
        /// XACT_E_TRANSACTIONCLOSED
        /// </summary>
        XACT_E_TRANSACTIONCLOSED = unchecked((int)(0x8004D083)),

        /// <summary>
        /// XACT_E_INVALIDLSN
        /// </summary>
        XACT_E_INVALIDLSN = unchecked((int)(0x8004D084)),

        /// <summary>
        /// XACT_E_REPLAYREQUEST
        /// </summary>
        XACT_E_REPLAYREQUEST = unchecked((int)(0x8004D085)),

        /// <summary>
        /// No information available.
        /// </summary>
        CONTEXT_E_FIRST = unchecked((int)(0x8004E000)),

        /// <summary>
        /// The root transaction wanted to commit, but transaction aborted
        /// </summary>
        CONTEXT_E_ABORTED = unchecked((int)(0x8004E002)),

        /// <summary>
        /// You made a method call on a <c>COM</c>+ component that has a transaction that has already aborted or in the process of aborting.
        /// </summary>
        CONTEXT_E_ABORTING = unchecked((int)(0x8004E003)),

        /// <summary>
        /// There is no MTS object context
        /// </summary>
        CONTEXT_E_NOCONTEXT = unchecked((int)(0x8004E004)),

        /// <summary>
        /// No information available.
        /// </summary>
        CONTEXT_E_WOULD_DEADLOCK = unchecked((int)(0x8004E005)),

        /// <summary>
        /// The component is configured to use synchronization and a thread has timed out waiting to enter the context.
        /// </summary>
        CONTEXT_E_SYNCH_TIMEOUT = unchecked((int)(0x8004E006)),

        /// <summary>
        /// You made a method call on a <c>COM</c>+ component that has a transaction that has already committed or aborted.
        /// </summary>
        CONTEXT_E_OLDREF = unchecked((int)(0x8004E007)),

        /// <summary>
        /// The specified role was not configured for the application
        /// </summary>
        CONTEXT_E_ROLENOTFOUND = unchecked((int)(0x8004E00C)),

        /// <summary>
        /// <c>COM</c>+ was unable to talk to the Microsoft Distributed Transaction Coordinator
        /// </summary>
        CONTEXT_E_TMNOTAVAILABLE = unchecked((int)(0x8004E00F)),

        /// <summary>
        /// An unexpected error occurred during <c>COM</c>+ Activation.
        /// </summary>
        CO_E_ACTIVATIONFAILED = unchecked((int)(0x8004E021)),

        /// <summary>
        /// <c>COM</c>+ Activation failed. Check the event log for more information
        /// </summary>
        CO_E_ACTIVATIONFAILED_EVENTLOGGED = unchecked((int)(0x8004E022)),

        /// <summary>
        /// <c>COM</c>+ Activation failed due to a catalog or configuration error.
        /// </summary>
        CO_E_ACTIVATIONFAILED_CATALOGERROR = unchecked((int)(0x8004E023)),

        /// <summary>
        /// <c>COM</c>+ activation failed because the activation could not be completed in the specified amount of time.
        /// </summary>
        CO_E_ACTIVATIONFAILED_TIMEOUT = unchecked((int)(0x8004E024)),

        /// <summary>
        /// <c>COM</c>+ Activation failed because an initialization function failed. Check the event log for more information.
        /// </summary>
        CO_E_INITIALIZATIONFAILED = unchecked((int)(0x8004E025)),

        /// <summary>
        /// The requested operation requires that JIT be in the current context and it is not
        /// </summary>
        CONTEXT_E_NOJIT = unchecked((int)(0x8004E026)),

        /// <summary>
        /// The requested operation requires that the current context have a Transaction, and it does not
        /// </summary>
        CONTEXT_E_NOTRANSACTION = unchecked((int)(0x8004E027)),

        /// <summary>
        /// The components threading model has changed after install into a <c>COM</c>+ Application. Please re-install component.
        /// </summary>
        CO_E_THREADINGMODEL_CHANGED = unchecked((int)(0x8004E028)),

        /// <summary>
        /// IIS intrinsics not available. Start your work with IIS.
        /// </summary>
        CO_E_NOIISINTRINSICS = unchecked((int)(0x8004E029)),

        /// <summary>
        /// An attempt to write a cookie failed.
        /// </summary>
        CO_E_NOCOOKIES = unchecked((int)(0x8004E02A)),

        /// <summary>
        /// An attempt to use a database generated a database specific error.
        /// </summary>
        CO_E_DBERROR = unchecked((int)(0x8004E02B)),

        /// <summary>
        /// The <c>COM</c>+ component you created must use object pooling to work.
        /// </summary>
        CO_E_NOTPOOLED = unchecked((int)(0x8004E02C)),

        /// <summary>
        /// The <c>COM</c>+ component you created must use object construction to work correctly.
        /// </summary>
        CO_E_NOTCONSTRUCTED = unchecked((int)(0x8004E02D)),

        /// <summary>
        /// The <c>COM</c>+ component requires synchronization, and it is not configured for it.
        /// </summary>
        CO_E_NOSYNCHRONIZATION = unchecked((int)(0x8004E02E)),

        /// <summary>
        /// The TxIsolation Level property for the <c>COM</c>+ component being created is stronger than the TxIsolationLevel for the
        /// "root" component for the transaction. The creation failed.
        /// </summary>
        CONTEXT_E_LAST = CO_E_ISOLEVELMISMATCH,

        /// <summary>
        /// The TxIsolation Level property for the <c>COM</c>+ component being created is stronger than the TxIsolationLevel for the
        /// "root" component for the transaction. The creation failed.
        /// </summary>
        CO_E_ISOLEVELMISMATCH = unchecked((int)(0x8004E02F)),

        COR_E_FILENOTFOUND = VSConstants.COR_E_FILENOTFOUND,

        COR_E_DIRECTORYNOTFOUND = VSConstants.COR_E_DIRECTORYNOTFOUND,

        /// <summary>
        /// General access denied error
        /// </summary>
        E_ACCESSDENIED = VSConstants.E_ACCESSDENIED,

        /// <summary>
        /// Invalid handle
        /// </summary>
        E_HANDLE = VSConstants.E_HANDLE,

        /// <summary>
        /// Error <c>HRESULT</c> for out of memory
        /// </summary>
        E_OUTOFMEMORY = VSConstants.E_OUTOFMEMORY,

        /// <summary>
        /// Error <c>HRESULT</c> for an invalid argument.
        /// </summary>
        E_INVALIDARG = VSConstants.E_INVALIDARG,

        /// <summary>
        /// Attempt to create a class object failed
        /// </summary>
        CO_E_CLASS_CREATE_FAILED = unchecked((int)(0x80080001)),

        /// <summary>
        /// OLE service could not bind object
        /// </summary>
        CO_E_SCM_ERROR = unchecked((int)(0x80080002)),

        /// <summary>
        /// RPC communication failed with OLE service
        /// </summary>
        CO_E_SCM_RPC_FAILURE = unchecked((int)(0x80080003)),

        /// <summary>
        /// Bad path to object
        /// </summary>
        CO_E_BAD_PATH = unchecked((int)(0x80080004)),

        /// <summary>
        /// Server execution failed
        /// </summary>
        CO_E_SERVER_EXEC_FAILURE = unchecked((int)(0x80080005)),

        /// <summary>
        /// OLE service could not communicate with the object server
        /// </summary>
        CO_E_OBJSRV_RPC_FAILURE = unchecked((int)(0x80080006)),

        /// <summary>
        /// Errors occurred accessing one or more objects - the ErrorInfo collection may have more detail
        /// </summary>
        COMADMIN_E_OBJECTERRORS = unchecked((int)(0x80110401)),

        /// <summary>
        /// One or more of the object's properties are missing or invalid
        /// </summary>
        COMADMIN_E_OBJECTINVALID = unchecked((int)(0x80110402)),

        /// <summary>
        /// The object was not found in the catalog
        /// </summary>
        COMADMIN_E_KEYMISSING = unchecked((int)(0x80110403)),

        /// <summary>
        /// The object is already registered
        /// </summary>
        COMADMIN_E_ALREADYINSTALLED = unchecked((int)(0x80110404)),

        /// <summary>
        /// Error occurred writing to the application file
        /// </summary>
        COMADMIN_E_APP_FILE_WRITEFAIL = unchecked((int)(0x80110407)),

        /// <summary>
        /// Error occurred reading the application file
        /// </summary>
        COMADMIN_E_APP_FILE_READFAIL = unchecked((int)(0x80110408)),

        /// <summary>
        /// Invalid version number in application file
        /// </summary>
        COMADMIN_E_APP_FILE_VERSION = unchecked((int)(0x80110409)),

        /// <summary>
        /// The file path is invalid
        /// </summary>
        COMADMIN_E_BADPATH = unchecked((int)(0x8011040A)),

        /// <summary>
        /// The application is already installed
        /// </summary>
        COMADMIN_E_APPLICATIONEXISTS = unchecked((int)(0x8011040B)),

        /// <summary>
        /// The role already exists
        /// </summary>
        COMADMIN_E_ROLEEXISTS = unchecked((int)(0x8011040C)),

        /// <summary>
        /// An error occurred copying the file
        /// </summary>
        COMADMIN_E_CANTCOPYFILE = unchecked((int)(0x8011040D)),

        /// <summary>
        /// One or more users are not valid
        /// </summary>
        COMADMIN_E_NOUSER = unchecked((int)(0x8011040F)),

        /// <summary>
        /// One or more users in the application file are not valid
        /// </summary>
        COMADMIN_E_INVALIDUSERIDS = unchecked((int)(0x80110410)),

        /// <summary>
        /// The component's CLSID is missing or corrupt
        /// </summary>
        COMADMIN_E_NOREGISTRYCLSID = unchecked((int)(0x80110411)),

        /// <summary>
        /// The component's progID is missing or corrupt
        /// </summary>
        COMADMIN_E_BADREGISTRYPROGID = unchecked((int)(0x80110412)),

        /// <summary>
        /// Unable to set required authentication level for update request
        /// </summary>
        COMADMIN_E_AUTHENTICATIONLEVEL = unchecked((int)(0x80110413)),

        /// <summary>
        /// The identity or password set on the application is not valid
        /// </summary>
        COMADMIN_E_USERPASSWDNOTVALID = unchecked((int)(0x80110414)),

        /// <summary>
        /// Application file CLSIDs or IIDs do not match corresponding DLLs
        /// </summary>
        COMADMIN_E_CLSIDORIIDMISMATCH = unchecked((int)(0x80110418)),

        /// <summary>
        /// Interface information is either missing or changed
        /// </summary>
        COMADMIN_E_REMOTEINTERFACE = unchecked((int)(0x80110419)),

        /// <summary>
        /// DllRegisterServer failed on component install
        /// </summary>
        COMADMIN_E_DLLREGISTERSERVER = unchecked((int)(0x8011041A)),

        /// <summary>
        /// No server file share available
        /// </summary>
        COMADMIN_E_NOSERVERSHARE = unchecked((int)(0x8011041B)),

        /// <summary>
        /// DLL could not be loaded
        /// </summary>
        COMADMIN_E_DLLLOADFAILED = unchecked((int)(0x8011041D)),

        /// <summary>
        /// The registered TypeLib ID is not valid
        /// </summary>
        COMADMIN_E_BADREGISTRYLIBID = unchecked((int)(0x8011041E)),

        /// <summary>
        /// Application install directory not found
        /// </summary>
        COMADMIN_E_APPDIRNOTFOUND = unchecked((int)(0x8011041F)),

        /// <summary>
        /// Errors occurred while in the component registrar
        /// </summary>
        COMADMIN_E_REGISTRARFAILED = unchecked((int)(0x80110423)),

        /// <summary>
        /// The file does not exist
        /// </summary>
        COMADMIN_E_COMPFILE_DOESNOTEXIST = unchecked((int)(0x80110424)),

        /// <summary>
        /// The DLL could not be loaded
        /// </summary>
        COMADMIN_E_COMPFILE_LOADDLLFAIL = unchecked((int)(0x80110425)),

        /// <summary>
        /// GetClassObject failed in the DLL
        /// </summary>
        COMADMIN_E_COMPFILE_GETCLASSOBJ = unchecked((int)(0x80110426)),

        /// <summary>
        /// The DLL does not support the components listed in the TypeLib
        /// </summary>
        COMADMIN_E_COMPFILE_CLASSNOTAVAIL = unchecked((int)(0x80110427)),

        /// <summary>
        /// The TypeLib could not be loaded
        /// </summary>
        COMADMIN_E_COMPFILE_BADTLB = unchecked((int)(0x80110428)),

        /// <summary>
        /// The file does not contain components or component information
        /// </summary>
        COMADMIN_E_COMPFILE_NOTINSTALLABLE = unchecked((int)(0x80110429)),

        /// <summary>
        /// Changes to this object and its sub-objects have been disabled
        /// </summary>
        COMADMIN_E_NOTCHANGEABLE = unchecked((int)(0x8011042A)),

        /// <summary>
        /// The delete function has been disabled for this object
        /// </summary>
        COMADMIN_E_NOTDELETEABLE = unchecked((int)(0x8011042B)),

        /// <summary>
        /// The server catalog version is not supported
        /// </summary>
        COMADMIN_E_SESSION = unchecked((int)(0x8011042C)),

        /// <summary>
        /// The component move was disallowed, because the source or destination application is either a system application or
        /// currently locked against changes
        /// </summary>
        COMADMIN_E_COMP_MOVE_LOCKED = unchecked((int)(0x8011042D)),

        /// <summary>
        /// The component move failed because the destination application no longer exists
        /// </summary>
        COMADMIN_E_COMP_MOVE_BAD_DEST = unchecked((int)(0x8011042E)),

        /// <summary>
        /// The system was unable to register the TypeLib
        /// </summary>
        COMADMIN_E_REGISTERTLB = unchecked((int)(0x80110430)),

        /// <summary>
        /// This operation can not be performed on the system application
        /// </summary>
        COMADMIN_E_SYSTEMAPP = unchecked((int)(0x80110433)),

        /// <summary>
        /// The component registrar referenced in this file is not available
        /// </summary>
        COMADMIN_E_COMPFILE_NOREGISTRAR = unchecked((int)(0x80110434)),

        /// <summary>
        /// A component in the same DLL is already installed
        /// </summary>
        COMADMIN_E_COREQCOMPINSTALLED = unchecked((int)(0x80110435)),

        /// <summary>
        /// The service is not installed
        /// </summary>
        COMADMIN_E_SERVICENOTINSTALLED = unchecked((int)(0x80110436)),

        /// <summary>
        /// One or more property settings are either invalid or in conflict with each other
        /// </summary>
        COMADMIN_E_PROPERTYSAVEFAILED = unchecked((int)(0x80110437)),

        /// <summary>
        /// The object you are attempting to add or rename already exists
        /// </summary>
        COMADMIN_E_OBJECTEXISTS = unchecked((int)(0x80110438)),

        /// <summary>
        /// The component already exists
        /// </summary>
        COMADMIN_E_COMPONENTEXISTS = unchecked((int)(0x80110439)),

        /// <summary>
        /// The registration file is corrupt
        /// </summary>
        COMADMIN_E_REGFILE_CORRUPT = unchecked((int)(0x8011043B)),

        /// <summary>
        /// The property value is too large
        /// </summary>
        COMADMIN_E_PROPERTY_OVERFLOW = unchecked((int)(0x8011043C)),

        /// <summary>
        /// Object was not found in registry
        /// </summary>
        COMADMIN_E_NOTINREGISTRY = unchecked((int)(0x8011043E)),

        /// <summary>
        /// This object is not poolable
        /// </summary>
        COMADMIN_E_OBJECTNOTPOOLABLE = unchecked((int)(0x8011043F)),

        /// <summary>
        /// A CLSID with the same GUID as the new application ID is already installed on this machine
        /// </summary>
        COMADMIN_E_APPLID_MATCHES_CLSID = unchecked((int)(0x80110446)),

        /// <summary>
        /// A role assigned to a component, interface, or method did not exist in the application
        /// </summary>
        COMADMIN_E_ROLE_DOES_NOT_EXIST = unchecked((int)(0x80110447)),

        /// <summary>
        /// You must have components in an application in order to start the application
        /// </summary>
        COMADMIN_E_START_APP_NEEDS_COMPONENTS = unchecked((int)(0x80110448)),

        /// <summary>
        /// This operation is not enabled on this platform
        /// </summary>
        COMADMIN_E_REQUIRES_DIFFERENT_PLATFORM = unchecked((int)(0x80110449)),

        /// <summary>
        /// Application Proxy is not exportable
        /// </summary>
        COMADMIN_E_CAN_NOT_EXPORT_APP_PROXY = unchecked((int)(0x8011044A)),

        /// <summary>
        /// Failed to start application because it is either a library application or an application proxy
        /// </summary>
        COMADMIN_E_CAN_NOT_START_APP = unchecked((int)(0x8011044B)),

        /// <summary>
        /// System application is not exportable
        /// </summary>
        COMADMIN_E_CAN_NOT_EXPORT_SYS_APP = unchecked((int)(0x8011044C)),

        /// <summary>
        /// Can not subscribe to this component (the component may have been imported)
        /// </summary>
        COMADMIN_E_CANT_SUBSCRIBE_TO_COMPONENT = unchecked((int)(0x8011044D)),

        /// <summary>
        /// An event class cannot also be a subscriber component
        /// </summary>
        COMADMIN_E_EVENTCLASS_CANT_BE_SUBSCRIBER = unchecked((int)(0x8011044E)),

        /// <summary>
        /// Library applications and application proxies are incompatible
        /// </summary>
        COMADMIN_E_LIB_APP_PROXY_INCOMPATIBLE = unchecked((int)(0x8011044F)),

        /// <summary>
        /// This function is valid for the base partition only
        /// </summary>
        COMADMIN_E_BASE_PARTITION_ONLY = unchecked((int)(0x80110450)),

        /// <summary>
        /// You cannot start an application that has been disabled
        /// </summary>
        COMADMIN_E_START_APP_DISABLED = unchecked((int)(0x80110451)),

        /// <summary>
        /// The specified partition name is already in use on this computer
        /// </summary>
        COMADMIN_E_CAT_DUPLICATE_PARTITION_NAME = unchecked((int)(0x80110457)),

        /// <summary>
        /// The specified partition name is invalid. Check that the name contains at least one visible character
        /// </summary>
        COMADMIN_E_CAT_INVALID_PARTITION_NAME = unchecked((int)(0x80110458)),

        /// <summary>
        /// The partition cannot be deleted because it is the default partition for one or more users
        /// </summary>
        COMADMIN_E_CAT_PARTITION_IN_USE = unchecked((int)(0x80110459)),

        /// <summary>
        /// The partition cannot be exported, because one or more components in the partition have the same file name
        /// </summary>
        COMADMIN_E_FILE_PARTITION_DUPLICATE_FILES = unchecked((int)(0x8011045A)),

        /// <summary>
        /// Applications that contain one or more imported components cannot be installed into a non-base partition
        /// </summary>
        COMADMIN_E_CAT_IMPORTED_COMPONENTS_NOT_ALLOWED = unchecked((int)(0x8011045B)),

        /// <summary>
        /// The application name is not unique and cannot be resolved to an application id
        /// </summary>
        COMADMIN_E_AMBIGUOUS_APPLICATION_NAME = unchecked((int)(0x8011045C)),

        /// <summary>
        /// The partition name is not unique and cannot be resolved to a partition id
        /// </summary>
        COMADMIN_E_AMBIGUOUS_PARTITION_NAME = unchecked((int)(0x8011045D)),

        /// <summary>
        /// The <c>COM</c>+ registry database has not been initialized
        /// </summary>
        COMADMIN_E_REGDB_NOTINITIALIZED = unchecked((int)(0x80110472)),

        /// <summary>
        /// The <c>COM</c>+ registry database is not open
        /// </summary>
        COMADMIN_E_REGDB_NOTOPEN = unchecked((int)(0x80110473)),

        /// <summary>
        /// The <c>COM</c>+ registry database detected a system error
        /// </summary>
        COMADMIN_E_REGDB_SYSTEMERR = unchecked((int)(0x80110474)),

        /// <summary>
        /// The <c>COM</c>+ registry database is already running
        /// </summary>
        COMADMIN_E_REGDB_ALREADYRUNNING = unchecked((int)(0x80110475)),

        /// <summary>
        /// This version of the <c>COM</c>+ registry database cannot be migrated
        /// </summary>
        COMADMIN_E_MIG_VERSIONNOTSUPPORTED = unchecked((int)(0x80110480)),

        /// <summary>
        /// The schema version to be migrated could not be found in the <c>COM</c>+ registry database
        /// </summary>
        COMADMIN_E_MIG_SCHEMANOTFOUND = unchecked((int)(0x80110481)),

        /// <summary>
        /// There was a type mismatch between binaries
        /// </summary>
        COMADMIN_E_CAT_BITNESSMISMATCH = unchecked((int)(0x80110482)),

        /// <summary>
        /// A binary of unknown or invalid type was provided
        /// </summary>
        COMADMIN_E_CAT_UNACCEPTABLEBITNESS = unchecked((int)(0x80110483)),

        /// <summary>
        /// There was a type mismatch between a binary and an application
        /// </summary>
        COMADMIN_E_CAT_WRONGAPPBITNESS = unchecked((int)(0x80110484)),

        /// <summary>
        /// The application cannot be paused or resumed
        /// </summary>
        COMADMIN_E_CAT_PAUSE_RESUME_NOT_SUPPORTED = unchecked((int)(0x80110485)),

        /// <summary>
        /// The <c>COM</c>+ Catalog Server threw an exception during execution
        /// </summary>
        COMADMIN_E_CAT_SERVERFAULT = unchecked((int)(0x80110486)),

        /// <summary>
        /// Moniker path could not be normalized
        /// </summary>
        MK_E_NO_NORMALIZED = unchecked((int)(0x80080007)),

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for object server is stopping when OLE service contacts it
        /// </summary>
        CO_E_SERVER_STOPPING = unchecked((int)(0x80080008)),

        /// <summary>
        /// An invalid root block pointer was specified
        /// </summary>
        MEM_E_INVALID_ROOT = unchecked((int)(0x80080009)),

        /// <summary>
        /// An allocation chain contained an invalid link pointer
        /// </summary>
        MEM_E_INVALID_LINK = unchecked((int)(0x80080010)),

        /// <summary>
        /// The requested allocation size was too large
        /// </summary>
        MEM_E_INVALID_SIZE = unchecked((int)(0x80080011)),

        /// <summary>
        /// Bad UID.
        /// </summary>
        NTE_BAD_UID = unchecked((int)(0x80090001)),

        /// <summary>
        /// Bad Hash.
        /// </summary>
        NTE_BAD_HASH = unchecked((int)(0x80090002)),

        /// <summary>
        /// Bad Key.
        /// </summary>
        NTE_BAD_KEY = unchecked((int)(0x80090003)),

        /// <summary>
        /// Bad Length.
        /// </summary>
        NTE_BAD_LEN = unchecked((int)(0x80090004)),

        /// <summary>
        /// Bad Data.
        /// </summary>
        NTE_BAD_DATA = unchecked((int)(0x80090005)),

        /// <summary>
        /// Invalid Signature.
        /// </summary>
        NTE_BAD_SIGNATURE = unchecked((int)(0x80090006)),

        /// <summary>
        /// Bad Version of provider.
        /// </summary>
        NTE_BAD_VER = unchecked((int)(0x80090007)),

        /// <summary>
        /// Invalid algorithm specified.
        /// </summary>
        NTE_BAD_ALGID = unchecked((int)(0x80090008)),

        /// <summary>
        /// Invalid flags specified.
        /// </summary>
        NTE_BAD_FLAGS = unchecked((int)(0x80090009)),

        /// <summary>
        /// Invalid type specified.
        /// </summary>
        NTE_BAD_TYPE = unchecked((int)(0x8009000A)),

        /// <summary>
        /// Key not valid for use in specified state.
        /// </summary>
        NTE_BAD_KEY_STATE = unchecked((int)(0x8009000B)),

        /// <summary>
        /// Hash not valid for use in specified state.
        /// </summary>
        NTE_BAD_HASH_STATE = unchecked((int)(0x8009000C)),

        /// <summary>
        /// Key does not exist.
        /// </summary>
        NTE_NO_KEY = unchecked((int)(0x8009000D)),

        /// <summary>
        /// Insufficient memory available for the operation.
        /// </summary>
        NTE_NO_MEMORY = unchecked((int)(0x8009000E)),

        /// <summary>
        /// Object already exists.
        /// </summary>
        NTE_EXISTS = unchecked((int)(0x8009000F)),

        /// <summary>
        /// Access denied.
        /// </summary>
        NTE_PERM = unchecked((int)(0x80090010)),

        /// <summary>
        /// Object was not found.
        /// </summary>
        NTE_NOT_FOUND = unchecked((int)(0x80090011)),

        /// <summary>
        /// Data already encrypted.
        /// </summary>
        NTE_DOUBLE_ENCRYPT = unchecked((int)(0x80090012)),

        /// <summary>
        /// Invalid provider specified.
        /// </summary>
        NTE_BAD_PROVIDER = unchecked((int)(0x80090013)),

        /// <summary>
        /// Invalid provider type specified.
        /// </summary>
        NTE_BAD_PROV_TYPE = unchecked((int)(0x80090014)),

        /// <summary>
        /// Provider's public key is invalid.
        /// </summary>
        NTE_BAD_PUBLIC_KEY = unchecked((int)(0x80090015)),

        /// <summary>
        /// Keyset does not exist
        /// </summary>
        NTE_BAD_KEYSET = unchecked((int)(0x80090016)),

        /// <summary>
        /// Provider type not defined.
        /// </summary>
        NTE_PROV_TYPE_NOT_DEF = unchecked((int)(0x80090017)),

        /// <summary>
        /// Provider type as registered is invalid.
        /// </summary>
        NTE_PROV_TYPE_ENTRY_BAD = unchecked((int)(0x80090018)),

        /// <summary>
        /// The keyset is not defined.
        /// </summary>
        NTE_KEYSET_NOT_DEF = unchecked((int)(0x80090019)),

        /// <summary>
        /// Keyset as registered is invalid.
        /// </summary>
        NTE_KEYSET_ENTRY_BAD = unchecked((int)(0x8009001A)),

        /// <summary>
        /// Provider type does not match registered value.
        /// </summary>
        NTE_PROV_TYPE_NO_MATCH = unchecked((int)(0x8009001B)),

        /// <summary>
        /// The digital signature file is corrupt.
        /// </summary>
        NTE_SIGNATURE_FILE_BAD = unchecked((int)(0x8009001C)),

        /// <summary>
        /// Provider DLL failed to initialize correctly.
        /// </summary>
        NTE_PROVIDER_DLL_FAIL = unchecked((int)(0x8009001D)),

        /// <summary>
        /// Provider DLL could not be found.
        /// </summary>
        NTE_PROV_DLL_NOT_FOUND = unchecked((int)(0x8009001E)),

        /// <summary>
        /// The Keyset parameter is invalid.
        /// </summary>
        NTE_BAD_KEYSET_PARAM = unchecked((int)(0x8009001F)),

        /// <summary>
        /// An internal error occurred.
        /// </summary>
        NTE_FAIL = unchecked((int)(0x80090020)),

        /// <summary>
        /// A base error occurred.
        /// </summary>
        NTE_SYS_ERR = unchecked((int)(0x80090021)),

        /// <summary>
        /// Provider could not perform the action since the context was acquired as silent.
        /// </summary>
        NTE_SILENT_CONTEXT = unchecked((int)(0x80090022)),

        /// <summary>
        /// The security token does not have storage space available for an additional container.
        /// </summary>
        NTE_TOKEN_KEYSET_STORAGE_FULL = unchecked((int)(0x80090023)),

        /// <summary>
        /// The profile for the user is a temporary profile.
        /// </summary>
        NTE_TEMPORARY_PROFILE = unchecked((int)(0x80090024)),

        /// <summary>
        /// The key parameters could not be set because the CSP uses fixed parameters.
        /// </summary>
        NTE_FIXEDPARAMETER = unchecked((int)(0x80090025)),

        /// <summary>
        /// Not enough memory is available to complete this request
        /// </summary>
        SEC_E_INSUFFICIENT_MEMORY = unchecked((int)(0x80090300)),

        /// <summary>
        /// The handle specified is invalid
        /// </summary>
        SEC_E_INVALID_HANDLE = unchecked((int)(0x80090301)),

        /// <summary>
        /// The function requested is not supported
        /// </summary>
        SEC_E_UNSUPPORTED_FUNCTION = unchecked((int)(0x80090302)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_NOT_SUPPORTED = SEC_E_UNSUPPORTED_FUNCTION,

        /// <summary>
        /// The specified target is unknown or unreachable
        /// </summary>
        SEC_E_TARGET_UNKNOWN = unchecked((int)(0x80090303)),

        /// <summary>
        /// The Local Security Authority cannot be contacted
        /// </summary>
        SEC_E_INTERNAL_ERROR = unchecked((int)(0x80090304)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_NO_SPM = SEC_E_INTERNAL_ERROR,

        /// <summary>
        /// The requested security package does not exist
        /// </summary>
        SEC_E_SECPKG_NOT_FOUND = unchecked((int)(0x80090305)),

        /// <summary>
        /// The caller is not the owner of the desired credentials
        /// </summary>
        SEC_E_NOT_OWNER = unchecked((int)(0x80090306)),

        /// <summary>
        /// The security package failed to initialize, and cannot be installed
        /// </summary>
        SEC_E_CANNOT_INSTALL = unchecked((int)(0x80090307)),

        /// <summary>
        /// The token supplied to the function is invalid
        /// </summary>
        SEC_E_INVALID_TOKEN = unchecked((int)(0x80090308)),

        /// <summary>
        /// The security package is not able to marshall the logon buffer, so the logon attempt has failed
        /// </summary>
        SEC_E_CANNOT_PACK = unchecked((int)(0x80090309)),

        /// <summary>
        /// The per-message Quality of Protection is not supported by the security package
        /// </summary>
        SEC_E_QOP_NOT_SUPPORTED = unchecked((int)(0x8009030A)),

        /// <summary>
        /// The security context does not allow impersonation of the client
        /// </summary>
        SEC_E_NO_IMPERSONATION = unchecked((int)(0x8009030B)),

        /// <summary>
        /// The logon attempt failed
        /// </summary>
        SEC_E_LOGON_DENIED = unchecked((int)(0x8009030C)),

        /// <summary>
        /// The credentials supplied to the package were not recognized
        /// </summary>
        SEC_E_UNKNOWN_CREDENTIALS = unchecked((int)(0x8009030D)),

        /// <summary>
        /// No credentials are available in the security package
        /// </summary>
        SEC_E_NO_CREDENTIALS = unchecked((int)(0x8009030E)),

        /// <summary>
        /// The message or signature supplied for verification has been altered
        /// </summary>
        SEC_E_MESSAGE_ALTERED = unchecked((int)(0x8009030F)),

        /// <summary>
        /// The message supplied for verification is out of sequence
        /// </summary>
        SEC_E_OUT_OF_SEQUENCE = unchecked((int)(0x80090310)),

        /// <summary>
        /// No authority could be contacted for authentication.
        /// </summary>
        SEC_E_NO_AUTHENTICATING_AUTHORITY = unchecked((int)(0x80090311)),

        /// <summary>
        /// The requested security package does not exist
        /// </summary>
        SEC_E_BAD_PKGID = unchecked((int)(0x80090316)),

        /// <summary>
        /// The context has expired and can no longer be used.
        /// </summary>
        SEC_E_CONTEXT_EXPIRED = unchecked((int)(0x80090317)),

        /// <summary>
        /// The supplied message is incomplete. The signature was not verified.
        /// </summary>
        SEC_E_INCOMPLETE_MESSAGE = unchecked((int)(0x80090318)),

        /// <summary>
        /// The credentials supplied were not complete, and could not be verified. The context could not be initialized.
        /// </summary>
        SEC_E_INCOMPLETE_CREDENTIALS = unchecked((int)(0x80090320)),

        /// <summary>
        /// The buffers supplied to a function was too small.
        /// </summary>
        SEC_E_BUFFER_TOO_SMALL = unchecked((int)(0x80090321)),

        /// <summary>
        /// The target principal name is incorrect.
        /// </summary>
        SEC_E_WRONG_PRINCIPAL = unchecked((int)(0x80090322)),

        /// <summary>
        /// The clocks on the client and server machines are skewed.
        /// </summary>
        SEC_E_TIME_SKEW = unchecked((int)(0x80090324)),

        /// <summary>
        /// The certificate chain was issued by an authority that is not trusted.
        /// </summary>
        SEC_E_UNTRUSTED_ROOT = unchecked((int)(0x80090325)),

        /// <summary>
        /// The message received was unexpected or badly formatted.
        /// </summary>
        SEC_E_ILLEGAL_MESSAGE = unchecked((int)(0x80090326)),

        /// <summary>
        /// An unknown error occurred while processing the certificate.
        /// </summary>
        SEC_E_CERT_UNKNOWN = unchecked((int)(0x80090327)),

        /// <summary>
        /// The received certificate has expired.
        /// </summary>
        SEC_E_CERT_EXPIRED = unchecked((int)(0x80090328)),

        /// <summary>
        /// The specified data could not be encrypted.
        /// </summary>
        SEC_E_ENCRYPT_FAILURE = unchecked((int)(0x80090329)),

        /// <summary>
        /// The specified data could not be decrypted.
        /// </summary>
        SEC_E_DECRYPT_FAILURE = unchecked((int)(0x80090330)),

        /// <summary>
        /// The client and server cannot communicate, because they do not possess a common algorithm.
        /// </summary>
        SEC_E_ALGORITHM_MISMATCH = unchecked((int)(0x80090331)),

        /// <summary>
        /// The security context could not be established due to a failure in the requested quality of service (e.g. mutual
        /// authentication or delegation).
        /// </summary>
        SEC_E_SECURITY_QOS_FAILED = unchecked((int)(0x80090332)),

        /// <summary>
        /// A security context was deleted before the context was completed. This is considered a logon failure.
        /// </summary>
        SEC_E_UNFINISHED_CONTEXT_DELETED = unchecked((int)(0x80090333)),

        /// <summary>
        /// The client is trying to negotiate a context and the server requires user-to-user but didn't send a TGT reply.
        /// </summary>
        SEC_E_NO_TGT_REPLY = unchecked((int)(0x80090334)),

        /// <summary>
        /// Unable to accomplish the requested task because the local machine does not have any IP addresses.
        /// </summary>
        SEC_E_NO_IP_ADDRESSES = unchecked((int)(0x80090335)),

        /// <summary>
        /// The supplied credential handle does not match the credential associated with the security context.
        /// </summary>
        SEC_E_WRONG_CREDENTIAL_HANDLE = unchecked((int)(0x80090336)),

        /// <summary>
        /// The crypto system or checksum function is invalid because a required function is unavailable.
        /// </summary>
        SEC_E_CRYPTO_SYSTEM_INVALID = unchecked((int)(0x80090337)),

        /// <summary>
        /// The number of maximum ticket referrals has been exceeded.
        /// </summary>
        SEC_E_MAX_REFERRALS_EXCEEDED = unchecked((int)(0x80090338)),

        /// <summary>
        /// The local machine must be a Kerberos KDC (domain controller) and it is not.
        /// </summary>
        SEC_E_MUST_BE_KDC = unchecked((int)(0x80090339)),

        /// <summary>
        /// The other end of the security negotiation is requires strong crypto but it is not supported on the local machine.
        /// </summary>
        SEC_E_STRONG_CRYPTO_NOT_SUPPORTED = unchecked((int)(0x8009033A)),

        /// <summary>
        /// The KDC reply contained more than one principal name.
        /// </summary>
        SEC_E_TOO_MANY_PRINCIPALS = unchecked((int)(0x8009033B)),

        /// <summary>
        /// Expected to find PA data for a hint of what etype to use, but it was not found.
        /// </summary>
        SEC_E_NO_PA_DATA = unchecked((int)(0x8009033C)),

        /// <summary>
        /// The client cert name does not matches the user name or the KDC name is incorrect.
        /// </summary>
        SEC_E_PKINIT_NAME_MISMATCH = unchecked((int)(0x8009033D)),

        /// <summary>
        /// Smartcard logon is required and was not used.
        /// </summary>
        SEC_E_SMARTCARD_LOGON_REQUIRED = unchecked((int)(0x8009033E)),

        /// <summary>
        /// A system shutdown is in progress.
        /// </summary>
        SEC_E_SHUTDOWN_IN_PROGRESS = unchecked((int)(0x8009033F)),

        /// <summary>
        /// An invalid request was sent to the KDC.
        /// </summary>
        SEC_E_KDC_INVALID_REQUEST = unchecked((int)(0x80090340)),

        /// <summary>
        /// The KDC was unable to generate a referral for the service requested.
        /// </summary>
        SEC_E_KDC_UNABLE_TO_REFER = unchecked((int)(0x80090341)),

        /// <summary>
        /// The encryption type requested is not supported by the KDC.
        /// </summary>
        SEC_E_KDC_UNKNOWN_ETYPE = unchecked((int)(0x80090342)),

        /// <summary>
        /// An unsupported preauthentication mechanism was presented to the kerberos package.
        /// </summary>
        SEC_E_UNSUPPORTED_PREAUTH = unchecked((int)(0x80090343)),

        /// <summary>
        /// The requested operation requires delegation to be enabled on the machine.
        /// </summary>
        SEC_E_DELEGATION_REQUIRED = unchecked((int)(0x80090345)),

        /// <summary>
        /// Client's supplied SSPI channel bindings were incorrect.
        /// </summary>
        SEC_E_BAD_BINDINGS = unchecked((int)(0x80090346)),

        /// <summary>
        /// The received certificate was mapped to multiple accounts.
        /// </summary>
        SEC_E_MULTIPLE_ACCOUNTS = unchecked((int)(0x80090347)),

        /// <summary>
        /// SEC_E_NO_KERB_KEY
        /// </summary>
        SEC_E_NO_KERB_KEY = unchecked((int)(0x80090348)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_CERT_WRONG_USAGE = unchecked((int)(0x80090349)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_DOWNGRADE_DETECTED = unchecked((int)(0x80090350)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_SMARTCARD_CERT_REVOKED = unchecked((int)(0x80090351)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_ISSUING_CA_UNTRUSTED = unchecked((int)(0x80090352)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_REVOCATION_OFFLINE_C = unchecked((int)(0x80090353)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_PKINIT_CLIENT_FAILURE = unchecked((int)(0x80090354)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_SMARTCARD_CERT_EXPIRED = unchecked((int)(0x80090355)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_NO_S4U_PROT_SUPPORT = unchecked((int)(0x80090356)),

        /// <summary>
        /// No information available.
        /// </summary>
        SEC_E_CROSSREALM_DELEGATION_FAILURE = unchecked((int)(0x80090357)),

        /// <summary>
        /// An error occurred while performing an operation on a cryptographic message.
        /// </summary>
        CRYPT_E_MSG_ERROR = unchecked((int)(0x80091001)),

        /// <summary>
        /// Unknown cryptographic algorithm.
        /// </summary>
        CRYPT_E_UNKNOWN_ALGO = unchecked((int)(0x80091002)),

        /// <summary>
        /// The object identifier is poorly formatted.
        /// </summary>
        CRYPT_E_OID_FORMAT = unchecked((int)(0x80091003)),

        /// <summary>
        /// Invalid cryptographic message type.
        /// </summary>
        CRYPT_E_INVALID_MSG_TYPE = unchecked((int)(0x80091004)),

        /// <summary>
        /// Unexpected cryptographic message encoding.
        /// </summary>
        CRYPT_E_UNEXPECTED_ENCODING = unchecked((int)(0x80091005)),

        /// <summary>
        /// The cryptographic message does not contain an expected authenticated attribute.
        /// </summary>
        CRYPT_E_AUTH_ATTR_MISSING = unchecked((int)(0x80091006)),

        /// <summary>
        /// The hash value is not correct.
        /// </summary>
        CRYPT_E_HASH_VALUE = unchecked((int)(0x80091007)),

        /// <summary>
        /// The index value is not valid.
        /// </summary>
        CRYPT_E_INVALID_INDEX = unchecked((int)(0x80091008)),

        /// <summary>
        /// The content of the cryptographic message has already been decrypted.
        /// </summary>
        CRYPT_E_ALREADY_DECRYPTED = unchecked((int)(0x80091009)),

        /// <summary>
        /// The content of the cryptographic message has not been decrypted yet.
        /// </summary>
        CRYPT_E_NOT_DECRYPTED = unchecked((int)(0x8009100A)),

        /// <summary>
        /// The enveloped-data message does not contain the specified recipient.
        /// </summary>
        CRYPT_E_RECIPIENT_NOT_FOUND = unchecked((int)(0x8009100B)),

        /// <summary>
        /// Invalid control type.
        /// </summary>
        CRYPT_E_CONTROL_TYPE = unchecked((int)(0x8009100C)),

        /// <summary>
        /// Invalid issuer and/or serial number.
        /// </summary>
        CRYPT_E_ISSUER_SERIALNUMBER = unchecked((int)(0x8009100D)),

        /// <summary>
        /// Cannot find the original signer.
        /// </summary>
        CRYPT_E_SIGNER_NOT_FOUND = unchecked((int)(0x8009100E)),

        /// <summary>
        /// The cryptographic message does not contain all of the requested attributes.
        /// </summary>
        CRYPT_E_ATTRIBUTES_MISSING = unchecked((int)(0x8009100F)),

        /// <summary>
        /// The streamed cryptographic message is not ready to return data.
        /// </summary>
        CRYPT_E_STREAM_MSG_NOT_READY = unchecked((int)(0x80091010)),

        /// <summary>
        /// The streamed cryptographic message requires more data to complete the decode operation.
        /// </summary>
        CRYPT_E_STREAM_INSUFFICIENT_DATA = unchecked((int)(0x80091011)),

        /// <summary>
        /// The length specified for the output data was insufficient.
        /// </summary>
        CRYPT_E_BAD_LEN = unchecked((int)(0x80092001)),

        /// <summary>
        /// An error occurred during encode or decode operation.
        /// </summary>
        CRYPT_E_BAD_ENCODE = unchecked((int)(0x80092002)),

        /// <summary>
        /// An error occurred while reading or writing to a file.
        /// </summary>
        CRYPT_E_FILE_ERROR = unchecked((int)(0x80092003)),

        /// <summary>
        /// Cannot find object or property.
        /// </summary>
        CRYPT_E_NOT_FOUND = unchecked((int)(0x80092004)),

        /// <summary>
        /// The object or property already exists.
        /// </summary>
        CRYPT_E_EXISTS = unchecked((int)(0x80092005)),

        /// <summary>
        /// No provider was specified for the store or object.
        /// </summary>
        CRYPT_E_NO_PROVIDER = unchecked((int)(0x80092006)),

        /// <summary>
        /// The specified certificate is self signed.
        /// </summary>
        CRYPT_E_SELF_SIGNED = unchecked((int)(0x80092007)),

        /// <summary>
        /// The previous certificate or CRL context was deleted.
        /// </summary>
        CRYPT_E_DELETED_PREV = unchecked((int)(0x80092008)),

        /// <summary>
        /// Cannot find the requested object.
        /// </summary>
        CRYPT_E_NO_MATCH = unchecked((int)(0x80092009)),

        /// <summary>
        /// The certificate does not have a property that references a private key.
        /// </summary>
        CRYPT_E_UNEXPECTED_MSG_TYPE = unchecked((int)(0x8009200A)),

        /// <summary>
        /// Cannot find the certificate and private key for decryption.
        /// </summary>
        CRYPT_E_NO_KEY_PROPERTY = unchecked((int)(0x8009200B)),

        /// <summary>
        /// Cannot find the certificate and private key to use for decryption.
        /// </summary>
        CRYPT_E_NO_DECRYPT_CERT = unchecked((int)(0x8009200C)),

        /// <summary>
        /// Not a cryptographic message or the cryptographic message is not formatted correctly.
        /// </summary>
        CRYPT_E_BAD_MSG = unchecked((int)(0x8009200D)),

        /// <summary>
        /// The signed cryptographic message does not have a signer for the specified signer index.
        /// </summary>
        CRYPT_E_NO_SIGNER = unchecked((int)(0x8009200E)),

        /// <summary>
        /// Final closure is pending until additional frees or closes.
        /// </summary>
        CRYPT_E_PENDING_CLOSE = unchecked((int)(0x8009200F)),

        /// <summary>
        /// The certificate is revoked.
        /// </summary>
        CRYPT_E_REVOKED = unchecked((int)(0x80092010)),

        /// <summary>
        /// No Dll or exported function was found to verify revocation.
        /// </summary>
        CRYPT_E_NO_REVOCATION_DLL = unchecked((int)(0x80092011)),

        /// <summary>
        /// The revocation function was unable to check revocation for the certificate.
        /// </summary>
        CRYPT_E_NO_REVOCATION_CHECK = unchecked((int)(0x80092012)),

        /// <summary>
        /// The revocation function was unable to check revocation because the revocation server was offline.
        /// </summary>
        CRYPT_E_REVOCATION_OFFLINE = unchecked((int)(0x80092013)),

        /// <summary>
        /// The certificate is not in the revocation server's database.
        /// </summary>
        CRYPT_E_NOT_IN_REVOCATION_DATABASE = unchecked((int)(0x80092014)),

        /// <summary>
        /// The string contains a non-numeric character.
        /// </summary>
        CRYPT_E_INVALID_NUMERIC_STRING = unchecked((int)(0x80092020)),

        /// <summary>
        /// The string contains a non-printable character.
        /// </summary>
        CRYPT_E_INVALID_PRINTABLE_STRING = unchecked((int)(0x80092021)),

        /// <summary>
        /// The string contains a character not in the 7 bit ASCII character set.
        /// </summary>
        CRYPT_E_INVALID_IA5_STRING = unchecked((int)(0x80092022)),

        /// <summary>
        /// The string contains an invalid X500 name attribute key, oid, value or delimiter.
        /// </summary>
        CRYPT_E_INVALID_X500_STRING = unchecked((int)(0x80092023)),

        /// <summary>
        /// The dwValueType for the CERT_NAME_VALUE is not one of the character strings. Most likely it is either a
        /// CERT_RDN_ENCODED_BLOB or CERT_TDN_OCTED_STRING.
        /// </summary>
        CRYPT_E_NOT_CHAR_STRING = unchecked((int)(0x80092024)),

        /// <summary>
        /// The Put operation can not continue. The file needs to be resized. However, there is already a signature present. A
        /// complete signing operation must be done.
        /// </summary>
        CRYPT_E_FILERESIZED = unchecked((int)(0x80092025)),

        /// <summary>
        /// The cryptographic operation failed due to a local security option setting.
        /// </summary>
        CRYPT_E_SECURITY_SETTINGS = unchecked((int)(0x80092026)),

        /// <summary>
        /// No DLL or exported function was found to verify subject usage.
        /// </summary>
        CRYPT_E_NO_VERIFY_USAGE_DLL = unchecked((int)(0x80092027)),

        /// <summary>
        /// The called function was unable to do a usage check on the subject.
        /// </summary>
        CRYPT_E_NO_VERIFY_USAGE_CHECK = unchecked((int)(0x80092028)),

        /// <summary>
        /// Since the server was offline, the called function was unable to complete the usage check.
        /// </summary>
        CRYPT_E_VERIFY_USAGE_OFFLINE = unchecked((int)(0x80092029)),

        /// <summary>
        /// The subject was not found in a Certificate Trust List (CTL).
        /// </summary>
        CRYPT_E_NOT_IN_CTL = unchecked((int)(0x8009202A)),

        /// <summary>
        /// None of the signers of the cryptographic message or certificate trust list is trusted.
        /// </summary>
        CRYPT_E_NO_TRUSTED_SIGNER = unchecked((int)(0x8009202B)),

        /// <summary>
        /// The public key's algorithm parameters are missing.
        /// </summary>
        CRYPT_E_MISSING_PUBKEY_PARA = unchecked((int)(0x8009202C)),

        /// <summary>
        /// OSS Certificate encode/decode error code base
        /// </summary>
        /// <remarks>See asn1code.h for a definition of the OSS runtime errors. The OSS error values are offset by <see cref="CRYPT_E_OSS_ERROR"/>.</remarks>
        CRYPT_E_OSS_ERROR = unchecked((int)(0x80093000)),

        /// <summary>
        /// OSS ASN.1 Error: Output Buffer is too small.
        /// </summary>
        OSS_MORE_BUF = unchecked((int)(0x80093001)),

        /// <summary>
        /// OSS ASN.1 Error: Signed integer is encoded as a unsigned integer.
        /// </summary>
        OSS_NEGATIVE_UINTEGER = unchecked((int)(0x80093002)),

        /// <summary>
        /// OSS ASN.1 Error: Unknown ASN.1 data type.
        /// </summary>
        OSS_PDU_RANGE = unchecked((int)(0x80093003)),

        /// <summary>
        /// OSS ASN.1 Error: Output buffer is too small, the decoded data has been truncated.
        /// </summary>
        OSS_MORE_INPUT = unchecked((int)(0x80093004)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_DATA_ERROR = unchecked((int)(0x80093005)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid argument.
        /// </summary>
        OSS_BAD_ARG = unchecked((int)(0x80093006)),

        /// <summary>
        /// OSS ASN.1 Error: Encode/Decode version mismatch.
        /// </summary>
        OSS_BAD_VERSION = unchecked((int)(0x80093007)),

        /// <summary>
        /// OSS ASN.1 Error: Out of memory.
        /// </summary>
        OSS_OUT_MEMORY = unchecked((int)(0x80093008)),

        /// <summary>
        /// OSS ASN.1 Error: Encode/Decode Error.
        /// </summary>
        OSS_PDU_MISMATCH = unchecked((int)(0x80093009)),

        /// <summary>
        /// OSS ASN.1 Error: Internal Error.
        /// </summary>
        OSS_LIMITED = unchecked((int)(0x8009300A)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_BAD_PTR = unchecked((int)(0x8009300B)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_BAD_TIME = unchecked((int)(0x8009300C)),

        /// <summary>
        /// OSS ASN.1 Error: Unsupported BER indefinite-length encoding.
        /// </summary>
        OSS_INDEFINITE_NOT_SUPPORTED = unchecked((int)(0x8009300D)),

        /// <summary>
        /// OSS ASN.1 Error: Access violation.
        /// </summary>
        OSS_MEM_ERROR = unchecked((int)(0x8009300E)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_BAD_TABLE = unchecked((int)(0x8009300F)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_TOO_Int32 = unchecked((int)(0x80093010)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_CONSTRAINT_VIOLATED = unchecked((int)(0x80093011)),

        /// <summary>
        /// OSS ASN.1 Error: Internal Error.
        /// </summary>
        OSS_FATAL_ERROR = unchecked((int)(0x80093012)),

        /// <summary>
        /// OSS ASN.1 Error: Multi-threading conflict.
        /// </summary>
        OSS_ACCESS_SERIALIZATION_ERROR = unchecked((int)(0x80093013)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_NULL_TBL = unchecked((int)(0x80093014)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_NULL_FCN = unchecked((int)(0x80093015)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_BAD_ENCRULES = unchecked((int)(0x80093016)),

        /// <summary>
        /// OSS ASN.1 Error: Encode/Decode function not implemented.
        /// </summary>
        OSS_UNAVAIL_ENCRULES = unchecked((int)(0x80093017)),

        /// <summary>
        /// OSS ASN.1 Error: Trace file error.
        /// </summary>
        OSS_CANT_OPEN_TRACE_WINDOW = unchecked((int)(0x80093018)),

        /// <summary>
        /// OSS ASN.1 Error: Function not implemented.
        /// </summary>
        OSS_UNIMPLEMENTED = unchecked((int)(0x80093019)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_OID_DLL_NOT_LINKED = unchecked((int)(0x8009301A)),

        /// <summary>
        /// OSS ASN.1 Error: Trace file error.
        /// </summary>
        OSS_CANT_OPEN_TRACE_FILE = unchecked((int)(0x8009301B)),

        /// <summary>
        /// OSS ASN.1 Error: Trace file error.
        /// </summary>
        OSS_TRACE_FILE_ALREADY_OPEN = unchecked((int)(0x8009301C)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_TABLE_MISMATCH = unchecked((int)(0x8009301D)),

        /// <summary>
        /// OSS ASN.1 Error: Invalid data.
        /// </summary>
        OSS_TYPE_NOT_SUPPORTED = unchecked((int)(0x8009301E)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_REAL_DLL_NOT_LINKED = unchecked((int)(0x8009301F)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_REAL_CODE_NOT_LINKED = unchecked((int)(0x80093020)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_OUT_OF_RANGE = unchecked((int)(0x80093021)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_COPIER_DLL_NOT_LINKED = unchecked((int)(0x80093022)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_CONSTRAINT_DLL_NOT_LINKED = unchecked((int)(0x80093023)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_COMPARATOR_DLL_NOT_LINKED = unchecked((int)(0x80093024)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_COMPARATOR_CODE_NOT_LINKED = unchecked((int)(0x80093025)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_MEM_MGR_DLL_NOT_LINKED = unchecked((int)(0x80093026)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_PDV_DLL_NOT_LINKED = unchecked((int)(0x80093027)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_PDV_CODE_NOT_LINKED = unchecked((int)(0x80093028)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_API_DLL_NOT_LINKED = unchecked((int)(0x80093029)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_BERDER_DLL_NOT_LINKED = unchecked((int)(0x8009302A)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_PER_DLL_NOT_LINKED = unchecked((int)(0x8009302B)),

        /// <summary>
        /// OSS ASN.1 Error: Program link error.
        /// </summary>
        OSS_OPEN_TYPE_ERROR = unchecked((int)(0x8009302C)),

        /// <summary>
        /// OSS ASN.1 Error: System resource error.
        /// </summary>
        OSS_MUTEX_NOT_CREATED = unchecked((int)(0x8009302D)),

        /// <summary>
        /// OSS ASN.1 Error: Trace file error.
        /// </summary>
        OSS_CANT_CLOSE_TRACE_FILE = unchecked((int)(0x8009302E)),

        /// <summary>
        /// ASN1 Certificate encode/decode error code base.
        /// </summary>
        /// <remarks>The ASN1 error values are offset by <see cref="CRYPT_E_ASN1_ERROR"/>.</remarks>
        CRYPT_E_ASN1_ERROR = unchecked((int)(0x80093100)),

        /// <summary>
        /// ASN1 internal encode or decode error.
        /// </summary>
        CRYPT_E_ASN1_INTERNAL = unchecked((int)(0x80093101)),

        /// <summary>
        /// ASN1 unexpected end of data.
        /// </summary>
        CRYPT_E_ASN1_EOD = unchecked((int)(0x80093102)),

        /// <summary>
        /// ASN1 corrupted data.
        /// </summary>
        CRYPT_E_ASN1_CORRUPT = unchecked((int)(0x80093103)),

        /// <summary>
        /// ASN1 value too large.
        /// </summary>
        CRYPT_E_ASN1_LARGE = unchecked((int)(0x80093104)),

        /// <summary>
        /// ASN1 constraint violated.
        /// </summary>
        CRYPT_E_ASN1_CONSTRAINT = unchecked((int)(0x80093105)),

        /// <summary>
        /// ASN1 out of memory.
        /// </summary>
        CRYPT_E_ASN1_MEMORY = unchecked((int)(0x80093106)),

        /// <summary>
        /// ASN1 buffer overflow.
        /// </summary>
        CRYPT_E_ASN1_OVERFLOW = unchecked((int)(0x80093107)),

        /// <summary>
        /// ASN1 function not supported for this PDU.
        /// </summary>
        CRYPT_E_ASN1_BADPDU = unchecked((int)(0x80093108)),

        /// <summary>
        /// ASN1 bad arguments to function call.
        /// </summary>
        CRYPT_E_ASN1_BADARGS = unchecked((int)(0x80093109)),

        /// <summary>
        /// ASN1 bad real value.
        /// </summary>
        CRYPT_E_ASN1_BADREAL = unchecked((int)(0x8009310A)),

        /// <summary>
        /// ASN1 bad tag value met.
        /// </summary>
        CRYPT_E_ASN1_BADTAG = unchecked((int)(0x8009310B)),

        /// <summary>
        /// ASN1 bad choice value.
        /// </summary>
        CRYPT_E_ASN1_CHOICE = unchecked((int)(0x8009310C)),

        /// <summary>
        /// ASN1 bad encoding rule.
        /// </summary>
        CRYPT_E_ASN1_RULE = unchecked((int)(0x8009310D)),

        /// <summary>
        /// ASN1 bad Unicode (UTF8).
        /// </summary>
        CRYPT_E_ASN1_UTF8 = unchecked((int)(0x8009310E)),

        /// <summary>
        /// ASN1 bad PDU type.
        /// </summary>
        CRYPT_E_ASN1_PDU_TYPE = unchecked((int)(0x80093133)),

        /// <summary>
        /// ASN1 not yet implemented.
        /// </summary>
        CRYPT_E_ASN1_NYI = unchecked((int)(0x80093134)),

        /// <summary>
        /// ASN1 skipped unknown extension(s).
        /// </summary>
        CRYPT_E_ASN1_EXTENDED = unchecked((int)(0x80093201)),

        /// <summary>
        /// ASN1 end of data expected
        /// </summary>
        CRYPT_E_ASN1_NOEOD = unchecked((int)(0x80093202)),

        /// <summary>
        /// The request subject name is invalid or too long.
        /// </summary>
        CERTSRV_E_BAD_REQUESTSUBJECT = unchecked((int)(0x80094001)),

        /// <summary>
        /// The request does not exist.
        /// </summary>
        CERTSRV_E_NO_REQUEST = unchecked((int)(0x80094002)),

        /// <summary>
        /// The request's current status does not allow this operation.
        /// </summary>
        CERTSRV_E_BAD_REQUESTSTATUS = unchecked((int)(0x80094003)),

        /// <summary>
        /// The requested property value is empty.
        /// </summary>
        CERTSRV_E_PROPERTY_EMPTY = unchecked((int)(0x80094004)),

        /// <summary>
        /// The certification authority's certificate contains invalid data.
        /// </summary>
        CERTSRV_E_INVALID_CA_CERTIFICATE = unchecked((int)(0x80094005)),

        /// <summary>
        /// Certificate service has been suspended for a database restore operation.
        /// </summary>
        CERTSRV_E_SERVER_SUSPENDED = unchecked((int)(0x80094006)),

        /// <summary>
        /// The certificate contains an encoded length that is potentially incompatible with older enrollment software.
        /// </summary>
        CERTSRV_E_ENCODING_LENGTH = unchecked((int)(0x80094007)),

        /// <summary>
        /// The operation is denied.
        /// </summary>
        /// <remarks>The user has multiple roles assigned and the certification authority is configured to enforce role separation</remarks>
        CERTSRV_E_ROLECONFLICT = unchecked((int)(0x80094008)),

        /// <summary>
        /// The operation is denied.
        /// </summary>
        /// <remarks>It can only be performed by a certificate manager that is allowed to manage certificates for
        /// the current requester.</remarks>
        CERTSRV_E_RESTRICTEDOFFICER = unchecked((int)(0x80094009)),

        /// <summary>
        /// Cannot archive private key. The certification authority is not configured for key archival.
        /// </summary>
        CERTSRV_E_KEY_ARCHIVAL_NOT_CONFIGURED = unchecked((int)(0x8009400A)),

        /// <summary>
        /// Cannot archive private key. The certification authority could not verify one or more key recovery certificates.
        /// </summary>
        CERTSRV_E_NO_VALID_KRA = unchecked((int)(0x8009400B)),

        /// <summary>
        /// The request is incorrectly formatted. The encrypted private key must be in an unauthenticated attribute in an outermost signature.
        /// </summary>
        /// <remarks></remarks>
        CERTSRV_E_BAD_REQUEST_KEY_ARCHIVAL = unchecked((int)(0x8009400C)),

        /// <summary>
        /// At least one security principal must have the permission to manage this CA.
        /// </summary>
        CERTSRV_E_NO_CAADMIN_DEFINED = unchecked((int)(0x8009400D)),

        /// <summary>
        /// The request contains an invalid renewal certificate attribute.
        /// </summary>
        CERTSRV_E_BAD_RENEWAL_CERT_ATTRIBUTE = unchecked((int)(0x8009400E)),

        /// <summary>
        /// An attempt was made to open a Certification Authority database session, but there are already too many active sessions.
        /// The server may need to be configured to allow additional sessions.
        /// </summary>
        CERTSRV_E_NO_DB_SESSIONS = unchecked((int)(0x8009400F)),

        /// <summary>
        /// A memory reference caused a data alignment fault.
        /// </summary>
        CERTSRV_E_ALIGNMENT_FAULT = unchecked((int)(0x80094010)),

        /// <summary>
        /// The permissions on this certification authority do not allow the current user to enroll for certificates.
        /// </summary>
        CERTSRV_E_ENROLL_DENIED = unchecked((int)(0x80094011)),

        /// <summary>
        /// The permissions on the certificate template do not allow the current user to enroll for this type of certificate.
        /// </summary>
        CERTSRV_E_TEMPLATE_DENIED = unchecked((int)(0x80094012)),

        /// <summary>
        /// No information available.
        /// </summary>
        CERTSRV_E_DOWNLEVEL_DC_SSL_OR_UPGRADE = unchecked((int)(0x80094013)),

        /// <summary>
        /// The requested certificate template is not supported by this CA.
        /// </summary>
        CERTSRV_E_UNSUPPORTED_CERT_TYPE = unchecked((int)(0x80094800)),

        /// <summary>
        /// The request contains no certificate template information.
        /// </summary>
        CERTSRV_E_NO_CERT_TYPE = unchecked((int)(0x80094801)),

        /// <summary>
        /// The request contains conflicting template information.
        /// </summary>
        CERTSRV_E_TEMPLATE_CONFLICT = unchecked((int)(0x80094802)),

        /// <summary>
        /// The request is missing a required Subject Alternate name extension.
        /// </summary>
        CERTSRV_E_SUBJECT_ALT_NAME_REQUIRED = unchecked((int)(0x80094803)),

        /// <summary>
        /// The request is missing a required private key for archival by the server.
        /// </summary>
        CERTSRV_E_ARCHIVED_KEY_REQUIRED = unchecked((int)(0x80094804)),

        /// <summary>
        /// The request is missing a required SMIME capabilities extension.
        /// </summary>
        CERTSRV_E_SMIME_REQUIRED = unchecked((int)(0x80094805)),

        /// <summary>
        /// The request was made on behalf of a subject other than the caller. The certificate template must be configured to
        /// require at least one signature to authorize the request.
        /// </summary>
        CERTSRV_E_BAD_RENEWAL_SUBJECT = unchecked((int)(0x80094806)),

        /// <summary>
        /// The request template version is newer than the supported template version.
        /// </summary>
        CERTSRV_E_BAD_TEMPLATE_VERSION = unchecked((int)(0x80094807)),

        /// <summary>
        /// The template is missing a required signature policy attribute.
        /// </summary>
        CERTSRV_E_TEMPLATE_POLICY_REQUIRED = unchecked((int)(0x80094808)),

        /// <summary>
        /// The request is missing required signature policy information.
        /// </summary>
        CERTSRV_E_SIGNATURE_POLICY_REQUIRED = unchecked((int)(0x80094809)),

        /// <summary>
        /// The request is missing one or more required signatures.
        /// </summary>
        CERTSRV_E_SIGNATURE_COUNT = unchecked((int)(0x8009480A)),

        /// <summary>
        /// One or more signatures did not include the required application or issuance policies. The request is missing one or more
        /// required valid signatures.
        /// </summary>
        CERTSRV_E_SIGNATURE_REJECTED = unchecked((int)(0x8009480B)),

        /// <summary>
        /// The request is missing one or more required signature issuance policies.
        /// </summary>
        CERTSRV_E_ISSUANCE_POLICY_REQUIRED = unchecked((int)(0x8009480C)),

        /// <summary>
        /// The UPN is unavailable and cannot be added to the Subject Alternate name.
        /// </summary>
        CERTSRV_E_SUBJECT_UPN_REQUIRED = unchecked((int)(0x8009480D)),

        /// <summary>
        /// The Active Directory GUID is unavailable and cannot be added to the Subject Alternate name.
        /// </summary>
        CERTSRV_E_SUBJECT_DIRECTORY_GUID_REQUIRED = unchecked((int)(0x8009480E)),

        /// <summary>
        /// The DNS name is unavailable and cannot be added to the Subject Alternate name.
        /// </summary>
        CERTSRV_E_SUBJECT_DNS_REQUIRED = unchecked((int)(0x8009480F)),

        /// <summary>
        /// The request includes a private key for archival by the server, but key archival is not enabled for the specified
        /// certificate template.
        /// </summary>
        CERTSRV_E_ARCHIVED_KEY_UNEXPECTED = unchecked((int)(0x80094810)),

        /// <summary>
        /// The public key does not meet the minimum size required by the specified certificate template.
        /// </summary>
        CERTSRV_E_KEY_LENGTH = unchecked((int)(0x80094811)),

        /// <summary>
        /// No information available.
        /// </summary>
        CERTSRV_E_SUBJECT_EMAIL_REQUIRED = unchecked((int)(0x80094812)),

        /// <summary>
        /// No information available.
        /// </summary>
        CERTSRV_E_UNKNOWN_CERT_TYPE = unchecked((int)(0x80094813)),

        /// <summary>
        /// No information available.
        /// </summary>
        CERTSRV_E_CERT_TYPE_OVERLAP = unchecked((int)(0x80094814)),

        /// <summary>
        /// The key is not exportable.
        /// </summary>
        XENROLL_E_KEY_NOT_EXPORTABLE = unchecked((int)(0x80095000)),

        /// <summary>
        /// You cannot add the root CA certificate into your local store.
        /// </summary>
        XENROLL_E_CANNOT_ADD_ROOT_CERT = unchecked((int)(0x80095001)),

        /// <summary>
        /// The key archival hash attribute was not found in the response.
        /// </summary>
        XENROLL_E_RESPONSE_KA_HASH_NOT_FOUND = unchecked((int)(0x80095002)),

        /// <summary>
        /// An unexpetced key archival hash attribute was found in the response.
        /// </summary>
        XENROLL_E_RESPONSE_UNEXPECTED_KA_HASH = unchecked((int)(0x80095003)),

        /// <summary>
        /// There is a key archival hash mismatch between the request and the response.
        /// </summary>
        XENROLL_E_RESPONSE_KA_HASH_MISMATCH = unchecked((int)(0x80095004)),

        /// <summary>
        /// Signing certificate cannot include SMIME extension.
        /// </summary>
        XENROLL_E_KEYSPEC_SMIME_MISMATCH = unchecked((int)(0x80095005)),

        /// <summary>
        /// A system-level error occurred while verifying trust.
        /// </summary>
        TRUST_E_SYSTEM_ERROR = unchecked((int)(0x80096001)),

        /// <summary>
        /// The certificate for the signer of the message is invalid or not found.
        /// </summary>
        TRUST_E_NO_SIGNER_CERT = unchecked((int)(0x80096002)),

        /// <summary>
        /// One of the counter signatures was invalid.
        /// </summary>
        TRUST_E_COUNTER_SIGNER = unchecked((int)(0x80096003)),

        /// <summary>
        /// The signature of the certificate can not be verified.
        /// </summary>
        TRUST_E_CERT_SIGNATURE = unchecked((int)(0x80096004)),

        /// <summary>
        /// The timestamp signature and/or certificate could not be verified or is malformed.
        /// </summary>
        TRUST_E_TIME_STAMP = unchecked((int)(0x80096005)),

        /// <summary>
        /// The digital signature of the object did not verify.
        /// </summary>
        TRUST_E_BAD_DIGEST = unchecked((int)(0x80096010)),

        /// <summary>
        /// A certificate's basic constraint extension has not been observed.
        /// </summary>
        TRUST_E_BASIC_CONSTRAINTS = unchecked((int)(0x80096019)),

        /// <summary>
        /// The certificate does not meet or contain the Authenticode financial extensions.
        /// </summary>
        TRUST_E_FINANCIAL_CRITERIA = unchecked((int)(0x8009601E)),

        /// <summary>
        /// Tried to reference a part of the file outside the proper range.
        /// </summary>
        MSSIPOTF_E_OUTOFMEMRANGE = unchecked((int)(0x80097001)),

        /// <summary>
        /// Could not retrieve an object from the file.
        /// </summary>
        MSSIPOTF_E_CANTGETOBJECT = unchecked((int)(0x80097002)),

        /// <summary>
        /// Could not find the head table in the file.
        /// </summary>
        MSSIPOTF_E_NOHEADTABLE = unchecked((int)(0x80097003)),

        /// <summary>
        /// The magic number in the head table is incorrect.
        /// </summary>
        MSSIPOTF_E_BAD_MAGICNUMBER = unchecked((int)(0x80097004)),

        /// <summary>
        /// The offset table has incorrect values.
        /// </summary>
        MSSIPOTF_E_BAD_OFFSET_TABLE = unchecked((int)(0x80097005)),

        /// <summary>
        /// Duplicate table tags or tags out of alphabetical order.
        /// </summary>
        MSSIPOTF_E_TABLE_TAGORDER = unchecked((int)(0x80097006)),

        /// <summary>
        /// A table does not start on a long word boundary.
        /// </summary>
        MSSIPOTF_E_TABLE_Int32UInt16 = unchecked((int)(0x80097007)),

        /// <summary>
        /// First table does not appear after header information.
        /// </summary>
        MSSIPOTF_E_BAD_FIRST_TABLE_PLACEMENT = unchecked((int)(0x80097008)),

        /// <summary>
        /// Two or more tables overlap.
        /// </summary>
        MSSIPOTF_E_TABLES_OVERLAP = unchecked((int)(0x80097009)),

        /// <summary>
        /// Too many pad bytes between tables or pad bytes are not 0.
        /// </summary>
        MSSIPOTF_E_TABLE_PADBYTES = unchecked((int)(0x8009700A)),

        /// <summary>
        /// File is too small to contain the last table.
        /// </summary>
        MSSIPOTF_E_FILETOOSMALL = unchecked((int)(0x8009700B)),

        /// <summary>
        /// A table checksum is incorrect.
        /// </summary>
        MSSIPOTF_E_TABLE_CHECKSUM = unchecked((int)(0x8009700C)),

        /// <summary>
        /// The file checksum is incorrect.
        /// </summary>
        MSSIPOTF_E_FILE_CHECKSUM = unchecked((int)(0x8009700D)),

        /// <summary>
        /// The signature does not have the correct attributes for the policy.
        /// </summary>
        MSSIPOTF_E_FAILED_POLICY = unchecked((int)(0x80097010)),

        /// <summary>
        /// The file did not pass the hints check.
        /// </summary>
        MSSIPOTF_E_FAILED_HINTS_CHECK = unchecked((int)(0x80097011)),

        /// <summary>
        /// The file is not an OpenType file.
        /// </summary>
        MSSIPOTF_E_NOT_OPENTYPE = unchecked((int)(0x80097012)),

        /// <summary>
        /// Failed on a file operation (open, map, read, write).
        /// </summary>
        MSSIPOTF_E_FILE = unchecked((int)(0x80097013)),

        /// <summary>
        /// A call to a CryptoAPI function failed.
        /// </summary>
        MSSIPOTF_E_CRYPT = unchecked((int)(0x80097014)),

        /// <summary>
        /// There is a bad version number in the file.
        /// </summary>
        MSSIPOTF_E_BADVERSION = unchecked((int)(0x80097015)),

        /// <summary>
        /// The structure of the DSIG table is incorrect.
        /// </summary>
        MSSIPOTF_E_DSIG_STRUCTURE = unchecked((int)(0x80097016)),

        /// <summary>
        /// A check failed in a partially constant table.
        /// </summary>
        MSSIPOTF_E_PCONST_CHECK = unchecked((int)(0x80097017)),

        /// <summary>
        /// Some kind of structural error.
        /// </summary>
        MSSIPOTF_E_STRUCTURE = unchecked((int)(0x80097018)),

        /// <summary>
        /// Unknown trust provider.
        /// </summary>
        TRUST_E_PROVIDER_UNKNOWN = unchecked((int)(0x800B0001)),

        /// <summary>
        /// The trust verification action specified is not supported by the specified trust provider.
        /// </summary>
        TRUST_E_ACTION_UNKNOWN = unchecked((int)(0x800B0002)),

        /// <summary>
        /// The form specified for the subject is not one supported or known by the specified trust provider.
        /// </summary>
        TRUST_E_SUBJECT_FORM_UNKNOWN = unchecked((int)(0x800B0003)),

        /// <summary>
        /// The subject is not trusted for the specified action.
        /// </summary>
        TRUST_E_SUBJECT_NOT_TRUSTED = unchecked((int)(0x800B0004)),

        /// <summary>
        /// Error due to problem in ASN.1 encoding process.
        /// </summary>
        DIGSIG_E_ENCODE = unchecked((int)(0x800B0005)),

        /// <summary>
        /// Error due to problem in ASN.1 decoding process.
        /// </summary>
        DIGSIG_E_DECODE = unchecked((int)(0x800B0006)),

        /// <summary>
        /// Reading / writing Extensions where Attributes are appropriate, and visa versa.
        /// </summary>
        DIGSIG_E_EXTENSIBILITY = unchecked((int)(0x800B0007)),

        /// <summary>
        /// Unspecified cryptographic failure.
        /// </summary>
        DIGSIG_E_CRYPTO = unchecked((int)(0x800B0008)),

        /// <summary>
        /// The size of the data could not be determined.
        /// </summary>
        PERSIST_E_SIZEDEFINITE = unchecked((int)(0x800B0009)),

        /// <summary>
        /// The size of the indefinite-sized data could not be determined.
        /// </summary>
        PERSIST_E_SIZEINDEFINITE = unchecked((int)(0x800B000A)),

        /// <summary>
        /// This object does not read and write self-sizing data.
        /// </summary>
        PERSIST_E_NOTSELFSIZING = unchecked((int)(0x800B000B)),

        /// <summary>
        /// No signature was present in the subject.
        /// </summary>
        TRUST_E_NOSIGNATURE = unchecked((int)(0x800B0100)),

        /// <summary>
        /// Generic trust failure.
        /// </summary>
        TRUST_E_FAIL = unchecked((int)(0x800B010B)),

        /// <summary>
        /// A certificate was explicitly revoked by its issuer.
        /// </summary>
        CERT_E_REVOKED = unchecked((int)(0x800B010C)),

        /// <summary>
        /// The certification path terminates with the test root which is not trusted with the current policy settings.
        /// </summary>
        CERT_E_UNTRUSTEDTESTROOT = unchecked((int)(0x800B010D)),

        /// <summary>
        /// The revocation process could not continue - the certificate(s) could not be checked.
        /// </summary>
        CERT_E_REVOCATION_FAILURE = unchecked((int)(0x800B010E)),

        /// <summary>
        /// The certificate's CN name does not match the passed value.
        /// </summary>
        CERT_E_CN_NO_MATCH = unchecked((int)(0x800B010F)),

        /// <summary>
        /// The certificate is not valid for the requested usage.
        /// </summary>
        CERT_E_WRONG_USAGE = unchecked((int)(0x800B0110)),

        /// <summary>
        /// The certificate was explicitly marked as untrusted by the user.
        /// </summary>
        TRUST_E_EXPLICIT_DISTRUST = unchecked((int)(0x800B0111)),

        /// <summary>
        /// A certification chain processed correctly, but one of the CA certificates is not trusted by the policy provider.
        /// </summary>
        CERT_E_UNTRUSTEDCA = unchecked((int)(0x800B0112)),

        /// <summary>
        /// The certificate has invalid policy.
        /// </summary>
        CERT_E_INVALID_POLICY = unchecked((int)(0x800B0113)),

        /// <summary>
        /// The certificate has an invalid name. The name is not included in the permitted list or is explicitly excluded.
        /// </summary>
        CERT_E_INVALID_NAME = unchecked((int)(0x800B0114)),

        /// <summary>
        /// An internal consistency check failed.
        /// </summary>
        SCARD_F_INTERNAL_ERROR = unchecked((int)(0x80100001)),

        /// <summary>
        /// The action was cancelled by an SCardCancel request.
        /// </summary>
        SCARD_E_CANCELLED = unchecked((int)(0x80100002)),

        /// <summary>
        /// The supplied handle was invalid.
        /// </summary>
        SCARD_E_INVALID_HANDLE = unchecked((int)(0x80100003)),

        /// <summary>
        /// One or more of the supplied parameters could not be properly interpreted.
        /// </summary>
        SCARD_E_INVALID_PARAMETER = unchecked((int)(0x80100004)),

        /// <summary>
        /// Registry startup information is missing or invalid.
        /// </summary>
        SCARD_E_INVALID_TARGET = unchecked((int)(0x80100005)),

        /// <summary>
        /// Not enough memory available to complete this command.
        /// </summary>
        SCARD_E_NO_MEMORY = unchecked((int)(0x80100006)),

        /// <summary>
        /// An internal consistency timer has expired.
        /// </summary>
        SCARD_F_WAITED_TOO_Int32 = unchecked((int)(0x80100007)),

        /// <summary>
        /// The data buffer to receive returned data is too small for the returned data.
        /// </summary>
        SCARD_E_INSUFFICIENT_BUFFER = unchecked((int)(0x80100008)),

        /// <summary>
        /// The specified reader name is not recognized.
        /// </summary>
        SCARD_E_UNKNOWN_READER = unchecked((int)(0x80100009)),

        /// <summary>
        /// The user-specified timeout value has expired.
        /// </summary>
        SCARD_E_TIMEOUT = unchecked((int)(0x8010000A)),

        /// <summary>
        /// The smart card cannot be accessed because of other connections outstanding.
        /// </summary>
        SCARD_E_SHARING_VIOLATION = unchecked((int)(0x8010000B)),

        /// <summary>
        /// The operation requires a Smart Card, but no Smart Card is currently in the device.
        /// </summary>
        SCARD_E_NO_SMARTCARD = unchecked((int)(0x8010000C)),

        /// <summary>
        /// The specified smart card name is not recognized.
        /// </summary>
        SCARD_E_UNKNOWN_CARD = unchecked((int)(0x8010000D)),

        /// <summary>
        /// The system could not dispose of the media in the requested manner.
        /// </summary>
        SCARD_E_CANT_DISPOSE = unchecked((int)(0x8010000E)),

        /// <summary>
        /// The requested protocols are incompatible with the protocol currently in use with the smart card.
        /// </summary>
        SCARD_E_PROTO_MISMATCH = unchecked((int)(0x8010000F)),

        /// <summary>
        /// The reader or smart card is not ready to accept commands.
        /// </summary>
        SCARD_E_NOT_READY = unchecked((int)(0x80100010)),

        /// <summary>
        /// One or more of the supplied parameters values could not be properly interpreted.
        /// </summary>
        SCARD_E_INVALID_VALUE = unchecked((int)(0x80100011)),

        /// <summary>
        /// The action was cancelled by the system, presumably to log off or shut down.
        /// </summary>
        SCARD_E_SYSTEM_CANCELLED = unchecked((int)(0x80100012)),

        /// <summary>
        /// An internal communications error has been detected.
        /// </summary>
        SCARD_F_COMM_ERROR = unchecked((int)(0x80100013)),

        /// <summary>
        /// An internal error has been detected, but the source is unknown.
        /// </summary>
        SCARD_F_UNKNOWN_ERROR = unchecked((int)(0x80100014)),

        /// <summary>
        /// An ATR obtained from the registry is not a valid ATR string.
        /// </summary>
        SCARD_E_INVALID_ATR = unchecked((int)(0x80100015)),

        /// <summary>
        /// An attempt was made to end a non-existent transaction.
        /// </summary>
        SCARD_E_NOT_TRANSACTED = unchecked((int)(0x80100016)),

        /// <summary>
        /// The specified reader is not currently available for use.
        /// </summary>
        SCARD_E_READER_UNAVAILABLE = unchecked((int)(0x80100017)),

        /// <summary>
        /// The operation has been aborted to allow the server application to exit.
        /// </summary>
        SCARD_P_SHUTDOWN = unchecked((int)(0x80100018)),

        /// <summary>
        /// The PCI Receive buffer was too small.
        /// </summary>
        SCARD_E_PCI_TOO_SMALL = unchecked((int)(0x80100019)),

        /// <summary>
        /// The reader driver does not meet minimal requirements for support.
        /// </summary>
        SCARD_E_READER_UNSUPPORTED = unchecked((int)(0x8010001A)),

        /// <summary>
        /// The reader driver did not produce a unique reader name.
        /// </summary>
        SCARD_E_DUPLICATE_READER = unchecked((int)(0x8010001B)),

        /// <summary>
        /// The smart card does not meet minimal requirements for support.
        /// </summary>
        SCARD_E_CARD_UNSUPPORTED = unchecked((int)(0x8010001C)),

        /// <summary>
        /// The Smart card resource manager is not running.
        /// </summary>
        SCARD_E_NO_SERVICE = unchecked((int)(0x8010001D)),

        /// <summary>
        /// The Smart card resource manager has shut down.
        /// </summary>
        SCARD_E_SERVICE_STOPPED = unchecked((int)(0x8010001E)),

        /// <summary>
        /// An unexpected card error has occurred.
        /// </summary>
        SCARD_E_UNEXPECTED = unchecked((int)(0x8010001F)),

        /// <summary>
        /// No Primary Provider can be found for the smart card.
        /// </summary>
        SCARD_E_ICC_INSTALLATION = unchecked((int)(0x80100020)),

        /// <summary>
        /// The requested order of object creation is not supported.
        /// </summary>
        SCARD_E_ICC_CREATEORDER = unchecked((int)(0x80100021)),

        /// <summary>
        /// This smart card does not support the requested feature.
        /// </summary>
        SCARD_E_UNSUPPORTED_FEATURE = unchecked((int)(0x80100022)),

        /// <summary>
        /// The identified directory does not exist in the smart card.
        /// </summary>
        SCARD_E_DIR_NOT_FOUND = unchecked((int)(0x80100023)),

        /// <summary>
        /// The identified file does not exist in the smart card.
        /// </summary>
        SCARD_E_FILE_NOT_FOUND = unchecked((int)(0x80100024)),

        /// <summary>
        /// The supplied path does not represent a smart card directory.
        /// </summary>
        SCARD_E_NO_DIR = unchecked((int)(0x80100025)),

        /// <summary>
        /// The supplied path does not represent a smart card file.
        /// </summary>
        SCARD_E_NO_FILE = unchecked((int)(0x80100026)),

        /// <summary>
        /// Access is denied to this file.
        /// </summary>
        SCARD_E_NO_ACCESS = unchecked((int)(0x80100027)),

        /// <summary>
        /// The smartcard does not have enough memory to store the information.
        /// </summary>
        SCARD_E_WRITE_TOO_MANY = unchecked((int)(0x80100028)),

        /// <summary>
        /// There was an error trying to set the smart card file object pointer.
        /// </summary>
        SCARD_E_BAD_SEEK = unchecked((int)(0x80100029)),

        /// <summary>
        /// The supplied PIN is incorrect.
        /// </summary>
        SCARD_E_INVALID_CHV = unchecked((int)(0x8010002A)),

        /// <summary>
        /// An unrecognized error code was returned from a layered component.
        /// </summary>
        SCARD_E_UNKNOWN_RES_MNG = unchecked((int)(0x8010002B)),

        /// <summary>
        /// The requested certificate does not exist.
        /// </summary>
        SCARD_E_NO_SUCH_CERTIFICATE = unchecked((int)(0x8010002C)),

        /// <summary>
        /// The requested certificate could not be obtained.
        /// </summary>
        SCARD_E_CERTIFICATE_UNAVAILABLE = unchecked((int)(0x8010002D)),

        /// <summary>
        /// Cannot find a smart card reader.
        /// </summary>
        SCARD_E_NO_READERS_AVAILABLE = unchecked((int)(0x8010002E)),

        /// <summary>
        /// A communications error with the smart card has been detected. Retry the operation.
        /// </summary>
        SCARD_E_COMM_DATA_LOST = unchecked((int)(0x8010002F)),

        /// <summary>
        /// The requested key container does not exist on the smart card.
        /// </summary>
        SCARD_E_NO_KEY_CONTAINER = unchecked((int)(0x80100030)),

        /// <summary>
        /// No information available.
        /// </summary>
        SCARD_E_SERVER_TOO_BUSY = unchecked((int)(0x80100031)),

        /// <summary>
        /// The reader cannot communicate with the smart card, due to ATR configuration conflicts.
        /// </summary>
        SCARD_W_UNSUPPORTED_CARD = unchecked((int)(0x80100065)),

        /// <summary>
        /// The smart card is not responding to a reset.
        /// </summary>
        SCARD_W_UNRESPONSIVE_CARD = unchecked((int)(0x80100066)),

        /// <summary>
        /// Power has been removed from the smart card, so that further communication is not possible.
        /// </summary>
        SCARD_W_UNPOWERED_CARD = unchecked((int)(0x80100067)),

        /// <summary>
        /// The smart card has been reset, so any shared state information is invalid.
        /// </summary>
        SCARD_W_RESET_CARD = unchecked((int)(0x80100068)),

        /// <summary>
        /// The smart card has been removed, so that further communication is not possible.
        /// </summary>
        SCARD_W_REMOVED_CARD = unchecked((int)(0x80100069)),

        /// <summary>
        /// Access was denied because of a security violation.
        /// </summary>
        SCARD_W_SECURITY_VIOLATION = unchecked((int)(0x8010006A)),

        /// <summary>
        /// The card cannot be accessed because the wrong PIN was presented.
        /// </summary>
        SCARD_W_WRONG_CHV = unchecked((int)(0x8010006B)),

        /// <summary>
        /// The card cannot be accessed because the maximum number of PIN entry attempts has been reached.
        /// </summary>
        SCARD_W_CHV_BLOCKED = unchecked((int)(0x8010006C)),

        /// <summary>
        /// The end of the smart card file has been reached.
        /// </summary>
        SCARD_W_EOF = unchecked((int)(0x8010006D)),

        /// <summary>
        /// The action was cancelled by the user.
        /// </summary>
        SCARD_W_CANCELLED_BY_USER = unchecked((int)(0x8010006E)),

        /// <summary>
        /// No PIN was presented to the smart card.
        /// </summary>
        SCARD_W_CARD_NOT_AUTHENTICATED = unchecked((int)(0x8010006F)),

        /// <summary>
        /// A non-empty line was encountered in the INF before the start of a section.
        /// </summary>
        SPAPI_E_EXPECTED_SECTION_NAME = unchecked((int)(0x800F0000)),

        /// <summary>
        /// A section name marker in the INF is not complete, or does not exist on a line by itself.
        /// </summary>
        SPAPI_E_BAD_SECTION_NAME_LINE = unchecked((int)(0x800F0001)),

        /// <summary>
        /// An INF section was encountered whose name exceeds the maximum section name length.
        /// </summary>
        SPAPI_E_SECTION_NAME_TOO_Int32 = unchecked((int)(0x800F0002)),

        /// <summary>
        /// The syntax of the INF is invalid.
        /// </summary>
        SPAPI_E_GENERAL_SYNTAX = unchecked((int)(0x800F0003)),

        /// <summary>
        /// The style of the INF is different than what was requested.
        /// </summary>
        SPAPI_E_WRONG_INF_STYLE = unchecked((int)(0x800F0100)),

        /// <summary>
        /// The required section was not found in the INF.
        /// </summary>
        SPAPI_E_SECTION_NOT_FOUND = unchecked((int)(0x800F0101)),

        /// <summary>
        /// The required line was not found in the INF.
        /// </summary>
        SPAPI_E_LINE_NOT_FOUND = unchecked((int)(0x800F0102)),

        /// <summary>
        /// The files affected by the installation of this file queue have not been backed up for uninstall.
        /// </summary>
        SPAPI_E_NO_BACKUP = unchecked((int)(0x800F0103)),

        /// <summary>
        /// The INF or the device information set or element does not have an associated install class.
        /// </summary>
        SPAPI_E_NO_ASSOCIATED_CLASS = unchecked((int)(0x800F0200)),

        /// <summary>
        /// The INF or the device information set or element does not match the specified install class.
        /// </summary>
        SPAPI_E_CLASS_MISMATCH = unchecked((int)(0x800F0201)),

        /// <summary>
        /// An existing device was found that is a duplicate of the device being manually installed.
        /// </summary>
        SPAPI_E_DUPLICATE_FOUND = unchecked((int)(0x800F0202)),

        /// <summary>
        /// There is no driver selected for the device information set or element.
        /// </summary>
        SPAPI_E_NO_DRIVER_SELECTED = unchecked((int)(0x800F0203)),

        /// <summary>
        /// The requested device registry key does not exist.
        /// </summary>
        SPAPI_E_KEY_DOES_NOT_EXIST = unchecked((int)(0x800F0204)),

        /// <summary>
        /// The device instance name is invalid.
        /// </summary>
        SPAPI_E_INVALID_DEVINST_NAME = unchecked((int)(0x800F0205)),

        /// <summary>
        /// The install class is not present or is invalid.
        /// </summary>
        SPAPI_E_INVALID_CLASS = unchecked((int)(0x800F0206)),

        /// <summary>
        /// The device instance cannot be created because it already exists.
        /// </summary>
        SPAPI_E_DEVINST_ALREADY_EXISTS = unchecked((int)(0x800F0207)),

        /// <summary>
        /// The operation cannot be performed on a device information element that has not been registered.
        /// </summary>
        SPAPI_E_DEVINFO_NOT_REGISTERED = unchecked((int)(0x800F0208)),

        /// <summary>
        /// The device property code is invalid.
        /// </summary>
        SPAPI_E_INVALID_REG_PROPERTY = unchecked((int)(0x800F0209)),

        /// <summary>
        /// The INF from which a driver list is to be built does not exist.
        /// </summary>
        SPAPI_E_NO_INF = unchecked((int)(0x800F020A)),

        /// <summary>
        /// The device instance does not exist in the hardware tree.
        /// </summary>
        SPAPI_E_NO_SUCH_DEVINST = unchecked((int)(0x800F020B)),

        /// <summary>
        /// The icon representing this install class cannot be loaded.
        /// </summary>
        SPAPI_E_CANT_LOAD_CLASS_ICON = unchecked((int)(0x800F020C)),

        /// <summary>
        /// The class installer registry entry is invalid.
        /// </summary>
        SPAPI_E_INVALID_CLASS_INSTALLER = unchecked((int)(0x800F020D)),

        /// <summary>
        /// The class installer has indicated that the default action should be performed for this installation request.
        /// </summary>
        SPAPI_E_DI_DO_DEFAULT = unchecked((int)(0x800F020E)),

        /// <summary>
        /// The operation does not require any files to be copied.
        /// </summary>
        SPAPI_E_DI_NOFILECOPY = unchecked((int)(0x800F020F)),

        /// <summary>
        /// The specified hardware profile does not exist.
        /// </summary>
        SPAPI_E_INVALID_HWPROFILE = unchecked((int)(0x800F0210)),

        /// <summary>
        /// There is no device information element currently selected for this device information set.
        /// </summary>
        SPAPI_E_NO_DEVICE_SELECTED = unchecked((int)(0x800F0211)),

        /// <summary>
        /// The operation cannot be performed because the device information set is locked.
        /// </summary>
        SPAPI_E_DEVINFO_LIST_LOCKED = unchecked((int)(0x800F0212)),

        /// <summary>
        /// The operation cannot be performed because the device information element is locked.
        /// </summary>
        SPAPI_E_DEVINFO_DATA_LOCKED = unchecked((int)(0x800F0213)),

        /// <summary>
        /// The specified path does not contain any applicable device INFs.
        /// </summary>
        SPAPI_E_DI_BAD_PATH = unchecked((int)(0x800F0214)),

        /// <summary>
        /// No class installer parameters have been set for the device information set or element.
        /// </summary>
        SPAPI_E_NO_CLASSINSTALL_PARAMS = unchecked((int)(0x800F0215)),

        /// <summary>
        /// The operation cannot be performed because the file queue is locked.
        /// </summary>
        SPAPI_E_FILEQUEUE_LOCKED = unchecked((int)(0x800F0216)),

        /// <summary>
        /// A service installation section in this INF is invalid.
        /// </summary>
        SPAPI_E_BAD_SERVICE_INSTALLSECT = unchecked((int)(0x800F0217)),

        /// <summary>
        /// There is no class driver list for the device information element.
        /// </summary>
        SPAPI_E_NO_CLASS_DRIVER_LIST = unchecked((int)(0x800F0218)),

        /// <summary>
        /// The installation failed because a function driver was not specified for this device instance.
        /// </summary>
        SPAPI_E_NO_ASSOCIATED_SERVICE = unchecked((int)(0x800F0219)),

        /// <summary>
        /// There is presently no default device interface designated for this interface class.
        /// </summary>
        SPAPI_E_NO_DEFAULT_DEVICE_INTERFACE = unchecked((int)(0x800F021A)),

        /// <summary>
        /// The operation cannot be performed because the device interface is currently active.
        /// </summary>
        SPAPI_E_DEVICE_INTERFACE_ACTIVE = unchecked((int)(0x800F021B)),

        /// <summary>
        /// The operation cannot be performed because the device interface has been removed from the system.
        /// </summary>
        SPAPI_E_DEVICE_INTERFACE_REMOVED = unchecked((int)(0x800F021C)),

        /// <summary>
        /// An interface installation section in this INF is invalid.
        /// </summary>
        SPAPI_E_BAD_INTERFACE_INSTALLSECT = unchecked((int)(0x800F021D)),

        /// <summary>
        /// This interface class does not exist in the system.
        /// </summary>
        SPAPI_E_NO_SUCH_INTERFACE_CLASS = unchecked((int)(0x800F021E)),

        /// <summary>
        /// The reference string supplied for this interface device is invalid.
        /// </summary>
        SPAPI_E_INVALID_REFERENCE_STRING = unchecked((int)(0x800F021F)),

        /// <summary>
        /// The specified machine name does not conform to UNC naming conventions.
        /// </summary>
        SPAPI_E_INVALID_MACHINENAME = unchecked((int)(0x800F0220)),

        /// <summary>
        /// A general remote communication error occurred.
        /// </summary>
        SPAPI_E_REMOTE_COMM_FAILURE = unchecked((int)(0x800F0221)),

        /// <summary>
        /// The machine selected for remote communication is not available at this time.
        /// </summary>
        SPAPI_E_MACHINE_UNAVAILABLE = unchecked((int)(0x800F0222)),

        /// <summary>
        /// The Plug and Play service is not available on the remote machine.
        /// </summary>
        SPAPI_E_NO_CONFIGMGR_SERVICES = unchecked((int)(0x800F0223)),

        /// <summary>
        /// The property page provider registry entry is invalid.
        /// </summary>
        SPAPI_E_INVALID_PROPPAGE_PROVIDER = unchecked((int)(0x800F0224)),

        /// <summary>
        /// The requested device interface is not present in the system.
        /// </summary>
        SPAPI_E_NO_SUCH_DEVICE_INTERFACE = unchecked((int)(0x800F0225)),

        /// <summary>
        /// The device's co-installer has additional work to perform after installation is complete.
        /// </summary>
        SPAPI_E_DI_POSTPROCESSING_REQUIRED = unchecked((int)(0x800F0226)),

        /// <summary>
        /// The device's co-installer is invalid.
        /// </summary>
        SPAPI_E_INVALID_COINSTALLER = unchecked((int)(0x800F0227)),

        /// <summary>
        /// There are no compatible drivers for this device.
        /// </summary>
        SPAPI_E_NO_COMPAT_DRIVERS = unchecked((int)(0x800F0228)),

        /// <summary>
        /// There is no icon that represents this device or device type.
        /// </summary>
        SPAPI_E_NO_DEVICE_ICON = unchecked((int)(0x800F0229)),

        /// <summary>
        /// A logical configuration specified in this INF is invalid.
        /// </summary>
        SPAPI_E_INVALID_INF_LOGCONFIG = unchecked((int)(0x800F022A)),

        /// <summary>
        /// The class installer has denied the request to install or upgrade this device.
        /// </summary>
        SPAPI_E_DI_DONT_INSTALL = unchecked((int)(0x800F022B)),

        /// <summary>
        /// One of the filter drivers installed for this device is invalid.
        /// </summary>
        SPAPI_E_INVALID_FILTER_DRIVER = unchecked((int)(0x800F022C)),

        /// <summary>
        /// The driver selected for this device does not support Windows XP.
        /// </summary>
        SPAPI_E_NON_WINDOWS_NT_DRIVER = unchecked((int)(0x800F022D)),

        /// <summary>
        /// The driver selected for this device does not support Windows.
        /// </summary>
        SPAPI_E_NON_WINDOWS_DRIVER = unchecked((int)(0x800F022E)),

        /// <summary>
        /// The third-party INF does not contain digital signature information.
        /// </summary>
        SPAPI_E_NO_CATALOG_FOR_OEM_INF = unchecked((int)(0x800F022F)),

        /// <summary>
        /// An invalid attempt was made to use a device installation file queue for verification of digital signatures relative to
        /// other platforms.
        /// </summary>
        SPAPI_E_DEVINSTALL_QUEUE_NONNATIVE = unchecked((int)(0x800F0230)),

        /// <summary>
        /// The device cannot be disabled.
        /// </summary>
        SPAPI_E_NOT_DISABLEABLE = unchecked((int)(0x800F0231)),

        /// <summary>
        /// The device could not be dynamically removed.
        /// </summary>
        SPAPI_E_CANT_REMOVE_DEVINST = unchecked((int)(0x800F0232)),

        /// <summary>
        /// Cannot copy to specified target.
        /// </summary>
        SPAPI_E_INVALID_TARGET = unchecked((int)(0x800F0233)),

        /// <summary>
        /// Driver is not intended for this platform.
        /// </summary>
        SPAPI_E_DRIVER_NONNATIVE = unchecked((int)(0x800F0234)),

        /// <summary>
        /// Operation not allowed in WOW64.
        /// </summary>
        SPAPI_E_IN_WOW64 = unchecked((int)(0x800F0235)),

        /// <summary>
        /// The operation involving unsigned file copying was rolled back, so that a system restore point could be set.
        /// </summary>
        SPAPI_E_SET_SYSTEM_RESTORE_POINT = unchecked((int)(0x800F0236)),

        /// <summary>
        /// An INF was copied into the Windows INF directory in an improper manner.
        /// </summary>
        SPAPI_E_INCORRECTLY_COPIED_INF = unchecked((int)(0x800F0237)),

        /// <summary>
        /// The Security Configuration Editor (SCE) APIs have been disabled on this Embedded product.
        /// </summary>
        SPAPI_E_SCE_DISABLED = unchecked((int)(0x800F0238)),

        /// <summary>
        /// No installed components were detected.
        /// </summary>
        SPAPI_E_ERROR_NOT_INSTALLED = unchecked((int)(0x800F1000)),

        /// <summary>
        /// Only <c>COM</c>+ Applications marked "queued" can be invoked using the "queue" moniker
        /// </summary>
        COMQC_E_APPLICATION_NOT_QUEUED = unchecked((int)(0x80110600)),

        /// <summary>
        /// At least one interface must be marked "queued" in order to create a queued component instance with the "queue" moniker
        /// </summary>
        COMQC_E_NO_QUEUEABLE_INTERFACES = unchecked((int)(0x80110601)),

        /// <summary>
        /// MSMQ is required for the requested operation and is not installed
        /// </summary>
        COMQC_E_QUEUING_SERVICE_NOT_AVAILABLE = unchecked((int)(0x80110602)),

        /// <summary>
        /// Unable to marshal an interface that does not support IPersistStream
        /// </summary>
        COMQC_E_NO_IPERSISTSTREAM = unchecked((int)(0x80110603)),

        /// <summary>
        /// The message is improperly formatted or was damaged in transit
        /// </summary>
        COMQC_E_BAD_MESSAGE = unchecked((int)(0x80110604)),

        /// <summary>
        /// An unauthenticated message was received by an application that accepts only authenticated messages
        /// </summary>
        COMQC_E_UNAUTHENTICATED = unchecked((int)(0x80110605)),

        /// <summary>
        /// The message was requeued or moved by a user not in the "QC Trusted User" role
        /// </summary>
        COMQC_E_UNTRUSTED_ENQUEUER = unchecked((int)(0x80110606)),

        /// <summary>
        /// Cannot create a duplicate resource of type Distributed Transaction Coordinator
        /// </summary>
        MSDTC_E_DUPLICATE_RESOURCE = unchecked((int)(0x80110701)),

        /// <summary>
        /// One of the objects being inserted or updated does not belong to a valid parent collection
        /// </summary>
        COMADMIN_E_OBJECT_PARENT_MISSING = unchecked((int)(0x80110808)),

        /// <summary>
        /// One of the specified objects cannot be found
        /// </summary>
        COMADMIN_E_OBJECT_DOES_NOT_EXIST = unchecked((int)(0x80110809)),

        /// <summary>
        /// The specified application is not currently running
        /// </summary>
        COMADMIN_E_APP_NOT_RUNNING = unchecked((int)(0x8011080A)),

        /// <summary>
        /// The partition(s) specified are not valid.
        /// </summary>
        COMADMIN_E_INVALID_PARTITION = unchecked((int)(0x8011080B)),

        /// <summary>
        /// <c>COM</c>+ applications that run as NT service may not be pooled or recycled
        /// </summary>
        COMADMIN_E_SVCAPP_NOT_POOLABLE_OR_RECYCLABLE = unchecked((int)(0x8011080D)),

        /// <summary>
        /// One or more users are already assigned to a local partition set.
        /// </summary>
        COMADMIN_E_USER_IN_SET = unchecked((int)(0x8011080E)),

        /// <summary>
        /// Library applications may not be recycled.
        /// </summary>
        COMADMIN_E_CANTRECYCLELIBRARYAPPS = unchecked((int)(0x8011080F)),

        /// <summary>
        /// Applications running as NT services may not be recycled.
        /// </summary>
        COMADMIN_E_CANTRECYCLESERVICEAPPS = unchecked((int)(0x80110811)),

        /// <summary>
        /// The process has already been recycled.
        /// </summary>
        COMADMIN_E_PROCESSALREADYRECYCLED = unchecked((int)(0x80110812)),

        /// <summary>
        /// A paused process may not be recycled.
        /// </summary>
        COMADMIN_E_PAUSEDPROCESSMAYNOTBERECYCLED = unchecked((int)(0x80110813)),

        /// <summary>
        /// Library applications may not be NT services.
        /// </summary>
        COMADMIN_E_CANTMAKEINPROCSERVICE = unchecked((int)(0x80110814)),

        /// <summary>
        /// The ProgID provided to the copy operation is invalid. The ProgID is in use by another registered CLSID.
        /// </summary>
        COMADMIN_E_PROGIDINUSEBYCLSID = unchecked((int)(0x80110815)),

        /// <summary>
        /// The partition specified as default is not a member of the partition set.
        /// </summary>
        COMADMIN_E_DEFAULT_PARTITION_NOT_IN_SET = unchecked((int)(0x80110816)),

        /// <summary>
        /// A recycled process may not be paused.
        /// </summary>
        COMADMIN_E_RECYCLEDPROCESSMAYNOTBEPAUSED = unchecked((int)(0x80110817)),

        /// <summary>
        /// Access to the specified partition is denied.
        /// </summary>
        COMADMIN_E_PARTITION_ACCESSDENIED = unchecked((int)(0x80110818)),

        /// <summary>
        /// Only Application Files (*.MSI files) can be installed into partitions.
        /// </summary>
        COMADMIN_E_PARTITION_MSI_ONLY = unchecked((int)(0x80110819)),

        /// <summary>
        /// Applications containing one or more legacy components may not be exported to 1.0 format.
        /// </summary>
        COMADMIN_E_LEGACYCOMPS_NOT_ALLOWED_IN_1_0_FORMAT = unchecked((int)(0x8011081A)),

        /// <summary>
        /// Legacy components may not exist in non-base partitions.
        /// </summary>
        COMADMIN_E_LEGACYCOMPS_NOT_ALLOWED_IN_NONBASE_PARTITIONS = unchecked((int)(0x8011081B)),

        /// <summary>
        /// A component cannot be moved (or copied) from the System Application, an application proxy or a non-changeable application
        /// </summary>
        COMADMIN_E_COMP_MOVE_SOURCE = unchecked((int)(0x8011081C)),

        /// <summary>
        /// A component cannot be moved (or copied) to the System Application, an application proxy or a non-changeable application
        /// </summary>
        COMADMIN_E_COMP_MOVE_DEST = unchecked((int)(0x8011081D)),

        /// <summary>
        /// A private component cannot be moved (or copied) to a library application or to the base partition
        /// </summary>
        COMADMIN_E_COMP_MOVE_PRIVATE = unchecked((int)(0x8011081E)),

        /// <summary>
        /// The Base Application Partition exists in all partition sets and cannot be removed.
        /// </summary>
        COMADMIN_E_BASEPARTITION_REQUIRED_IN_SET = unchecked((int)(0x8011081F)),

        /// <summary>
        /// Alas, Event Class components cannot be aliased.
        /// </summary>
        COMADMIN_E_CANNOT_ALIAS_EVENTCLASS = unchecked((int)(0x80110820)),

        /// <summary>
        /// Access is denied because the component is private.
        /// </summary>
        COMADMIN_E_PRIVATE_ACCESSDENIED = unchecked((int)(0x80110821)),

        /// <summary>
        /// The specified SAFER level is invalid.
        /// </summary>
        COMADMIN_E_SAFERINVALID = unchecked((int)(0x80110822)),

        /// <summary>
        /// The specified user cannot write to the system registry
        /// </summary>
        COMADMIN_E_REGISTRY_ACCESSDENIED = unchecked((int)(0x80110823)),

        /// <summary>
        /// No information available.
        /// </summary>
        COMADMIN_E_PARTITIONS_DISABLED = unchecked((int)(0x80110824)),

        /// <summary>
        /// The protected data needs to be re-protected.
        /// </summary>
        CRYPT_I_NEW_PROTECTION_REQUIRED = 0x00091012,

        /// <summary>
        /// Successful drop took place
        /// </summary>
        DRAGDROP_S_FIRST = DRAGDROP_S_DROP,

        /// <summary>
        /// Successful drop took place
        /// </summary>
        DRAGDROP_S_DROP = 0x00040100,

        /// <summary>
        /// Drag-drop operation canceled
        /// </summary>
        DRAGDROP_S_CANCEL = 0x00040101,

        /// <summary>
        /// Use the default cursor
        /// </summary>
        DRAGDROP_S_USEDEFAULTCURSORS = 0x00040102,

        /// <summary>
        /// No information available.
        /// </summary>
        DRAGDROP_S_LAST = 0x0004010F,

        /// <summary>
        /// The specified event is currently not being audited.
        /// </summary>
        ERROR_AUDITING_DISABLED = unchecked((int)(0xC0090001)),

        /// <summary>
        /// The SID filtering operation removed all SIDs.
        /// </summary>
        ERROR_ALL_SIDS_FILTERED = unchecked((int)(0xC0090002)),

        /// <summary>
        /// Failed to open a file.
        /// </summary>
        NS_E_FILE_OPEN_FAILED = (int)(0xC00D001DL - 0x01_00_00_00_00),

        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        NTE_OP_OK = S_OK,

        /// <summary>
        /// Use the registry database to provide the requested information
        /// </summary>
        OLE_S_FIRST = OLE_S_USEREG,

        /// <summary>
        /// Use the registry database to provide the requested information
        /// </summary>
        OLE_S_USEREG = 0x00040000,

        /// <summary>
        /// Success, but static
        /// </summary>
        OLE_S_STATIC = 0x00040001,

        /// <summary>
        /// Macintosh clipboard format
        /// </summary>
        OLE_S_MAC_CLIPFORMAT = 0x00040002,

        /// <summary>
        /// No information available.
        /// </summary>
        OLE_S_LAST = 0x000400FF,

        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        S_OK = VSConstants.S_OK,

        /// <summary>
        /// Incorrect function.
        /// </summary>
        S_FALSE = VSConstants.S_FALSE,

        /// <summary>
        /// The underlying file was converted to compound file format.
        /// </summary>
        STG_S_CONVERTED = 0x00030200,

        /// <summary>
        /// The storage operation should block until more data is available.
        /// </summary>
        STG_S_BLOCK = 0x00030201,

        /// <summary>
        /// The storage operation should retry immediately.
        /// </summary>
        STG_S_RETRYNOW = 0x00030202,

        /// <summary>
        /// The notified event sink will not influence the storage operation.
        /// </summary>
        STG_S_MONITORING = 0x00030203,

        /// <summary>
        /// Multiple opens prevent consolidated. (commit succeeded).
        /// </summary>
        STG_S_MULTIPLEOPENS = 0x00030204,

        /// <summary>
        /// Consolidation of the storage file failed. (commit succeeded).
        /// </summary>
        STG_S_CONSOLIDATIONFAILED = 0x00030205,

        /// <summary>
        /// Consolidation of the storage file is inappropriate. (commit succeeded).
        /// </summary>
        STG_S_CANNOTCONSOLIDATE = 0x00030206,

        /// <summary>
        /// No information available.
        /// </summary>
        MARSHAL_S_FIRST = 0x00040120,

        /// <summary>
        /// No information available.
        /// </summary>
        MARSHAL_S_LAST = 0x0004012F,

        /// <summary>
        /// Data has same FORMATETC
        /// </summary>
        DATA_S_FIRST = DATA_S_SAMEFORMATETC,

        /// <summary>
        /// Data has same FORMATETC
        /// </summary>
        DATA_S_SAMEFORMATETC = 0x00040130,

        /// <summary>
        /// No information available.
        /// </summary>
        DATA_S_LAST = 0x0004013F,

        /// <summary>
        /// View is already frozen
        /// </summary>
        VIEW_S_FIRST = VIEW_S_ALREADY_FROZEN,

        /// <summary>
        /// View is already frozen
        /// </summary>
        VIEW_S_ALREADY_FROZEN = 0x00040140,

        /// <summary>
        /// No information available.
        /// </summary>
        VIEW_S_LAST = 0x0004014F,

        /// <summary>
        /// No information available.
        /// </summary>
        REGDB_S_FIRST = 0x00040150,

        /// <summary>
        /// No information available.
        /// </summary>
        REGDB_S_LAST = 0x0004015F,

        /// <summary>
        /// Invalid verb for OLE object
        /// </summary>
        OLEOBJ_S_FIRST = OLEOBJ_S_INVALIDVERB,

        /// <summary>
        /// Invalid verb for OLE object
        /// </summary>
        OLEOBJ_S_INVALIDVERB = 0x00040180,

        /// <summary>
        /// Verb number is valid but verb cannot be done now
        /// </summary>
        OLEOBJ_S_CANNOT_DOVERB_NOW = 0x00040181,

        /// <summary>
        /// Invalid window handle passed
        /// </summary>
        OLEOBJ_S_INVALIDHWND = 0x00040182,

        /// <summary>
        /// No information available.
        /// </summary>
        OLEOBJ_S_LAST = 0x0004018F,

        /// <summary>
        /// No information available.
        /// </summary>
        CLIENTSITE_S_FIRST = 0x00040190,

        /// <summary>
        /// No information available.
        /// </summary>
        CLIENTSITE_S_LAST = 0x0004019F,

        /// <summary>
        /// Message is too long, some of it had to be truncated before displaying
        /// </summary>
        INPLACE_S_FIRST = INPLACE_S_TRUNCATED,

        /// <summary>
        /// Message is too long, some of it had to be truncated before displaying
        /// </summary>
        INPLACE_S_TRUNCATED = 0x000401A0,

        /// <summary>
        /// No information available.
        /// </summary>
        INPLACE_S_LAST = 0x000401AF,

        /// <summary>
        /// No information available.
        /// </summary>
        ENUM_S_FIRST = 0x000401B0,

        /// <summary>
        /// No information available.
        /// </summary>
        ENUM_S_LAST = 0x000401BF,

        /// <summary>
        /// Unable to convert OLESTREAM to IStorage
        /// </summary>
        CONVERT10_S_FIRST = CONVERT10_S_NO_PRESENTATION,

        /// <summary>
        /// Unable to convert OLESTREAM to IStorage
        /// </summary>
        CONVERT10_S_NO_PRESENTATION = 0x000401C0,

        /// <summary>
        /// No information available.
        /// </summary>
        CONVERT10_S_LAST = 0x000401CF,

        /// <summary>
        /// No information available.
        /// </summary>
        CLIPBRD_S_FIRST = 0x000401D0,

        /// <summary>
        /// No information available.
        /// </summary>
        CLIPBRD_S_LAST = 0x000401DF,

        /// <summary>
        /// No information available.
        /// </summary>
        MK_S_FIRST = 0x000401E0,

        /// <summary>
        /// Moniker reduced to itself
        /// </summary>
        MK_S_REDUCED_TO_SELF = 0x000401E2,

        /// <summary>
        /// Common prefix is this moniker
        /// </summary>
        MK_S_ME = 0x000401E4,

        /// <summary>
        /// Common prefix is input moniker
        /// </summary>
        MK_S_HIM = 0x000401E5,

        /// <summary>
        /// Common prefix is both monikers
        /// </summary>
        MK_S_US = 0x000401E6,

        /// <summary>
        /// Moniker is already registered in running object table
        /// </summary>
        MK_S_MONIKERALREADYREGISTERED = 0x000401E7,

        /// <summary>
        /// No information available.
        /// </summary>
        MK_S_LAST = 0x000401EF,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for no information available.
        /// </summary>
        CO_S_FIRST = 0x000401F0,

        /// <summary>
        /// <c>COM</c> Error <c>HRESULT</c> for no information available.
        /// </summary>
        CO_S_LAST = 0x000401FF,

        /// <summary>
        /// An event was able to invoke some but not all of the subscribers
        /// </summary>
        EVENT_S_FIRST = EVENT_S_SOME_SUBSCRIBERS_FAILED,

        /// <summary>
        /// An event was able to invoke some but not all of the subscribers
        /// </summary>
        EVENT_S_SOME_SUBSCRIBERS_FAILED = 0x00040200,

        /// <summary>
        /// An event was delivered but there were no subscribers
        /// </summary>
        EVENT_S_NOSUBSCRIBERS = 0x00040202,

        /// <summary>
        /// No information available.
        /// </summary>
        EVENT_S_LAST = 0x0004021F,

        /// <summary>
        /// The task is ready to run at its next scheduled time.
        /// </summary>
        SCHED_S_TASK_READY = 0x00041300,

        /// <summary>
        /// The task is currently running.
        /// </summary>
        SCHED_S_TASK_RUNNING = 0x00041301,

        /// <summary>
        /// The task will not run at the scheduled times because it has been disabled.
        /// </summary>
        SCHED_S_TASK_DISABLED = 0x00041302,

        /// <summary>
        /// The task has not yet run.
        /// </summary>
        SCHED_S_TASK_HAS_NOT_RUN = 0x00041303,

        /// <summary>
        /// There are no more runs scheduled for this task.
        /// </summary>
        SCHED_S_TASK_NO_MORE_RUNS = 0x00041304,

        /// <summary>
        /// One or more of the properties that are needed to run this task on a schedule have not been set.
        /// </summary>
        SCHED_S_TASK_NOT_SCHEDULED = 0x00041305,

        /// <summary>
        /// The last run of the task was terminated by the user.
        /// </summary>
        SCHED_S_TASK_TERMINATED = 0x00041306,

        /// <summary>
        /// Either the task has no triggers or the existing triggers are disabled or not set.
        /// </summary>
        SCHED_S_TASK_NO_VALID_TRIGGERS = 0x00041307,

        /// <summary>
        /// Event triggers don't have set run times.
        /// </summary>
        SCHED_S_EVENT_TRIGGER = 0x00041308,

        /// <summary>
        /// The function completed successfully, but must be called again to complete the context
        /// </summary>
        SEC_I_CONTINUE_NEEDED = 0x00090312,

        /// <summary>
        /// The function completed successfully, but CompleteToken must be called
        /// </summary>
        SEC_I_COMPLETE_NEEDED = 0x00090313,

        /// <summary>
        /// The function completed successfully, but both CompleteToken and this function must be called to complete the context
        /// </summary>
        SEC_I_COMPLETE_AND_CONTINUE = 0x00090314,

        /// <summary>
        /// The logon was completed, but no network authority was available. The logon was made using locally known information
        /// </summary>
        SEC_I_LOCAL_LOGON = 0x00090315,

        /// <summary>
        /// The context has expired and can no longer be used.
        /// </summary>
        SEC_I_CONTEXT_EXPIRED = 0x00090317,

        /// <summary>
        /// The credentials supplied were not complete, and could not be verified. Additional information can be returned from the context.
        /// </summary>
        SEC_I_INCOMPLETE_CREDENTIALS = 0x00090320,

        /// <summary>
        /// The context data must be renegotiated with the peer.
        /// </summary>
        SEC_I_RENEGOTIATE = 0x00090321,

        /// <summary>
        /// There is no LSA mode context associated with this context.
        /// </summary>
        SEC_I_NO_LSA_CONTEXT = 0x00090323,

        VS_S_PROJECTFORWARDED = VSConstants.VS_S_PROJECTFORWARDED,

        VS_S_TBXMARKER = VSConstants.VS_S_TBXMARKER,

        VS_S_PROJECT_SAFEREPAIRREQUIRED = VSConstants.VS_S_PROJECT_SAFEREPAIRREQUIRED,

        VS_S_PROJECT_UNSAFEREPAIRREQUIRED = VSConstants.VS_S_PROJECT_UNSAFEREPAIRREQUIRED,

        VS_S_PROJECT_ONEWAYUPGRADEREQUIRED = VSConstants.VS_S_PROJECT_ONEWAYUPGRADEREQUIRED,

        VS_S_INCOMPATIBLEPROJECT = VSConstants.VS_S_INCOMPATIBLEPROJECT,

        /// <summary>
        /// An asynchronous operation was specified. The operation has begun, but its outcome is not known yet.
        /// </summary>
        XACT_S_FIRST = XACT_S_ASYNC,

        /// <summary>
        /// An asynchronous operation was specified. The operation has begun, but its outcome is not known yet.
        /// </summary>
        XACT_S_ASYNC = 0x0004D000,

        /// <summary>
        /// XACT_S_DEFECT
        /// </summary>
        XACT_S_DEFECT = 0x0004D001,

        /// <summary>
        /// The method call succeeded because the transaction was read-only.
        /// </summary>
        XACT_S_READONLY = 0x0004D002,

        /// <summary>
        /// The transaction was successfully aborted. However, this is a coordinated transaction, and some number of enlisted
        /// resources were aborted outright because they could not support abort-retaining semantics
        /// </summary>
        XACT_S_SOMENORETAIN = 0x0004D003,

        /// <summary>
        /// No changes were made during this call, but the sink wants another chance to look if any other sinks make further changes.
        /// </summary>
        XACT_S_OKINFORM = 0x0004D004,

        /// <summary>
        /// The sink is content and wishes the transaction to proceed. Changes were made to one or more resources during this call.
        /// </summary>
        XACT_S_MADECHANGESCONTENT = 0x0004D005,

        /// <summary>
        /// The sink is for the moment and wishes the transaction to proceed, but if other changes are made following this return by
        /// other event sinks then this sink wants another chance to look
        /// </summary>
        XACT_S_MADECHANGESINFORM = 0x0004D006,

        /// <summary>
        /// The transaction was successfully aborted. However, the abort was non-retaining.
        /// </summary>
        XACT_S_ALLNORETAIN = 0x0004D007,

        /// <summary>
        /// An abort operation was already in progress.
        /// </summary>
        XACT_S_ABORTING = 0x0004D008,

        /// <summary>
        /// The resource manager has performed a single-phase commit of the transaction.
        /// </summary>
        XACT_S_SINGLEPHASE = 0x0004D009,

        /// <summary>
        /// The local transaction has not aborted.
        /// </summary>
        XACT_S_LOCALLY_OK = 0x0004D00A,

        /// <summary>
        /// The resource manager has requested to be the coordinator (last resource manager) for the transaction.
        /// </summary>
        XACT_S_LAST = XACT_S_LASTRESOURCEMANAGER,

        /// <summary>
        /// The resource manager has requested to be the coordinator (last resource manager) for the transaction.
        /// </summary>
        XACT_S_LASTRESOURCEMANAGER = 0x0004D010,

        /// <summary>
        /// No information available.
        /// </summary>
        CONTEXT_S_FIRST = 0x0004E000,

        /// <summary>
        /// No information available.
        /// </summary>
        CONTEXT_S_LAST = 0x0004E02F,
    }

    public static class HResultExtension
    {
        #region Public Methods

        public static bool Failed(int hr)
        {
            return hr < ToHResultCode(HResult.S_OK) || IsError(hr);
        }

        public static int GetSeverity(int hr)
        {
            return hr >> 31 & HResultMask.SEVERITY_BIT;
        }

        public static bool IsError(int hr)
        {
            return (ulong)hr >> 31 == HResultMask.SEVERITY_ERROR;
        }

        public static HResult MakeHResult(ulong severity, ulong facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, (ulong)FacilityCode.FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, (ulong)FacilityCode.FACILITY_OPC, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));

            return severity != 0UL && severity != 1UL
                ? throw new ArgumentException($"Parameter {nameof(severity)} with value '{severity}' is invalid.", nameof(severity))
                : (HResult)Enum.ToObject(typeof(HResult), ToHResultCode(severity, facilityCode, code));
        }

        public static bool Succeeded(int hr)
        {
            return hr >= ToHResultCode(HResult.S_OK) && !IsError(hr);
        }

        public static int ToHResultCode(HResult hr)
        {
            return (int)hr;
        }

        public static int ToHResultCode(ulong severity, ulong facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, (ulong)FacilityCode.FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, (ulong)FacilityCode.FACILITY_OPC, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));

            return severity != 0UL && severity != 1UL
                ? throw new ArgumentException($"Parameter {nameof(severity)} with value '{severity}' is invalid.", nameof(severity))
                : (int)(severity << 31 | facilityCode << 16 | (ulong)WinErrorExtension.ToWinErrorCode(code));
        }

        /// <summary>
        /// </summary>
        /// <param name="facilityCode"></param>
        /// <param name="code">        </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ToHResultCode(int facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), ToHResultCode(HResult.S_OK), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, FacilityCode.FACILITY_NULL.ToInt32(), nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, FacilityCode.FACILITY_OPC.ToInt32(), nameof(facilityCode));

            return (int)(WinErrorExtension.ToWinErrorCode(code) <= ToHResultCode(HResult.S_OK) ? WinErrorExtension.ToWinErrorCode(code) : WinErrorExtension.ToWinErrorCode(code) | (int)facilityCode << 16 | HResultMask.SEVERITY_MASK);
        }

        public static int ToHResultCode(this int ntStatus)
        {
            return ntStatus | FacilityCodeMask.FACILITY_NT_BIT;
        }

        #endregion Public Methods
    }

    public static class HResultMask
    {
        #region Public Fields

        public const int SEVERITY_BIT = 0x1;
        public const ulong SEVERITY_ERROR = 1;
        public const int SEVERITY_MASK = unchecked((int)(0x80000000));

        #endregion Public Fields
    }
}
