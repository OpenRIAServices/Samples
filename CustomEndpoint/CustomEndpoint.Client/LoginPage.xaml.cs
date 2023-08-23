using OpenRiaServices.Client;
using OpenRiaServices.Client.Authentication;
using CustomEndpoint.Web;
using System;
using System.Windows;
using System.Windows.Controls;


namespace CustomEndpoint.Client
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            DomainContext.DomainClientFactory = new OpenRiaServices.Client.Web.BinaryDomainClientFactory()
            {
                ServerBaseUri = new Uri(Uri.Text, UriKind.Absolute)
            };

            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as WebContext.Current.
            WebContext.Current.Authentication = new FormsAuthentication()
            {
                DomainContext = new AuthenticationDomainService1()
            };

            LoginButton.IsEnabled = false;
            WebContext.Current.Authentication.Login(new LoginParameters(
                this.UserName.Text, this.Password.Text), OnLoginCompleted, null);
        }

        private void OnLoginCompleted(LoginOperation obj)
        {
            LoginButton.IsEnabled = true;
            if (obj.HasError)
            {
                MessageBox.Show($"Login failed: {obj.Error.Message}");
                obj.MarkErrorAsHandled();
            }
            else
            {
                MessageBox.Show($"LoginSuccess: {obj.LoginSuccess}");
                if (obj.LoginSuccess)
                {
                    this.NavigationService.Navigate(new MainPage());
                }
            }
        }
    }
}
