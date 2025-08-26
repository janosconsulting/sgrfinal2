using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ERP.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_End(object sender, EventArgs e)
        {

            //  Response.redirect("SessionExpired.aspx");

        }
    }
}
