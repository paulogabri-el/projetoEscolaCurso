using Escola.Mapeamento;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Escola.Models
{
    public class EscolaContext : IdentityDbContext
    {
        public EscolaContext(DbContextOptions<EscolaContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfiguration(new AlunoMapeamento());
            modelBuilder.ApplyConfiguration(new CursoMapeamento());
            modelBuilder.ApplyConfiguration(new MatriculaMapeamento());

        }

        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
    }
}
