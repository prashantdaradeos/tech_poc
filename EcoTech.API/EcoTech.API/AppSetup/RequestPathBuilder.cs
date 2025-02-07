




namespace EcoTech.API.StartupExtentions;
public static class RequestPathBuilder
{
    public static WebApplication ConfigureRequestPath(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseCors(AppConstants.CorsPolicyName).
               UseHttpsRedirection().
               UseAuthentication().
               UseAuthorization().
               UseFastEndpoints(options =>
                {
                    options.Endpoints.RoutePrefix = AppConstants.RoutePrefix;
                    options.Endpoints.Configurator = ep =>
                    {
                        ep.Options(b => b.AddEndpointFilter<ExceptionFilter>());
                    };
                });
        
        return app;
    }

}
