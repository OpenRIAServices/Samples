using OpenRiaServices.Hosting;
using OpenRiaServices.Hosting.Wcf.Configuration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CustomEndpoint.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Older solution, but does not use internal types
            // DomainServicesSection.Current.Endpoints.Add(new System.Configuration.ProviderSettings("binary2", typeof(BinaryEndpointFactory).AssemblyQualifiedName));

            // Remove default binary REST
            var configuration = DomainServiceHostingConfiguration.Current;
            configuration.EndpointFactories.Clear();
            configuration.EndpointFactories.Add(new BinaryEndpointFactory());
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

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