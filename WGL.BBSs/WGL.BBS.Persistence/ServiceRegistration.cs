using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Utility.Behaviours;

namespace WGL.BBS.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services)
        {
            #region Repositories
            /// Dependency Register for Classes....             
            /// 
            //services.AddSingleton(typeof(DapperContext));
            //services.AddTransient(typeof(ICustomerAccountRepositoryAsync), typeof(CustomerAccountRepositoryAsync));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            #endregion
        }
    }
}