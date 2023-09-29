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

            builder.Property(x => x.DataCriacao)
                .HasDefaultValueSql("getdate()");

            builder.HasOne(x => x.Aluno)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(m => m.AlunoId)
                .IsRequired();

            builder.HasOne(x => x.Curso)
                .WithMany(x => x.Matriculas)
                .HasForeignKey(x => x.CursoId)
                .IsRequired();
        }
    }
}
