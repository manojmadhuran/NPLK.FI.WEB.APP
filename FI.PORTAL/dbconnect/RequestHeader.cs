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
    
    public partial class RequestHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestHeader()
        {
            this.RequestDetails = new HashSet<RequestDetail>();
            this.RequestImages = new HashSet<RequestImage>();
        }
    
        public decimal ReqID { get; set; }
        public Nullable<decimal> CusCode { get; set; }
        public string SalesCode { get; set; }
        public Nullable<decimal> AllowCreditLimit { get; set; }
        public string InitialComment { get; set; }
        public Nullable<int> EnhancementStatus { get; set; }
        public Nullable<int> CurrentStatusLevel { get; set; }
        public string ReqStatus { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string FileName { get; set; }
        public string RequestType { get; set; }
        public string RecentAction { get; set; }
        public Nullable<decimal> CreditExposure { get; set; }
        public Nullable<decimal> SAPupdateAmount { get; set; }
    
        public virtual CurrentStatusLvlMaster CurrentStatusLvlMaster { get; set; }
        public virtual EnhancementTextMaster EnhancementTextMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestDetail> RequestDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestImage> RequestImages { get; set; }
    }
}
