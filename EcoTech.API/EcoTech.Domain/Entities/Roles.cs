

namespace EcoTech.Domain.Entities;

public class Roles
{
    public string Contact {  get; set; }=string.Empty;
}

public class RolesSpRequest:Roles;
public class RolesSpResponse : DbResponse;



