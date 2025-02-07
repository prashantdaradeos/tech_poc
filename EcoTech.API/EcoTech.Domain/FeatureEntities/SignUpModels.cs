
namespace EcoTech.Domain.FeatureEntities;

public class SignUpRequestDto : SignUpSpRequest, IRequest<Response<SignUpResponseDto>>;

public record struct SignUpResponseDto(string Message);

public class SignUpRequestDtoValidator : Validator<SignUpRequestDto>
{
    public SignUpRequestDtoValidator()
    {
        RuleFor(x => x.ClientName).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Matches(AppConstants.EnglishLangRegex).
            WithMessage(AppConstants.EnglishLangErrorMessage).
            Length(2, 200).
            WithMessage(string.Format(AppConstants.LengthErrorMessage, 2, 200));
       /* RuleFor(x => x.ClientMobileNumber).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Matches(AppConstants.MobileNumberRegex).
            WithMessage(AppConstants.MobileNumberRegexErrorMessage);*/
        RuleFor(x => x.UserMobileNumber).
            NotEmpty().
            WithMessage(AppConstants.EmptyErrorMessage).
            Matches(AppConstants.MobileNumberRegex).
            WithMessage(AppConstants.MobileNumberRegexErrorMessage);
        RuleFor(x => x.Name).
           NotEmpty().
           WithMessage(AppConstants.EmptyErrorMessage).
           Matches(AppConstants.EnglishLangRegex).
           WithMessage(AppConstants.EnglishLangErrorMessage).
           Length(2,104).
           WithMessage(string.Format(AppConstants.LengthErrorMessage,2,104));
        RuleFor(x => x.Address).
           Matches(AppConstants.EnglishLangRegex).
           WithMessage(AppConstants.EnglishLangErrorMessage).
           Length(2, 400).
           WithMessage(string.Format(AppConstants.LengthErrorMessage, 2, 400)).
           When(x=>!string.IsNullOrWhiteSpace(x.Address));
        RuleFor(x => x.GSTIN).
           Matches(AppConstants.PANOrGSTNoRegex).
           WithMessage(AppConstants.PANOrGSTNoErrorMessage).
           Length(10,15).
           WithMessage(string.Format(AppConstants.LengthErrorMessage, 10,15)).
           When(x => !string.IsNullOrWhiteSpace(x.GSTIN));
        RuleFor(x => x.UserEmail).
          Matches(AppConstants.EmailRegex).
          WithMessage(AppConstants.EmailErrorMessage).
          When(x => !string.IsNullOrWhiteSpace(x.UserEmail));
        /*RuleFor(x => x.ClientEmail).
          Matches(AppConstants.EmailRegex).
          WithMessage(AppConstants.EmailErrorMessage).
          When(x => !string.IsNullOrWhiteSpace(x.ClientEmail));*/
    }
}