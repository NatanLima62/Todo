using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Entities;

namespace Todo.Infra.Mappings;

public class TodoTaskMapping : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder
            .Property(u => u.Nome)
            .HasMaxLength(80)
            .IsRequired();
        
        builder
            .Property(u => u.Descricao)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(u => u.Ativo)
            .HasDefaultValue(true);
        
        builder
            .HasOne(tt => tt.Usuario)
            .WithMany(u => u.Tasks)
            .HasForeignKey(tl => tl.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(tt => tt.Todo)
            .WithMany(tl => tl.Tasks)
            .HasForeignKey(tl => tl.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}