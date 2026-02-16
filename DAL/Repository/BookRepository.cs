using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class BookRepository(LibraryContext context) : IRepository<Book>
{
    public void Add(Book entity)
    {
        context.Books.Add(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = context.Books.Find(id);
        if (book != null)
        {
            context.Books.Remove(book);
            context.SaveChanges();
        }
    }

    public void Update(Book entity)
    {
        var existingBook = context.Books.Find(entity.Id);
        if (existingBook != null)
        {
            existingBook.Title = entity.Title;
            existingBook.Author = entity.Author;
            existingBook.PublicationYear = entity.PublicationYear;
            existingBook.UserId = entity.UserId;

            context.SaveChanges();
        }
    }

    public IEnumerable<Book> Find(Func<Book, bool> predicate) =>
        context.Books.Include(b => b.User).Where(predicate).ToList();

    public Book? Get(int id) =>
        context.Books.Include(b => b.User).FirstOrDefault(b => b.Id == id);

    public IEnumerable<Book> GetAll() =>
        context.Books.Include(b => b.User).ToList();
}
