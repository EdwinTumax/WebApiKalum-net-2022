using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase    
    {
        private readonly KalumDbContext DbContext;
        public CarreraTecnicaController(KalumDbContext _DbContext)
        {
            this.DbContext = _DbContext;
        }
        [HttpGet]
        public ActionResult<List<CarreraTecnica>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c => c.Aspirantes).ToList();
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                return new NoContentResult();
            }    
            return Ok(carrerasTecnicas);    
        }
    }
}