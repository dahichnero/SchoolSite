using SchkalkaB.Models;

namespace SchkalkaB.ViewModels
{
    public class TimetableViewModel
    {
        public Student Student { get; set; }
        public TimeTable TimeTable { get; set; }
        public Parent Mother { get; set; }
        public Parent Father { get; set; }
        public Class Class { get; set; }
        public List<TimeTable> timeTables { get; set; }

        public Teacher Teacher { get; set; }
        public List<TimeTable> FirstSmena { get; set; }
        public List<TimeTable> LastSmena { get;  set; }

        public int TimeTableId { get; set; }
        public string? Homework { get; set; }
        public int NumLesson { get; set; }
        public int Smena {  get; set; }
        public Subject Subject { get; set; }

        public List<Event> Events { get; set; }

        public Director Director { get; set; }

        public List<Class> Classes { get; set; }

        public List<TimeTable> AllTimetables { get; set; }

        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Director> Directors { get; set; }

        public DateTime DateMy { get; set; }
    }
}
