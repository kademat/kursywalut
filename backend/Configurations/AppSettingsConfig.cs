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
            return section.Get<T>();
        }
    }
}