using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class TeacherSubject
{
    public int TeacherSubjectId { get; set; }

    public int Teacher { get; set; }

    public int Subject { get; set; }

    public virtual Subject SubjectNavigation { get; set; } = null!;

    public virtual Teacher TeacherNavigation { get; set; } = null!;

    public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
}
