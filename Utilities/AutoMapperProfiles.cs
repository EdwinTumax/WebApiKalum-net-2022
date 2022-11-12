using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IQueryable<CarreraTecnica>, List<CarreraTecnicaListDTO>>();
            CreateMap<IQueryable<ExamenAdmision>, List<ExamenAdmisionListDTO>>();
            CreateMap<IQueryable<Jornada>, List<JornadaListDTO>>();

            CreateMap<AlumnoCreateDTO,Alumno>();
            CreateMap<Alumno,AlumnoCreateDTO>();
            CreateMap<Alumno, AlumnoListDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionListDTO>();
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<AspiranteListDTO,Aspirante>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<AspiranteCreateDTO,Aspirante>();
            //CreateMap<Inscripcion,InscripcionListDTO>();
            //CreateMap<List<Inscripcion>,List<InscripcionListDTO>>();
            //CreateMap<List<Aspirante>,List<AspiranteListDTO>>();
        }
    }
}