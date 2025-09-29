using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Autor>().HasData(
                new Autor { AutorId = 1, Nome = "Ana C. Silva", Email = "ana.silva@email.com" },
                new Autor { AutorId = 2, Nome = "Bruno Alves", Email = "bruno.alves@email.com" }
            );

            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    PostId = 1,
                    Titulo = "Primeiros Passos com C#",
                    Conteudo = "C# é uma linguagem poderosa e versátil...",
                    DataCriacao = new DateTime(2024, 1, 15),
                    AutorId = 1 
                },
                new Post
                {
                    PostId = 2,
                    Titulo = "POO em C# na Prática",
                    Conteudo = "A Programação Orientada a Objetos é fundamental...",
                    DataCriacao = new DateTime(2024, 2, 20),
                    AutorId = 1 
                },
                new Post
                {
                    PostId = 3,
                    Titulo = "Introdução ao ASP.NET Core",
                    Conteudo = "ASP.NET Core permite criar aplicações web modernas...",
                    DataCriacao = new DateTime(2024, 3, 10),
                    AutorId = 2 
                },
                 new Post
                 {
                     PostId = 4,
                     Titulo = "Consultas com LINQ",
                     Conteudo = "LINQ revolucionou a forma como consultamos dados...",
                     DataCriacao = new DateTime(2024, 5, 22),
                     AutorId = 2 
                 }
            );


        }
    }
}
