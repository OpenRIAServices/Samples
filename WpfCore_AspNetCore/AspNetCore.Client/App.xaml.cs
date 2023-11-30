﻿using AspNetCore.Client;
using OpenRiaServices.Client;
using OpenRiaServices.Client.Authentication;
using System;
using System.Windows;

namespace AspNetCore
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
            DomainContext.DomainClientFactory = new OpenRiaServices.Client.DomainClients.BinaryHttpDomainClientFactory(new Uri("https://localhost:50694/", UriKind.Absolute), new System.Net.Http.HttpClientHandler());

            Resources["WebContext"] = new WebContext()
            {
                Authentication = new FormsAuthentication()
                {
                    DomainContext = new AspNetCore.Hosting.AspNetCore.Services.MyAuthenticationContext(),
                },
            };
        }
    }
}
