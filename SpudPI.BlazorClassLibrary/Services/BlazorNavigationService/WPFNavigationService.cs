using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpudPI.BlazorClassLibrary
{
    public class WPFNavigationService : IWPFNavigationService
    {
        public event Action? OnNavigateToConnectionVerificationPageRequested;

        public void NavigateToConnectionVerificationPage()
        {
            OnNavigateToConnectionVerificationPageRequested?.Invoke();
        }
    }
}
