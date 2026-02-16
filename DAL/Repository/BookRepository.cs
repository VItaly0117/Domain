using DAL.Context;
using Domain.Models;

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
            

            context.SaveChanges();
        }
    }

    public IEnumerable<Book> Find(Func<Book, bool> predicate) =>
        context.Books.Where(predicate).ToList();

    public Book? Get(int id) =>
        context.Books.Find(id);

    public IEnumerable<Book> GetAll() =>
        context.Books.ToList();
}
