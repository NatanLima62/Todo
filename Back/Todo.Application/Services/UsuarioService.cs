using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Todo.Application.Contracts;
using Todo.Application.Dtos.Usuario;
using Todo.Application.Notifications;
using Todo.Core.Enums;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class UsuarioService : BaseService, IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IFileService _fileService;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(IMapper mapper, INotificator notificator, IUsuarioRepository usuarioRepository, IFileService fileService, IPasswordHasher<Usuario> passwordHasher) : base(mapper,
        notificator)
    {
        _usuarioRepository = usuarioRepository;
        _fileService = fileService;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioDto?> ObterPorId(int id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario != null)
            return Mapper.Map<UsuarioDto>(usuario);

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto dto)
    {
        
        if (dto.Foto != null && (dto.Foto.FileName.EndsWith(".jpg") || dto.Foto.FileName.EndsWith(".png") || dto.Foto.FileName.EndsWith(".jif")))
        {
            Notificator.Handle("Aceito apenas arquivos JPG, PNG ou JIF");
            return null;
        }
        
        var usuario = Mapper.Map<Usuario>(dto);
        if (!await Validar(usuario))
        {
            return null;
        }

        if (dto.Foto != null)
        {
            usuario.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotosUsuarios);
        }

        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);

        if (await _usuarioRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<UsuarioDto>(usuario);
        }
        
        Notificator.Handle("Erro ao salvar o usuário");
        return null;
    }

    public async Task<UsuarioDto?> Atualizar(int id, AtualizarUsuarioDto dto)
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

        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, usuario);
        if (!await Validar(usuario))
        {
            return null;
        }

        if (dto.Foto != null && !await ManterFoto(dto.Foto, usuario))
        {
            return null;
        }

        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);

        if (await _usuarioRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<UsuarioDto>(usuario);
        }
        
        Notificator.Handle("Erro ao atualizar o usuário");
        return null;
    }

    public async Task Remover(int id)
    {
        var usuario = await _usuarioRepository.ObterPorId(id);
        if (usuario == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        _usuarioRepository.Remover(usuario);
        if (!await _usuarioRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Nao foi possível remover o usuário");
        }
    }
    
    private async Task<bool> ManterFoto(IFormFile foto, Usuario usuario)
    {
        if (!string.IsNullOrWhiteSpace(usuario.Foto) && !_fileService.Apagar(new Uri(usuario.Foto)))
        {
            Notificator.Handle("Não foi possível remover a foto anterior.");
            return false;
        }

        usuario.Foto = await _fileService.Upload(foto, EUploadPath.FotosUsuarios);
        return true;
    }

    private async Task<bool> Validar(Usuario usuario)
    {
        if (!usuario.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var usuarioExistente = await _usuarioRepository.FistOrDefault(u => u.Email == usuario.Email);
        if (usuarioExistente != null)
        {
            Notificator.Handle("Não foi possível cadastrar o usuário. Identificação em uso");
        }

        return !Notificator.HasNotification;
    }
}