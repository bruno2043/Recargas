using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }

        public decimal Precio { get; set; }
        public ICollection<Compra> Compras { get; set; }
        public ICollection<PedidoProducto> PedidoProducto { get; set; }
    }
}