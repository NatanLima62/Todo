using AutoMapper;
using Todo.Application.Dtos.Administrador;
using Todo.Application.Dtos.TodoList;
using Todo.Application.Dtos.TodoTask;
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
        CreateMap<AtualizarAdministradorDto, Administrador>().ReverseMap();
        
        CreateMap<TodoTaskDto, TodoTask>().ReverseMap();
        CreateMap<AdicionarTodoTaskDto, TodoTask>().ReverseMap();
        CreateMap<AtualizarTodoTaskDto, TodoTask>().ReverseMap();
        
        CreateMap<TodoListDto, TodoList>().ReverseMap();
        CreateMap<AdicionarTodoListDto, TodoList>().ReverseMap();
        CreateMap<AtualizarTodoListDto, TodoList>().ReverseMap();
    }
}