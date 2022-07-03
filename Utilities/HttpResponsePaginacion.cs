using WebApiKalum.Dtos;

namespace WebApiKalum.Utilities
{
    public class HttpResponsePaginacion<T> : PaginacionDTO<T>
    {
        public HttpResponsePaginacion(IQueryable<T> source, int number)
        {
            this.Number = number;
            int cantidadRegistrosPorPagina = 5;
            int totalRegistros = source.Count();
            this.TotalPages = (int) Math.Ceiling((Double)totalRegistros/cantidadRegistrosPorPagina);
            this.Content = source.Skip(cantidadRegistrosPorPagina * Number).Take(cantidadRegistrosPorPagina).ToList();
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