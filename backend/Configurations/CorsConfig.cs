using Microsoft.AspNetCore.Cors.Infrastructure;

namespace backend.Configurations
{
    public static class CorsConfig
    {
        public static void ConfigureAllowReactAppPolicy(CorsPolicyBuilder policy)
        {
            policy.WithOrigins("https://localhost:3000", "https://tlmap.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    }
}