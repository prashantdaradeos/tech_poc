

namespace EcoTech.API.Features.Users;

public class SendOtp: Endpoint<OtpRequestDto, Response<OtpResponseDto>>
{
    
    public override void Configure()
    {
        Post(AppConstants.SendOtpURL);
        AllowAnonymous();
        Validator<OtpRequestDtoValidator>();
        Throttle(hitLimit: 8, durationSeconds: 60);
    }

    public override async Task HandleAsync(OtpRequestDto req, CancellationToken ct)
    {
      IMediator mediator = TryResolve<IMediator>()!;
      await SendAsync(await mediator.Send(req));
    }
}
