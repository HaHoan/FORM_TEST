//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class USER_TEST_DETAIL
    {
        public int ID { get; set; }
        public int ID_USER_TEST { get; set; }
        public int ID_QUESTION { get; set; }
        public string ANSWER { get; set; }
        public string COMMENT { get; set; }
        public int POINT { get; set; }
    
        public virtual QUESTION QUESTION { get; set; }
        public virtual USER_TEST USER_TEST { get; set; }
    }
}
