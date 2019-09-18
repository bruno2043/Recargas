using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Persona")]
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Persona")]
        public string Nome { get; set; }
        public string Telefono { get; set; }
        public string Observaciones { get; set; }

        public ICollection<Punto> Puntos { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}