using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Aspirante // : IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(12,MinimumLength = 12, ErrorMessage = "El campo número de expediente debe ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellidos {get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombres {get;set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion {get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Telefono {get; set;}
        [EmailAddress(ErrorMessage = "El correo electrónico no es valido")]
        public string Email {get; set; }
        public string Estatus {get; set; } = "NO ASIGNADO";
        public string CarreraId {get; set; }
        public string JornadaId {get; set; }
        public string ExamenId {get; set; }
        public virtual CarreraTecnica CarreraTecnica {get; set; }
        public virtual Jornada Jornada {get; set; }
        public virtual ExamenAdmision ExamenAdmision {get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(NoExpediente))
            {   
                if(!NoExpediente.Contains("-"))
                {
                    yield return new ValidationResult("El número de expediente es invalido, no contiene un '-' ", new string[]{nameof(NoExpediente)});                           
                } 
                else 
                {
                    int guion = NoExpediente.IndexOf("-");                
                    string exp = NoExpediente.Substring(0,guion);
                    string numero = NoExpediente.Substring(guion+1,NoExpediente.Length - 4);
                    if(!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
                    {
                        yield return new ValidationResult("El número de expediente no contiene la nomenclatura adecuada", new string[]{nameof(NoExpediente)});                                               
                    }
                }

            }
        }*/

    }
}