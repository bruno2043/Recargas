using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    public enum Status
    {
        [Description("Pedido")]
        Pedido =0,
        [Description("Entregue")]
        Entregue =1,
        [Description("Anulado")]
        Anulado =2
    }
    [Table("PedidoProducto")]
    public class PedidoProducto
    {
        public int PedidoId { get; set; }
        public virtual Pedido Pedido { get; set; }

        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }

        public DateTime Fecha_Modificacion { get; set; }

        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public Status Status { get; set; }
    }
}