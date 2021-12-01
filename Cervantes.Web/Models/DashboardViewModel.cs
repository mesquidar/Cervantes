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
    }
}
