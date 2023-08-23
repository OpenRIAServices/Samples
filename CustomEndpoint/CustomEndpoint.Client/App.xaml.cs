using CustomEndpoint.Web;
using OpenRiaServices.Client;
using OpenRiaServices.Client.Authentication;
using OpenRiaServices.Client.Web;
using System;
using System.Windows;

namespace CustomEndpoint.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // DomainClientFactory and DomainContext for authentication is setup at login time
            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication()
            {
                DomainContextType = typeof(AuthenticationDomainService1).AssemblyQualifiedName
            };
        }
    }
}