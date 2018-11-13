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
    /// The Settings class manages Module Settings
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : SettingsBase
    {

        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    LoadModuleList();
                    LoadCategoryList();

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                ModuleController modules = new ModuleController();
                if (this.ddlCategory.SelectedIndex > -1)
                {
                    modules.UpdateModuleSetting(this.ModuleId, "CategoryId", ddlCategory.SelectedValue.ToString());
                }

                if (this.ddlModules.SelectedIndex > -1)
                {
                    modules.UpdateModuleSetting(this.ModuleId, "ModuleId", ddlModules.SelectedValue.ToString());
                }


            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        public void LoadModuleList()
        {
            ModuleController mc = new ModuleController();
            foreach (ModuleInfo mi in mc.GetModulesByDefinition(PortalId, "Engage: Publish"))
            {
                ddlModules.Items.Add(new ListItem(mi.ModuleTitle + " " + mi.TabID.ToString(), mi.ModuleID.ToString()));
            }

            if(Settings.Contains("ModuleId"))
            ddlModules.SelectedValue = Settings["ModuleId"].ToString();
            
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

