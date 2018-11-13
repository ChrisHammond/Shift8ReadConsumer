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
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Shift8Read.Dnn.Consumer.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "Shift8Read_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object Field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        //AddConsumerUrl(ConsumerUrlInfo csi)
        //UpdateConsumerUrl(ConsumerUrlInfo csi)

        public override void AddConsumerUrl(ConsumerUrlInfo csi)
        {
            SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "spAddConsumerUrl", csi.Name, csi.Url, csi.UserId, csi.Approved, csi.AutoApproved, csi.PortalId, csi.CategoryId, csi.StripHtml, csi.DefaultThumbnail, csi.BaseWebsiteUrl);
        }

        public override void UpdateConsumerUrl(ConsumerUrlInfo csi)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "spUpdateConsumerUrl", csi.Id, csi.Name, csi.Url, csi.UserId, csi.Approved, csi.AutoApproved, csi.LastChecked, csi.CategoryId, csi.StripHtml, csi.DefaultThumbnail, csi.BaseWebsiteUrl);
        }

        public override IDataReader GetConsumerUrl(int urlId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetConsumerUrl", urlId);
        }

        public override IDataReader GetUserConsumerUrls(int userId, int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetConsumerUrlsForUser", userId, portalId);
        }

        public override IDataReader GetConsumerUrls(bool approved, int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetConsumerUrls", approved, portalId);
        }

        public override void DeleteConsumerUrl(int urlId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "spDeleteConsumerUrl", urlId);
        }


        public override void AddConsumerCategoryKeyword(ConsumerCategoryKeywordInfo ccki)
        {//dnn_Shift8Read_spAddConsumerKeywordCategory
            SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "spAddConsumerCategoryKeyword", ccki.Keyword, ccki.CategoryId, ccki.PortalId);
        }

        public override void UpdateConsumerCategoryKeyword(ConsumerCategoryKeywordInfo ccki)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "spUpdateConsumerCategoryKeyword", ccki.Id, ccki.Keyword, ccki.CategoryId);
        }

        public override IDataReader GetConsumerCategoryKeyword(int id)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetConsumerCategoryKeyword", id);
        }

        public override IDataReader GetConsumerCategoryKeywords(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetConsumerCategoryKeywords", portalId);
        }

        public override void DeleteConsumerCategoryKeyword(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "spDeleteConsumerCategoryKeyword", id);
        }

        #endregion

    }

}