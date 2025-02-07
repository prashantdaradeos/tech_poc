using FluentValidation;

namespace EcoTech.API.Features.Users;

public class AvailableUser : Endpoint<AvailableUserRequestDto, Response<AvailableUserResponseDto>>
{


    public override void Configure()
    {
        Post(AppConstants.UserAvailabilityURL);
        AllowAnonymous();
        Validator<AvailableUserRequestDtoValidator>();
        Throttle(hitLimit: 10, durationSeconds: 60);
    }

    public override async Task HandleAsync(AvailableUserRequestDto req, CancellationToken ct)
    {
        IMediator mediator = TryResolve<IMediator>()!;
        await SendAsync(await mediator.Send(req));
    }
}
