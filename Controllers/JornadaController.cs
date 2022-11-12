using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornadas")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;
        
        public JornadaController (KalumDbContext _DbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> Get()        
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso de consulta de jornadas en la base de datos");
            jornadas = await DbContext.Jornada.ToListAsync();            
            if(jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen jornadas");
                return new NoContentResult();
            }
            List<JornadaListDTO> lista = Mapper.Map<List<JornadaListDTO>>(jornadas);
            Logger.LogInformation("Se ejecuto la petición de forma extitosa");    
            return Ok(lista);    
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<PaginationResponse<JornadaListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Jornada.AsQueryable();
            int registros = await queryable.CountAsync();
            if(registros == 0)
            {
                return NoContent();
            }
            else
            {
                var jornadas = await queryable.OrderBy(jornada => jornada.NombreCorto).Paginar(page).ToListAsync();
                PaginationResponse<JornadaListDTO> response = new PaginationResponse<JornadaListDTO>(Mapper.Map<List<JornadaListDTO>>(jornadas),page,registros);
                return Ok(response);
            }            
        }

        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<JornadaListDTO>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {   Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(Mapper.Map<JornadaListDTO>(jornada));
        }

        [HttpPost]    
        public async Task<ActionResult<JornadaListDTO>> Post([FromBody] JornadaCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una carrera técnica nueva");            
            Jornada nuevo = Mapper.Map<Jornada>(value);
            nuevo.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();            
            Logger.LogInformation("Finalizando el proceso de agregar una Jornada");
            return new CreatedAtRouteResult("GetJornada",new {id = nuevo.JornadaId}, Mapper.Map<JornadaListDTO>(nuevo));    
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminación del registro");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No se encontro ninguna jornada con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la jornada con el id {id}");
                return jornada;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] JornadaCreateDTO value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la jornada con el id {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(ct => ct.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No existe la carrera técnica con el Id {id}");
                return BadRequest();
            }
            jornada.NombreCorto = value.NombreCorto;
            jornada.Descripcion = value.Descripcion;
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctament");
            return NoContent();
        }

    }
}