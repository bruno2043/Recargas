using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Compra")]
    public class Compra
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}