//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Premiumstress.Blog.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class BlogVideo
    {
        public int ID { get; set; }
        public string VideoLink { get; set; }
        public Nullable<int> BlogID { get; set; }
    
        public virtual Blog Blog { get; set; }
    }
}
