
namespace EcoTech.Domain.AppSetup;

/*public record struct Response<T>(T Data,
            string UniqueId = "",
            string Message = "Success",
            bool IsSuccess = true,
            object? ObjectProperties = null);*/



public record struct Response<T>
{
    public T Data { get; set; }
    public string UniqueId { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public object? ObjectProperties { get; set; }

    public Response(
        T data,
        string uniqueId = "",
        string message =default!,
        bool isSuccess = true,
        object? objectProperties = null)
    {
        Data = data;
        UniqueId = uniqueId;
        IsSuccess = isSuccess;
        if (isSuccess)
        {
            Message = string.IsNullOrWhiteSpace(message)? "Success" : message;
        }
        else
        {
            Message = string.IsNullOrWhiteSpace(message)? "Failure" : message;
        }
        ObjectProperties = objectProperties;
    }
}


public interface IDbResponse
{
     bool Result { get; set; }   
     string Message { get; set; }
}
public class DbResponse : IDbResponse
{
    public bool Result { get; set; }
    public string Message { get; set; } = string.Empty;
}
public class GenericDbResponse : DbResponse
{
    public int NumberOfRowsAffected { get; set; } = 0;
    public bool IsSuccess { get; set; } = true;
}
public class SkipResponse;


