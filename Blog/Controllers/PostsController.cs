using Blog.Data;
using BlogMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogContext _context;

        // O BlogContext é injetado automaticamente pelo ASP.NET Core
        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: /Posts ou /Posts/Index
        public async Task<IActionResult> Index()
        {
            // === CONSULTA LINQ 1: Consulta com dados de duas classes (JOIN) ===
            // Busca todos os posts e inclui os dados do Autor relacionado.
            // O Include() gera um JOIN no SQL para evitar múltiplas consultas ao banco (problema N+1).
            var posts = await _context.Posts
                                      .Include(p => p.Autor) // Inclui o Autor
                                      .OrderByDescending(p => p.DataCriacao) // Ordena pelos mais recentes
                                      .ToListAsync();

            // Envia a lista de posts para a View
            return View(posts);
        }

        // GET: /Posts/AutoresEstatisticas
        public async Task<IActionResult> AutoresEstatisticas()
        {
            // === CONSULTA LINQ 2: Consulta com função de grupo (GROUP BY) ===
            // Contar quantos posts cada autor escreveu.
            var estatisticas = await _context.Autores
                .Select(autor => new AutorEstatisticaViewModel
                {
                    NomeAutor = autor.Nome,
                    ContagemPosts = autor.Posts.Count()
                })
                .Where(e => e.ContagemPosts > 0) // Apenas autores que têm posts
                .OrderByDescending(e => e.ContagemPosts)
                .ToListAsync();

            return View(estatisticas);
        }

        // NOVO MÉTODO
        // GET: /Posts/AutoresProlificos
        public async Task<IActionResult> AutoresProlificos()
        {
            // === CONSULTA LINQ 3: Filtro principal (WHERE) e de grupo (HAVING) ===
            // Encontrar autores que publicaram mais de 1 post no ano de 2024.

            var anoReferencia = 2024;
            var limitePosts = 1;

            var autoresProlificos = await _context.Posts
                .Where(p => p.DataCriacao.Year == anoReferencia) // Filtro principal (SQL WHERE)
                .GroupBy(p => p.Autor.Nome)                        // Agrupamento (SQL GROUP BY)
                .Select(grupo => new AutorEstatisticaViewModel
                {
                    NomeAutor = grupo.Key,
                    ContagemPosts = grupo.Count()
                })
                .Where(resultado => resultado.ContagemPosts > limitePosts) // Filtro de grupo (SQL HAVING)
                .ToListAsync();

            // Passa os parâmetros para a View para exibir um título dinâmico
            ViewData["Ano"] = anoReferencia;
            ViewData["Limite"] = limitePosts;

            return View(autoresProlificos);
        }

    }
}
