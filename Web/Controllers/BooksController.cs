using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

public class BooksController(IRepository<Book> bookRepository, IRepository<User> userRepository) : Controller
{
    public IActionResult Index()
    {
        var books = bookRepository.GetAll();
        return View(books);
    }

    public IActionResult Details(int id)
    {
        var book = bookRepository.Get(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    public IActionResult Create()
    {
        ViewData["UserId"] = new SelectList(userRepository.GetAll(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Title,Author,PublicationYear,UserId")] Book book)
    {
        if (ModelState.IsValid)
        {
            bookRepository.Add(book);
            return RedirectToAction(nameof(Index));
        }
        ViewData["UserId"] = new SelectList(userRepository.GetAll(), "Id", "Name", book.UserId);
        return View(book);
    }

    public IActionResult Edit(int id)
    {
        var book = bookRepository.Get(id);
        if (book == null)
        {
            return NotFound();
        }
        ViewData["UserId"] = new SelectList(userRepository.GetAll(), "Id", "Name", book.UserId);
        return View(book);
    }

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
            try
            {
                bookRepository.Update(book);
            }
            catch (Exception)
            {
                if (bookRepository.Get(book.Id) == null)
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
        ViewData["UserId"] = new SelectList(userRepository.GetAll(), "Id", "Name", book.UserId);
        return View(book);
    }

    public IActionResult Delete(int id)
    {
        var book = bookRepository.Get(id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        bookRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
