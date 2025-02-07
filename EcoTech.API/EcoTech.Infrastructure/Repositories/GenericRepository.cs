
using EcoTech.Shared.Constants;
using EcoTech.Shared.Helpers;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace EcoTech.Infrastructure.Repositories;

public class GenericRepository:IGenericRepository
{
    private IApplicationManager _applicationManager {  get; set; }
    public GenericRepository(IApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }
    public async Task<List<string[]>> ExecuteSpToStringArray(string spName, string dbName=default!,
        bool spNeedParameters=true,object spEntity=default!, List<SqlParameter> param = default!, 
        int[] indexArray = default!, int tillIndex = -1)
    {
        if(spNeedParameters && param == null && spEntity != null)
        {
            param = GetParamFromObject(spEntity);
        }
        if(string.IsNullOrWhiteSpace(dbName) )
        {
            dbName = _applicationManager.GetConnectionString(dbName);
        }
        List<string[]> genericList = new();
        int numberOfProperties = 0;
        if (tillIndex != -1)
        {
            numberOfProperties = tillIndex;
            indexArray = Enumerable.Range(0, numberOfProperties).ToArray();
        }
        else if (indexArray != null)
        {
            numberOfProperties = indexArray.Length;
        }
      
        try
        {
            using (SqlConnection connection = new SqlConnection(dbName))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(param != null ? param.ToArray() : Array.Empty<SqlParameter>());
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            string[] objectValues = new string[numberOfProperties];
                            for (int i = 0; i < indexArray!.Length; i++)
                            {
                                bool isNotNull = !reader.IsDBNull(indexArray[i]);
                                if (isNotNull)
                                {
                                    objectValues[i] = Convert.ToString(reader[indexArray[i]])!;
                                }
                                else
                                {
                                    objectValues[i] = string.Empty;
                                }
                            }
                            genericList.Add(objectValues);
                        }
                    }
                }
            }
            return genericList;

        }
        catch (Exception ex)
        {
            StringBuilder info = new StringBuilder().Append(spName).Append(Environment.NewLine).Append(dbName).
                Append(Environment.NewLine)
                .Append(JsonSerializer.ToJsonString(spEntity)).Append(Environment.NewLine);
            foreach (var oneparam in param)
            {
                info.Append(oneparam.ParameterName).Append(oneparam.Value);
            }
            info.Append(JsonSerializer.ToJsonString(indexArray)).Append(Environment.NewLine )
                .Append(tillIndex.ToString());
            throw new EcoTechException(info, ex);
           
        }
    }

    public async Task<List<IDbResponse>[]> ExecuteSpToObjects(string spName, string dbName = default!,
        bool spNeedParameters = true, DomainAppConstants.SpResponse[] returnObjects = default!, object spEntity = default!, 
        List<SqlParameter> param = default!)
    {

        if (spNeedParameters && param == null && spEntity != null)
        {
            param = GetParamFromObject(spEntity);
        }
        if (string.IsNullOrWhiteSpace(dbName))
        {
            dbName = _applicationManager.GetConnectionString(dbName);
        }
        List<IDbResponse>[] allTables = new List<IDbResponse>[returnObjects == null ? 1 : returnObjects.Length];
        
        try
        {
            using (SqlConnection connection = new SqlConnection(dbName))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(param != null ? param.ToArray() : Array.Empty<SqlParameter>());
                    if (returnObjects == null)
                    {
                        GenericDbResponse genericDbResponse = new();
                        genericDbResponse.NumberOfRowsAffected = await command.ExecuteNonQueryAsync();
                        allTables[0] = new List<IDbResponse>() { genericDbResponse };
                    }
                    else
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            for (int i = 0; i < returnObjects.Length && reader.HasRows; i++)
                            {

                                int objectTypeIndex = (int)returnObjects[i];
                                if (objectTypeIndex == 0)
                                {
                                    reader.NextResult();
                                    continue;
                                }
                                Type type = DomainAppConstants.SpResponseModelTypeArray[objectTypeIndex];
                                var list = new List<IDbResponse>();
                                while (reader.Read())
                                {
                                    IDbResponse dbResponseObject = (IDbResponse)Activator.CreateInstance(type)!;
                                    for (int j = 0; j < AppConstants.SpResponsePropertyInfoCache[objectTypeIndex].Length; j++)
                                    {
                                        PropertyInfo prop = AppConstants.SpResponsePropertyInfoCache[objectTypeIndex][j];
                                        bool isNotNull = !reader.IsDBNull(j);
                                        if (isNotNull)
                                        {
                                            if (prop.PropertyType == AppConstants.StringType)
                                            {

                                                prop.SetValue(dbResponseObject, reader.GetString(j));
                                            }
                                            else if (prop.PropertyType == AppConstants.IntType)
                                            {
                                                prop.SetValue(dbResponseObject, reader.GetInt32(j));
                                            }
                                            else if (prop.PropertyType == AppConstants.BoolType)
                                            {
                                                prop.SetValue(dbResponseObject, Convert.ToBoolean(reader.GetValue(j)));
                                            }
                                            else if (prop.PropertyType == AppConstants.LongType)
                                            {
                                                prop.SetValue(dbResponseObject, reader.GetInt64(j));
                                            }
                                            else if (prop.PropertyType == AppConstants.DateTimeType)
                                            {
                                                prop.SetValue(dbResponseObject, reader.GetDateTime(j));

                                            }
                                            else if (prop.PropertyType == AppConstants.GuidType)
                                            {
                                                prop.SetValue(dbResponseObject, reader.GetGuid(j));

                                            }


                                        }
                                    }
                                    list.Add(dbResponseObject);
                                }
                                allTables[i] = list;
                               reader.NextResult();
                            }

                        }
                    }
                    

                }
            }

        }
        catch (Exception ex)
        {
            StringBuilder info =new StringBuilder().Append( spName).Append(Environment.NewLine).Append(dbName).
                Append(Environment.NewLine).Append(JsonSerializer.ToJsonString(returnObjects)).Append(Environment.NewLine)
                .Append(JsonSerializer.ToJsonString(spEntity)).Append(Environment.NewLine);
            foreach(var oneparam in param)
            {
                info.Append(oneparam.ParameterName).Append(oneparam.Value);
            }
            
            throw new EcoTechException(info, ex);

        }
        return allTables;
    }
    public List<SqlParameter> GetParamFromObject(object obj)
    {

        DomainAppConstants.SpRequest spParamModel;
        if (Enum.IsDefined(DomainAppConstants.SpRequestEnumType, obj.GetType().Name))
        {
            spParamModel= (DomainAppConstants.SpRequest)Enum.Parse(DomainAppConstants.SpRequestEnumType, obj.GetType().Name);
        }
        else
        {
            spParamModel = (DomainAppConstants.SpRequest)Enum.Parse(DomainAppConstants.SpRequestEnumType, obj.GetType().BaseType!.Name);
        }
        
        List<SqlParameter> paramList = new();
        int srNo = (int)spParamModel;
        PropertyInfo[] spParameters = AppConstants.SpRequestPropertyInfoCache[srNo];
        SqlParameter param;
        foreach (var parameter in spParameters)
        {
            string? value = Convert.ToString(AppConstants.CachedPropertyAccessorDelegates[srNo][parameter.Name](obj));
            param = new($"{AppConstants.AtTheRateString}{parameter.Name}", string.IsNullOrWhiteSpace(value) ? DBNull.Value : value);
            paramList.Add(param);
        }
        return paramList;
    }

    public async ValueTask LogErrors(string uniqueId = default!, string information = default!)
    {
        LogErrorsSpRequest spRequest = new()
        {
            UniqueId = uniqueId,
            Information = information
        };
      await  ExecuteSpToObjects(SpConstants.USP_Log_Exceptions,spEntity: spRequest);
    }
}
