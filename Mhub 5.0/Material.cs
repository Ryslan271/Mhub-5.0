//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mhub_5._0
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.ProductMaterial = new HashSet<ProductMaterial>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public Nullable<int> IdTypeMaterial { get; set; }
        public Nullable<int> CountPackaged { get; set; }
        public Nullable<int> idUnit { get; set; }
        public Nullable<int> CountInDtock { get; set; }
        public Nullable<int> Min { get; set; }
        public Nullable<int> Price { get; set; }
    
        public virtual TypeMaterial TypeMaterial { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
    }
}
