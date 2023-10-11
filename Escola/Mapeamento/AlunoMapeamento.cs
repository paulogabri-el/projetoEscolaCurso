using Escola.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Escola.Mapeamento
{
    public class AlunoMapeamento : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Aluno");

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.DataNascimento)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.CPF)
                .HasColumnType("varchar(15)")
                .IsRequired();

            builder.HasIndex(x => x.CPF)
                .IsUnique();

            builder.Property(x => x.DataCriacao)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(x => x.DataAlteracao)
                .HasColumnType("datetime2")
                .IsRequired(false);

        }
    }
}
