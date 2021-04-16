using Microsoft.Extensions.Configuration;

using MyTinyBank.Core.Config;

namespace MyTinyBank.Core.Implementation.Config
{
    public static class ConfigurationExtensions
    {
        public static AppConfig ReadAppConfiguration(this IConfiguration @this)
        {
            return new AppConfig() 
            {
                DatabaseConnectionString = @this.GetConnectionString("MyTinyBank")
            };
        }
    }
}
