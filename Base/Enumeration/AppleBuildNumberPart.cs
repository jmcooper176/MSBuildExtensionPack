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
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using MSBuild.ExtensionPack.Base.SystemAttribute;

    [Flags]
    public enum AppleBuildNumberPart : int
    {
        /// <summary>
        /// No increment of any part of the Apple build number will be attempted.
        /// </summary>
        [Display(Name = "No Increment of Any Part", ShortName = "No Increment")]
        [Description("No increment of any part of the Apple build number will be attempted.")]
        None = 0,

        /// <summary>
        /// The build major part of the Apple build number will be incremented.
        /// </summary>
        [Display(Name = "Increment Build Major", ShortName = "Increment Major")]
        [Description("The build major part of the Apple build number will be incremented.")]
        Major = 1,

        /// <summary>
        /// The release character part of the Apple build number will be incremented.
        /// </summary>
        [Display(Name = "Increment Release Character of Build Number", ShortName = "Increment Release Character")]
        [Description("The release character part of the Apple build number will be incremented.")]
        ReleaseChar = 2,

        /// <summary>
        /// The build revision part of the Apple build number will be incremented.
        /// </summary>
        [Display(Name = "Increment Build Revision", ShortName = "Increment Revision")]
        [Description("The build revision part of the Apple build number will be incremented.")]
        Revision = 4,

        /// <summary>
        /// All parts of the Apple build number will be incremented.
        /// </summary>
        [Display(Name = "Increment All Parts of the Build Number", ShortName = "Increment All")]
        [Description("All parts of the Apple build number will be incremented.")]
        All = Major | ReleaseChar | Revision
    }

    /// <summary>
    /// Implements extension methods for the <see cref="AppleBuildNumberPart"/> enumeration to extract fields associated with the
    /// <see cref="DisplayAttribute"/> and <see cref="DescriptionAttribute"/>.
    /// </summary>
    public static class AppleBuildNumberPartExtension
    {
        #region Public Methods

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateField"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateField"/>.</returns>
        public static bool? GetAutoGenerateField(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateField;
        }

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateFilter"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateFilter"/>.</returns>
        public static bool? GetAutoGenerateFilter(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateFilter;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string? GetDescription(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDescriptionAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetDescription2(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DescriptionAttribute"/> or <see langref="null"/> if no <see cref="DescriptionAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DescriptionAttribute? GetDescriptionAttribute(this AppleBuildNumberPart value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DescriptionAttribute, AppleBuildNumberPart>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the <see cref="DisplayAttribute"/> on an <see cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DisplayAttribute"/> or <see langref="null"/> if no <see cref="DisplayAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DisplayAttribute? GetDisplayAttribute(this AppleBuildNumberPart value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DisplayAttribute, AppleBuildNumberPart>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the group name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the group name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetGroupName(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.GroupName;
        }

        /// <summary>
        /// Extension method to recover the name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetName(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>An <see cref="int"/> or <see langref="null"/> representing the order property of the <see cref="DisplayAttribute"/>.</returns>
        public static int? GetOrder(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Order;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the prompt for the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetPrompt(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Prompt;
        }

        /// <summary>
        /// Extension method to recover the resource <see cref="Type"/> from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="Type"/> or <see langref="null"/> representing the resource <see cref="Type"/> of the <see cref="DisplayAttribute"/>.</returns>
        public static Type? GetResourceType(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ResourceType;
        }

        /// <summary>
        /// Extension method to recover the short name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="AppleBuildNumberPart"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="AppleBuildNumberPart"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the short name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetShortName(this AppleBuildNumberPart value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ShortName;
        }

        #endregion Public Methods
    }
}
