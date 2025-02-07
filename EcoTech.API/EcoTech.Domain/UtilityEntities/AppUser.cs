
namespace EcoTech.Domain.UtilityEntities;

public class AppUser
{
    public string[] Roles { get; set; } = default!;
    public Dictionary<string,string> Claims { get; set; }=new();
}
