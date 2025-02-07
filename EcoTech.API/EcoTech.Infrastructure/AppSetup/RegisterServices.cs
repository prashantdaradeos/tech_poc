

namespace EcoTech.Infrastructure.AppSetup;

public static class RegisterServices
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IConfigurationBuilder configurationBuilder)
    {
       

        #region DependencyInjection
        services.AddScoped<IGenericRepository, GenericRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        #endregion


        return services;
    }
}
