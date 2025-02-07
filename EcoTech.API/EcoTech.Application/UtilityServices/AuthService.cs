




namespace EcoTech.Application.UtilityServices;

public class AuthService: IAuthService
{
    private readonly IHttpContextAccessor _httpContext;

    public byte[] Key { get; set; }
    public byte[] IV { get; set; }
    public string JwtSigningKey { get; set; }

    //private ApplicationManager _applicationManager;
    public AuthService(IApplicationManager applicationManager,IHttpContextAccessor httpContext)
        {
        if (applicationManager.AppSettings.Configuration.UseDynamicSecrets.Equals(AppConstants.True,StringComparison.OrdinalIgnoreCase))
        {               
            JwtSigningKey = AppConstants.JwtSigningKey;
            Key = AppConstants.DynamicEncryptionKey;
            IV = AppConstants.DynamicEncryptionIV;
        }
        else
        {
            JwtSigningKey = applicationManager.AppSettings.Secrets.JwtSiginingKey;
            Key = Encoding.UTF8.GetBytes(applicationManager.AppSettings.Secrets.AESKey);
            IV = Encoding.UTF8.GetBytes(applicationManager.AppSettings.Secrets.AESIV);
        }
        //_applicationManager = applicationManager;
        _httpContext = httpContext;
       
    }


    public string EncryptString(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key =  Key;
        aes.IV = IV;

        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            using StreamWriter sw = new StreamWriter(cs);
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string DecryptString(string base64String)
    {
        byte[] cipherText = Convert.FromBase64String(base64String);
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using MemoryStream ms = new MemoryStream(cipherText);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public string CreateJwtToken(int validForMinutes = 15, string[] roles = default!, string[] evenNoOfClaimsInfo = default!)
    {
        Claim[] claims = evenNoOfClaimsInfo == null? Array.Empty<Claim>():new Claim[evenNoOfClaimsInfo.Length/2];
        for (int i = 0; i < claims.Length; i=i+2)
        {
            claims[i] = new Claim(evenNoOfClaimsInfo![i], EncryptString(evenNoOfClaimsInfo[i + 1]));
        }
        
        for(int j = 0; roles != null && j < roles.Length;j++)
        {
            roles[j]=AppConstants.EncryptedAppRolesDictionary[roles[j]];
        }
        return JwtBearer.CreateToken(options =>
        {
            options.SigningKey = JwtSigningKey;
            options.ExpireAt = DateTime.UtcNow.AddMinutes(validForMinutes);
            options.User.Roles.AddRange(roles ?? Array.Empty<string>());
            options.User.Claims.AddRange (claims);
        });
    }
    public string CreateRefreshJwtToken()
    {            
        return JwtBearer.CreateToken(options =>
        {
            options.SigningKey = JwtSigningKey;
            options.ExpireAt = DateTime.UtcNow.AddMinutes(20);
            options.User.Claims.AddRange(_httpContext.HttpContext!.User.Claims);
        });         
    }
    public AppUser GetAppUser()
    {
        AppUser appUser = new();
        StringBuilder roles = new();
        foreach (var claim in _httpContext.HttpContext!.User.Claims)
        {
            if (claim.Type.Equals(AppConstants.Role, StringComparison.OrdinalIgnoreCase))
            {
                roles = roles.Append(DecryptString(claim.Value)).Append(AppConstants.CommaAsChar);

            }
            else
            {
                appUser.Claims.Add(claim.Type, claim.Value);
            }

        }
        if (roles.Length > 0)
        {
            roles.Length--;
            appUser.Roles = roles.ToString().Split(AppConstants.CommaAsChar);
        }
        return appUser;
    }
}
