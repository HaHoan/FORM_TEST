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
    
    public partial class QUESTION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QUESTION()
        {
            this.USER_TEST_DETAIL = new HashSet<USER_TEST_DETAIL>();
        }
    
        public int ID { get; set; }
        public string Question1 { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int ID_EXAM { get; set; }
        public string TYPE_QUESTION { get; set; }
    
        public virtual EXAM EXAM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_TEST_DETAIL> USER_TEST_DETAIL { get; set; }
        public List<ANSWER_MULTICHOICE> LIST_ANSWER { get; set; }
    }
}
