//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Investment.ADOApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class BrokerageAccount
    {
        public int IdBrokerage { get; set; }
        public int IdUser { get; set; }
        public Nullable<int> IdStock { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<int> Amount { get; set; }
    
        public virtual Stock Stock { get; set; }
        public virtual User User { get; set; }
    }
}
