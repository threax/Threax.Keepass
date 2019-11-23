using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;

namespace KeePassWeb.ModelSchemas
{
    [RequireAuthorization(typeof(Roles), nameof(Roles.EditValues))]
    public abstract class Value
    {
        [Required]
        [MaxLength(450)]
        public String Name { get; set; }
    }
}
