﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.CORE
{
    public class Note
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
        /// User who created project
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Id user
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }

    }
}
