namespace WebApiKalum.Entities
{
    public class Inscripcion
    {
        public string InscripcionId { get; set; }
        public string Carne { get; set; }
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public string Ciclo { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}