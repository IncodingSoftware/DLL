using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reccit.Authentication.Contract.Abstract
{
    abstract class Checkin
    {
        public abstract int GetUser(string token, string type);
    }
}
