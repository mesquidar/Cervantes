using Cervantes.CORE;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cervantes.Web.Models
{
    public class UserViewModel : IdentityUser
    {
        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// User avatar
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// user Bio description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User Position
        /// </summary>
        public string Position { get; set; }

        public List<string> Role { get; set; }

        public ApplicationUser User { get; set; }
    }
}
