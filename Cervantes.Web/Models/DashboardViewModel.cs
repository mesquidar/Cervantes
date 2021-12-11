using Cervantes.CORE;
using System.Collections.Generic;

namespace Cervantes.Web.Models
{
    public class DashboardViewModel
    {
        public int ProjectNumber { get; set; }
        public int VulnNumber { get; set; }
        public int TasksNumber { get; set; }
        public int ClientNumber { get; set; }
        public IEnumerable<Project> ActiveProjects { get; set; }
        public IEnumerable<Client> RecentClients { get; set; }
        public IEnumerable<Document> RecentDocuments { get; set; }
        public IEnumerable<Vuln> RecentVulns { get; set; }

        public int ProjectPercetagesActive { get; set; }
        public int ProjectPercetagesArchived { get; set; }
        public int ProjectPercetagesWaiting { get; set; }

        public int OpenReported { get; set; }
        public int OpenUnresolved { get; set; }
        public int ConfirmedExploited { get; set; }
        public int ConfirmedUnexploited { get; set; }
        public int ResolvedMitigated { get; set; }
        public int ResolvedRemediated { get; set; }
        public int ClosedMitigated { get; set; }
        public int ClosedRemedaited { get; set; }
        public int ClosedRejected { get; set; }

    }
}
