using SchkalkaB.Models;
using System.Linq.Expressions;

namespace SchkalkaB.Domain.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindWhere(Expression<Func<T, bool>> predicate);

        Task<T> FindWhereOne(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

        Task<List<TimeTable>> FindTimeTable(Expression<Func<TimeTable, bool>> predicate);

        Task<List<Student>> FindStudents();

        Task<List<Teacher>> FindTeachers();
        Task<List<Director>> FindDirectors();

        //Task<List<TimeTable>> FindTimeTableTeacher(Expression<Func<TimeTable, bool>> predicate);

    }
}
