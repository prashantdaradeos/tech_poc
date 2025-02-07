

using EcoTech.Domain.FeatureEntities;

namespace EcoTech.Application.FeaturesHandler.Users;


public class RefreshTokenHandler : IRequestHandler<RefreshTokenEmptyRequest, Response<RefreshTokenResponseDto>>
{

    private readonly IAuthService _authService;

    public RefreshTokenHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public ValueTask<Response<RefreshTokenResponseDto>> Handle(RefreshTokenEmptyRequest request, CancellationToken cancellationToken)
    {
        string refreshJwtToken = _authService.CreateRefreshJwtToken();
        return ValueTask.FromResult<Response<RefreshTokenResponseDto>>(new(new(refreshJwtToken)));
    }


}


