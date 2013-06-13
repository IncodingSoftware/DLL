using System;
using System.Collections.Generic;
using Reccit.DataAccess.Model;
using Reccit.DataAccess.Manager;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using FastMember;
using System.ComponentModel;

namespace Reccit.Facebook
{
    public class Object
    {
        public string Token {get;set;}
        public int FacebookId { get; set; }

        private readonly string _getFriendsUrl = "https://graph.facebook.com/{0}/friends?access_token={1}&fields=name,gender,picture";
        private readonly string _getUserProfileUrl = "https://graph.facebook.com/me?access_token={0}";
        public int CreateReccitUser()
        {
            //reccit will make use of facebookid for consistency
            int userid = FacebookId;
            var jobid = Guid.NewGuid();
            try
            {
                //get friends
                var reccitUsers = GetFriends(jobid);
                
                //get user profile data
                var user = GetUser(jobid);
                if(user != null)
                   reccitUsers.Add(user);

                var ReccitUsertable = Reccit.Utility.Generics.ToDataTable<sqlblk_ReccitUser>(reccitUsers);
              
                //save friends
                var dalManager = new ReccitManager();
                dalManager.SaveUsers(ReccitUsertable,FacebookId,jobid);
                //map user to friends
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return userid;
        }

        public sqlblk_ReccitUser GetUser(Guid jobid)
        {
            var user = new sqlblk_ReccitUser();
            var getUserProfileUrl = string.Format(_getUserProfileUrl, Token);
            var webHelper = new Reccit.Web.Helper();
            var response = webHelper.GetData(getUserProfileUrl, Reccit.Web.Type.RequestType.GET);
            var myProfile = JObject.Parse(response);
            //foreach (var i in myProfile.Children()) 
	        {
                user.sqlblkJobID = jobid;
                user.FirstName = myProfile["name"].ToString().Replace("\"", "");
                user.LastName = null;
                user.FacebookID = Convert.ToInt64(myProfile["id"].ToString().Replace("\"", ""));
                user.Gender = null != myProfile["gender"] && myProfile["gender"].ToString().Replace("\"", "") == "male" ? true : false;
                user.Phone = "";
                user.Email="";
                user.DOB=null;
                user.IsActive=true;
                user.DateCreated = DateTime.Now;
	        }
            return user;

        }

        public IList<sqlblk_ReccitUser> GetFriends(Guid jobid)
        {   
            var getFriendUrl = string.Format(_getFriendsUrl, FacebookId, Token);

            var webHelper = new Reccit.Web.Helper();
            var response = webHelper.GetData(getFriendUrl, Reccit.Web.Type.RequestType.GET);
            var myFriends = JObject.Parse(response);
            //var dt = new DataTable("ReccitUser");
            //var dc = dt.Columns.Add("ReccitUserID", typeof(Int32));
           
            IList<sqlblk_ReccitUser> reccitUsers = new List<sqlblk_ReccitUser>();
            foreach (var i in myFriends["data"].Children())
            {
                reccitUsers.Add(new sqlblk_ReccitUser() 
                {
                    sqlblkJobID =jobid,
                    FirstName = i["name"].ToString().Replace("\"", ""),
                    LastName = null,
                    FacebookID = Convert.ToInt64(i["id"].ToString().Replace("\"", "")),
                    Gender = null != i["gender"] && i["gender"].ToString().Replace("\"", "") == "male" ? true : false,
                    Phone = "",
                    Email="",
                    DOB=null,
                    IsActive=true,
                    DateCreated = DateTime.Now

                });
            }
           
            return reccitUsers;
        }
    }
}
