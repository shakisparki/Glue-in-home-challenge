using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Deliveries.Core.Enums
{
    public enum Errors
    {
        [Description("Internal Server Error")]
        Fail = 0,

        [Description("Success")]
        Pass,

        NotFound,

        [Description("Invalid state transition")]
        BadState,
        
        [Description("Access window doesn't permit operation")]
        NoAccess,

        NoContent,

        Created
    }
}
