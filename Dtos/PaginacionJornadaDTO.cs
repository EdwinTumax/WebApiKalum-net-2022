namespace WebApiKalum.Dtos
{
    public class PaginacionJornadaDTO
    {
        public int Number {get; set; }
        public int TotalPages {get; set; }
        public bool First {get; set; }
        public bool Last {get; set; }

        public List<JornadaListDTO> Content {get; set; }

        public PaginacionJornadaDTO(List<JornadaListDTO> lista, int number, int registros)
        {
            this.Number = number;
            int cantidadRegistrosPorPagina = 5;
            int totalRegistros = registros;
            this.TotalPages = (int) Math.Ceiling((Double)totalRegistros/cantidadRegistrosPorPagina);
            this.Content = lista;       
            if(this.Number == 0)
            {
                this.First = true;
            }
            else if((this.Number + 1) == this.TotalPages)
            {
                this.Last = true;
            }
        }
    }
}