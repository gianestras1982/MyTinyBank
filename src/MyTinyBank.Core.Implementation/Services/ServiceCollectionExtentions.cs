using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTinyBank.Core.Config;
using MyTinyBank.Core.Implementation.Config;
using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Implementation.Services;

namespace MyTinyBank.Core.Implementation.Services
{

    public static class ServiceCollectionExtentions
    {
        public static void AddAppServices(this IServiceCollection @this, IConfiguration config)
        {
            @this.AddSingleton<AppConfig>(config.ReadAppConfiguration());

            @this.AddDbContext<MyTinyBankDbContext>(
                (serviceProvider, optionsBuilder) => {

                    var appConfig = serviceProvider.GetRequiredService<AppConfig>();

                    optionsBuilder.UseSqlServer(appConfig.DatabaseConnectionString);
                });

            @this.AddScoped<ICustomerService, CustomerService>();
            @this.AddScoped<IAccountService, AccountService>();
            @this.AddScoped<ICardService, CardService>();


        }
    }

}
