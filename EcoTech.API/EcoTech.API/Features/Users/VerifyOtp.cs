
namespace EcoTech.API.Features.Users;

public class VerifyOtp : Endpoint<VerifyOtpRequestDto, Response<VerifyOtpResponseDto>>
{
    public override void Configure()
    {
        Post(AppConstants.VerifyOtpURL);
        AllowAnonymous();
        Validator<VerifyOtpRequestDtoValidator>();
        Throttle(hitLimit: 3, durationSeconds: 60);
    }

    public override async Task HandleAsync(VerifyOtpRequestDto req, CancellationToken ct)
    {
        IMediator mediator = TryResolve<IMediator>()!;
        await SendAsync(await mediator.Send(req));
    }

}
