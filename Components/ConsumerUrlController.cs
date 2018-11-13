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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using Shift8Read.Dnn.Consumer.Data;



namespace Shift8Read.Dnn.Consumer
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for Shift8ReadConsumer
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class ConsumerUrlController : IPortable
    {

        public ConsumerUrlController()
        {

        }

        #region Public Methods
            //add consumer

        public void AddConsumerUrl(ConsumerUrlInfo cui)
        {
            DataProvider.Instance().AddConsumerUrl(cui);
        }

        public void UpdateConsumerUrl(ConsumerUrlInfo cui)
        {
            DataProvider.Instance().UpdateConsumerUrl(cui);
        }
        
        //get consumer

        public ConsumerUrlInfo GetConsumerUrl(int urlId)
        {
            return CBO.FillObject<ConsumerUrlInfo>(DataProvider.Instance().GetConsumerUrl(urlId));
        }

        public List<ConsumerUrlInfo> GetUserConsumerUrls(int userId, int portalId)
        {
            return CBO.FillCollection<ConsumerUrlInfo>(DataProvider.Instance().GetUserConsumerUrls(userId, portalId));
        }

        public List<ConsumerUrlInfo> GetConsumerUrls(bool approved, int portalId)
        {
            return CBO.FillCollection<ConsumerUrlInfo>(DataProvider.Instance().GetConsumerUrls(approved, portalId));
        }


        public static void DeleteConsumerUrl(int urlId)
        {
            DataProvider.Instance().DeleteConsumerUrl(urlId);
        }


        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {
            //string strXML = "";

            //List<Shift8ReadConsumerInfo> colShift8ReadConsumers = GetShift8ReadConsumers(ModuleID);
            //if (colShift8ReadConsumers.Count != 0)
            //{
            //    strXML += "<Shift8ReadConsumers>";

            //    foreach (Shift8ReadConsumerInfo objShift8ReadConsumer in colShift8ReadConsumers)
            //    {
            //        strXML += "<Shift8ReadConsumer>";
            //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objShift8ReadConsumer.Content) + "</content>";
            //        strXML += "</Shift8ReadConsumer>";
            //    }
            //    strXML += "</Shift8ReadConsumers>";
            //}

            //return strXML;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            //XmlNode xmlShift8ReadConsumers = DotNetNuke.Common.Globals.GetContent(Content, "Shift8ReadConsumers");
            //foreach (XmlNode xmlShift8ReadConsumer in xmlShift8ReadConsumers.SelectNodes("Shift8ReadConsumer"))
            //{
            //    Shift8ReadConsumerInfo objShift8ReadConsumer = new Shift8ReadConsumerInfo();
            //    objShift8ReadConsumer.ModuleId = ModuleID;
            //    objShift8ReadConsumer.Content = xmlShift8ReadConsumer.SelectSingleNode("content").InnerText;
            //    objShift8ReadConsumer.CreatedByUser = UserID;
            //    AddShift8ReadConsumer(objShift8ReadConsumer);
            //}

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        #endregion

    }

}
