using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision")]

    public class ExamenAdminisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdminisionController> Logger;
        private readonly IMapper Mapper;

        public ExamenAdminisionController(KalumDbContext _DbContext, ILogger<ExamenAdminisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet("search", Name = "GetAnio")]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> Get([FromQuery] int anio)
        {
            List<ExamenAdmision> examenesAdmision = await this.DbContext.ExamenAdmision.Where( e =>  e.FechaExamen.Year == anio).ToListAsync();
            if(examenesAdmision == null || examenesAdmision.Count == 0)
            {
                return NoContent();
            }    
            List<ExamenAdmisionListDTO> response = Mapper.Map<List<ExamenAdmisionListDTO>>(examenesAdmision);
            return Ok(response);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.ExamenAdmision.AsQueryable();
            int registros = await queryable.CountAsync();
            if (registros == 0)
            {
                return NoContent();
            }
            else
            {
                var examenes = await queryable.OrderBy(examenes => examenes.FechaExamen).Paginar(page).ToListAsync();
                PaginationResponse<ExamenAdmisionListDTO> response = new PaginationResponse<ExamenAdmisionListDTO>(Mapper.Map<List<ExamenAdmisionListDTO>>(examenes), page, registros);
                return Ok(response);
            }
        }

        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmisionListDTO>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning("No existe el examen de admisión con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(Mapper.Map<ExamenAdmision,ExamenAdmisionListDTO>(examenAdmision));
        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmisionListDTO>> Post([FromBody] ExamenAdmisionCreateDTO value)
        {
            Logger.LogDebug("Iniciando el proceso de registro de la información");
            ExamenAdmision nuevo = Mapper.Map<ExamenAdmision>(value);
            nuevo.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de registro de forma exitosa");
            return new CreatedAtRouteResult("GetCarreraTecnica", new { id = nuevo.ExamenId }, Mapper.Map<ExamenAdmisionListDTO>(nuevo));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminación del registro");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No se encontro ningun examen de admisión con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el examen de admisión con el id {id}");
                return examenAdmision;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmisionCreateDTO value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la fecha de examen de admisión con el id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ct => ct.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No existe la carrera técnica con el Id {id}");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }

    }
}