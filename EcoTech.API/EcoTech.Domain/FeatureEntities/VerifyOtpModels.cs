
namespace EcoTech.Domain.FeatureEntities;

public class Verifyotp: Otp
{
    public string Otp { get; set; } = string.Empty;
}
public class VerifyOtpRequestDto : Verifyotp, IRequest<Response<VerifyOtpResponseDto>>
{
    public string Purpose { get; set; } = string.Empty;
};
public record struct VerifyOtpResponseDto(bool OtpVerified, string Data);
public class VerifyOtpRequestDtoValidator : Validator<VerifyOtpRequestDto>
{
    public VerifyOtpRequestDtoValidator()
    {
        RuleFor(x => x.Contact).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Matches(AppConstants.MobileOrEmailRegex).
            WithMessage(AppConstants.MobileOrEmailErrorMessage);
        RuleFor(x => x.Otp).
            Matches(AppConstants.OtpRegex).
            WithMessage(AppConstants.OtpRegexErrorMessage);
        RuleFor(x => x.Purpose).
            Must(purpose => purpose.Equals(AppConstants.Verification) || purpose.Equals(AppConstants.Login)).
            WithMessage(string.Format(AppConstants.MustWithinValuesErrorMessage,nameof(VerifyOtpRequestDto.Purpose),
            string.Join(AppConstants.CommaAsString, [AppConstants.Verification, AppConstants.Login])));
    }
}
