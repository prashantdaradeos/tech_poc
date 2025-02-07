
using EcoTech.Domain.UtilityEntities;
using System.Security.Claims;

namespace EcoTech.Application.UtilityServiceContracts;

public interface IAuthService
{
    string EncryptString(string plainText);
    string DecryptString(string base64String);
    string CreateJwtToken(int validForMinutes = 20,string[] roles= default!, string[] evenNoOfClaimsInfo = default!);
    string CreateRefreshJwtToken();
    AppUser GetAppUser();
}

