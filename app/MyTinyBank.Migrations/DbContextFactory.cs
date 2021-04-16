using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyTinyBank.Core.Implementation.Config;
using MyTinyBank.Core.Implementation.Data;

namespace MyTinyBank.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MyTinyBankDbContext>
    {
        public MyTinyBankDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var config = configuration.ReadAppConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<MyTinyBankDbContext>();

            optionsBuilder.UseSqlServer(
                config.DatabaseConnectionString,
                options => {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                });

            return new MyTinyBankDbContext(optionsBuilder.Options);
        }
    }
}
