//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MFC
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employ
    {
        public int id_employ { get; set; }
        public string emp_name { get; set; }
        public int id_role { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
