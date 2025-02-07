

namespace EcoTech.API.Features.Users;

public class GetRefreshToken : EndpointWithoutRequest<Response<RefreshTokenResponseDto>>
{
 
    public override void Configure()
    {
        Post(AppConstants.GetRefreshTokenURL);
        Roles(AppConstants.EncryptedAppRolesDictionary.Values.ToArray());
        Throttle(hitLimit: 1, durationSeconds: 600);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        IMediator mediator = TryResolve<IMediator>()!;
        await SendAsync(await mediator.Send(new RefreshTokenEmptyRequest()));
    }

}

