using System;
using System.Linq;
using AspNetCore.Client.Avalonia.ViewModels;
using AspNetCore.Client.Avalonia.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using OpenRiaServices.Client;

namespace AspNetCore.Client.Avalonia
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			DomainContext.DomainClientFactory = new OpenRiaServices.Client.DomainClients.BinaryHttpDomainClientFactory(new Uri("https://localhost:50694/", UriKind.Absolute), new System.Net.Http.HttpClientHandler());

			
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
				// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
				DisableAvaloniaDataAnnotationValidation();
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainViewModel()
				};
			}
			else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
			{
				singleViewPlatform.MainView = new MainView
				{
					DataContext = new MainViewModel()
				};
			}

			base.OnFrameworkInitializationCompleted();
		}

		private void DisableAvaloniaDataAnnotationValidation()
		{
			// Get an array of plugins to remove
			var dataValidationPluginsToRemove =
				 BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

			// remove each entry found
			foreach (var plugin in dataValidationPluginsToRemove)
			{
				BindingPlugins.DataValidators.Remove(plugin);
			}
		}
	}
}