using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Dtos
{
    public class JornadaCreateDTO
    {
        [StringLength(2,MinimumLength = 2, ErrorMessage = "La cantidad minima de caracteres es {2} y maxima es {1} para el campo {0}")]        
        public string NombreCorto {get;set;}
        public string Descripcion {get;set;}
    }
}