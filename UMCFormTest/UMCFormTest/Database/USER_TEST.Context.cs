﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UMCFormTest.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UMC_TESTEntities : DbContext
    {
        public UMC_TESTEntities()
            : base("name=UMC_TESTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<QUESTION> QUESTIONs { get; set; }
        public virtual DbSet<STAFF> STAFFs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<EXAM> EXAMs { get; set; }
        public virtual DbSet<USER_TEST> USER_TEST { get; set; }
        public virtual DbSet<USER_TEST_DETAIL> USER_TEST_DETAIL { get; set; }
    }
}