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
                //.ForMember(fk => fk.Usuario, fk => fk.MapFrom(x => x.Usuario))
                .ReverseMap();

            CreateMap<Usuarios, UsuariosDto>()
                //.ForMember(fk => fk.DatosPersonales, fk => fk.MapFrom(x => x.DatosPersonales))
                .ReverseMap();

            CreateMap<Usuarios, AuthRegister>()
                .ReverseMap();

            CreateMap<DatosPersonales, AuthRegister>()
                .ReverseMap();
        }
    }
}
