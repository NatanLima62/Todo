using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Entities;

namespace Todo.Infra.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder
            .Property(u => u.Nome)
            .HasMaxLength(80)
            .IsRequired();
        
        builder
            .Property(u => u.Email)
            .HasMaxLength(120)
            .IsRequired();
        
        builder
            .Property(u => u.Senha)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .Property(u => u.Foto)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}