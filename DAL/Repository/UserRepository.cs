using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserRepository(LibraryContext context) : IRepository<User>
{
    public void Add(User entity)
    {
        context.Users.Add(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = context.Users.Find(id);
        if (user != null)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }

    public void Update(User entity)
    {
        var existingUser = context.Users.Find(entity.Id);
        if (existingUser != null)
        {
            existingUser.Name = entity.Name;
            existingUser.Email = entity.Email;
            context.SaveChanges();
        }
    }

    public IEnumerable<User> Find(Func<User, bool> predicate) =>
        context.Users.Where(predicate).ToList();

    public User? Get(int id) =>
        context.Users.Include(u => u.Books).FirstOrDefault(u => u.Id == id);

    public IEnumerable<User> GetAll() =>
        context.Users.ToList();
}
