namespace DAL.Repository;

public interface IRepository<T> where T : class
{
    T? Get(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Func<T, bool> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}