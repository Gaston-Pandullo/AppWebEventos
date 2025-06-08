using System.ComponentModel.DataAnnotations;

namespace AppWebEventos.ViewModels
{
    public class InvitadoFormViewModel
    {
        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        public bool MayorDeEdad { get; set; }

        public bool ConfirmoAsistencia { get; set; }

        [Required]
        public int EventoId { get; set; }
    }
}
