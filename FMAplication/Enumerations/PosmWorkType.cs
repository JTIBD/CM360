using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Enumerations
{
    public enum PosmWorkType
    {
        [Description("Installation")]
        Installation,

        [Description("Repair")]
        Repair,

        [Description("Removal")]
        Removal,

        [Description("Removal & re-installation")]
        RemovalAndReInstallation
    }
}
