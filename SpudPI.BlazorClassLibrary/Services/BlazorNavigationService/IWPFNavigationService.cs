namespace SpudPI.BlazorClassLibrary
{
    public interface IWPFNavigationService
    {
        event Action OnNavigateToConnectionVerificationPageRequested;
        void NavigateToConnectionVerificationPage();
    }
}
