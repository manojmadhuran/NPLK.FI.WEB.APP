//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FI.PORTAL.dbconnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoginDetail
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Role { get; set; }
        public string Password { get; set; }
    
        public virtual CurrentStatusLvlMaster CurrentStatusLvlMaster { get; set; }
    }
}