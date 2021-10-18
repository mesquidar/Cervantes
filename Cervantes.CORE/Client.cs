using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.CORE
{
    public class Client
    {
        /// <summary>
        /// Porject Note Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Note Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Note description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Note Name
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Note description
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// Note description
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// Note description
        /// </summary>
        public string ContactPhone { get; set; }


    }
}
