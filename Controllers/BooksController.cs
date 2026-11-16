using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext context;

        public BooksController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> items = await context.Books.Include(x => x.Author).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            Book item = await context.Books.Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.BookId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadAuthors();
            return View(new BookForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookForm model)
        {
            if (!await context.Authors.AnyAsync(a => a.AuthorId == model.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Выберите существующего автора.");
            }
            if (!ModelState.IsValid)
            {
                await LoadAuthors(model.AuthorId);
                return View(model);
            }
            Book item = new Book();
            item.Title = model.Title;
            item.AuthorId = model.AuthorId;
            context.Books.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Book item = await context.Books.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            BookForm model = new BookForm();
            model.Title = item.Title;
            model.AuthorId = item.AuthorId;
            await LoadAuthors(item.AuthorId);
            ViewBag.ItemId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookForm model)
        {
            ViewBag.ItemId = id;
            Book item = await context.Books.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            if (!await context.Authors.AnyAsync(a => a.AuthorId == model.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Выберите существующего автора.");
            }
            if (!ModelState.IsValid)
            {
                await LoadAuthors(model.AuthorId);
                return View(model);
            }
            item.Title = model.Title;
            item.AuthorId = model.AuthorId;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Book item = await context.Books.Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.BookId == id);
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
            Book item = await context.Books.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            context.Books.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task LoadAuthors(int selectedId = 0)
        {
            List<Author> authors = await context.Authors.OrderBy(a => a.Name).ToListAsync();
            ViewBag.Authors = new SelectList(authors, "AuthorId", "Name", selectedId);
            ViewBag.HasAuthors = authors.Count > 0;
        }
    }
}
