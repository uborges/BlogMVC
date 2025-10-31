using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class AutoresController : Controller
    {
        private readonly BlogContext _context;

        public AutoresController(BlogContext context)
        {
            _context = context;
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autores.ToListAsync());
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,Nome,Email")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorId,Nome,Email")] Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
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
            return View(autor);
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.AutorId == id);
        }
    }
}
