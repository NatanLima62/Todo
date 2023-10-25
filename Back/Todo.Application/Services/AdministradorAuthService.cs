using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using Todo.Application.Contracts;
using Todo.Application.Dtos;
using Todo.Application.Dtos.Administrador.Auth;
using Todo.Application.Notifications;
using Todo.Core.Enums;
using Todo.Core.Extensions;
using Todo.Core.Settings;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;

namespace Todo.Application.Services;

public class AdministradorAuthService : BaseService, IAdministradorAuthService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AdministradorAuthService(IMapper mapper, INotificator notificator,
        IAdministradorRepository administradorRepository, IPasswordHasher<Administrador> passwordHasher, IJwtService jwtService, IOptions<JwtSettings> jwtSettings) : base(mapper,
        notificator)
    {
        _administradorRepository = administradorRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenDto?> Login(AdministradorLoginDto administradorLogin)
    {
        var administrador = await _administradorRepository.ObterPorEmail(administradorLogin.Email);
        if (administrador is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(administrador, administrador.Senha, administradorLogin.Senha);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new TokenDto
        {
            Token = await GerarToken(administrador)
        };
    }

    private async Task<string> GerarToken(Administrador administrador)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, administrador.Nome));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, administrador.Email));
        claimsIdentity.AddClaim(new Claim("TipoUsuario", ETipoUsuario.Administrador.ToDescriptionString()));

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