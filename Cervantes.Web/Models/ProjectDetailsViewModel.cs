using Cervantes.CORE;
using System.Collections.Generic;

namespace Cervantes.Web.Models
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<ProjectUser> ProjectUsers { get; set; }
        public IEnumerable<Target> Targets { get; set; }
        public IEnumerable<CORE.Task> Tasks { get; set; }
        public IEnumerable<ProjectNote> ProjectNotes { get; set; }
        public IEnumerable<ProjectAttachment> ProjectAttachments { get; set; }

        public IEnumerable<Vuln> Vulns { get; set; }


    }
}
