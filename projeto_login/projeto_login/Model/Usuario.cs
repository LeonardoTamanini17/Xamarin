using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace projeto_login.Model
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public long Telefone { get; set; }
        public DateTime DataAniversario { get; set; }
    }
}
