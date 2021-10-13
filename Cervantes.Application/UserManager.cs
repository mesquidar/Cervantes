using Cervantes.CORE;
using Cervantes.CORE.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.Application
{
    public class UserManager: GenericManager<ApplicationUser>, IUserManager
    {
        /// <summary>
        /// User Manager Constructor
        /// </summary>
        /// <param name="context"></param>
        public UserManager(IApplicationDbContext context) : base(context)
        {
        }

        public ApplicationUser GetByUserId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
