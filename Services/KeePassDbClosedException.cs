using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassWeb.Services
{
    public class KeePassDbClosedException : Exception
    {
        public KeePassDbClosedException(String message)
            : base(message)
        {

        }
    }
}
