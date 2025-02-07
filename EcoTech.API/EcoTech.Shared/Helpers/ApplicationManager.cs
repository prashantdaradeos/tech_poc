
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EcoTech.Shared.Helpers;

public interface IApplicationManager
{
     ApplicationSettings AppSettings { get; }
    string GetConnectionString(string dbName = default!);
}
public class ApplicationManager:IApplicationManager
{
    public  ApplicationSettings AppSettings { get; }
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public ApplicationManager(IOptions<ApplicationSettings> appSettings, IConfiguration configuration
        , IMemoryCache memoryCache)
    {
        AppSettings = appSettings.Value;
        _configuration = configuration;
        _memoryCache = memoryCache;

        if (AppSettings.Configuration.UseEnvironmentalConnectionStrings.Equals(AppConstants.True))
        {
           AppSettings.ConnectionStrings.EcoTech =
           string.Format(AppConstants.GenericConnectionString,
           Environment.GetEnvironmentVariable(AppConstants.EchoTechServerEnvironmentName),
           Environment.GetEnvironmentVariable(AppConstants.EchoTechDataBaseEnvironmentName),
           Environment.GetEnvironmentVariable(AppConstants.EchoTechUserIdEnvironmentName),
           Environment.GetEnvironmentVariable(AppConstants.EchoTechPasswordEnvironmentName));
        }
    }

    public string GetConnectionString(string dbName=default!)
    {


        return dbName switch
        {
            
            _=> AppSettings.ConnectionStrings.EcoTech
        };
    }
}
