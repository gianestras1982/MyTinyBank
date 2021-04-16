using MyTinyBank.Core.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyTinyBank.Core.Implementation.Config;
using MyTinyBank.Core.Implementation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bank.ConsoleApp
{
    public class DbFactory
    {
        public DbContextOptionsBuilder<MyTinyBankDbContext> GetCon()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            AppConfig appConfg = configuration.ReadAppConfiguration();

            DbContextOptionsBuilder<MyTinyBankDbContext> dbCntxtOptnsBldr = new DbContextOptionsBuilder<MyTinyBankDbContext>();

            dbCntxtOptnsBldr.UseSqlServer(appConfg.DatabaseConnectionString,
                options => {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                });

            return dbCntxtOptnsBldr;
        }
    }
}
