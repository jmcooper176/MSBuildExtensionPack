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
using System.Globalization;

namespace MSBuild.ExtensionPack.Framework
{
    /// <summary>
    /// Represents a closure, including values for the default, input, and output parameters.
    /// </summary>
    /// <remarks>
    /// <para>A "parameter index" is a 0-based index into the array of parameters. It may refer to a default, input, or output parameter.</para>
    /// </remarks>
    public class Closure
    {
        #region Private Fields

        /// <summary>
        /// The arguments (and return values) for this closure.
        /// </summary>
        private readonly object[] arguments;

        /// <summary>
        /// The underlying method definition.
        /// </summary>
        private readonly MethodDefinition methodDefinition;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Closure"/> class. Creates a new closure, allocating space for the parameters.
        /// </summary>
        /// <param name="methodDefinition">The method definition used to create the new closure.</param>
        public Closure(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
            this.arguments = new object[methodDefinition.NumberOfParameters];
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Gets an input parameter's CLI <see cref="Type"/>.
        /// </summary>
        /// <param name="inputIndex">The zero-based index of the input parameter to retrieve.</param>
        /// <returns>The <see cref="Type"/> of the input parameter.</returns>
        public Type GetInputParameterType(int inputIndex)
        {
            int i = this.methodDefinition.GetInputArgumentIndex(inputIndex);
            if (i == -1)
            {
                throw new ArgumentOutOfRangeException("DynamicExecute closure input index out of bounds: " + inputIndex.ToString(CultureInfo.CurrentCulture));
            }

            return this.methodDefinition.CompiledMethod.GetParameters()[i].ParameterType;
        }

        /// <summary>
        /// Gets an input parameter's CLI <see cref="Type"/>.
        /// </summary>
        /// <param name="inputName">The name of the input parameter to retrieve.</param>
        /// <returns>The <see cref="Type"/> of the input parameter.</returns>
        public Type GetInputParameterType(string inputName)
        {
            int i = this.methodDefinition.GetInputArgumentIndex(inputName);
            if (i == -1)
            {
                throw new KeyNotFoundException("DynamicExecute closure input name not recognized: " + inputName);
            }

            return this.methodDefinition.CompiledMethod.GetParameters()[i].ParameterType;
        }

        /// <summary>
        /// Gets an output argument value.
        /// </summary>
        /// <param name="outputName">The name of the output argument to retrieve.</param>
        /// <returns>The value of the output argument.</returns>
        public object GetOutput(string outputName)
        {
            int i = this.methodDefinition.GetOutputArgumentIndex(outputName);
            if (i == -1)
            {
                throw new KeyNotFoundException("DynamicExecute closure output name not recognized: " + outputName);
            }

            return this.arguments[i];
        }

        /// <summary>
        /// Executes the underlying compiled method, with the currently-defined arguments.
        /// </summary>
        public void Run()
        {
            this.methodDefinition.CompiledMethod.Invoke(null, this.arguments);
        }

        /// <summary>
        /// Sets an input argument to a value.
        /// </summary>
        /// <param name="inputIndex">The zero-based index of the input parameter to set.</param>
        /// <param name="value">     The value to set.</param>
        public void SetArgument(int inputIndex, object value)
        {
            int i = this.methodDefinition.GetInputArgumentIndex(inputIndex);
            if (i == -1)
            {
                throw new ArgumentOutOfRangeException("DynamicExecute closure input index out of bounds: " + inputIndex.ToString(CultureInfo.CurrentCulture));
            }

            this.arguments[i] = value;
        }

        /// <summary>
        /// Sets an input argument to a value.
        /// </summary>
        /// <param name="inputName">The name of the input parameter to set.</param>
        /// <param name="value">    The value to set.</param>
        public void SetArgument(string inputName, object value)
        {
            int i = this.methodDefinition.GetInputArgumentIndex(inputName);
            if (i == -1)
            {
                throw new KeyNotFoundException("DynamicExecute closure input name not recognized: " + inputName);
            }

            this.arguments[i] = value;
        }

        /// <summary>
        /// Sets a default argument to a value. Invalid default parameter indices are ignored.
        /// </summary>
        /// <param name="defaultParameterIndex">The zero-based index of the default parameter to set.</param>
        /// <param name="value">                The value to set.</param>
        public void SetDefaultArgument(int defaultParameterIndex, object value)
        {
            int i = this.methodDefinition.GetDefaultArgumentIndex(defaultParameterIndex);
            if (i != -1)
            {
                this.arguments[i] = value;
            }
        }

        /// <summary>
        /// Gets an output argument value. Returns <see langref="null"/> if the output argument does not exist.
        /// </summary>
        /// <param name="outputIndex">The zero-based index of the output argument to retrieve.</param>
        /// <returns>The value of the output argument, or <see langref="null"/> if the index is out of bounds.</returns>
        public object TryGetOutput(int outputIndex)
        {
            int i = this.methodDefinition.GetOutputArgumentIndex(outputIndex);
            if (i == -1)
            {
                return null;
            }

            return this.arguments[i];
        }

        #endregion Public Methods
    }
}
