//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reccit.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCheckin
    {
        public int UserCheckinID { get; set; }
        public long FBUserID { get; set; }
        public int PlaceID { get; set; }
        public Nullable<bool> IsReccit { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Review { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual Place Place { get; set; }
    }
}
