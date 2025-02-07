
using Microsoft.Data.SqlClient;

namespace EcoTech.Domain.RepositoryContracts;

public interface IGenericRepository
{
    Task<List<string[]>> ExecuteSpToStringArray(string spName, string connectionString=default!, bool spNeedParameters = true,
        object spEntity = default!, List<SqlParameter> param = default!, int[] indexArray = default!, int tillIndex = -1);
   Task<List<IDbResponse>[]> ExecuteSpToObjects(string spName, string dbName = default!,
      bool spNeedParameters = true, DomainAppConstants.SpResponse[] returnObjects = default!, object spEntity = default!,
      List<SqlParameter> param = default!);
    List<SqlParameter> GetParamFromObject(object obj);
    ValueTask LogErrors(string uniqueId=default!, string information=default!);
}
