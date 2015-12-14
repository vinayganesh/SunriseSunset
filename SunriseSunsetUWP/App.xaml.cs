using Caliburn.Micro;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Navigation;
using SunriseSunsetUWP.ViewModels;
using SunriseSunsetUWP.IServices;
using SunriseSunsetUWP.Services;
using SunriseSunsetUWP.Views;

namespace SunriseSunsetUWP
{
    public sealed partial class App
    {
        private WinRTContainer _container;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.PerRequest<LandingPageViewModel>();

            _container.Singleton<IGeoCoderService, GeoCoderService>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            DisplayRootViewFor<LandingPageViewModel>();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
