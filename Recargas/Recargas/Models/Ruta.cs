using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    [Table("Ruta")]
    public class Ruta
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Ruta")]
        public string Nome { get; set; }

        public ICollection<Punto> Puntos { get; set; }
    }
}