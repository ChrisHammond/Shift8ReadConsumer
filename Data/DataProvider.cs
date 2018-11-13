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
using System.Collections;
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace Shift8Read.Dnn.Consumer.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class for the data access layer
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

        #region Shared/Static Methods
        // singleton reference to the instantiated object 
        //private static DataProvider provider = ((DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "Engage.Dnn.Publish.Data", ""));

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "Shift8Read.Dnn.Consumer.Data.SqlDataprovider,Shift8ReadConsumer";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString.ToString();
            newConnection.Open();
            return newConnection;
        }

        #endregion


        #region Abstract methods

        public abstract IDataReader GetUserConsumerUrls(int userId, int portalId);

        public abstract IDataReader GetConsumerUrl(int urlId);
        public abstract IDataReader GetConsumerUrls(bool approved, int portalId);

        public abstract void AddConsumerUrl(ConsumerUrlInfo csi);

        public abstract void AddConsumerCategoryKeyword(ConsumerCategoryKeywordInfo ccki);
        public abstract void UpdateConsumerCategoryKeyword(ConsumerCategoryKeywordInfo ccki);
        public abstract IDataReader GetConsumerCategoryKeyword(int id);
        public abstract IDataReader GetConsumerCategoryKeywords(int portalId);

        public abstract void DeleteConsumerCategoryKeyword(int id);

        public abstract void UpdateConsumerUrl(ConsumerUrlInfo csi);

        public abstract void DeleteConsumerUrl(int urlId);



        #endregion

    }

}