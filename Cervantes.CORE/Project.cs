using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.CORE
{
    public class Project
    {
        /// <summary>
        /// Project Id
        /// </summary>
        public int Id {  get; set; }
        /// <summary>
        /// Project Name
        /// </summary>
        public string Name {  get; set; }
        /// <summary>
        /// Project Description
        /// </summary>
        public string Description {  get; set; }
        /// <summary>
        /// Project Start Date
        /// </summary>
        public DateTime StartDate {  get; set; }
        /// <summary>
        /// Project End Date
        /// </summary>
        public DateTime EndDate {  get; set; }

        /// <summary>
        /// User who created project
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Id user
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }

        /// <summary>
        /// Client asssociated to project
        /// </summary>
        public virtual Client Client { get; set; }

        /// <summary>
        /// Client ID
        /// </summary>
        [ForeignKey("Client")]
        public string ClientId { get; set; }


    }
}
