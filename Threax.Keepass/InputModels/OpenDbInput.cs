using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;

namespace Threax.Keepass.InputModels
{
    [HalModel]
    [CacheEndpointDoc]
    public class OpenDbInput
    {
        [PasswordUiType]
        public String DatabasePassword { get; set; }
    }
}
