using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using AutoMapper;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [Route("v1/KalumManagement/Aspirantes")]
    [ApiController]
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;

        public AspiranteController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpPost]
        public async Task<ActionResult<AspiranteListDTO>> Post([FromBody] AspiranteCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso para almacenar un registro de aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carrera técnica con el id {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if (jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest();
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if (examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admisión con el id {value.ExamenId}");
                return BadRequest();
            }
            Aspirante aspirante = Mapper.Map<Aspirante>(value);

            await DbContext.Aspirante.AddAsync(aspirante);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha creato el aspirante con exito");
            return new CreatedAtRouteResult("GetAspirante", new { id = aspirante.NoExpediente }, Mapper.Map<AspiranteListDTO>(aspirante));
        }
        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de aspirante");
            List<Aspirante> lista = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();
            if (lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registros en la base de datos");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La consulta se ejecuto con éxito");
            return Ok(aspirantes);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).AsQueryable();
            int registros = await queryable.CountAsync();
            if (registros == 0)
            {
                return NoContent();
            }
            else
            {
                var aspirantes = await queryable.OrderBy(aspirante => aspirante.NoExpediente).Paginar(page).ToListAsync();
                PaginationResponse<AspiranteListDTO> response = new PaginationResponse<AspiranteListDTO>(Mapper.Map<List<AspiranteListDTO>>(aspirantes), page, registros);
                return Ok(response);
            }
        }

        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<AspiranteListDTO>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning("No existe el aspirante con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(Mapper.Map<AspiranteListDTO>(aspirante));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] AspiranteUpdateDTO value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la carrera técnica con el id {id}");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if (aspirante == null)
            {
                Logger.LogWarning($"No existe la carrera técnica con el Id {id}");
                return BadRequest();
            }
            
            aspirante.Nombres = value.Nombres;
            aspirante.Apellidos = value.Apellidos;
            aspirante.Direccion = value.Direccion;
            aspirante.Email = value.Email;
            aspirante.Telefono = value.Telefono;
            aspirante.Estatus = value.Estatus;
            aspirante.CarreraId = value.CarreraId;
            aspirante.JornadaId = value.JornadaId;
            aspirante.ExamenId = value.ExamenId;
                        
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctament");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AspiranteListDTO>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminación del registro");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning($"No se encontro ninguna jornada con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.Aspirante.Remove(aspirante);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el aspirante con el id {id}");
                return Mapper.Map<AspiranteListDTO>(aspirante);
            }
        }

    }
}