using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Halcyon.Ext.ValueProviders;

namespace Threax.Keepass.InputModels 
{
    [HalModel]
    public partial class ItemInput
    {
        public String Name { get; set; }

        [HiddenUiType]
        public bool IsGroup { get; set; }

        public String UserName { get; set; }

        public String Url { get; set; }

        [TextAreaUiType()]
        public String Notes { get; set; }

    }
}
