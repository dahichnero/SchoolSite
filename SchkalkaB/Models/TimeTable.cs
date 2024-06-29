using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class TimeTable
{
    public int TimeTableId { get; set; }

    public int DayOfWeek { get; set; }

    public int NumLesson { get; set; }

    public int Smena { get; set; }

    public int TeacherSubject { get; set; }

    public int Class { get; set; }

    public string Cabinet { get; set; } = null!;

    public string? HomeWork { get; set; }

    public virtual Class ClassNavigation { get; set; } = null!;

    public virtual DayOfWeek DayOfWeekNavigation { get; set; } = null!;

    public virtual TeacherSubject TeacherSubjectNavigation { get; set; } = null!;
}
