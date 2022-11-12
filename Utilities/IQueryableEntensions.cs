using WebApiKalum.Dtos;

namespace WebApiKalum.Utilities
{
    public static class IQueryableEntensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, int number)
        {
            //this.Number = number;
            //int cantidadRegistrosPorPagina = 5;
            //int totalRegistros = source.Count();
            //this.TotalPages = (int) Math.Ceiling((Double)totalRegistros/cantidadRegistrosPorPagina);

            //source.Skip(cantidadRegistrosPorPagina * Number).Take(cantidadRegistrosPorPagina).ToList();

            return queryable
                .Skip((5 * number))
                .Take(5);
        }
    }
}