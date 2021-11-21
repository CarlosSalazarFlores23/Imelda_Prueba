//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TDV_Carlos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venta()
        {
            this.Comentario = new HashSet<Comentario>();
        }
    
        public int id_venta { get; set; }
        public System.DateTime fecha_venta { get; set; }
        public int cantidad { get; set; }
        public decimal precio_venta { get; set; }
        public int status { get; set; }
        public int id_producto { get; set; }
        public int id_usuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual Productos Productos { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
