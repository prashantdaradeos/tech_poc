


using Refit;

namespace EcoTech.Application.Integrations;

public interface IPostManService
{
    [Post("/post")]
    Task<PostManResponse> GetPostMan([Body] PostManRequest postManRequest);
}
public class PostManHeaders : DelegatingHandler
{
    private readonly IApplicationManager _applicationManager;

    public PostManHeaders(IApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add(AppConstants.AuthorizationHeader, AppConstants.Bearer+ _applicationManager.AppSettings.Communication.PostManAuthorizationToken);
        return base.SendAsync(request, cancellationToken);
    }
}
public class PostManRequest
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public List<string> Hobbies { get; set; }
    public Address Address { get; set; }
}
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
}
public class PostManResponse
{
    public Dictionary<string, string> Args { get; set; }
    public Data Data { get; set; }
    public Dictionary<string, string> Files { get; set; }
    public Dictionary<string, string> Form { get; set; }
    public Headers Headers { get; set; }
    public Json Json { get; set; }
    public string Url { get; set; }
}

public class Data
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public List<string> Hobbies { get; set; }
    public Address Address { get; set; }
}

public class Headers
{
    public string XForwardedProto { get; set; }
    public string Host { get; set; }
    public string ContentLength { get; set; }
    public string Accept { get; set; }
    public string AcceptEncoding { get; set; }
    public string ContentType { get; set; }
    public string UserAgent { get; set; }
    public string Authorization { get; set; }
}

public class Json
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public List<string> Hobbies { get; set; }
    public Address Address { get; set; }
}



