
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
using System.Collections.Generic;
using System.Globalization;
//using System.Xml;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using System;
using System.Data;
//using DotNetNuke.Services.Search;

namespace Shift8Read.Dnn.Consumer
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for Shift8ReadConsumer
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class ConsumerUrlInfo : IHydratable
    {


        #region "Private Properties"

        private DateTime lastChecked;

        #endregion

        #region "Public Properties"

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime LastChecked
        {
            get
            {
                return lastChecked;
            }
            set {
                if (value != null)
                    lastChecked = value;
                else
                    lastChecked = Convert.ToDateTime("1/1/1900");
            }
        }

        public bool Approved { get; set; }

        public bool AutoApproved { get; set; }

        public int PortalId { get; set; }

        public int CategoryId { get; set; }

        public bool StripHtml { get; set; }

        public string DefaultThumbnail { get; set; }

        public string BaseWebsiteUrl { get; set; }

        #endregion


        #region Public Methods
        public ConsumerUrlInfo()
        {
            this.Approved = false;
        }


        #endregion

        #region "IHydratable Implementation"

        public int  KeyID
        {
            get { return Id; }
            set { Id = value; }
        }

        public void Fill(IDataReader idr)
        {
            //TODO: check invariant culture info for dates
            Id = Convert.ToInt32(Null.SetNull(idr["Id"],Id));
            Name = Convert.ToString(Null.SetNull(idr["Name"], Name));
            Url = Convert.ToString(Null.SetNull(idr["Url"], Url));
            UserId = Convert.ToInt32(Null.SetNull(idr["UserId"], UserId));
            DateCreated = Convert.ToDateTime(Null.SetNull(idr["DateCreated"], DateCreated));
            LastUpdated = Convert.ToDateTime(Null.SetNull(idr["LastUpdated"], LastUpdated));

            DateTime result;
            DateTime.TryParse(idr["LastChecked"].ToString(), out result);
            if (result <Convert.ToDateTime("1/1/1900"))
            {
                LastChecked = Convert.ToDateTime("1/1/1900");
            }
            else
            {
                LastChecked = Convert.ToDateTime(Null.SetNull(idr["LastChecked"], LastChecked));
            }

            Approved = Convert.ToBoolean(Null.SetNull(idr["Approved"], Approved));
            AutoApproved = Convert.ToBoolean(Null.SetNull(idr["AutoApproved"], AutoApproved));
            PortalId = Convert.ToInt32(Null.SetNull(idr["PortalId"], PortalId));
            CategoryId = Convert.ToInt32(Null.SetNull(idr["CategoryId"], CategoryId));
            StripHtml = Convert.ToBoolean(Null.SetNull(idr["StripHtml"], StripHtml));
            DefaultThumbnail = Convert.ToString(Null.SetNull(idr["DefaultThumbnail"], DefaultThumbnail));
            BaseWebsiteUrl = Convert.ToString(Null.SetNull(idr["baseWebsiteUrl"], BaseWebsiteUrl));
        }

        #endregion

    }

}
