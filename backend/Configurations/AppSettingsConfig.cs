namespace backend.Configurations
{
    public class AppSettingsConfig
    {
        /// <summary>
        /// Generyczna klasa do odczytu konfiguracji
        /// Jej zadaniem jest odczyt konfiguracji np. z appsettings.json
        /// </summary>
        public T GetConfig<T>(IConfiguration configuration, string sectionName) where T : class
        {
            var section = configuration.GetSection(sectionName);
#pragma warning disable CS8603 // Possible null reference return.
            return section.Get<T>();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}