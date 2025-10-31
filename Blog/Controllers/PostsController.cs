using Blog.Data;
using Blog.Models;
using BlogMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: /Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            // Passa a lista de autores para a View para popular o dropdown (Select List)
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nome");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Titulo,Conteudo,AutorId")] Post post)
        {
            // Remove a validação da DataCriacao, pois ela é preenchida automaticamente no modelo
            // e não deve ser enviada do formulário.
            ModelState.Remove("DataCriacao"); 

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Se a validação falhar, retorna a View com os dados e a lista de autores
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nome", post.AutorId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nome", post.AutorId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Titulo,Conteudo,DataCriacao,AutorId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            // Remove a validação da DataCriacao, pois ela é preenchida automaticamente no modelo
            // e não deve ser enviada do formulário.
            ModelState.Remove("DataCriacao");

            if (ModelState.IsValid)
            {
                try
                {
                    // Garante que a DataCriacao original seja mantida ao editar
                    var postOriginal = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId == id);
                    if (postOriginal != null)
                    {
                        post.DataCriacao = postOriginal.DataCriacao;
                    }
                    
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nome", post.AutorId);
            return View(post);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
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

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // NOVO MÉTODO
        // GET: /Posts/AutoresProlificos?anoReferencia=2024&limitePosts=1
        public async Task<IActionResult> AutoresProlificos(int? anoReferencia, int? limitePosts)
        {
            // === CONSULTA LINQ 3: Filtro principal (WHERE) e de grupo (HAVING) ===
            // Encontrar autores que publicaram mais de N posts no ano X.

            // Define valores padrão se não forem fornecidos
            int ano = anoReferencia ?? DateTime.Now.Year;
            int limite = limitePosts ?? 1;

            var autoresProlificos = await _context.Posts
                .Where(p => p.DataCriacao.Year == ano) // Filtro principal (SQL WHERE)
                .GroupBy(p => p.Autor.Nome)                        // Agrupamento (SQL GROUP BY)
                .Select(grupo => new AutorEstatisticaViewModel
                {
                    NomeAutor = grupo.Key,
                    ContagemPosts = grupo.Count()
                })
                .Where(resultado => resultado.ContagemPosts > limite) // Filtro de grupo (SQL HAVING)
                .ToListAsync();

            // Passa os parâmetros para a View para exibir um título dinâmico
            ViewData["Ano"] = ano;
            ViewData["Limite"] = limite;

            return View(autoresProlificos);
        }

    }
}
