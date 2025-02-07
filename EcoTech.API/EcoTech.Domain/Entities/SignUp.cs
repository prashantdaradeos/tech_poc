


namespace EcoTech.Domain.Entities;

public class SignUp
{
    public string ClientName { get; set; } = string.Empty;
   // public string ClientMobileNumber { get; set; } = string.Empty;
    public string UserMobileNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string GSTIN { get; set; } = string.Empty; 
    public string UserEmail { get; set; } = string.Empty;
   // public string ClientEmail { get; set; } = string.Empty;

}
public class SignUpSpRequest: SignUp;

public class SignUpSpResponse: DbResponse;
