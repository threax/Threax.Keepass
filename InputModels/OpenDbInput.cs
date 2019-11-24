using Halcyon.HAL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;

namespace KeePassWeb.InputModels
{
    [HalModel]
    public class OpenDbInput
    {
        [PasswordUiType]
        public String DatabasePassword { get; set; }
    }
}
