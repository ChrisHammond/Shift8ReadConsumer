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
    /// The Controller class for Shift8ReadConsumerCategoryKeywordInfo
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class ConsumerCategoryKeywordInfo : DotNetNuke.Entities.Modules.IHydratable
    {

        #region "Private Properties"
        private int id;
        private string keyword;
        private int categoryId;
        private int portalId;
        #endregion

        #region "Public Properties"

        public int Id
        {
            get
            {
                return id;
            }
            set { id = value; }
        }

        public string Keyword
        {
            get
            {
                return keyword;
            }
            set { keyword = value; }
        }

        public int CategoryId
        {
            get
            {
                return categoryId;
            }
            set { categoryId = value; }
        }

        public int PortalId
        {
            get
            {
                return portalId;
            }
            set { portalId = value; }
        }

        #endregion


        #region Public Methods
        public ConsumerCategoryKeywordInfo()
        {
            
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
            Keyword = Convert.ToString(Null.SetNull(idr["Keyword"], Keyword));
            PortalId = Convert.ToInt32(Null.SetNull(idr["PortalId"], PortalId));
            CategoryId = Convert.ToInt32(Null.SetNull(idr["CategoryId"], CategoryId));
        }

        #endregion

    }

}
