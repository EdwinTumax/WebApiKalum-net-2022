using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class AlumnoCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Carne { get; set; }
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

    }
}