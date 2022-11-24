using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FMAplication;
using FMAplication.Domain;
using FMAplication.Repositories;
using FMAplication.RequestModels.Reports;
using FMAplication.Services.Notification;
using FMAplication.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

using Microsoft.Extensions.Configuration;

namespace DataMigrator
{
    class Program
    {
      
        public static IConfiguration Configuration { get; private set; }
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Configuration = SetupConfiguration(args);
            var provider = initializeDependencies();
            var ledgerPopulator = provider.GetService<ILedgerPopulator>();
            await ledgerPopulator.RepairAllLedger();
        }

        private static IConfiguration SetupConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }

        static ServiceProvider initializeDependencies()
        {
            
            var services = new ServiceCollection();
            
            services.Configure<ApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
            Configuration.GetSection("AzureSettings").Get<AzureSettings>();


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, FMAplication.UnitOfWorks.UnitOfWork>();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();

            services.AddScoped<ILedgerPopulator,LedgerPopulator>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
