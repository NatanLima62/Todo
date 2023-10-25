using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using Todo.Application.Contracts;
using Todo.Application.Dtos;
using Todo.Application.Dtos.Usuario.Auth;
using Todo.Application.Notifications;
using Todo.Core.Enums;
using Todo.Core.Extensions;
using Todo.Core.Settings;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class UsuarioAuthService : BaseService, IUsuarioAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public UsuarioAuthService(IMapper mapper, INotificator notificator,
        IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher, IJwtService jwtService, IOptions<JwtSettings> jwtSettings) : base(mapper,
        notificator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(UsuarioLoginDto login)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(login.Email);
        if (usuario is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, login.Senha);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new TokenDto
        {
            Token = await GerarToken(usuario)
        };
    }

    private async Task<string> GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
        claimsIdentity.AddClaim(new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString()));

        var key = await _jwtService.GetCurrentSigningCredentials();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Emissor,
            Audience = _jwtSettings.ComumValidoEm,
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }
}