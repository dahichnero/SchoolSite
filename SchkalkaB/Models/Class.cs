using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string NameClass { get; set; } = null!;

    public int ClassRuk { get; set; }

    public virtual Teacher ClassRukNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
}
