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
    
    public partial class CurrentStatusLvlMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CurrentStatusLvlMaster()
        {
            this.RequestHeaders = new HashSet<RequestHeader>();
            this.LoginDetails = new HashSet<LoginDetail>();
        }
    
        public int CurrStatusID { get; set; }
        public string StatusText { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestHeader> RequestHeaders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginDetail> LoginDetails { get; set; }
    }
}