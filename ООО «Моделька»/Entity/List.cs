//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ООО__Моделька_.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class List
    {
        public int ListOrderID { get; set; }
        public string ListSet { get; set; }
        public int ListCount { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Set Set { get; set; }
    }
}
