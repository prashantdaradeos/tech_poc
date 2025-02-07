

using EcoTech.Application.AppSetup;
using EcoTech.Domain.FeatureEntities;
using EcoTech.Domain.RepositoryContracts;

namespace EcoTech.Application.FeaturesHandler.Users;

public class SignUpHandler : IRequestHandler<SignUpRequestDto, Response<SignUpResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SignUpHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
       
    }
    public async ValueTask<Response<SignUpResponseDto>> Handle(SignUpRequestDto request, CancellationToken cancellationToken)
    {
        SignUpSpResponse spResponse = await _userRepository.SignUpUser(request);
        return new(new(spResponse.Message),isSuccess: spResponse.Result);
    }


}


