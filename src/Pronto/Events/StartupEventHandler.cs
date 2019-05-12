using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.UI.JavaScript;

namespace Pronto.Events
{
    public class StartupEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;

        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            var urlHelper =
                new UrlHelper(
                    new RequestContext(
                        new HttpContextWrapper(
                            HttpContext.Current),
                        new RouteData()));

            if (!e.ContainsKey("Pronto"))
                e.Add("Pronto", new Dictionary<string, object>
            {
                {
                    "GetDashboardDataApiUrl",
                    urlHelper.GetUmbracoApiServiceBaseUrl<Controllers.ProntoBackofficeApiController>(
                        controller => controller.GetDashboardData())
                }
            });
        }
    }
}
