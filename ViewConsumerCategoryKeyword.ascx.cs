/*
' Copyright (c) 2004-2011 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Data;
using System.Globalization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules.Actions;

namespace Shift8Read.Dnn.Consumer
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewShift8ReadConsumer class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class ViewConsumerCategoryKeyword : Shift8ReadModuleBase, IActionable
    {

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                //check if admin, if so do different
                if (!Page.IsPostBack)
                {
                    LoadKeywords();
                }
                lnkAddUrl.NavigateUrl = EditUrl();

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        private void LoadKeywords()
        {
            ConsumerCategoryKeywordController cuc = new ConsumerCategoryKeywordController();
            List<ConsumerCategoryKeywordInfo> cui = cuc.GetConsumerCategoryKeywords(PortalId);
            gvConsumerUrls.DataSource = cui;
            gvConsumerUrls.DataBind();
        }


        protected string BuildEditUrl(object id)
        {
            return EditUrl("", "", "", "id=" + id.ToString());
        }


        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();

                return actions;
            }
        }


        #endregion

    }

}
