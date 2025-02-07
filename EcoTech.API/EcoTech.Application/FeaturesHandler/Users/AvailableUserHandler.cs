




using EcoTech.Domain.Entities;

namespace EcoTech.Application.FeaturesHandler.Users;

public class AvailableUserHandler : IRequestHandler<AvailableUserRequestDto, Response<AvailableUserResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AvailableUserHandler(IUserRepository userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async ValueTask<Response<AvailableUserResponseDto>> Handle(AvailableUserRequestDto request, CancellationToken cancellationToken)
    {
        AvailableUserSpRequest spRequest = _mapper.Map(request);
        AvailableUserSpResponse spResponse=await _userRepository.CheckUserAvailability(spRequest);
        return new(new(spResponse.Result, spResponse.Message));
    }
}
