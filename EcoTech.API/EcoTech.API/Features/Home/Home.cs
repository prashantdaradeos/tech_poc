namespace EcoTech.API.Features.Home;


public class Login : EndpointWithoutRequest
{
   

    public override void Configure()
    {
        Get("/home");
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {      
        await SendAsync(new Response<string>("Welcome to EcoTech"));
    }
}

