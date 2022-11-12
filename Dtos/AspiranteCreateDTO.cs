using System.ComponentModel.DataAnnotations;
using WebApiKalum.Helpers;

namespace WebApiKalum.Dtos
{
    public class AspiranteCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "El campo número de expediente debe ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido")]
        public string Email { get; set; }
        public string Estatus { get; set; } = "NO ASIGNADO";
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public string ExamenId { get; set; }

    }
}