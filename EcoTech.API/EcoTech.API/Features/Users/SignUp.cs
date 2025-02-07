namespace EcoTech.API.Features.Users;

public class SignUp : Endpoint<SignUpRequestDto, Response<SignUpResponseDto>>
{


    public override void Configure()
    {
        Post(AppConstants.ClientSignUpURL);
        AllowAnonymous();
        Validator<SignUpRequestDtoValidator>();
        Throttle(hitLimit: 3, durationSeconds: 60);
    }

    public override async Task HandleAsync(SignUpRequestDto req, CancellationToken ct)
    {
        IMediator mediator = TryResolve<IMediator>()!;
         await SendAsync(await mediator.Send(req));
    }
}
