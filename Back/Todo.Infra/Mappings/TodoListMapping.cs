using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Entities;

namespace Todo.Infra.Mappings;

public class TodoListMapping : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
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
            .HasOne(tl => tl.Usuario)
            .WithMany(u => u.Todos)
            .HasForeignKey(tl => tl.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}