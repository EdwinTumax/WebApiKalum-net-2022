using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using WebApiKalum.Dtos;
using WebApiKalum.Services;

namespace WebApiKalum.Utilities
{
    public class UtilsService : IUtilsService
    {
        public IConfiguration Configuration {get;}
        private readonly ILogger<UtilsService> Logger;        

        public UtilsService(IConfiguration Configuration, ILogger<UtilsService> Logger)
        {
            this.Configuration = Configuration;    
            this.Logger = Logger;
        }
        public async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
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

        public async Task<bool> CrearExpedienteAsync(AspiranteCreateDTO value)
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
                channel.BasicPublish(this.Configuration.GetValue<string>("RabbitConfiguration:CandidateRecordExchange"),"",null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
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