using AutoMapper;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DatosPersonales, DatosPersonalesDto>().ReverseMap();
            CreateMap<Usuarios, UsuariosDto>().ReverseMap();
        }
    }
}
