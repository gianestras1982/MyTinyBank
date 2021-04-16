using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

using MyTinyBank.Core.Implementation.Config;
using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Implementation.Services;

namespace MyTinyBank.Core.Tests
{
    public class MyTinyBankFixture : IDisposable
    {
        public IServiceScope Scope { get; private set; }

        public MyTinyBankFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddAppServices(config);

            Scope = serviceCollection.BuildServiceProvider().CreateScope();
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}
