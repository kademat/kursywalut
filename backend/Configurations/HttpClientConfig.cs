namespace backend.Configurations
{
    public class HttpClientConfig
    {
        public static void Configure(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.nbp.pl/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}