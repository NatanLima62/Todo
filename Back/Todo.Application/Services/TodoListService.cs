using AutoMapper;
using Todo.Application.Contracts;
using Todo.Application.Dtos.TodoList;
using Todo.Application.Notifications;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class TodoListService : BaseService, ITodoListService
{
    private readonly ITodoListRepository _todoListRepository;
    public TodoListService(IMapper mapper, INotificator notificator, ITodoListRepository todoListRepository) : base(mapper, notificator)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<TodoListDto?> ObterPorId(int id)
    {
        var todoList = await _todoListRepository.ObterPorId(id);
        if (todoList != null)
            return Mapper.Map<TodoListDto>(todoList);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<TodoListDto?> Adicionar(AdicionarTodoListDto dto)
    {
        var todoList = Mapper.Map<TodoList>(dto);
        if (!await Validar(todoList))
        {
            return null;
        }

        if (await _todoListRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<TodoListDto>(todoList);
        }
        
        Notificator.Handle("Erro ao salvar o Todo");
        return null;
    }

    public async Task<TodoListDto?> Atualizar(int id, AtualizarTodoListDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não comferem");
            return null;
        }
        
        var todoList = await _todoListRepository.ObterPorId(id);
        if (todoList == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, todoList);
        if (!await Validar(todoList))
        {
            return null;
        }

        if (await _todoListRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<TodoListDto>(todoList);
        }
        
        Notificator.Handle("Erro ao atualizar o Todo");
        return null;
    }

    public async Task Remover(int id)
    {
        var todoList = await _todoListRepository.ObterPorId(id);
        if (todoList == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        _todoListRepository.Remover(todoList);
        if (!await _todoListRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Nao foi possível remover o Todo");
        }
    }
    
    private async Task<bool> Validar(TodoList todoList)
    {
        if (!todoList.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var todoListExistente = await _todoListRepository.FistOrDefault(u => u.Nome == todoList.Nome);
        if (todoListExistente != null)
        {
            Notificator.Handle("Já existe um Todo com o nome informado");
        }

        return !Notificator.HasNotification;
    }
}