using System;
using System.Collections.Generic;
using Reccit.Authentication.Contract.Abstract;

namespace Reccit.Authentication.Contract.Factory
{
    public static class CheckinFactory
    {
        public static Object GetDriver(string type) {
            switch (type.ToLower())
            {
                case "facebook":
                    return new Reccit.Facebook.Object();
                default:
                    return null;
            }
        }
    }
}
