using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnica,CarreraTecnicaListDTO>();
            CreateMap<CarreraTecnicaCreateDTO,CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada,JornadaCreateDTO>();
            CreateMap<ExamenAdmision,ExamenAdmisionCreateDTO>();
            CreateMap<Aspirante,AspiranteListDTO>().ConstructUsing( e => new AspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
        }
    }
}