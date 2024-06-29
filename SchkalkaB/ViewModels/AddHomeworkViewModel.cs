using SchkalkaB.Models;

namespace SchkalkaB.ViewModels
{
    public class AddHomeworkViewModel
    {
        public int TimeTableId { get; set; }
        public string Homework { get; set; }

        public int DayOfWeek { get; set; }
        public int NumLesson { get; set; }
        public int Smena { get; set; }
        public string Cabinet { get; set; }
        public int TeacherSubject { get; set; }
        public int Class { get; set; }
    }
}
