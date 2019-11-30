using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;

namespace Threax.Keepass.Database 
{
    public partial class EntryEntity : ICreatedModified
    {
        [Key]
        public Guid ItemId { get; set; }

        public String Name { get; set; }

        public String UserName { get; set; }

        public String Url { get; set; }

        public String Notes { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
