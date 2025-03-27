using ClientRegistry.Data.Context;
using ClientRegistry.Data.Repository;
using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Notification;
using ClientRegistry.Service.Services;

namespace ClientRegistry.MVC.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();

            services.AddScoped<INotificator, Notificator>();

            services.AddScoped<IPagedResultRepository<Client>, PagedResultRepository<Client>>();

            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IClientService, ClientService>();

            return services;
        }
    }
}
