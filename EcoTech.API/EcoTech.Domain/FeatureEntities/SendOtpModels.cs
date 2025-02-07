

namespace EcoTech.Domain.FeatureEntities;


public class OtpRequestDto : Otp, IRequest<Response<OtpResponseDto>>
{
    public string CommunicationChannel { get; set; } = string.Empty;

}
public record struct OtpResponseDto(bool OtpSent, string Data);

public class OtpRequestDtoValidator : Validator<OtpRequestDto>
{
    public OtpRequestDtoValidator()
    {
        RuleFor(x => x.Contact).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Matches(AppConstants.MobileOrEmailRegex).
            WithMessage(AppConstants.MobileOrEmailErrorMessage);
        RuleFor(x =>x.CommunicationChannel).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Must(operation => operation.Equals(SpConstants.Mobile) ||operation.Equals(SpConstants.Email)).
            WithMessage(string.Format(AppConstants.MustWithinValuesErrorMessage,
            nameof(OtpRequestDto.CommunicationChannel),
            string.Join(AppConstants.CommaAsString, [SpConstants.Mobile,
                SpConstants.Email])));
    }
}


