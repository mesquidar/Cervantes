using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cervantes.CORE;
using Cervantes.Contracts;

namespace Cervantes.DAL
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Implemnt save async method
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        //public DbSet<ApplicationUser> Users { get; set; }

    }
}
