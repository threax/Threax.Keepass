using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.Keepass.Services
{
    public class KeePassDbClosedException : Exception
    {
        public KeePassDbClosedException(String message)
            : base(message)
        {

        }
    }
}
