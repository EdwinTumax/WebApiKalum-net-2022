namespace WebApiKalum.Entities
{
    public class Jornada
    {
        public string JornadaId { get; set; }
        public string NombreCorto { get; set; }
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes{get; set;}
        public virtual List<Inscripcion> Inscripciones{get; set;}
    }
}

