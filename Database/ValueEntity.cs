using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Halcyon.HAL.Attributes;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.Tracking;
using KeePassWeb.Models;

namespace KeePassWeb.Database 
{
    public partial class ValueEntity : IValue, IValueId, ICreatedModified
    {
        [Key]
        public Guid ValueId { get; set; }

        [Required(ErrorMessage = "Name must have a value.")]
        [MaxLength(450, ErrorMessage = "Name must be less than 450 characters.")]
        public String Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
