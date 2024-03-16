using Microsoft.Extensions.DependencyInjection;
using SpudPI.Shared;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SpudPI.WPF.Client
{
    /// <summary>
    /// Interaktionslogik für ConnectionVerificationPage.xaml
    /// </summary>
    public partial class ConnectionVerificationPage : Page
    {
        private INavigationService _navigationService;
        public ConnectionVerificationPage()
        {
            InitializeComponent();

            txtBoxBaseAddress.Text = Properties.Settings.Default.BaseAddress;
            txtBoxBaseAddressPort.Text = Properties.Settings.Default.BaseAddressPort;

            _navigationService = (Application.Current as App)!.ServiceProvider.GetRequiredService<INavigationService>();
        }

        private static async Task<bool> IsConnectionValidAsync(string fullAddress)
        {
            string pingAPIEndpoint = $"/api/Ping/{Guid.NewGuid()}";


            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{fullAddress}{pingAPIEndpoint}");
                var isSuccessful = response.IsSuccessStatusCode;

                if (isSuccessful)
                {
                    // Deserialize the JSON response
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };


                    var serviceResponse = JsonSerializer.Deserialize<ServiceResponse<Guid>>(responseContent, options);

                    if (serviceResponse is not null && serviceResponse.Success == true)
                    {
                        System.Windows.MessageBox.Show($"Testing successful:{System.Environment.NewLine}Status Code: {response.StatusCode}{System.Environment.NewLine}Response: {serviceResponse.Message}",
                            "Test Connection",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"Testing successful but unexpected response:{System.Environment.NewLine}Status Code: {response.StatusCode}{System.Environment.NewLine}Response: {responseContent}",
                            "Test Connection",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show($"Testing unsuccessful:{System.Environment.NewLine}Status Code: {response.StatusCode}",
                        "Test Connection",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Testing unsuccessful:{System.Environment.NewLine}{ex.Message}",
                    "Test Connection",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private async void BtnTestAddress_Click(object sender, RoutedEventArgs e)
        {
            gridSetUpForm.IsEnabled = false;

            if (string.IsNullOrEmpty(txtBoxBaseAddress.Text) || string.IsNullOrEmpty(txtBoxBaseAddressPort.Text))
            {
                System.Windows.MessageBox.Show($"Please enter a valid Address and Port",
                    "Test Connection",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                gridSetUpForm.IsEnabled = true;
                return;
            }

            btnTestAddress.Content = "Testing";

            var selectedProtocol = groupProtocol
                .Children
                .OfType<RadioButton>()
                .First(r => r.IsChecked.HasValue && r.IsChecked.Value)
                .Tag.ToString()!;

            _navigationService.GetHttpBaseAddress().Port = txtBoxBaseAddressPort.Text;
            _navigationService.GetHttpBaseAddress().Address = txtBoxBaseAddress.Text;
            _navigationService.GetHttpBaseAddress().UseHttps = selectedProtocol.Equals("https", StringComparison.OrdinalIgnoreCase);


            Cursor = System.Windows.Input.Cursors.Wait;
            bool isValidConnection = await IsConnectionValidAsync(_navigationService.GetHttpBaseAddress().FullAddress);
            Cursor = System.Windows.Input.Cursors.Arrow;

            if (isValidConnection)
            {
                btnStartSoundboard.IsEnabled = true;
            }

            btnTestAddress.Content = "Test Connection";

            gridSetUpForm.IsEnabled = true;
        }

        private void BtnStartSoundboard_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BaseAddress = txtBoxBaseAddress.Text;
            Properties.Settings.Default.BaseAddressPort = txtBoxBaseAddressPort.Text;
            Properties.Settings.Default.Save(); // Saves the settings
            _navigationService.NavigateToBlazorWebViewPage();
        }

        private void PreviewTextInputPort(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BaseAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnStartSoundboard.IsEnabled = false;
        }
    }
}
