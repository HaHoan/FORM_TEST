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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GA_UMCEntities : DbContext
    {
        public GA_UMCEntities()
            : base("name=GA_UMCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<sp_Get_All_Staff_2_Result> sp_Get_All_Staff_2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_Get_All_Staff_2_Result>("sp_Get_All_Staff_2");
        }
    }
}
