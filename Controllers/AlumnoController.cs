using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [Route("v1/KalumManagement/Alumnos")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de Alumnos");
            List<Alumno> lista = await DbContext.Alumno.ToListAsync();
            if (lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registros en la base de datos");
                return new NoContentResult();
            }
            List<AlumnoListDTO> alumnos = Mapper.Map<List<AlumnoListDTO>>(lista);
            Logger.LogInformation("La consulta se ejecuto con éxito");
            return Ok(alumnos);
        }

        [HttpGet("{id}", Name = "GetAlumno")]
        public async Task<ActionResult<AlumnoListDTO>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(Mapper.Map<AlumnoListDTO>(alumno));
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<PaginationResponse<AlumnoListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Alumno.AsQueryable();
            int registros = await queryable.CountAsync();
            if (registros == 0)
            {
                return NoContent();
            }
            else
            {
                var alumnos = await queryable.OrderBy(alumno => alumno.Carne).Paginar(page).ToListAsync();
                PaginationResponse<AlumnoListDTO> response = new PaginationResponse<AlumnoListDTO>(Mapper.Map<List<AlumnoListDTO>>(alumnos), page, registros);
                return Ok(response);
            }
        }

        [HttpGet("search", Name = "GetEmail")]
        public async Task<ActionResult<AlumnoListDTO>> GetAspiranteByEmail([FromQuery] string email)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el email " + email);
            var alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Email == email);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el email " + email);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(Mapper.Map<AlumnoListDTO>(alumno));
        }

        [HttpPost]
        public async Task<ActionResult<AlumnoListDTO>> Post([FromBody] AlumnoCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un alumno nuevo");
            Alumno nuevo = Mapper.Map<Alumno>(value);
            await DbContext.Alumno.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una Jornada");
            return new CreatedAtRouteResult("GetAlumno", new { id = nuevo.Carne }, Mapper.Map<AlumnoListDTO>(nuevo));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] AlumnoUpdateDTO value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización del alumno con el id {id}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning($"No existe el alumno con el Id {id}");
                return BadRequest();
            }
            alumno.Apellidos = value.Apellidos;
            alumno.Nombres = value.Nombres;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AlumnoListDTO>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminación del registro");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == id);
            if (alumno == null)
            {
                Logger.LogWarning($"No se encontro ningun alumno con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el alumno con el id {id}");
                return Mapper.Map<AlumnoListDTO>(alumno);
            }
        }

    }
}