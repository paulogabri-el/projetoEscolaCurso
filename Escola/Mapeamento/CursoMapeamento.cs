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
            
            builder.Property(x => x.Descricao)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(x => x.QtdMaxAlunos)
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
        }
    }
}
