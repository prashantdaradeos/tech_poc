
namespace EcoTech.Domain.RepositoryContracts;

public interface  IUserRepository:IGenericRepository
{
    ValueTask<OtpSpResponse> ManageOtp(string contact, string otp, string operation);
    ValueTask<SignUpSpResponse> SignUpUser(SignUpSpRequest spRequest);
    ValueTask<AvailableUserSpResponse> CheckUserAvailability(AvailableUserSpRequest spRequest);
    ValueTask<List<IDbResponse>[]> VerifyOtp(OtpSpRequest spRequest);
}
