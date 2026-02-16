using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;

        public BooksController(IRepository<Book> bookRepository, IRepository<User> userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll());
        }

        // GET: Books/Details/5
        public IActionResult Details(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Author,PublicationYear,UserId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Add(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Name", book.UserId);
            return View(book);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Name", book.UserId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Author,PublicationYear,UserId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bookRepository.Update(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Name", book.UserId);
            return View(book);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
