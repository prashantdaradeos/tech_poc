

namespace EcoTech.Domain.FeatureEntities;



public class AvailableUserRequestDto: AvailableUser,IRequest<Response<AvailableUserResponseDto>>;


public record struct AvailableUserResponseDto(bool IsMobileNumberAvailable,string message);

public class AvailableUserRequestDtoValidator : Validator<AvailableUserRequestDto>
{
    public AvailableUserRequestDtoValidator()
    {
        RuleFor(x => x.UserUniqueId).
                NotEmpty().
                WithMessage(AppConstants.EmptyErrorMessage).
                Matches(AppConstants.MobileOrEmailRegex).
                WithMessage(AppConstants.MobileOrEmailErrorMessage);
        RuleFor(x => x.Operation).
                Must(operation => operation.Equals(SpConstants.UserMobileRegistration) ||
                operation.Equals(SpConstants.UserEmailRegistration) ||
                operation.Equals(SpConstants.ClientMobileRegistration) ||
                operation.Equals(SpConstants.ClientEmailRegistration)).
                WithMessage(string.Format(AppConstants.MustWithinValuesErrorMessage,nameof(AvailableUserRequestDto.Operation),
                string.Join(AppConstants.CommaAsString, [SpConstants.UserMobileRegistration, 
                    SpConstants.UserEmailRegistration, SpConstants.ClientMobileRegistration, 
                    SpConstants.ClientEmailRegistration])));
    }
}
