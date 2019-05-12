using Pronto.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Pronto.Controllers
{
    [PluginController("Pronto")]
    public class ProntoBackofficeApiController : UmbracoAuthorizedJsonController
    {

        public ProntoBackofficeApiController()
        {
        }

        public DashboardData GetDashboardData()
        {
            var prontoContentItem = ApplicationContext.Services.ContentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "prontoDashboard");
            if(prontoContentItem != null)
            {
                var dashboardData = new DashboardData();
                dashboardData.Title = prontoContentItem.GetValue<string>("title");
                dashboardData.Subtitle = prontoContentItem.GetValue<string>("subtitle");
                dashboardData.UsefulLinks = GetDashboardLinksFromContent(prontoContentItem, "usefulLinks");

                return dashboardData;
            }
            return null;
        }

        private static List<DashboardLink> GetDashboardLinksFromContent(IContent prontoContentItem, string propertyAlias)
        {
            var usefulLinks = prontoContentItem.GetValue(propertyAlias);
            var usefulLinkItems = Newtonsoft.Json.Linq.JArray.Parse(usefulLinks.ToString());

            List<DashboardLink> usefulLinkList = new List<DashboardLink>();
            foreach (var item in usefulLinkItems)
            {
                DashboardLink link = new DashboardLink();
                link.Title = item["title"].ToString();
                link.Description = item["description"].ToString();
                link.Url = item["url"].ToString();
                usefulLinkList.Add(link);
            }

            return usefulLinkList;
        }
    }
}
