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
    
    public partial class Paqueterias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paqueterias()
        {
            this.orden = new HashSet<orden>();
        }
    
        public int id_paqueteria { get; set; }
        public string nombre { get; set; }
        public string razon_social { get; set; }
        public string rfc { get; set; }
        public string pagina_web { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public int estatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orden> orden { get; set; }
    }
}
