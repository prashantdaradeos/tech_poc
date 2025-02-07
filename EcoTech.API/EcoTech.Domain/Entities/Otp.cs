
namespace EcoTech.Domain.Entities;

public class Otp
{
    public string Contact { get; set; } = string.Empty;

}
public class OtpSpRequest : Otp
{
    public string Otp { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
}
public class OtpSpResponse : DbResponse;

