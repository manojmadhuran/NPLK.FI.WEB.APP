﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CRP_SYSEntities : DbContext
    {
        public CRP_SYSEntities()
            : base("name=CRP_SYSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<NEW_CUS_COMPANIES> NEW_CUS_COMPANIES { get; set; }
        public virtual DbSet<NEW_CUS_EVAL> NEW_CUS_EVAL { get; set; }
        public virtual DbSet<NEW_CUS_IMAGE> NEW_CUS_IMAGE { get; set; }
        public virtual DbSet<NEW_CUS_PAINT_BRANDS> NEW_CUS_PAINT_BRANDS { get; set; }
        public virtual DbSet<NEW_CUS_FINAL_APPROVAL> NEW_CUS_FINAL_APPROVAL { get; set; }
        public virtual DbSet<NEW_CUS_OWNERS> NEW_CUS_OWNERS { get; set; }
        public virtual DbSet<NEW_CUS_HEADER> NEW_CUS_HEADER { get; set; }
        public virtual DbSet<viewNEW_CUS_HEADER> viewNEW_CUS_HEADER { get; set; }
    }
}
