using AutoMapper;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DatosPersonales, DatosPersonalesDto>()
                .ForMember(fk => fk.Usuarios, fk => fk.MapFrom(x => x.Usuarios))
                .ReverseMap()
                .PreserveReferences();

            CreateMap<Usuarios, UsuariosDto>()
                .ForMember(fk => fk.DatosPersonales, fk => fk.MapFrom(x => x.DatosPersonales))
                .ReverseMap()
                .PreserveReferences();

            CreateMap<Usuarios, AuthRegister>()
                .ReverseMap();

            CreateMap<DatosPersonales, AuthRegister>()
                .ReverseMap();

            CreateMap<Vehiculos, VehiculosDto>()
                .ForMember(fk => fk.Usuarios, fk => fk.Ignore())
                .ReverseMap()
                .PreserveReferences();

            CreateMap<Rutas, RutasDto>()
                .ForMember(fk => fk.Usuarios, fk => fk.Ignore())
                .ForMember(fk => fk.Vehiculos, fk => fk.Ignore())
                .ReverseMap()
                .PreserveReferences();

            //CreateMap<RutasDto, Rutas>()
            //    .ForMember(fk => fk.Usuarios, fk => fk.MapFrom(x => x.Usuarios))
            //    .ForMember(fk => fk.Vehiculos, fk => fk.MapFrom(x => x.Vehiculos))
            //    .ReverseMap();
        }
    }
}
