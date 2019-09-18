using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Pagamento")]
    public class Pagamento
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}