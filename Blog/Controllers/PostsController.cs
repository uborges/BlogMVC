using Blog.Data;
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
    }
}
