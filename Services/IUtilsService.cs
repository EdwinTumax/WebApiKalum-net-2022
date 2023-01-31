using WebApiKalum.Dtos;

namespace WebApiKalum.Services
{
    public interface IUtilsService
    {
        public Task<bool> CrearSolicitudAsync(EnrollmentDTO value);
        public Task<bool> CrearExpedienteAsync(AspiranteCreateDTO value);
    }
}