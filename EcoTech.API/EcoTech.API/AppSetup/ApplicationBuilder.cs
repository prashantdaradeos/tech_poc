




using Microsoft.Extensions.Configuration;

namespace EcoTech.API.StartupExtentions;
public static class ApplicationBuilder
{
    public static IServiceCollection ConfigureAPIApplication(this IServiceCollection services, IConfiguration configuration, IConfigurationBuilder configurationBuilder)
    {
       
        services.AddCors(options =>
        {
            options.AddPolicy(AppConstants.CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        /* #region KeyVaultConfigurations
         if (configuration.GetSection("ApplicationSettings:Azure:UseAzure").Value == "true")
         {
             var keyVaultEndpoint = new Uri(configuration.GetSection("ApplicationSettings:Azure:VaultUrl").Value);
             var clientId = configuration.GetSection("ApplicationSettings:Azure:ClientId").Value;
             var clientSecret = configuration.GetSection("ApplicationSettings:Azure:ClientSecret").Value;
             var tenantId = configuration.GetSection("ApplicationSettings:Azure:AzureAd").Value;
             var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
             configurationBuilder.AddAzureKeyVault(keyVaultEndpoint, credential);
         }
         #endregion*/




        #region Dependency Injection
        
        services.AddHttpContextAccessor();
        services.AddAuthenticationJwtBearer(s =>
        s.SigningKey = configuration.GetSection(AppConstants.UseDynamicSecretsSection).Value.
                                    Equals(AppConstants.True) ?
                                        AppConstants.JwtSigningKey :
                                        configuration.GetSection(AppConstants.JwtSiginingKeySection).Value ) 
        .AddAuthorization()
        .AddFastEndpoints(options =>
        {
            
        })
        /*.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null;
        })*/;
        

        #endregion

        return services.ConfigureAPIOnce(configuration, configurationBuilder);

    }
    private static IServiceCollection ConfigureAPIOnce(this IServiceCollection services, IConfiguration configuration, IConfigurationBuilder configurationBuilder)
    {
        int j = 0;
        foreach (string role in AppConstants.AppRoles) 
        {
            string encryptedRole = EncryptString(configuration, role);
            AppConstants.EncryptedAppRolesDictionary.Add(role, encryptedRole);
            AppConstants.EncryptedAppRoles[j++] = encryptedRole;
        }
        //AppConstants.EncryptedLeadRole = EncryptString(configuration,AppConstants.LeadRole);
        //AppConstants.EncryptedUserRole = EncryptString(configuration, AppConstants.UserRole);
        //AppConstants.EncryptedGroupAdminRole = EncryptString(configuration, AppConstants.GroupAdminRole);
        //AppConstants.EncryptedAppRoles = [AppConstants.EncryptedLeadRole, AppConstants.EncryptedUserRole, AppConstants.EncryptedGroupAdminRole];

        #region DB Configuration
        AppConstants.SpResponsePropertyInfoCache = new PropertyInfo[Enum.GetValues(typeof(DomainAppConstants.SpResponse)).Length][];
        for(int i = 0;i< DomainAppConstants.SpResponseModelTypeArray.Length; i++)
        {
            AppConstants.SpResponsePropertyInfoCache[i] = DomainAppConstants.SpResponseModelTypeArray[i].GetProperties();
        }
        int numberOfRequestModels = Enum.GetValues(typeof(DomainAppConstants.SpRequest)).Length;
        AppConstants.SpRequestPropertyInfoCache = new PropertyInfo[numberOfRequestModels][];
        AppConstants.CachedPropertyAccessorDelegates = new Dictionary<string, Func<object, object>>[numberOfRequestModels];
        for(int i = 0; i < DomainAppConstants.SpRequestModelTypeArray.Length; i++)
        {
            AppConstants.SpRequestPropertyInfoCache[i] = DomainAppConstants.SpRequestModelTypeArray[i].GetProperties();
            AppConstants.CachedPropertyAccessorDelegates[i] = new Dictionary<string, Func<object, object>>();
            foreach(var property in AppConstants.SpRequestPropertyInfoCache[i])
            {
                AppConstants.CachedPropertyAccessorDelegates[i].Add(property.Name, Extensions.CreateGetter
                    (DomainAppConstants.SpRequestModelTypeArray[i],property.Name,property));
            }
        }
        #endregion

        return services;
    }
    private static string EncryptString(IConfiguration configuration,string plainText)
    {
        using Aes aes = Aes.Create();
        if (configuration.GetSection(AppConstants.UseDynamicSecretsSection).Value!.
                                    Equals(AppConstants.True))
        {
            aes.Key = AppConstants.DynamicEncryptionKey;
            aes.IV = AppConstants.DynamicEncryptionIV;
        }
        else
        {
            aes.Key = Encoding.UTF8.GetBytes(configuration.GetSection(AppConstants.EncryptionKeySection).Value!);
            aes.IV = Encoding.UTF8.GetBytes(configuration.GetSection(AppConstants.EncryptionIVSection).Value!);

        }
        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            using StreamWriter sw = new StreamWriter(cs);
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

}
