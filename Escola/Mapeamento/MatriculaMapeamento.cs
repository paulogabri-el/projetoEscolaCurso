using Escola.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Mapeamento
{
    public class MatriculaMapeamento : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matricula");

            builder.Property(x => x.Ativa)
                .IsRequired();

            builder.HasOne(x => x.Aluno)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Curso)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(x => x.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(x => x.DataAlteracao)
                .HasColumnType("datetime2")
                .IsRequired(false);
        }
    }
}
