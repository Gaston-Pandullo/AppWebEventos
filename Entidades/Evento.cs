using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaEvento { get; set; }

        [Required]
        public string UbicacionGoogleMaps { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
        public bool Realizado { get; set; } = false;
        public List<Invitado> Invitados { get; set; } = new();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
