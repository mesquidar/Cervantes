
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Cervantes.CORE;
using Cervantes.CORE.Contracts;
using Cervantes.DAL;

namespace Cervantes.Application
{
    public class RoleManager : GenericManager<IdentityRole>, IRoleManager
    {
        /// <summary>
        /// Role Manager Constructor
        /// </summary>
        /// <param name="context">contexto de datos</param>
        public RoleManager(IApplicationDbContext context) : base(context)
        {
        }
    }
}

