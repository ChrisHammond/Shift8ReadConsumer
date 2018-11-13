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
using System.Globalization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish;
using System.Web.UI.WebControls;


namespace Shift8Read.Dnn.Consumer
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditShift8ReadConsumer class is used to manage content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class EditCategoryKeyword : Shift8ReadModuleBase
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
                if (!Page.IsPostBack)
                {
                    if (CkId > 0)
                    {
                        ConsumerCategoryKeywordController cuc = new ConsumerCategoryKeywordController();
                        ConsumerCategoryKeywordInfo cui = cuc.GetConsumerCategoryKeyword(CkId);
                        pnlMessage.Visible = false;
                        txtKeyword.Text = cui.Keyword;                    
                        LoadCategoryList();
                        ListItem li = ddlCategory.Items.FindByValue(cui.CategoryId.ToString());
                        if (li != null)
                        {
                            ddlCategory.SelectedItem.Selected = false;
                            ddlCategory.Items.FindByValue(cui.CategoryId.ToString()).Selected = true;
                        }
                        lblId.Text = CkId.ToString();
                    }
                    else
                    {
                        LoadCategoryList();
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        private int _id;
        public int CkId
        {
            get 
            {
                object o = Request.QueryString["id"];
                if (o != null)
                {
                    _id = Convert.ToInt32(o.ToString());
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        protected void lbSubmit_Click(object sender, EventArgs e)
        {
            //save the feed
            if (lblId.Text == string.Empty)
            {
                ConsumerCategoryKeywordInfo cui = new ConsumerCategoryKeywordInfo();
                DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();
                cui.Keyword = objSecurity.InputFilter(txtKeyword.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.PortalId = PortalId;
                cui.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                ConsumerCategoryKeywordController cuc = new ConsumerCategoryKeywordController();
                cuc.AddConsumerCategoryKeyword(cui);

            }
            else
            {
                //update existing
                ConsumerCategoryKeywordController cuc = new ConsumerCategoryKeywordController();
                ConsumerCategoryKeywordInfo cui = cuc.GetConsumerCategoryKeyword(CkId);

                cui.Id = CkId;
                DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();
                cui.PortalId = PortalId;
                cui.Keyword = objSecurity.InputFilter(txtKeyword.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);

                cuc.UpdateConsumerCategoryKeyword(cui);

            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            ConsumerCategoryKeywordController.DeleteConsumerCategoryKeyword(Convert.ToInt16(lblId.Text));
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        public void LoadCategoryList()
        {
            ddlCategory.DataSource = Category.GetCategoriesHierarchy(PortalId);
            ddlCategory.DataBind();
            if (Settings.Contains("CategoryId"))
                ddlCategory.SelectedValue = Settings["CategoryId"].ToString();
        }
    }
}