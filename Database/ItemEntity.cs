using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;

namespace KeePassWeb.Database 
{
    public partial class ItemEntity : ICreatedModified
    {
        [Key]
        public String ItemId { get; set; }

        public bool IsGroup { get; set; }

        public String Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
