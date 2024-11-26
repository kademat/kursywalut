using Microsoft.Extensions.Options;

namespace backend.Configurations
{
    public class NbpHttpClientConfig
    {
        private readonly NbpApiSettings _nbpApiSettings;

        public NbpHttpClientConfig(IOptions<NbpApiSettings> apiSettings)
        {
            _nbpApiSettings = apiSettings.Value;
        }

        public void Configure(HttpClient client)
        {
            client.BaseAddress = new Uri(_nbpApiSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", _nbpApiSettings.AcceptHeader);
        }
    }
}