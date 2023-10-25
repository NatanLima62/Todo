using AutoMapper;
using Todo.Application.Dtos.Administrador;
using Todo.Application.Dtos.Usuario;
using Todo.Domain.Entities;

namespace Todo.Application.Configurations.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UsuarioDto, Usuario>().ReverseMap();
        CreateMap<AdicionarUsuarioDto, Usuario>().ReverseMap();
        CreateMap<AtualizarUsuarioDto, Usuario>().ReverseMap();
        
        CreateMap<AdministradorDto, Administrador>().ReverseMap();
        CreateMap<AdicionarAdministradorDto, Administrador>().ReverseMap();
        CreateMap<Dtos.Administrador.AtualizarAdministradorDto, Administrador>().ReverseMap();
    }
}