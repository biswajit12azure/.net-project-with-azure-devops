using Dapper;
using LazyCache;
using System.Data;
using WGL.Auth.Application.Interfaces.UserPortalRoleMapping;
using WGL.Auth.Domain.Entities;
using WGL.Utility.DBContext;

namespace WGL.Auth.Persistence.Repositories.UserPortalRoleMapping
{
    public class UserPortalRoleMappingRepositoryAsync(ICacheProvider cacheProvider, DBContext dapperContext) : IUserPortalRoleMappingRepositoryAsync
    {
        private readonly ICacheProvider _cacheProvider = cacheProvider;
        private readonly DBContext _dapperContext = dapperContext;
        private readonly IDbConnection _dbconnection = dapperContext.CreateConnection();
        public async Task<List<PortalRoleAccessResponse>> GetUserPortalRoleMappings(UserPortalRoleMappingParams userPortalRoleMapping)
        {
            DynamicParameters Params = new();
            Params.Add("@PortalID", userPortalRoleMapping.PortalID);
            Params.Add("@RoleAccessMappingID", userPortalRoleMapping.RoleAccessMappingID);
            Params.Add("@AccessID", userPortalRoleMapping.AccessID);
            Params.Add("@RoleID", userPortalRoleMapping.RoleID);
            Params.Add("@IsActive", userPortalRoleMapping.IsActive);
            Params.Add("@CreatedBy", userPortalRoleMapping.CreatedBy);
            Params.Add("@StatementType", "Select");
           
            var accessNames = _dbconnection.Query<(int AccessID, string AccessName)>(
                "SELECT AccessID, AccessName FROM PortalAccessMapping"
            ).ToDictionary(x => x.AccessID, x => x.AccessName);
        
            var result = _dbconnection.Query<PortalRoleAccessResponse, PortalRole, FeatureAccess, PortalRoleAccessResponse>(
                "Sp_RoleAccessMapping_CRUD",
                (portalRoleAccessResponse, portalRole, featureAccess) =>
                {
                    if (portalRoleAccessResponse.PortalRoleAccess == null)
                        portalRoleAccessResponse.PortalRoleAccess = new List<PortalRole>();

                    var existingRole = portalRoleAccessResponse.PortalRoleAccess.FirstOrDefault(r => r.RoleID == portalRole.RoleID);
                    if (existingRole == null)
                    {
                        portalRole.FeatureAccess = new List<FeatureAccess>();
                        portalRoleAccessResponse.PortalRoleAccess.Add(portalRole);
                        existingRole = portalRole;
                    }

                    if (featureAccess != null)
                    {
                        featureAccess.AccessName = accessNames.TryGetValue(featureAccess.AccessID, out var accessName) ? accessName : null;

                        if (!existingRole.FeatureAccess.Any(f => f.AccessID == featureAccess.AccessID))
                        {
                            existingRole.FeatureAccess.Add(featureAccess);
                        }
                    }

                    return portalRoleAccessResponse;
                },
                Params,
                splitOn: "RoleID,AccessID",
                commandType: CommandType.StoredProcedure
            );

            // Group by PortalId and PortalName for JSON structure
            //var groupedResult = result
            //    .GroupBy(r => new { r.PortalID, r.PortalName })
            //    .Select(group => new PortalRoleAccessResponse
            //    {
            //        PortalID = group.Key.PortalID,
            //        PortalName = group.Key.PortalName,
            //        PortalRoleAccess = group.SelectMany(g => g.PortalRoleAccess).ToList()
            //    })
            //    .ToList(); // Ensure conversion to List<PortalRoleAccessResponse>

            //return groupedResult;
           var groupedResult = result
          .GroupBy(r => new { r.PortalID, r.PortalName })
          .Select(group => new PortalRoleAccessResponse
          {
              PortalID = group.Key.PortalID,
              PortalName = group.Key.PortalName,
              PortalRoleAccess = group
                  .SelectMany(g => g.PortalRoleAccess)
                  .GroupBy(role => new { role.RoleID, role.RoleName })
                  .Select(roleGroup => new PortalRole
                  {
                      RoleID = roleGroup.Key.RoleID,
                      RoleName = roleGroup.Key.RoleName,
                      FeatureAccess = roleGroup
                          .SelectMany(role => role.FeatureAccess)
                          .GroupBy(access => access.AccessID)
                          .Select(accessGroup => accessGroup.First())
                          .ToList()
                  }).ToList()
          }).ToList();

         return groupedResult;
        }

        public async Task<int> Sp_CreateUserPortalMappings(UserPortalRoleMappingParams userPortalRoleMappingParams)
        {
            DynamicParameters Params = new();
            Params.Add("@PortalID", userPortalRoleMappingParams.PortalID);
            Params.Add("@RoleAccessMappingID", userPortalRoleMappingParams.RoleAccessMappingID);
            Params.Add("@AccessID", userPortalRoleMappingParams.AccessID);
            Params.Add("@RoleID", userPortalRoleMappingParams.RoleID);
            Params.Add("@IsActive", userPortalRoleMappingParams.IsActive);
            Params.Add("@CreatedBy", userPortalRoleMappingParams.CreatedBy);
            Params.Add("@StatementType", "Insert");

            var result= await _dbconnection.ExecuteAsync("Sp_RoleAccessMapping_CRUD", Params, commandType: CommandType.StoredProcedure);
            return result;
        
        }
        public async Task<int> BulkUpdateAccessMappings(IEnumerable<BulkUpdateRequest> items)
        {
            //var dataTable = new DataTable();
            //dataTable.Columns.Add("@RoleAccessMappingID", typeof(int));
            //dataTable.Columns.Add("@IsActive", typeof(bool));
            //foreach (var item in items)
            //{
            //    dataTable.Rows.Add(item.RoleAccessMappingID);
            //    dataTable.Rows.Add(item.IsActive);
            //}
            var result = 0;
            
            var parameters = new DynamicParameters();
            foreach (var item in items)
            {
                parameters.Add("@RoleAccessMappingID", item.RoleAccessMappingID);
                parameters.Add("@IsActive", item.IsActive);
                parameters.Add("@StatementType", "Update");
                result = await _dbconnection.ExecuteAsync("Sp_RoleAccessMapping_CRUD", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
            
        }
    }

}
