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
using System.Reflection;

namespace MSBuild.ExtensionPack
{
    /// <summary>
    /// Represents a compiled DynamicExecute method definition.
    /// </summary>
    public class MethodDefinition
    {
        #region Private Fields

        /// <summary>
        /// The actual compiled method.
        /// </summary>
        private readonly MethodInfo compiledMethod;

        /// <summary>
        /// The names of input parameters for this method.
        /// </summary>
        private readonly IEnumerable<string> inputs;

        /// <summary>
        /// The number of default parameters for this method.
        /// </summary>
        private readonly int numberOfDefaultParameters;

        /// <summary>
        /// The names of output parameters for this method.
        /// </summary>
        private readonly IEnumerable<string> outputs;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDefinition"/> class, creating a new method definition.
        /// </summary>
        /// <param name="compiledMethod">           The underlying compiled method.</param>
        /// <param name="numberOfDefaultParameters">The number of default parameters for this method.</param>
        /// <param name="inputs">                   The inputs for the method.</param>
        /// <param name="outputs">                  The outputs for the method.</param>
        public MethodDefinition(MethodInfo compiledMethod, int numberOfDefaultParameters, IEnumerable<string> inputs, IEnumerable<string> outputs)
        {
            this.compiledMethod = compiledMethod;
            this.numberOfDefaultParameters = numberOfDefaultParameters;
            this.inputs = inputs.ToArray();
            this.outputs = outputs.ToArray();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Returns the actual compiled method.
        /// </summary>
        public MethodInfo CompiledMethod => this.compiledMethod;

        /// <summary>
        /// Returns the total number of parameters required to invoke the method (including default, explicit, and return values).
        /// </summary>
        public int NumberOfParameters => this.numberOfDefaultParameters + this.inputs.Length + this.outputs.Length;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the argument index for the given default parameter, or -1 if it is not defined.
        /// </summary>
        /// <param name="defaultIndex">The zero-based index of the default parameter.</param>
        /// <returns>The argument index for the default parameter, or -1 if that parameter is not defined.</returns>
        public int GetDefaultArgumentIndex(int defaultIndex)
        {
            if (defaultIndex >= 0 && defaultIndex < this.numberOfDefaultParameters)
            {
                return defaultIndex;
            }

            return -1;
        }

        /// <summary>
        /// Returns the argument index for the given input parameter, or -1 if it is not defined.
        /// </summary>
        /// <param name="inputIndex">The zero-based index of the input parameter.</param>
        /// <returns>The argument index for the input parameter, or -1 if that parameter is not defined.</returns>
        public int GetInputArgumentIndex(int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= this.inputs.Length)
            {
                return -1;
            }

            return this.numberOfDefaultParameters + inputIndex;
        }

        /// <summary>
        /// Returns the argument index for the given input parameter, or -1 if it is not defined.
        /// </summary>
        /// <param name="inputName">The name of the input parameter.</param>
        /// <returns>The argument index for the input parameter, or -1 if that parameter is not defined.</returns>
        public int GetInputArgumentIndex(string inputName)
        {
            for (int i = 0; i != this.inputs.Length; ++i)
            {
                if (this.inputs[i] == inputName)
                {
                    return this.numberOfDefaultParameters + i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the argument index for the given output parameter, or -1 if it is not defined.
        /// </summary>
        /// <param name="outputIndex">The zero-based index of the output parameter.</param>
        /// <returns>The argument index for the output parameter, or -1 if that parameter is not defined.</returns>
        public int GetOutputArgumentIndex(int outputIndex)
        {
            if (outputIndex < 0 || outputIndex >= this.outputs.Length)
            {
                return -1;
            }

            return this.numberOfDefaultParameters + this.inputs.Length + outputIndex;
        }

        /// <summary>
        /// Returns the argument index for the given output parameter, or -1 if it is not defined.
        /// </summary>
        /// <param name="outputName">The name of the output parameter.</param>
        /// <returns>The argument index for the output parameter, or -1 if that parameter is not defined.</returns>
        public int GetOutputArgumentIndex(string outputName)
        {
            for (int i = 0; i != this.outputs.Length; ++i)
            {
                if (this.outputs[i] == outputName)
                {
                    return this.numberOfDefaultParameters + this.inputs.Length + i;
                }
            }

            return -1;
        }

        #endregion Public Methods
    }
}
