
namespace EcoTech.Domain.Entities;
public class AvailableUser
{
    public string UserUniqueId { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
}

public class AvailableUserSpRequest: AvailableUser;

public class AvailableUserSpResponse : DbResponse;
