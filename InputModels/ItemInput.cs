using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace KeePassWeb.InputModels 
{
    [HalModel]
    public partial class ItemInput
    {
        public String Name { get; set; }

        public bool IsGroup { get; set; }

    }
}
