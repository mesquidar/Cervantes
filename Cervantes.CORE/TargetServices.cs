using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.CORE
{
    public class TargetServices
    {
        /// <summary>
        /// Id Target Services
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User who created targetservice
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Id user
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }
        /// <summary>
        /// Target associated
        /// </summary>
        public virtual Target Target { get; set; }

        /// <summary>
        /// Id taregt
        /// </summary>
        [ForeignKey("Target")]
        public int TargetId { get; set; }
        /// <summary>
        /// Name of the service
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Port of the service
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Version of the service
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Notes of the service
        /// </summary>
        public string Note { get; set; }



    }
}
