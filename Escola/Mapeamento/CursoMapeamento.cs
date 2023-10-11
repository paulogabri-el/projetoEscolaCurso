using Escola.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Mapeamento
{
    public class CursoMapeamento : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("Curso");

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(x => x.QtdMaxAlunos)
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(x => x.DataAlteracao)
                .HasColumnType("datetime2")
                .IsRequired(false);
        }
    }
}
