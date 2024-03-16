namespace SpudPI.WPF.Client
{
    public class HttpBaseAddress
    {
        public string? Address { get; set; }
        public string? Port { get; set; }
        public bool? UseHttps { get; set; }

        public string FullAddress => $"{(UseHttps == true ? "https" : "http")}://{Address}:{Port}";
    }
}
