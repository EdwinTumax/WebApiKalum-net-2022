using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaCreateDTO
    {
        [StringLength(128,MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y maxima es {1} para el campo {0}")]        
        public string Nombre {get; set;}
    }
}