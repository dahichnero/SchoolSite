using Microsoft.EntityFrameworkCore;
using SchkalkaB.Data;
using SchkalkaB.Domain.Services;
using SchkalkaB.Models;
using System.Linq.Expressions;

namespace SchkalkaB.Infrastructure
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly SchkalkaDbContext context;

        public EFRepository(SchkalkaDbContext context)
        {
            this.context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> FindAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> FindWhere(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> FindWhereOne(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TimeTable>> FindTimeTable(Expression<Func<TimeTable,bool>> predicate)
        {
            return await context.TimeTables.Include(x=>x.ClassNavigation).Include(z=>z.TeacherSubjectNavigation).ThenInclude(c=>c.SubjectNavigation).Where(predicate).ToListAsync();
        }

        public async Task<List<Student>> FindStudents()
        {
            return await context.Students.Include(z=>z.ClassNavigation).Include(z=>z.UserINavigation).ToListAsync();
        }

        public async Task<List<Teacher>> FindTeachers()
        {
            return await context.Teachers.Include(z=>z.UserlNavigation).ToListAsync();
        }

        public async Task<List<Director>> FindDirectors()
        {
            return await context.Directors.Include(z =>z.StatusNavigation).Include(z => z.UserINavigation).ToListAsync();
        }
    }
}
