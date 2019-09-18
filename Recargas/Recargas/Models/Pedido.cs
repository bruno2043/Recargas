using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha_Pedido { get; set; }
        public DateTime Fecha_Modificacion { get; set; }

        public int PuntoId { get; set; }
        public Punto Punto { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; }
        public ICollection<PedidoProducto> PedidoProductos { get; set; }

        [NotMapped]
        public PedidoProducto PedidoProducto { get; set; }

        [NotMapped]
        public decimal TotalEntregue { get; set; }
        [NotMapped]
        public decimal TotalPago { get; set; }
        [NotMapped]
        public decimal TotalDeuda { get; set; }

    }
}