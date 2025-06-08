using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Entidades
{
    public class Invitado
    {
        public int Id { get; set; }

        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        public bool MayorDeEdad { get; set; }

        public bool ConfirmoAsistencia { get; set; }

        // Relación con Evento
        public int EventoId { get; set; }

        [ValidateNever]
        public Evento Evento { get; set; } = null!;
    }
}
