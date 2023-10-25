using AutoMapper;
using Todo.Application.Contracts;
using Todo.Application.Dtos.TodoTask;
using Todo.Application.Notifications;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class TodoTaskService : BaseService, ITodoTaskService
{
    private readonly ITodoTaskRepository _todoTaskRepository;
    public TodoTaskService(IMapper mapper, INotificator notificator, ITodoTaskRepository todoTaskRepository) : base(mapper, notificator)
    {
        _todoTaskRepository = todoTaskRepository;
    }

    public async Task<TodoTaskDto?> ObterPorId(int id)
    {
        var todoTask = await _todoTaskRepository.ObterPorId(id);
        if (todoTask != null)
            return Mapper.Map<TodoTaskDto>(todoTask);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<TodoTaskDto?> Adicionar(AdicionarTodoTaskDto dto)
    {
        var todoTask = Mapper.Map<TodoTask>(dto);
        if (!await Validar(todoTask))
        {
            return null;
        }

        if (await _todoTaskRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<TodoTaskDto>(todoTask);
        }
        
        Notificator.Handle("Erro ao salvar a Task");
        return null;
    }

    public async Task<TodoTaskDto?> Atualizar(int id, AtualizarTodoTaskDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não comferem");
            return null;
        }
        
        var todoTask = await _todoTaskRepository.ObterPorId(id);
        if (todoTask == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, todoTask);
        if (!await Validar(todoTask))
        {
            return null;
        }

        if (await _todoTaskRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<TodoTaskDto>(todoTask);
        }
        
        Notificator.Handle("Erro ao atualizar a Task");
        return null;
    }

    public async Task Remover(int id)
    {
        var todoTask = await _todoTaskRepository.ObterPorId(id);
        if (todoTask == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        _todoTaskRepository.Remover(todoTask);
        if (!await _todoTaskRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Nao foi possível remover a Task");
        }
    }
    
    private async Task<bool> Validar(TodoTask todoTask)
    {
        if (!todoTask.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var todoTaskExistente = await _todoTaskRepository.FistOrDefault(u => u.Nome == todoTask.Nome && u.TodoId == todoTask.TodoId);
        if (todoTaskExistente != null)
        {
            Notificator.Handle("Já existe uma Task com o nome informado");
        }

        return !Notificator.HasNotification;
    }
}