using SchkalkaB.Models;

namespace SchkalkaB.Domain.Services
{
    public interface ITimetable
    {
        Task<List<TimeTable>> GetAllTimeTableAsync();
        Task<List<TimeTable>> TimeTableStudentAsync(int dayAdd);

        Task<List<TimeTable>> TimeTableDirectorAsync(int dayAdd);

        Task<List<TimeTable>> TimeTableTeacherAsync(int dayAdd, int smena);
        Task<Student> GetStudentAsync();
        Task<Parent> GetMotherAsync();
        Task<Parent> GetFatherAsync();

        Task<Class> GetClassAsync();

        Task<Teacher> GetTeacherAsync();

        Task<TimeTable> FindTimeTableAsync(int id);

        Task<Student> FindStudentAsync(int id);
        Task<Teacher> FindTeacherAsync(int id);
        Task<Director> FindDirectorAsync(int id);

        Task<Event> FindEventAsync(int id);

        Task<List<Event>> GetEventsAsync();

        Task<List<Event>> GetAllEventsAsync();
        

        Task<List<Gender>> GetGendersAsync();
        Task<List<User>> GetUsersAsync();

        Task<List<Class>> GetClassesAsync();

        Task<List<Status>> GetStatusesAsync();

        Task<List<Class>> GetAllClassesAsync();
        Task<Director> GetDirectorAsync();
        Task<List<Teacher>> GetTeachersAsync();

        Task<List<Student>> GetStudentsAsync();
        Task<List<Teacher>> GetTeacherssAsync();
        Task<List<Director>> GetDirectorsAsync();

    }
}
