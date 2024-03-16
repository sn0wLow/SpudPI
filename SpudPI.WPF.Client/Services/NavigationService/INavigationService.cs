using System.Windows.Controls;

namespace SpudPI.WPF.Client
{
    public interface INavigationService
    {
        public HttpBaseAddress GetHttpBaseAddress();
        public void SetMainFrame(Frame mainFrame);
        public void NavigateToBlazorWebViewPage();
        public void NavigateToConnnectionVerificationPage();
    }
}
