



using EcoTech.Domain.RepositoryContracts;
using EcoTech.Infrastructure.Repositories;

namespace EcoTech.API.Filters;

public class ExceptionFilter : IEndpointFilter
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IGenericRepository _genericRepository;

    public ExceptionFilter(IBackgroundTaskQueue taskQueue,IGenericRepository genericRepository)
    {
        _taskQueue = taskQueue;
        _genericRepository = genericRepository;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string uniqueId = context.HttpContext.Request.Headers[AppConstants.UniqueId]!;
        try
        {
            return await next(context);
        }
        catch (EcoTechException ex)
        {
            string exceptionInformation = GetExceptionInformation(ex, context.HttpContext) + Environment.NewLine + ex.Information;
            exceptionInformation = exceptionInformation.Replace(AppConstants.RemoveFromException, string.Empty);
             LogException(uniqueId, exceptionInformation);
            await WriteResponse(ex, context.HttpContext, exceptionInformation);
            return default;
        }
        catch (Exception ex)
        {

            string exceptionInformation = GetExceptionInformation(ex, context.HttpContext);
             LogException(uniqueId, exceptionInformation);
            await WriteResponse(ex, context.HttpContext, exceptionInformation);
            return default;
        }

    }
    public  void LogException(string uniqueId, string exceptionInfo)
    {
        _taskQueue.QueueBackgroundWorkItem(async token =>
        {
            await _genericRepository.LogErrors(uniqueId, exceptionInfo);           
        });
    }
    public Task WriteResponse(Exception ex, HttpContext context, string exceptionInformation)
    {
        Response<string> response = new Response<string>();
        response.Data = AppConstants.ErrorMessage;
        string env = Environment.GetEnvironmentVariable(AppConstants.ApplicationEnvironmentVaraible)!;
        if (env.Equals(AppConstants.ProductionEnvironment, StringComparison.OrdinalIgnoreCase))
        {
            response.Message = AppConstants.Failure;
        }
        else
        {
            response.Message = exceptionInformation ?? AppConstants.Failure;
        }
        response.IsSuccess = false;
        context.Response.ContentType = AppConstants.JsonRequestType;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsync(JsonSerializer.ToJsonString(response));

    }
    public string GetExceptionInformation(Exception ex, HttpContext context)
    {
        string requestPath = context.Request.Path + Environment.NewLine;
        if (context!.Request.QueryString.HasValue)
        {
            requestPath = JsonSerializer.ToJsonString(context.Request.QueryString.Value) + Environment.NewLine;
        }
        string asyncException = ex.ToString();
        string[] strings = asyncException.Split(Environment.NewLine);
        int index = Array.FindIndex(strings, s => s.Contains(AppConstants.ExceptionInformationDevider));
        string information = string.Join(Environment.NewLine, strings.Take(index));
        return requestPath + information;
    }


}
