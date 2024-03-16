using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using SpudPI.BlazorClassLibrary;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows;

namespace SpudPI.WPF.Client
{

    /// <summary>
    /// Interaktionslogik für BlazorWebViewPage.xaml
    /// </summary>
    public partial class BlazorWebViewPage : System.Windows.Controls.Page
    {
        private static string[] WebViewContextMenuItemsToRemove =
            ["share", "webSelect", "webCapture",
            "inspectElement", "saveAs", "reload",
            "copyLinkToHighlight", "openLinkInNewWindow", "saveLinkAs", "copyLinkLocation"];

        private INavigationService _navigationService;

        public BlazorWebViewPage()
        {
            InitializeComponent();
            _navigationService = (Application.Current as App)!.ServiceProvider.GetRequiredService<INavigationService>();

            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            services.AddBlazoredLocalStorage();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(_navigationService.GetHttpBaseAddress().FullAddress) });

            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddScoped<IVoicemodService, VoicemodService>();
            services.AddSingleton<IMSBStorageService, WPFMSBStorageService>();
            services.AddSingleton<IPlatformService, WpfPlatformService>();
            services.AddSingleton<IWPFNavigationService, WPFNavigationService>();



#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif

            blazorWebView.Services = services.BuildServiceProvider();

            var navigationService = blazorWebView.Services.GetService<IWPFNavigationService>();

            navigationService!.OnNavigateToConnectionVerificationPageRequested += () =>
            {
                MessageBoxResult result = MessageBox.Show("Are You sure you want to close the Soundboard and return to the SpudPI Connection Page?",
                "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _navigationService.NavigateToConnnectionVerificationPage();
                }
            };

            blazorWebView.BlazorWebViewInitializing += WebViewInitializing;

            blazorWebView.BlazorWebViewInitialized += BlazorWebView_Initialized;
        }

        private void WebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
        {
            var userDataFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "WebView2UserData");
            Directory.CreateDirectory(userDataFolder);

            e.UserDataFolder = userDataFolder;
        }

        private void BlazorWebView_Initialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            e.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            e.WebView.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
        }

        private void CoreWebView2_ContextMenuRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2ContextMenuRequestedEventArgs e)
        {
            Debug.WriteLine($"IsEditable? {e.ContextMenuTarget.IsEditable}");



            //if (e.ContextMenuTarget.IsEditable || !e.ContextMenuTarget.HasLinkUri)
            //{

            //foreach (var menuItem in e.MenuItems)
            //{
            //    Debug.WriteLine($"Menu Item: {menuItem.Name}");
            //}

            // For editable elements such as <input> and <textarea> we enable the context menu but remove items we don't want in this app
            var menuIndexesToRemove =
                e.MenuItems
                    .Select((m, i) => (m, i))
                    .Where(m => WebViewContextMenuItemsToRemove.Contains(m.m.Name))
                    .Select(m => m.i)
                    .Reverse();

            foreach (var menuIndexToRemove in menuIndexesToRemove)
            {
                //Debug.WriteLine($"Removing {e.MenuItems[menuIndexToRemove].Name}...");
                e.MenuItems.RemoveAt(menuIndexToRemove);
            }

            //System.Windows.MessageBox.Show(String.Join(" ", e.MenuItems.Select(x => x.Name)));

            // Trim extra separators from the end
            while (e.MenuItems.Any() && e.MenuItems.Last().Kind == Microsoft.Web.WebView2.Core.CoreWebView2ContextMenuItemKind.Separator)
            {
                e.MenuItems.RemoveAt(e.MenuItems.Count - 1);
            }
            //}
            //else
            //{
            //    // For non-editable elements such as <div> we disable the context menu
            //    e.Handled = true;
            //}
        }
    }
}
