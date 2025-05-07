using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Application.Interfaces.Master;
using WGL.Auth.Domain.Entities;
using WGL.Utility.DBContext;

namespace WGL.Auth.Persistence.Repositories.Master
{
    public class PortalDetailsRepositoryAsync(DBContext dapperContext) : IPortalDetailsRepository
    {
        private readonly DBContext _dapperContext=dapperContext;
        private readonly IDbConnection _dbconnection = dapperContext.CreateConnection();

        public async Task<List<PortalDTO>> GetPortalDetails()
        {
            DynamicParameters Params = new();           
            Params.Add("@StatementType", "Select");
            return (await _dbconnection.QueryAsync<PortalDTO>("Sp_Portal_CRUD", Params, commandType: CommandType.StoredProcedure)).ToList();
        }
    }
}
