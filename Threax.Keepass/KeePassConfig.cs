using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.Keepass
{
    public class KeePassConfig
    {
        public String DbFile { get; set; }

        public String KeyFile { get; set; }

        public int TimeoutMs { get; set; } = 3 * 60 * 1000; //3 Minutes
    }
}
