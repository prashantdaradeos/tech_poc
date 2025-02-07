

using System.Text;

namespace EcoTech.Domain.UtilityEntities;

public class EcoTechException :Exception
{
    public StringBuilder Information { get; set; }

    public EcoTechException(StringBuilder info,Exception ex) :base(ex.Message,ex)
    {
        Information=info;
    }
}

public class LogErrorsSpRequest
{
    public string UniqueId { get; set; } = string.Empty;

    public string Information { get; set; }=string.Empty;

}
