using SchkalkaB.Data;
using SchkalkaB.Domain.Services;
using SchkalkaB.Models;
using System;
using System.Text.Json;

namespace SchkalkaB.Infrastructure
{
    public class TimeTableSearch : ITimetable
    {
        private readonly IRepository<TimeTable> timetables;
        private readonly IRepository<Student> students;
        private readonly IRepository<Parent> parents;
        private readonly IRepository<Class> classes;
        private readonly IRepository<Teacher> teachers;
        private readonly IRepository<Event> events;
        private readonly IRepository<User> users;
        private readonly IRepository<Gender> genders;
        private readonly IRepository<Status> statuses;
        private readonly IRepository<Director> directors;
        SchkalkaDbContext context =new SchkalkaDbContext();

        public TimeTableSearch(IRepository<TimeTable> timetables, IRepository<Student> students, IRepository<Parent> parents, IRepository<Class> classes, IRepository<Teacher> teachers, IRepository<Event> events, IRepository<User> users, IRepository<Gender> genders, IRepository<Status> statuses, IRepository<Director> directors)
        {
            this.timetables = timetables;
            this.students = students;
            this.parents = parents;
            this.classes = classes;
            this.teachers = teachers;
            this.events = events;
            this.users = users;
            this.genders = genders;
            this.statuses = statuses;
            this.directors = directors;
        }

        public async Task<Director> FindDirectorAsync(int id)
        {
            return await directors.FindAsync(id);
        }

        public async Task<Event> FindEventAsync(int id)
        {
            return await events.FindAsync(id);
        }

        public async Task<Student> FindStudentAsync(int id)
        {
            return await students.FindAsync(id);
        }

        public async Task<Teacher> FindTeacherAsync(int id)
        {
            return await teachers.FindAsync(id);
        }

        public async Task<TimeTable> FindTimeTableAsync(int id)
        {
            return await timetables.FindAsync(id);
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await classes.GetAllAsync();
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await events.GetAllAsync();
        }

        public async Task<List<TimeTable>> GetAllTimeTableAsync()
        {
            return await timetables.GetAllAsync();
        }

        public async Task<Class> GetClassAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Student st = context.Students.FirstOrDefault(z => z.UserI == user.UserId);
            return await classes.FindWhereOne(x => x.ClassId == st.Class);
        }

        public async Task<List<Class>> GetClassesAsync()
        {
            return await classes.GetAllAsync();
        }

        public async Task<Director> GetDirectorAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            return await directors.FindWhereOne(x => x.UserI == user.UserId);
        }

        public async Task<List<Director>> GetDirectorsAsync()
        {
            return await directors.FindDirectors();
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            DateOnly date = DateOnly.Parse(DateTime.Today.ToShortDateString());
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Teacher teacher = context.Teachers.FirstOrDefault(z => z.Userl == user.UserId);
            return await events.FindWhere(z=>z.Teacher==teacher.TeacherId && z.Date>=date);
        }

        public async Task<Parent> GetFatherAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Student st = context.Students.FirstOrDefault(z => z.UserI == user.UserId);
            StudentParent stP=context.StudentParents.FirstOrDefault(x=>x.Student==st.StudentId && x.ParentNavigation.StatusParent==1);
            return await parents.FindWhereOne(x => x.ParentsId == stP.Parent);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await genders.GetAllAsync();
        }

        public async Task<Parent> GetMotherAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Student st = context.Students.FirstOrDefault(z => z.UserI == user.UserId);
            StudentParent stP = context.StudentParents.FirstOrDefault(x => x.Student == st.StudentId && x.ParentNavigation.StatusParent == 2);
            return await parents.FindWhereOne(x => x.ParentsId == stP.Parent);
        }

        public async Task<List<Status>> GetStatusesAsync()
        {
            return await statuses.GetAllAsync();
        }

        public async Task<Student> GetStudentAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            return await students.FindWhereOne(x => x.UserI == user.UserId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await students.FindStudents();
        }

        public async Task<Teacher> GetTeacherAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            return await teachers.FindWhereOne(x => x.Userl == user.UserId);
        }

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            return await teachers.GetAllAsync();
        }

        public async Task<List<Teacher>> GetTeacherssAsync()
        {
            return await teachers.FindTeachers();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await users.GetAllAsync();
        }

        public async Task<List<TimeTable>> TimeTableDirectorAsync(int dayAdd)
        {
            int day =(int)DateTime.Today.AddDays(dayAdd).DayOfWeek;
            if (day == 0)
            {
                day = 7;
            }
            return await timetables.FindTimeTable(z=>z.DayOfWeek== day);
        }

        public async Task<List<TimeTable>> TimeTableStudentAsync(int dayAdd)
        {
            User user;
            int day=(int)DateTime.Today.AddDays(dayAdd).DayOfWeek;
            if (day == 0)
            {
                day = 7;
            }
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Student st = context.Students.FirstOrDefault(z => z.UserI == user.UserId);
            return await timetables.FindTimeTable(x => x.Class == st.Class && x.DayOfWeek==day);
        }

        public async Task<List<TimeTable>> TimeTableTeacherAsync(int dayAdd, int smena)
        {
            User user;
            int day = (int)DateTime.Today.AddDays(dayAdd).DayOfWeek;
            if (day == 0)
            {
                day = 7;
            }
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            Teacher teacher = context.Teachers.FirstOrDefault(z => z.Userl == user.UserId);
            return await timetables.FindTimeTable(x=>x.TeacherSubjectNavigation.Teacher==teacher.TeacherId && x.DayOfWeek == day && x.Smena==smena);
        }
    }
}
