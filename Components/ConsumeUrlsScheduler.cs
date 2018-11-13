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

namespace Shift8Read.Dnn.Consumer
{

    using System.Collections.Generic;
    using System.Globalization;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Services.Exceptions;

    using System;
    using System.Collections;
    using System.Data;
    using Engage.Dnn.Publish;
    using Engage.Dnn.Publish.Util;
    using System.Text;

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for MemberSegment
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class ConsumeUrlsScheduler : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        public ConsumeUrlsScheduler(DotNetNuke.Services.Scheduling.ScheduleHistoryItem shi)
        {
            ScheduleHistoryItem = shi;
        }

        public List<ConsumerCategoryKeywordInfo> ccki;

        public override void DoWork()
        {
            try
            {
                Progressing();
                //DO THE WORK
                string returnMessage = LoadUrls();
                returnMessage += " <br /> Consumer Urls Parsed Successfully";
                ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote(returnMessage);

            }
            catch (Exception exc)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote("Consumer Urls Parse Failed, did you configure the settings?");
                Errored(ref exc);
            }
        }
        private string LoadUrls()
        {
            var sb = new StringBuilder(500);


            var cuc = new ConsumerUrlController();
            //do this for each portal?

            var pc = new PortalController();
            ArrayList portals = pc.GetPortals();
            foreach (PortalInfo p in portals)
            {
                //load the categorykeyword listing for portals
                var cckc = new ConsumerCategoryKeywordController();
                ccki = cckc.GetConsumerCategoryKeywords(p.PortalID);

                var mc = new ModuleController();
                ModuleInfo mi = mc.GetModuleByDefinition(p.PortalID, "Shift8ReadConsumer");
                if (mi != null)
                {
                    Hashtable settings = mc.GetModuleSettings(mi.ModuleID);

                    if (settings.Contains("CategoryId") && settings.Contains("ModuleId"))
                    {
                        List<ConsumerUrlInfo> cl = cuc.GetConsumerUrls(true, p.PortalID);

                        foreach (ConsumerUrlInfo cui in cl)
                        {
                            int categoryId = cui.CategoryId > 0 ? cui.CategoryId : Convert.ToInt32(settings["CategoryId"]);
                            int moduleId = Convert.ToInt32(settings["ModuleId"]);

                            sb.Append(ConsumeSpecificUrl(categoryId, moduleId, p.PortalID, cui));

                            cuc.UpdateConsumerUrl(cui);
                        }
                        //Clear the cache
                        Utility.ClearPublishCache(p.PortalID);
                    }
                    else
                    {
                        sb.Append("<br /> Module settings not configured<br />");
                    }
                }
            }
            return sb.ToString();
        }

        public string ConsumeSpecificUrl(int categoryId, int moduleId, int portalId, ConsumerUrlInfo cui)
        {
            var sb = new StringBuilder(100);
            RssToolkit.Rss.RssDocument rss;
            try
            {

                rss = RssToolkit.Rss.RssDocument.Load(new Uri(cui.Url));
                //no matter what the feed comes in as, we should force it to be RSS
                //rss = RssToolkit.Rss.RssDocument.Load(rss.ToXml(RssToolkit.Rss.DocumentType.Rss));
            }
            catch (Exception exc)
            {
                Exceptions.ProcessSchedulerException(exc);
                sb.AppendFormat(" Feed Failed On Initial Request For: {0} <br />", cui.Name);
                return sb.ToString();

            }
            if (rss != null)
            {
                //IEnumerable ie = rss.SelectItems();
                //sb.Append(" <br /> Getting Feed ");
                //sb.Append(cui.Name.ToString());
                //sb.Append("<br />");

                DataSet dsRss = rss.ToDataSet();
                var dataView = new DataView(dsRss.Tables["item"]);
                dataView.Table.Columns.Add("dateColumn", typeof(DateTime));

                try
                {
                    foreach (DataRowView dr in dataView)
                    {
                        try
                        {
                            string date = dr["pubdateparsed"].ToString();
                            dr["dateColumn"] = Convert.ToDateTime(date.ToString(CultureInfo.InvariantCulture));
                        }
                        catch (Exception exc)
                        {
                            var exc1 = new Exception(exc.Message + String.Format("Feed Failed On Conversion For: {0} {1}", dr["pubdate"], cui.Name));

                            throw exc1;
                        }
                    }
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessSchedulerException(exc);
                    return sb.ToString();
                }
                dataView.Sort = " dateColumn asc ";
                try
                {
                    foreach (DataRowView dr in dataView)//dsRss.Tables["item"].Rows)
                    {
                        //todo: how to verify timestamps?

                        //if (cui.LastChecked < Convert.ToDateTime(dr["pubdate"]))
                        //check for existance?

                        if (!Item.DoesItemExist(dr["Title"].ToString().Trim(), cui.UserId, categoryId))
                        {
                            string articleText = dr["Description"].ToString().Trim();
                            //only filter if the setting is enabled
                            if (cui.StripHtml)
                            {
                                var ps = new PortalSecurity();
                                articleText = DotNetNuke.Common.Utilities.HtmlUtils.StripTags(articleText, false);
                                if(articleText.Length>510)
                                {
                                    articleText = articleText.Substring(0, 510);
                                    articleText += " ... ";
                                }
                            }
                            
                            //check for missing paths to images and downloads
                            if(cui.BaseWebsiteUrl!=string.Empty)
                            {
                                articleText = articleText.Replace("src=\"/portals", "src=\"http://" + cui.BaseWebsiteUrl + "/portals");
                                articleText = articleText.Replace("src=\"/Portals", "src=\"http://" + cui.BaseWebsiteUrl + "/portals");
                            }
                            
                            string articleTitle = dr["Title"].ToString().Trim();
                            Article a = Article.Create(articleTitle, articleText, articleText, cui.UserId, categoryId, moduleId, portalId);
                            a.Url = dr["Link"].ToString();
                            if (cui.DefaultThumbnail.Trim() != string.Empty)
                            {
                                a.Thumbnail = cui.DefaultThumbnail;
                            }

                            //look for keywords and add related categories
                            foreach (ConsumerCategoryKeywordInfo cki in ccki)
                            {
                                if (articleText.Contains(cki.Keyword) || articleTitle.Contains(cki.Keyword))
                                {
                                    var irel = new ItemRelationship
                                                   {
                                                       RelationshipTypeId =
                                                           RelationshipType.ItemToRelatedCategory.GetId(),
                                                       ParentItemId = cki.CategoryId
                                                   };
                                    a.Relationships.Add(irel);
                                }
                            }

                            string pubdate = dr["pubdate"].ToString();
                            pubdate = pubdate.Replace("PDT", "");
                            pubdate = pubdate.Replace("PST", "");
                            if (pubdate.Trim() == string.Empty)
                            {
                                pubdate = DateTime.Now.ToString();
                            }
                            a.StartDate = pubdate;
                            a.CreatedDate = pubdate;
                            a.LastUpdated = pubdate;

                            a.ApprovalStatusId = cui.AutoApproved ? ApprovalStatus.Approved.GetId() : ApprovalStatus.Waiting.GetId();
                            a.NewWindow = true;

                            //create itemversionsetting for the authorname
                            //AuthorName setting
                            Setting setting = Setting.AuthorName;
                            setting.PropertyValue = cui.Name;
                            var itemVersionSetting = new ItemVersionSetting(setting);
                            a.VersionSettings.Add(itemVersionSetting);

                            //
                            //setting = Setting.ArticleAttachment;
                            //setting.PropertyValue = dr["enclosure"].ToString();
                            //ItemVersionSetting itemVersionSetting = new ItemVersionSetting(setting);
                            //a.VersionSettings.Add(itemVersionSetting);

                            a.Save(cui.UserId);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessSchedulerException(exc);
                    sb.AppendFormat(" Feed Failed: {0} <br /> ", cui.Name);
                }

                cui.LastChecked = DateTime.UtcNow;
                return sb.ToString();
                //cuc.UpdateConsumerUrl(cui);
                //
            }
            return sb.ToString();
        }
    }
}