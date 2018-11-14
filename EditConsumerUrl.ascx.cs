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
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Controllers;

namespace Shift8Read.Dnn.Consumer
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditShift8ReadConsumer class is used to manage content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class EditConsumerUrl : Shift8ReadModuleBase
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
                    pnlAdmin.Visible = false;
                    //todo: check if we're loading an existing URL or not
                    if (UrlId > 0)
                    {
                        ConsumerUrlController cuc = new ConsumerUrlController();
                        ConsumerUrlInfo cui = cuc.GetConsumerUrl(UrlId);
                        if (UserId == cui.UserId || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
                        {
                            pnlUrlEdit.Visible = true;
                            pnlMessage.Visible = false;
                            txtName.Text = cui.Name;
                            txtUrl.Text = cui.Url;
                            lblId.Text = cui.Id.ToString();
                            lblLastChecked.Text = cui.LastChecked.ToString();
                            txtUserId.Text = cui.UserId.ToString();
                            chkApproved.Checked = cui.Approved;
                            chkAutoApproved.Checked = cui.AutoApproved;
                            

                            if (UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
                            {
                                pnlAdmin.Visible = true;
                                chkApproved.Checked = cui.Approved;
                                chkAutoApproved.Checked = cui.AutoApproved;
                                chkStripHTML.Checked = cui.StripHtml;
                                txtDefaultThumbnail.Text = cui.DefaultThumbnail;
                                txtBaseWebsiteUrl.Text = cui.BaseWebsiteUrl;
                                LoadCategoryList();
                                ListItem li = ddlCategory.Items.FindByValue(cui.CategoryId.ToString());
                                if (li != null)
                                {
                                    ddlCategory.SelectedItem.Selected = false;
                                    ddlCategory.Items.FindByValue(cui.CategoryId.ToString()).Selected = true;
                                }
                            }
                        }
                        else
                        {
                            pnlUrlEdit.Visible = false;
                            pnlMessage.Visible = true;
                            lblMessage.Text = Localization.GetString("Unauthorized", LocalResourceFile);
                        }
                    }
                    else
                    {
                        chkApproved.Checked = false;
                        chkAutoApproved.Checked = false;
                        if (UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
                        {
                            pnlUrlEdit.Visible = true;
                            pnlMessage.Visible = false;
                            pnlAdmin.Visible = true;

                            chkApproved.Checked = true;
                            chkAutoApproved.Checked = true;
                            chkStripHTML.Checked = false;
                            LoadCategoryList();
                        }
                        txtUserId.Text = UserId.ToString();
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        private int urlId;
        public int UrlId
        {
            get 
            { 
                object o = Request.QueryString["urlid"];
                if (o != null)
                {
                    urlId = Convert.ToInt32(o.ToString());
                }
                return urlId;
            }
            set
            {
                urlId = value;
            }
        }


        protected void lbSubmit_Click(object sender, EventArgs e)
        {
            //save the feed
            if (lblId.Text == string.Empty)
            {
                ConsumerUrlInfo cui = new ConsumerUrlInfo();
                DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();
                cui.Name = objSecurity.InputFilter(txtName.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.Url= objSecurity.InputFilter(txtUrl.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.PortalId = PortalId;
                cui.UserId = UserInfo.UserID;
                cui.AutoApproved = chkAutoApproved.Checked;
                cui.Approved = chkApproved.Checked;
                cui.LastChecked = Convert.ToDateTime("1/1/1900");
                cui.StripHtml = chkStripHTML.Checked;

                cui.DefaultThumbnail = objSecurity.InputFilter(txtDefaultThumbnail.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);


                var baseUrl = objSecurity.InputFilter(txtBaseWebsiteUrl.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                if (baseUrl != string.Empty)
                {
                    //check for HTTP:// and https://
                    //check for final /
                    baseUrl = baseUrl.ToLower().Replace("http://", "").Replace("https://", "");

                    if (baseUrl.IndexOf(("/")) > -1)
                    {
                        baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("/"));
                    }
                    //cui.BaseWebsiteUrl =     
                }
                cui.BaseWebsiteUrl = baseUrl;
                
                ConsumerUrlController cuc = new ConsumerUrlController();
                cuc.AddConsumerUrl(cui);
                //add the user to the authors role

                //string authorRole = HostSettings.GetHostSetting("PublishAuthorRole" + this.PortalId);
                string authorRole = HostController.Instance.GetString("PublishAuthorRole" + this.PortalId);
                    //HostSettings.GetHostSetting("PublishAuthorRole" + this.PortalId);
                if (authorRole != string.Empty)
                {
                    RoleController rc = new RoleController();
                    RoleInfo ri = rc.GetRoleByName(PortalId, authorRole);
                    rc.AddUserRole(PortalId, UserInfo.UserID, ri.RoleID, DateTime.MaxValue, DateTime.MinValue);
                }
            }
            else
            {
                //update existing
                ConsumerUrlController cuc = new ConsumerUrlController();
                ConsumerUrlInfo cui = cuc.GetConsumerUrl(UrlId);

                cui.Id = UrlId;
                DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();
                cui.Name = objSecurity.InputFilter(txtName.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.Url = objSecurity.InputFilter(txtUrl.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                cui.Approved = chkApproved.Checked;
                cui.AutoApproved = chkAutoApproved.Checked;
                cui.PortalId = PortalId;
                cui.UserId = Convert.ToInt32(txtUserId.Text);
                cui.LastChecked = Convert.ToDateTime(lblLastChecked.Text).ToUniversalTime();
                cui.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                cui.StripHtml = chkStripHTML.Checked;
                cui.DefaultThumbnail = objSecurity.InputFilter(txtDefaultThumbnail.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                
                
                var baseUrl = objSecurity.InputFilter(txtBaseWebsiteUrl.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup);
                if(baseUrl!=string.Empty)
                {
                    //check for HTTP:// and https://
                    //check for final /
                    baseUrl = baseUrl.ToLower().Replace("http://", "").Replace("https://", "");
                    
                    if(baseUrl.IndexOf(("/"))>-1)
                    {
                        baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("/"));
                    }
                    //cui.BaseWebsiteUrl =     
                }
                cui.BaseWebsiteUrl = baseUrl;

                cuc.UpdateConsumerUrl(cui);

                //string authorRole = HostSettings.GetHostSetting("PublishAuthorRole" + this.PortalId);
                string authorRole = HostController.Instance.GetString("PublishAuthorRole" + this.PortalId);
                if (authorRole != string.Empty)
                {
                    RoleController rc = new RoleController();
                    RoleInfo ri = rc.GetRoleByName(PortalId, authorRole);
                    rc.AddUserRole(PortalId, cui.UserId, ri.RoleID, DateTime.MaxValue, DateTime.MinValue);
                }

            }
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            ConsumerUrlController.DeleteConsumerUrl(Convert.ToInt16(lblId.Text));
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