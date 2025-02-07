using System.Reflection;
using System.Text;

namespace EcoTech.Shared.Constants;

public enum StringConstants
{
    
}

public static class AppConstants
{
    #region APP URLs
    public const string UserAvailabilityURL = "/users/user-availability";
    public const string ClientSignUpURL = "/users/client-sign-up";
    public const string SendOtpURL = "/users/send-otp";
    public const string VerifyOtpURL = "/users/verify-otp";
    public const string GetRefreshTokenURL = "/users/get-refresh-token";
    #endregion

    #region Configuration Constants
    public const string CorsPolicyName = "AllowAll";
    public const string JwtSiginingKeySection = "ApplicationSettings:Secrets:JwtSiginingKey";
    public const string EncryptionKeySection = "ApplicationSettings:Secrets:AESKey";
    public const string EncryptionIVSection = "ApplicationSettings:Secrets:AESIV";
    public const string UseDynamicSecretsSection = "ApplicationSettings:Configuration:UseDynamicSecrets";
    public const string ApplicationSettingsSection = "ApplicationSettings";
    public const string RoutePrefix = "api";
    public const string ApplicationEnvironmentVaraible="ASPNETCORE_ENVIRONMENT";
    public const string ProductionEnvironment="Production";
    public const string UATEnvironment="UAT";
    public const string DevelopmentEnvironment="Development";
    public const string UniqueId = "UniqueId";
    public static string JwtSigningKey { get; } = Guid.NewGuid().ToString("N");
    public static byte[] DynamicEncryptionKey { get; } = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N"));
    public static byte[] DynamicEncryptionIV { get; } = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16));
    public static string SQL_FILE_PATH { get; set; } = default!;
    #endregion

    #region Environment Variables
    public static string EchoTechServerEnvironmentName = "EchoTechServer";
    public static string EchoTechDataBaseEnvironmentName = "EchoTechDataBase";
    public static string EchoTechUserIdEnvironmentName = "EchoTechUserId";
    public static string EchoTechPasswordEnvironmentName = "EchoTechPassword";
    public const string GenericConnectionString= "Data Source={0}; Initial Catalog={1};User ID={2};Password={3};Connect Timeout=30; Encrypt=False;TrustServerCertificate=True;";
    #endregion


    #region Types of files and Request
    public const string JsonRequestType = "application/json";
    #endregion

    #region Application Constants
    public const string True = "True";
    public const string HyphenAsString = "_";
    public const string CommaAsString = ",";
    public const char CommaAsChar = ',';
    public const string AtTheRateString = "@";
    public const string NextLineString = "\r\n";

    #endregion

    #region Roles
    public const string Role = "Role";
    public const string LeadRole = "Lead";
    public const string UserRole = "Manager";
    public const string GroupAdminRole = "Admin";
    public const string SuperAdminRole = "SuperAdmin";

    public static readonly string[] AppRoles = [LeadRole, UserRole, GroupAdminRole];
    public static string[] EncryptedAppRoles { get; set; } = new string[AppRoles.Length];
    //public static string EncryptedLeadRole { get; set; }=string.Empty;
    //public static string EncryptedUserRole { get; set; } = string.Empty;
    //public static string EncryptedGroupAdminRole { get; set; } = string.Empty;
    public static Dictionary<string,string> EncryptedAppRolesDictionary { get; set; } =new();


    #endregion

    #region DataBase Helpers

    public const string EcoTechDataBase = "EcoTechDataBase";

    //Not In USE From Here
    public static string[] EchotechSqlFilePath = ["wwwroot","Echotech.sql"];
    public static string[] EchotechSqlFilePathForContainer = ["app"];
    public static string[] DataBaseSaperaters { get; } = ["GO\r\n", "GO ", "GO\t"];
    //Till here
   
    public static PropertyInfo[][] SpRequestPropertyInfoCache { get; set; } = default!;
    public static PropertyInfo[][] SpResponsePropertyInfoCache { get; set; } = default!;
    public static Type ListType { get; } = typeof(List<>);
    public static Dictionary<string, Func<object, object>>[] CachedPropertyAccessorDelegates { get; set; } = default!;

    #endregion

    #region DataTypes 
    public static Type StringType { get; } = typeof(string);
    public static Type IntType { get; } = typeof(int);
    public static Type LongType { get; } = typeof(long);
    public static Type BoolType { get; } = typeof(bool); 
    public static Type GuidType { get; } = typeof(Guid);
    public static Type DateTimeType { get; } = typeof(DateTime);
    #endregion

    #region Fluent Validation Constants
    public const string MobileNumberRegex = @"^[6-9]\d{9}$";
    public const string MobileNumberRegexErrorMessage = "Please enter 10 digit valid Mobile Number";
    public const string EmptyErrorMessage = "This field can not be empty";
    public const string OtpRegex = @"^\d{5}$";
    public const string OtpRegexErrorMessage = "Please enter 5 digit valid Otp";
    public const string EnglishLangRegex = "^[a-zA-Z0-9\\s!#&'()*+,-./:;<>?@_\"]+$";
    public const string EnglishLangErrorMessage = "Only English alphabets allowed";
    public const string LengthErrorMessage = "This field must have minimum {0} and maximum {1} charachters";
    public const string FixedLengthErrorMessage = "This field must have {0} charachters";
    public const string PANOrGSTNoRegex = @"^([A-Z]{5}[0-9]{4}[A-Z]{1}){1}$|^([0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}){1}$";
    public const string PANOrGSTNoErrorMessage = "Please enetr valid PAN or GST number";
    public const string EmailRegex = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
    public const string EmailErrorMessage = "Please enetr valid Email";
    public const string MustWithinValuesErrorMessage = "The value for {0} must be one of the following:\r\n {1}";
    public const string MobileOrEmailRegex = @"^[6-9]\d{9}$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public const string MobileOrEmailErrorMessage = "Please enetr valid Mobile number or Email";
    public const string Verification = "Verification";
    public const string Login = "Login";



    #endregion

    #region StringConstants
    public const string OtpInvalidMessage = "Please enter correct 5 digit Otp";
    public const string ErrorMessage = "Error has Occured";
    public const string Failure = "Failure";
    public const string ExceptionInformationDevider = "FastEndpoints.Endpoint";
    public const string RemoveFromException = "---> System.Exception: exception testing\r\n   --- End of inner exception stack trace ---";
    public const string OtpGenerationFailed = "Otp Generation Failed";


    #endregion

    #region Http API Constants
    public const string AuthorizationHeader = "Authorization";
    public const string ContentTypeHeader = "Content-Type";
    public const string Bearer = "Bearer";


    #endregion
}
