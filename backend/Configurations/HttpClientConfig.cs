namespace backend.Configurations
{
    public class HttpClientConfig
    {
        public static void Configure(HttpClient client)
        {
            // Tu zalecanym podejściem by było pobranie tych danych z appsettings.json
            client.BaseAddress = new Uri("https://api.nbp.pl/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}