using Microsoft.VisualBasic.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text.Json;
using WebApiKalum.Utilities;
using WebApiKalum.Services;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        public IConfiguration Configuration {get;}
        public IUtilsService UtilsService {get;}
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;        

        public InscripcionController(KalumDbContext _DbContext, ILogger<InscripcionController> _Logger, IConfiguration _Configuration, IUtilsService _UtilsService)
        {
            this.Logger = _Logger;
            this.DbContext = _DbContext;
            this.Configuration = _Configuration;
            this.UtilsService = _UtilsService;
        }

        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>> EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);            
            if(aspirante == null)
            {               
                return NoContent();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await this.UtilsService.CrearSolicitudAsync(value);       
            if(respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El proceso de inscripcion se ha realizado con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503,value);
            }
        }

        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = this.Configuration.GetValue<string>("RabbitConfiguration:HostName");;
            factory.VirtualHost = this.Configuration.GetValue<string>("RabbitConfiguration:VirtualHost");;
            factory.Port = this.Configuration.GetValue<int>("RabbitConfiguration:Port");
            factory.UserName = this.Configuration.GetValue<string>("RabbitConfiguration:UserName");
            factory.Password = this.Configuration.GetValue<string>("RabbitConfiguration:Password");
            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish(this.Configuration.GetValue<string>("RabbitConfiguration:EnrollmentExchange"),"",null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                await Task.Delay(100);
                proceso = true;
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                conexion.Close();
            }
            return proceso;
        }


    }
}