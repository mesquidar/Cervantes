using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cervantes.CORE
{
    public class Log
    {
        /// <summary>
        /// Porject Note Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Level { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Message { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string StackTrace { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Exception { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Logger { get; set; }
        /// <summary>
        /// Note Name
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string Url { get; set; }

    }
}
