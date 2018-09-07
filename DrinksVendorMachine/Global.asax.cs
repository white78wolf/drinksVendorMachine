using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DrinksVendorMachine
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            var user = System.Web.HttpContext.Current.User;
            SimpleConfigurationPrincipal.SetAuthenticatedPrincipal(user);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Request.IsSecureConnection.Equals(false) &&
                System.Web.HttpContext.Current.Request.IsLocal.Equals(false))
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] +
                                  System.Web.HttpContext.Current.Request.RawUrl);
            }
        }
        
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}