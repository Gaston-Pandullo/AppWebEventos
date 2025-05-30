using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Userame { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateOnly FechaCreado { get; set; }
        public bool Estado { get; set; }
    }
}
