using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Reccit.Authentication.Contract
{
    [ServiceContract]
    public interface IAuth
    {
        //[WebGet(UriTemplate = "?oauth_token={maskUrl}&type={type}", ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //int Authenticate(string token, string type);

        [WebGet(UriTemplate = "Authenticate?oauth_token={oauth_token}&type={type}&facebookid={facebookid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int Authenticate(string oauth_token, string type, int facebookid);
    }
}
