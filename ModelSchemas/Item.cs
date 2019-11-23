using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threax.AspNetCore.Models;

namespace KeePassWeb.ModelSchemas
{
    [KeyType(typeof(String))]
    public class Item
    {
        public String Name { get; set; }

        public bool IsGroup { get; set; }
    }
}
