// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

// Ignore Spelling: Username

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using MSBuild.ExtensionPack.Base.Logging;

using System.Management;
using System.Runtime.InteropServices;
using System.Security;

namespace MSBuild.ExtensionPack.Base.Wmi
{
    /// <summary>
    /// Implements a class for supporting <c>WMI</c> access and remote execution of <c>MSBuild</c><see
    /// cref="Microsoft.Build.Utilities.Task"/> and <see cref="ToolTask"/>.
    /// </summary>
    public class Initialize
    {
        #region Private Fields

        /// <summary>
        /// Back storage for <see cref="SecurePassword"/> because <see cref="ConnectionOptions.SecurePassword"/> can only be set; it
        /// has no get accessor.
        /// </summary>
        private SecureString securePassword;

        #endregion Private Fields

        #region Private Methods

        private void UpdateReadWriteProperty(ConnectionOptions? options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            this.Authentication = options.Authentication;
            this.Authority = options.Authority;
            this.EnabledPrivileges = options.EnablePrivileges;
            this.Impersonation = options.Impersonation;
            this.Locale = options.Locale;
            this.Username = options.Username;
        }

        #endregion Private Methods

        #region Protected Methods

        protected static string ConvertFromSecureString(SecureString? cipher)
        {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));

            IntPtr binaryString = Marshal.SecureStringToBSTR(cipher);

