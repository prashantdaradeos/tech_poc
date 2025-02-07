
using EcoTech.Domain.Entities;
using EcoTech.Domain.FeatureEntities;

namespace EcoTech.Application.FeaturesHandler.Users;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpRequestDto, Response<VerifyOtpResponseDto>>
{

    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public VerifyOtpHandler(IAuthService authService,IUserRepository userRepository,IMapper mapper)
    {
        _authService = authService;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async ValueTask<Response<VerifyOtpResponseDto>> Handle(VerifyOtpRequestDto request, CancellationToken cancellationToken)
    {

        OtpSpRequest spRequest = _mapper.Map(request);
        spRequest.Operation = SpConstants.Verify;
        List<IDbResponse>[] response= await _userRepository.VerifyOtp(spRequest);
        OtpSpResponse spResponse = (OtpSpResponse)response[0][0];

        if (request.Purpose.Equals(AppConstants.Verification))
        {
            return new(new(spResponse.Result, spResponse.Message));
        }
        else if (spResponse.Result && request.Purpose.Equals(AppConstants.Login))
        {
            RolesSpResponse rolesSpResponse= (RolesSpResponse)response[1][0];
            string jwtToken= rolesSpResponse.Result ? _authService.CreateJwtToken(20, [rolesSpResponse.Message]) : rolesSpResponse.Message;
            return new(new(rolesSpResponse.Result, jwtToken));
        }
        return new(new(false, string.Empty), message: AppConstants.OtpInvalidMessage, isSuccess: false);

    }


}


