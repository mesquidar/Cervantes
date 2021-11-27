using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cervantes.CORE
{
    public enum VulnStatus
    {
        OpenReported = 0,
        OpenUnresolved = 1,
        ConfirmedExploited = 2,
        ConfirmedUnexploited = 3,
        ResolvedMitigated = 4,
        ResolvedRemediated = 5,
        ClosedMitigated = 6,
        ClosedRemediated = 7,
        ClosedRejected = 8
    }
}