            try
            {
                return binaryString != IntPtr.Zero ? Marshal.PtrToStringBSTR(binaryString) : string.Empty;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(binaryString);
            }
        }

        protected static SecureString ConvertToSecureString(string? clearText)
        {
            ArgumentNullException.ThrowIfNull(clearText, nameof(clearText));

            SecureString result = new();

            Array.ForEach(clearText.ToCharArray(), c => result.AppendChar(c));
            return result;
        }

        #endregion Protected Methods

        #region Public Fields

        public const string DEFAULT_PATH = "";
        public const int OPTIMAL_INITIAL_STRINGBUILDER = 16;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        public Initialize(TaskLoggingHelper logger)
        {
            this.Log = logger;
            this.MachineName = Environment.MachineName;

            _ = IsLocalMachineOrCanRemote(canExecuteRemotely: false);

            this.Context = [];
            this.Options ??= new();
            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(TaskLoggingHelper logger, ManagementPath path)
        {
            this.Log = logger;
            this.MachineName = path.Server;

            _ = IsLocalMachineOrCanRemote(canExecuteRemotely: false);

            this.Context = [];
            this.Options ??= new();
            this.Scope = GetManagementScope(path);
        }

        public Initialize(TaskLoggingHelper logger, string computerName)
        {
            Log = logger;
            MachineName = computerName;

            _ = IsLocalMachineOrCanRemote(canExecuteRemotely: true);

            this.Context = [];
            this.Options ??= new();
            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">  Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="password">Specifies the clear text password for the current user; primarily for remote execution.</param>
        /// <remarks>
        /// This constructor is marked with the <see cref="ObsoleteAttribute"/> as using clear text <paramref name="password"/> is a
        /// high security vulnerability.
        /// </remarks>
        public Initialize(TaskLoggingHelper logger, string password, string computerName)
            : this(logger, computerName)
        {
            this.Options ??= new() { SecurePassword = ConvertToSecureString(password) };

            if (this.Options is not null)
            {
                this.Options.SecurePassword = ConvertToSecureString(password);
            }

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(TaskLoggingHelper logger, string password, ManagementPath path)
                    : this(logger, path)
        {
            this.Options ??= new() { SecurePassword = ConvertToSecureString(password) };

            if (this.Options is not null)
            {
                this.Options.SecurePassword = ConvertToSecureString(password);
            }

            this.Scope = GetManagementScope(path);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">  Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="password">
        /// Specifies the <see cref="SecureString"/> password for the current user; primarily for remote execution.
        /// </param>
        public Initialize(TaskLoggingHelper logger, SecureString password, string computerName)
            : this(logger, computerName)
        {
            this.Options ??= new() { SecurePassword = password };

            if (this.Options is not null)
            {
                this.Options.SecurePassword = password;
            }

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(TaskLoggingHelper logger, SecureString password, ManagementPath path)
                    : this(logger, path)
        {
            this.Options ??= new() { SecurePassword = password };

            if (this.Options is not null)
            {
                this.Options.SecurePassword = password;
            }

            this.Scope = GetManagementScope(path);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">          Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="locale">          Specifies the locale to be used for the connection. The default is "DEFAULTLOCALE".</param>
        /// <param name="userName">        
        /// Specifies the user name for the connection. If <see langref="null"/>, the credentials of the currently logged-on user
        /// are used.
        /// </param>
        /// <param name="password">        
        /// Specifies the clear text password for <paramref name="userName"/>; primarily for remote execution.
        /// </param>
        /// <param name="authority">       Specifies the authority to be used to authenticate the <paramref name="userName"/>.</param>
        /// <param name="impersonation">   
        /// Specifies one of the enumeration of <see cref="ImpersonationLevel"/> representing the <c>COM</c> impersonation level to
        /// be used for the connection.
        /// </param>
        /// <param name="authentication">  
        /// Specifies one of the enumeration of <see cref="AuthenticationLevel"/> representing the <c>COM</c> authentication level
        /// to be used for the connection.
        /// </param>
        /// <param name="enablePrivileges">
        /// Set to <see langref="true"/> to enable special user privileges. The default is <see langref="false"/>.
        /// </param>
        /// <remarks>
        /// This constructor is marked with the <see cref="ObsoleteAttribute"/> as using clear text <paramref name="password"/> is a
        /// high security vulnerability.
        /// </remarks>
        public Initialize(
            TaskLoggingHelper logger,
            string? locale,
            string? userName,
            string password,
            string authority,
            ImpersonationLevel impersonation,
            AuthenticationLevel authentication,
            string computerName,
            bool enablePrivileges = false)
            : this(logger, password, computerName)
        {
            this.Options ??= new()
            {
                Locale = locale ?? "DEFAULTLOCALE",
                Username = userName,
                Authority = authority,
                Impersonation = impersonation,
                Authentication = authentication,
                EnablePrivileges = enablePrivileges,
            };

            if (this.Options is not null)
            {
                this.Options.Locale = locale ?? "DEFAULTLOCALE";
                this.Options.Username = userName;
                this.Options.Authority = authority;
                this.Options.Authentication = authentication;
                this.Options.EnablePrivileges = enablePrivileges;
            }

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(
                    TaskLoggingHelper logger,
                    string? locale,
                    string? userName,
                    string password,
                    string authority,
                    ImpersonationLevel impersonation,
                    AuthenticationLevel authentication,
                    ManagementPath path,
                    bool enablePrivileges = false)
                    : this(logger, password, path)
        {
            this.Options ??= new()
            {
                Locale = locale ?? "DEFAULTLOCALE",
                Username = userName,
                SecurePassword = ConvertToSecureString(password),
                Authority = authority,
                Impersonation = impersonation,
                Authentication = authentication,
                EnablePrivileges = enablePrivileges,
            };

            if (this.Options is not null)
            {
                this.Options.Locale = locale ?? "DEFAULTLOCALE";
                this.Options.Username = userName;
                this.Options.Authority = authority;
                this.Options.Impersonation = impersonation;
                this.Options.Authentication = authentication;
                this.Options.EnablePrivileges = enablePrivileges;
            }

            this.Scope = GetManagementScope(path);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">          Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="locale">          Specifies the locale to be used for the connection. The default is "DEFAULTLOCALE".</param>
        /// <param name="userName">        
        /// Specifies the user name for the connection. If <see langref="null"/>, the credentials of the currently logged-on user
        /// are used.
        /// </param>
        /// <param name="password">        
        /// Specifies the <see cref="SecureString"/> password for <paramref name="userName"/>; primarily for remote execution.
        /// </param>
        /// <param name="authority">       Specifies the authority to be used to authenticate the <paramref name="userName"/>.</param>
        /// <param name="impersonation">   
        /// Specifies one of the enumeration of <see cref="ImpersonationLevel"/> representing the <c>COM</c> impersonation level to
        /// be used for the connection.
        /// </param>
        /// <param name="authentication">  
        /// Specifies one of the enumeration of <see cref="AuthenticationLevel"/> representing the <c>COM</c> authentication level
        /// to be used for the connection.
        /// </param>
        /// <param name="enablePrivileges">
        /// Set to <see langref="true"/> to enable special user privileges. The default is <see langref="false"/>.
        /// </param>
        public Initialize(
            TaskLoggingHelper logger,
            string? locale,
            string? userName,
            SecureString password,
            string authority,
            ImpersonationLevel impersonation,
            AuthenticationLevel authentication,
            string computerName,
            bool enablePrivileges = false)
            : this(logger, password, computerName)
        {
            this.Options ??= new()
            {
                Locale = locale ?? "DEFAULTLOCALE",
                Username = userName,
                SecurePassword = password,
                Authority = authority,
                Impersonation = impersonation,
                Authentication = authentication,
                EnablePrivileges = enablePrivileges,
            };

            if (this.Options is not null)
            {
                this.Options.Locale = locale ?? "DEFAULTLOCALE";
                this.Options.Username = userName;
                this.Options.Authority = authority;
                this.Options.Impersonation = impersonation;
                this.Options.Authentication = authentication;
                this.Options.EnablePrivileges = enablePrivileges;
            }

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(
                    TaskLoggingHelper logger,
                    string? locale,
                    string? userName,
                    SecureString password,
                    string authority,
                    ImpersonationLevel impersonation,
                    AuthenticationLevel authentication,
                    ManagementPath path,
                    bool enablePrivileges = false)
                    : this(logger, password, path)
        {
            this.Options ??= new()
            {
                Locale = locale ?? "DEFAULTLOCALE",
                Username = userName,
                SecurePassword = password,
                Authority = authority,
                Impersonation = impersonation,
                Authentication = authentication,
                EnablePrivileges = enablePrivileges,
            };

            if (this.Options is not null)
            {
                this.Options.Locale = locale ?? "DEFAULTLOCALE";
                this.Options.Username = userName;
                this.Options.Authority = authority;
                this.Options.Impersonation = impersonation;
                this.Options.Authentication = authentication;
                this.Options.EnablePrivileges = enablePrivileges;
            }

            this.Scope = GetManagementScope(path);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">          Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="locale">          Specifies the locale to be used for the connection. The default is "DEFAULTLOCALE".</param>
        /// <param name="userName">        
        /// Specifies the user name for the connection. If <see langref="null"/>, the credentials of the currently logged-on user
        /// are used.
        /// </param>
        /// <param name="password">        
        /// Specifies the clear text password for <paramref name="userName"/>; primarily for remote execution.
        /// </param>
        /// <param name="authority">       Specifies the authority to be used to authenticate the <paramref name="userName"/>.</param>
        /// <param name="impersonation">   
        /// Specifies one of the enumeration of <see cref="ImpersonationLevel"/> representing the <c>COM</c> impersonation level to
        /// be used for the connection.
        /// </param>
        /// <param name="authentication">  
        /// Specifies one of the enumeration of <see cref="AuthenticationLevel"/> representing the <c>COM</c> authentication level
        /// to be used for the connection.
        /// </param>
        /// <param name="context">         
        /// Specifies a provider-specific <see cref="ManagementNamedValueCollection"/> of named-value pairs to be passed through to
        /// the provider.
        /// </param>
        /// <param name="enablePrivileges">
        /// Set to <see langref="true"/> to enable special user privileges. The default is <see langref="false"/>.
        /// </param>
        /// <remarks>
        /// This constructor is marked with the <see cref="ObsoleteAttribute"/> as using clear text <paramref name="password"/> is a
        /// high security vulnerability.
        /// </remarks>
        public Initialize(
            TaskLoggingHelper logger,
            string? locale,
            string? userName,
            string? password,
            string authority,
            ImpersonationLevel impersonation,
            AuthenticationLevel authentication,
            string computerName,
            ManagementNamedValueCollection context,
            bool enablePrivileges = false)
            : this(logger, computerName)
        {
            this.Context = context;

            this.Options = new(
                locale ?? "DEFAULTLOCALE",
                userName,
                ConvertToSecureString(password),
                authority,
                impersonation,
                authentication,
                enablePrivileges,
                this.Context,
                Initialize.Timeout);

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(
                    TaskLoggingHelper logger,
                    string? locale,
                    string? userName,
                    string? password,
                    string authority,
                    ImpersonationLevel impersonation,
                    AuthenticationLevel authentication,
                    ManagementPath path,
                    ManagementNamedValueCollection context,
                    bool enablePrivileges = false)
                    : this(logger, locale, userName, ConvertToSecureString(password), authority, impersonation, authentication, path, context, enablePrivileges)
        {
            this.Context = context;

            this.Options = new(
                locale ?? "DEFAULTLOCALE",
                userName,
                ConvertToSecureString(password),
                authority,
                impersonation,
                authentication,
                enablePrivileges,
                this.Context,
                Initialize.Timeout);

            this.Scope = GetManagementScope(path);
        }

        /// <summary>
        /// Initializes a new instance of the <c>WMI</c><see cref="Initialize"/> class.
        /// </summary>
        /// <param name="logger">          Specifies the <see cref="TaskLoggingHelper"/> to use for logging and setting exit return.</param>
        /// <param name="locale">          Specifies the locale to be used for the connection. The default is "DEFAULTLOCALE".</param>
        /// <param name="userName">        
        /// Specifies the user name for the connection. If <see langref="null"/>, the credentials of the currently logged-on user
        /// are used.
        /// </param>
        /// <param name="password">        
        /// Specifies the <see cref="SecureString"/> password for <paramref name="userName"/>; primarily for remote execution.
        /// </param>
        /// <param name="authority">       Specifies the authority to be used to authenticate the <paramref name="userName"/>.</param>
        /// <param name="impersonation">   
        /// Specifies one of the enumeration of <see cref="ImpersonationLevel"/> representing the <c>COM</c> impersonation level to
        /// be used for the connection.
        /// </param>
        /// <param name="authentication">  
        /// Specifies one of the enumeration of <see cref="AuthenticationLevel"/> representing the <c>COM</c> authentication level
        /// to be used for the connection.
        /// </param>
        /// <param name="context">         
        /// Specifies a provider-specific <see cref="ManagementNamedValueCollection"/> of named-value pairs to be passed through to
        /// the provider.
        /// </param>
        /// <param name="enablePrivileges">
        /// Set to <see langref="true"/> to enable special user privileges. The default is <see langref="false"/>.
        /// </param>
        public Initialize(
            TaskLoggingHelper logger,
            string? locale,
            string? userName,
            SecureString password,
            string authority,
            ImpersonationLevel impersonation,
            AuthenticationLevel authentication,
            string computerName,
            ManagementNamedValueCollection context,
            bool enablePrivileges = false)
            : this(logger, computerName)
        {
            this.Context = context;

            this.Options = new(
                locale ?? "DEFAULTLOCALE",
                userName,
                password,
                authority,
                impersonation,
                authentication,
                enablePrivileges,
                this.Context,
                Initialize.Timeout);

            this.Scope = GetManagementScope(DEFAULT_PATH);
        }

        public Initialize(
                    TaskLoggingHelper logger,
                    string? locale,
                    string? userName,
                    SecureString password,
                    string authority,
                    ImpersonationLevel impersonation,
                    AuthenticationLevel authentication,
                    ManagementPath path,
                    ManagementNamedValueCollection context,
                    bool enablePrivileges = false)
                    : this(logger, path)
        {
            this.Context = context;

            this.Options = new(
                locale ?? "DEFAULTLOCALE",
                userName,
                password,
                authority,
                impersonation,
                authentication,
                enablePrivileges,
                this.Context,
                Initialize.Timeout);

            this.Scope = GetManagementScope(path);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating the <see cref="TimeSpan"/> timeout value for establish a connection.
        /// </summary>
        /// <remarks>Currently required but does nothing.</remarks>
        public static TimeSpan Timeout => TimeSpan.MaxValue;

        /// <summary>
        /// Gets or sets a value indicating the <c>COM</c><see cref="AuthenticationLevel"/> to be used for <c>WMI</c> for
        /// connections to establish <see cref="ManagementScope"/>.
        /// </summary>
        public virtual AuthenticationLevel Authentication
        {
            get => this.Options.Authentication;
            set => this.Options.Authentication = value;
        }

        /// <summary>
        /// Gets or sets the authority to be used to authenticate the current user or the specified <see cref="Username"/> user on
        /// this connection to establish <see cref="ManagementScope"/>.
        /// </summary>
        public virtual string? Authority
        {
            get => this.Options.Authority;
            set => this.Options.Authority = value;
        }

        /// <summary>
        /// Gets a value indicating the <c>WMI</c> provider-specific named-value pairs to be passed through to the provider.
        /// </summary>
        public ManagementNamedValueCollection Context { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable special user privileges on this connection to establish <see cref="ManagementScope"/>.
        /// </summary>
        /// <remarks>
        /// This property should only be set when performing an operation that requires special <see
        /// cref="OperatingSystem.IsWindows"/> user privileges. An example would be shutdown.
        /// </remarks>
        public virtual bool EnabledPrivileges
        {
            get => this.Options.EnablePrivileges;
            set => this.Options.EnablePrivileges = value;
        }

        /// <summary>
        /// Gets or sets a value indicating the <c>COM</c> impersonation level to be used for operations on this connection to
        /// establish <see cref="ManagementScope"/>.
        /// </summary>
        public virtual ImpersonationLevel Impersonation
        {
            get => this.Options.Impersonation;
            set => this.Options.Impersonation = value;
        }

        /// <summary>
        /// Gets or sets a value indicating the locale to be used for this connection to establish <see cref="ManagementScope"/>.
        /// </summary>
        /// <remarks>
        /// For Microsoft locale identifiers, the format of the string is "MS_xxx", where <italic>xxx</italic> is a string in
        /// hexadecimal form that indicates the Locale Identification (LCID); for example, American English would appear as "MS_409".
        /// </remarks>
        public virtual string Locale
        {
            get => this.Options.Locale;
            set => this.Options.Locale = value;
        }

        /// <summary>
        /// Gets a value indicating the <see cref="TaskLoggingHelper"/> to use for logging.
        /// </summary>
        public TaskLoggingHelper Log { get; }

        /// <summary>
        /// Gets a value indicating the computer name for <see cref="BaseTask"/> or <see cref="BaseToolTask"/> execution.
        /// </summary>
        /// <remarks>Defaults to <see cref="Environment.MachineName"/> which indicates local execution.</remarks>
        public virtual string MachineName { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="ConnectionOptions"/> for the connection to establish <see cref="ManagementScope"/>.
        /// </summary>
        public ConnectionOptions Options { get; }

        /// <summary>
        /// Sets a value indicating the clear text password for either the current user or <see cref="Username"/>.
        /// </summary>
        [Obsolete("Use of property 'Password' exposes credentials in clear text")]
        public virtual string? Password
        {
            private get => ConvertFromSecureString(this.securePassword);

            set
            {
                this.securePassword = ConvertToSecureString(value);
                this.Options.SecurePassword = this.securePassword;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the current <see cref="ManagementScope"/> instance from the constructor.
        /// </summary>
        public ManagementScope? Scope { get; }

        /// <summary>
        /// Sets a value indicating the <see cref="SecureString"/> password to use with either the current user or <see cref="Username"/>.
        /// </summary>
        public virtual SecureString SecurePassword
        {
            private get => this.securePassword;

            set
            {
                this.securePassword = value;
                this.Options.SecurePassword = this.securePassword;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the user name to use for the connection.
        /// </summary>
        /// <remarks>If <see langref="null"/>, the currently logged-on user will be used.</remarks>
        public virtual string? Username { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <param name="server">       </param>
        /// <param name="className">    </param>
        /// <param name="nameSpacePath"></param>
        /// <param name="relativePath"> </param>
        /// <param name="options">      </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="options"/> is <see langref="null"/>.</exception>
        public ManagementScope GetManagementScope(string? server, string? className, string? nameSpacePath, string? relativePath, ConnectionOptions? options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            ManagementPath managementPath = new();

            managementPath.Server = server ?? Environment.MachineName;
            managementPath.NamespacePath = nameSpacePath ?? DEFAULT_PATH;

            if (string.IsNullOrEmpty(relativePath) && !string.IsNullOrEmpty(className))
            {
                managementPath.ClassName = className;
            }
            else if (!string.IsNullOrEmpty(relativePath))
            {
                managementPath.RelativePath = relativePath;
            }

            this.Log.LogTaskMessage(() => true, MessageImportance.Low, $"ManagementScope Set: \\\\{managementPath.Path}");
            this.UpdateReadWriteProperty(options);

            return new ManagementScope(managementPath, options);
        }

        /// <summary>
        /// Get the <see cref="ManagementScope"/> for the string <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Specifies the <c>WMI</c> path to connect with to establish <see cref="ManagementScope"/>.</param>
        /// <returns>A <see cref="ManagementScope"/> instance associated with <see cref="Options"/> and <paramref name="path"/>.</returns>
        public ManagementScope GetManagementScope(string path)
        {
            return GetManagementScope(path, this.Options);
        }

        /// <summary>
        /// </summary>
        /// <param name="server">       </param>
        /// <param name="className">    </param>
        /// <param name="nameSpacePath"></param>
        /// <param name="relativePath"> </param>
        /// <returns></returns>
        public ManagementScope GetManagementScope(string? server, string? className, string? nameSpacePath, string? relativePath)
        {
            return GetManagementScope(server, className, nameSpacePath, relativePath, this.Options);
        }

        /// <summary>
        /// Gets the <see cref="ManagementScope"/> for the <see cref="ManagementPath"/><paramref name="path"/>.
        /// </summary>
        /// <param name="path">
        /// Specifies the <see cref="System.Management"/><see cref="ManagementPath"/> to connect with to establish <see cref="ManagementScope"/>.
        /// </param>
        /// <returns>A <see cref="ManagementScope"/> instance associated with <see cref="Options"/> and <paramref name="path"/>.</returns>
        public ManagementScope GetManagementScope(ManagementPath path)
        {
            return GetManagementScope(path, this.Options);
        }

        /// <summary>
        /// Gets the <see cref="ManagementScope"/> for the string <paramref name="path"/> and <see
        /// cref="ConnectionOptions"/><paramref name="options"/>.
        /// </summary>
        /// <param name="path">   Specifies the <c>WMI</c> path to connect with to establish <see cref="ManagementScope"/>.</param>
        /// <param name="options">Specifies the <see cref="ConnectionOptions"/> to use for the connection.</param>
        /// <returns>
        /// A <see cref="ManagementScope"/> instance associated with <see cref="ConnectionOptions"/><paramref name="options"/> and
        /// <paramref name="path"/>.
        /// </returns>
        /// <remarks>
        /// The accessible properties of <see cref="Options"/> are updated. The notable two exceptions are <see cref="Password"/>
        /// and <see cref="SecurePassword"/>.
        /// </remarks>
        public ManagementScope GetManagementScope(string path, ConnectionOptions? options)
        {
            ManagementPath managementPath = new()
            {
                Server = this.MachineName,
                NamespacePath = path
            };

            return GetManagementScope(managementPath, options);
        }

        /// <summary>
        /// Gets the <see cref="ManagementScope"/> for the <see cref="ManagementPath"/><paramref name="path"/> and <see
        /// cref="ConnectionOptions"/><paramref name="options"/>.
        /// </summary>
        /// <param name="path">   
        /// Specifies the <see cref="System.Management"/><see cref="ManagementPath"/> to connect with to establish <see cref="ManagementScope"/>.
        /// </param>
        /// <param name="options">Specifies the <see cref="ConnectionOptions"/> to use for the connection.</param>
        /// <returns>
        /// A <see cref="ManagementScope"/> instance associated with <see cref="ConnectionOptions"/><paramref name="options"/> and
        /// <see cref="ManagementPath"/><paramref name="path"/>.
        /// </returns>
        /// <remarks>
        /// The accessible properties of <see cref="Options"/> are updated. The notable two exceptions are <see cref="Password"/>
        /// and <see cref="SecurePassword"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="options"/> is <see langref="null"/>.</exception>
        public ManagementScope GetManagementScope(ManagementPath path, ConnectionOptions? options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            this.Log.LogTaskMessage(() => true, MessageImportance.Low, $"ManagementScope Set: \\\\{path.Path}");
            this.UpdateReadWriteProperty(options);

            return new ManagementScope(path, options);
        }

        /// <summary>
        /// /// Determines whether the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> is targeting the local machine.
        /// </summary>
        /// <returns><see langref="true"/> if execution is on the local machine; otherwise <see langref="false"/>.</returns>
        public virtual bool IsLocalMachineOnly()
        {
            return string.Equals(this.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> is targeting the local machine or can
        /// execute remotely.
        /// </summary>
        /// <param name="canExecuteRemotely">
        /// Specify <see langref="true"/> if the current <see cref="BaseTask.TaskAction"/> or <see cref="BaseToolTask.TaskAction"/>
        /// can run against a remote machine (enabled); otherwise specify <see langref="false"/> (disabled).
        /// </param>
        /// <returns><see langref="true"/> if execution is on the local machine; otherwise <paramref name="canExecuteRemotely"/>.</returns>
        /// <remarks>
        /// <see cref="TaskLoggingHelper.HasLoggedErrors"/> will be set if execution is on a remote machine and <paramref
        /// name="canExecuteRemotely"/> is <see langref="false"/>.
        /// </remarks>
        public virtual bool IsLocalMachineOrCanRemote(bool canExecuteRemotely)
        {
            if (IsLocalMachineOnly())
            {
                return true;
            }
            else
            {
                this.Log.LogTaskError(
                    () => !canExecuteRemotely,
                    "This task does not support remote execution. Please remove the MachineName: {0}",
                    this.MachineName);
                return canExecuteRemotely;
            }
        }

        #endregion Public Methods
    }
}
