using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext context;

        public AuthorsController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Author> items = await context.Authors.Include(x => x.Books).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            Author item = await context.Authors.Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.AuthorId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AuthorForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Author item = new Author();
            item.Name = model.Name;
            context.Authors.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Author item = await context.Authors.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            AuthorForm model = new AuthorForm();
            model.Name = item.Name;
            ViewBag.ItemId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorForm model)
        {
            ViewBag.ItemId = id;
            Author item = await context.Authors.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            item.Name = model.Name;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Author item = await context.Authors.Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.AuthorId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Author item = await context.Authors.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            // Связанные книги удаляются базой автоматически.
            context.Authors.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
