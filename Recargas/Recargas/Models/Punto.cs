using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Punto")]
    public class Punto
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Punto")]
        public string Nome { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        public int RutaId { get; set; }
        [DisplayName("Ruta")]
        public Ruta Ruta { get; set; }

        [NotMapped]
        public List<Ruta> Rutas { get; set; }

        public int ProprietarioId { get; set; }

        [DisplayName("Proprietário")]
        public Persona Proprietario { get; set; }

        [NotMapped]
        public List<Persona> Personas { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}