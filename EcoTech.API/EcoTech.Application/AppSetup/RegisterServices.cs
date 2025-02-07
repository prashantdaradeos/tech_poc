using EcoTech.Application.BackGroundServices;
using EcoTech.Shared.Helpers;
using EcoTech.Application.Integrations;
using EcoTech.Application.UtilityServiceContracts;
using EcoTech.Application.UtilityServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace EcoTech.Application.AppSetup;

public static class RegisterServices
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services,
        IConfiguration configuration, IConfigurationBuilder configurationBuilder)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection(AppConstants.ApplicationSettingsSection));
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
          
        });

       



        #region DependencyInjection
        services.AddSingleton<IApplicationManager, ApplicationManager>();
        services.AddSingleton<IMapper,MapperlyMappings>();

        services.AddMemoryCache();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<DeleteOTPService>();
        services.AddHostedService<QueuedHostedService>();     
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<PostManHeaders>();
        #endregion

        #region API Http Configuration

        services.AddRefitClient<IPostManService>().ConfigureHttpClient((sp, httpClient) =>
        {
            IApplicationManager applicationManager = sp.GetRequiredService<IApplicationManager>();
            httpClient.BaseAddress = new Uri(applicationManager.AppSettings.Communication.PostManBaseAddress);
        }).AddHttpMessageHandler<PostManHeaders>();

        #endregion

        #region 

        #endregion
        return services;//.ConfigureApplicationOnce(configuration);
    }
    /*public static IServiceCollection ConfigureApplicationOnce(this IServiceCollection services,
       IConfiguration configuration)
    {
       
        #region 
        services.bui
        #endregion
        return services;
    }*/
}
