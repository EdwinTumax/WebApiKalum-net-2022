using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId {get;set;}
        public string Nombre {get; set;}
        public List<AspiranteListDTO> Aspirantes {get; set; }
    }
}