using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recargas.Models
{
    public enum Role
    {
        [Description("Admin")]
        Admin = 0,
        [Description("Funcionario")]
        Funcionario = 1,
        [Description("Cliente")]
        Cliente = 2
    }
    [Table("Account")]
    public class Account
    {
        [Key]
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public Role Role { get; set; }
    }
}