﻿using Cervantes.CORE;
using Cervantes.CORE.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.Application
{
    public class ClientManager : GenericManager<Client>, IClientManager
    {
        /// <summary>
        /// Client Manager Constructor
        /// </summary>
        /// <param name="context"></param>
        public ClientManager(IApplicationDbContext context) : base(context)
        {
        }
    }
}
