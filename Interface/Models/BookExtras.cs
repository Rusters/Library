//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Interface.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookExtras
    {
        public int id { get; set; }
        public System.DateTime date_out { get; set; }
        public System.DateTime date_get { get; set; }
        public int cost { get; set; }
    
        public virtual Readers Readers { get; set; }
        public virtual ArticulBooks ArticulBooks { get; set; }
    }
}