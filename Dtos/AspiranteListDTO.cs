namespace WebApiKalum.Dtos
{
    public class AspiranteListDTO
    {
        public string NoExpediente { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public CarreraTecnicaCreateDTO CarreraTecnica { get; set; }
        public JornadaCreateDTO Jornada { get; set; }
        public ExamenAdmisionCreateDTO ExamenAdmision { get; set; }
    }
}