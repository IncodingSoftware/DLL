using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reccit.DataAccess;
using Reccit.DataAccess.Model;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Reccit.DataAccess.Manager
{
    public class ReccitManager
    {
        //saveuser
        public static readonly string _connectionString = ConfigurationManager.AppSettings["ReccitContext"].ToString();
        private static readonly string _reccitUserInsert = "Insert into ReccitUser(FirstName,LastName,Photo,Gender,Phone,Email,DOB,DateCreated,IsActive,FacebookID,TwitterHandler,FourSquareID) select sru.FirstName,sru.LastName,sru.Photo,sru.Gender,sru.Phone,sru.Email,sru.DOB,sru.DateCreated,sru.IsActive,sru.FacebookID,sru.TwitterHandler,sru.FourSquareID from sqlblk_ReccitUser sru where sru.facebookid not in (select facebookid from ReccitUser) and sru.sqlblkjobid ='{0}';";
        private static readonly string _sqlblkReccitUserDelete = "Delete from sqlblk_ReccitUser where sqlblkjobid='{0}';";
        private static readonly string _userFriendMap = "Insert into UserFriendMap(UserID,FriendID) Select {0},facebookid from sqlblk_ReccitUser where sqlblkjobid = '{1}'and facebookid not in (select facebookid from UserFriendMap where userid ={0});";
        public IList<ReccitUser> SaveUsers(IList<ReccitUser> reccitUser)
        {
            try
            {
                using (var db = new Entities())
                {
                    foreach (var item in reccitUser)
                    {
                        if (db.ReccitUsers.Where(i => i.FacebookID == item.FacebookID).Count() == 0)
                        {
                            db.ReccitUsers.Add(item);
                        }

                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return reccitUser;
        }

        public void SaveUsers(DataTable users, int userFbid, Guid jobid)
        {
          
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                   SqlBulkCopy bc = new SqlBulkCopy(con,
                      SqlBulkCopyOptions.CheckConstraints |
                      SqlBulkCopyOptions.FireTriggers |
                      SqlBulkCopyOptions.KeepNulls, tran);

                    bc.BatchSize = 1000;
                    bc.DestinationTableName = "sqlblk_ReccitUser";
                    bc.WriteToServer(users.CreateDataReader());
                    tran.Commit();

                    var builder = new StringBuilder();
                    builder.Append(string.Format(_reccitUserInsert, jobid));
                    builder.Append("  ");
                    builder.Append(string.Format(_userFriendMap, userFbid, jobid));
                    builder.Append("  ");
                    builder.Append(string.Format(_sqlblkReccitUserDelete, jobid));
                    var command = new SqlCommand(builder.ToString(), con);
                    command.ExecuteNonQuery();

                    
                }
                con.Close();

            }

           // return users;

        }

        //savefriends
        //savecheckins
        //saveplace
    }
}
