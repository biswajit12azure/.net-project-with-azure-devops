using Microsoft.Extensions.DependencyInjection;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Application.Interfaces.Generic;
using WGL.Auth.Application.Interfaces.UserPortalRoleMapping;
using WGL.Utility.DBContext;
using WGL.Auth.Persistence.Repositories.Account;
using WGL.Auth.Persistence.Repositories.Generic;
using WGL.Auth.Persistence.Repositories.UserPortalRoleMapping;
using WGL.Utility.EmailService;
using WGL.Utility.BlobFiles.FileInterface;
using WGL.Utility.BlobFiles.FileRepository;
using Azure.Storage.Blobs;
using WGL.Auth.Application.Interfaces.Master;
using WGL.Auth.Persistence.Repositories.Master;

namespace WGL.Auth.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services)
        {
            #region Repositories
            /// Dependency Register for Classes....             
            /// 
            //services.AddSingleton(typeof(DapperContext));
            services.AddSingleton(typeof(DBContext));
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient(typeof(IAccountRepositoryAsync), typeof(AccountRepositoryAsync));
            services.AddTransient(typeof(IUserPortalRoleMappingRepositoryAsync), typeof(UserPortalRoleMappingRepositoryAsync));
            services.AddTransient(typeof(IEmailServiceAsync), typeof(EmailService));
            services.AddTransient(typeof(IBlobStorageRepository), typeof(BlobStorageRepository));
            services.AddTransient(typeof(IPortalDetailsRepository),typeof(PortalDetailsRepositoryAsync));
            
            #endregion
        }
    }
}
