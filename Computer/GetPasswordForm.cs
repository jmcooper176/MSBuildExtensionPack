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
namespace MSBuild.ExtensionPack.Computer.Extended
{
    using System;

    /// <summary>
    /// Gets a user's AD validated password
    /// </summary>
    /// <seealso cref="Form"/>
    public partial class GetPasswordForm : Form
    {
        private readonly ContextOptions contextOptions;
        private readonly ContextType contextType;
        private readonly string domain;
        private readonly string user;

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.ProcessCancel();
        }

        private void ButtonOk_Click(object sender, System.EventArgs e)
        {
            this.ProcessOk();
        }

        private void CheckBoxMask_CheckedChanged(object sender, System.EventArgs e)
        {
            this.textBoxPassword.UseSystemPasswordChar = this.checkBoxMask.Checked;
        }

        private void GetPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.UserCanceled = true;
            }
        }

        private void ProcessCancel()
        {
            this.UserCanceled = true;
            this.Password = string.Empty;
            this.Close();
        }

        private void ProcessOk()
        {
            try
            {
                PrincipalContext pcontext = new PrincipalContext(this.contextType, this.domain);
                using (pcontext)
                {
                    if (pcontext.ValidateCredentials(this.user, this.textBoxPassword.Text, this.contextOptions) == false)
                    {
                        this.labelPassword.ForeColor = System.Drawing.Color.DarkRed;
                        this.textBoxPassword.BackColor = System.Drawing.Color.Coral;
                        this.pictureBoxLock.Visible = true;
                        this.textBoxPassword.Select();
                    }
                    else
                    {
                        this.Password = this.textBoxPassword.Text;
                        this.labelPassword.ForeColor = System.Drawing.Color.DarkGreen;
                        this.textBoxPassword.BackColor = System.Drawing.Color.WhiteSmoke;
                        this.pictureBoxLock.Visible = false;
                        this.pictureBoxOpenLock.Visible = true;
                        this.Refresh();
                        System.Threading.Thread.Sleep(400);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                this.Close();
            }
        }

        private void TextBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.ProcessOk();
                    break;

                case Keys.Escape:
                    this.ProcessCancel();
                    break;
            }
        }

        public GetPasswordForm(string user, string domain, ContextType type, ContextOptions options)
        {
            this.InitializeComponent();

            if (!string.IsNullOrEmpty(domain))
            {
                this.Text += domain + @"\";
            }

            this.Text += user;
            this.user = user;
            this.domain = domain;
            this.contextType = type;
            this.contextOptions = options;
        }

        public Exception Exception { get; set; }

        public string Password { get; set; }

        public bool UserCanceled { get; set; }
    }
}
