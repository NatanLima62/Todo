using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Todo.Application.Contracts;
using Todo.Application.Dtos.Administrador;
using Todo.Application.Notifications;
using Todo.Core.Enums;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class AdministradorService : BaseService, IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IFileService _fileService;
    private readonly IPasswordHasher<Administrador> _passwordHasher;

    public AdministradorService(IMapper mapper, INotificator notificator,  IFileService fileService, IPasswordHasher<Administrador> passwordHasher, IAdministradorRepository administradorRepository) : base(mapper,
        notificator)
    {
        _fileService = fileService;
        _passwordHasher = passwordHasher;
        _administradorRepository = administradorRepository;
    }

    public async Task<AdministradorDto?> ObterPorId(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador != null)
            return Mapper.Map<AdministradorDto>(administrador);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto)
    {
        
        if (dto.Foto != null && (dto.Foto.FileName.EndsWith(".jpg") || dto.Foto.FileName.EndsWith(".png") || dto.Foto.FileName.EndsWith(".jif")))
        {
            Notificator.Handle("Aceito apenas arquivos JPG, PNG ou JIF");
            return null;
        }
        
        var administrador = Mapper.Map<Administrador>(dto);
        if (!await Validar(administrador))
        {
            return null;
        }

        if (dto.Foto != null)
        {
            administrador.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotosUsuarios);
        }

        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);

        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.Handle("Erro ao salvar o administrador");
        return null;
    }

    public async Task<AdministradorDto?> Atualizar(int id, AtualizarAdministradorDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não comferem");
            return null;
        }
        
        if (dto.Foto != null && (dto.Foto.FileName.EndsWith(".jpg") || dto.Foto.FileName.EndsWith(".png") || dto.Foto.FileName.EndsWith(".jif")))
        {
            Notificator.Handle("Aceito apenas arquivos JPG, PNG ou JIF");
            return null;
        }

        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, administrador);
        if (!await Validar(administrador))
        {
            return null;
        }

        if (dto.Foto != null && !await ManterFoto(dto.Foto, administrador))
        {
            return null;
        }

        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);

        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.Handle("Erro ao atualizar o usuário");
        return null;
    }

    public async Task Remover(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        _administradorRepository.Remover(administrador);
        if (!await _administradorRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Nao foi possível remover o usuário");
        }
    }
    
    private async Task<bool> ManterFoto(IFormFile foto, Administrador administrador)
    {
        if (!string.IsNullOrWhiteSpace(administrador.Foto) && !_fileService.Apagar(new Uri(administrador.Foto)))
        {
            Notificator.Handle("Não foi possível remover a foto anterior.");
            return false;
        }

        administrador.Foto = await _fileService.Upload(foto, EUploadPath.FotosUsuarios);
        return true;
    }

    private async Task<bool> Validar(Administrador administrador)
    {
        if (!administrador.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var administradorExistente = await _administradorRepository.FistOrDefault(u => u.Email == administrador.Email);
        if (administradorExistente != null)
        {
            Notificator.Handle("Não foi possível cadastrar o administrador. Identificação em uso");
        }

        return !Notificator.HasNotification;
    }
}