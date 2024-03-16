using System.Windows.Controls;

namespace SpudPI.WPF.Client
{
    public class NavigationService : INavigationService
    {
        private Frame? _mainFrame;

        public HttpBaseAddress _httpBaseAddress { get; } = new();

        public HttpBaseAddress GetHttpBaseAddress()
        {
            return this._httpBaseAddress;
        }

        public void SetMainFrame(Frame mainFrame)
        {
            this._mainFrame = mainFrame;
        }

        public void NavigateToBlazorWebViewPage()
        {
            this._mainFrame!.Navigate(new BlazorWebViewPage());
        }

        public void NavigateToConnnectionVerificationPage()
        {
            this._mainFrame!.Navigate(new ConnectionVerificationPage());
        }
    }
}
