using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherName { get; set; } = null!;

    public string TeacherLastName { get; set; } = null!;

    public string TeacherSurName { get; set; } = null!;

    public int Userl { get; set; }

    public DateOnly Birth { get; set; }

    public string? Image { get; set; }

    public int Gender { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

    public virtual User UserlNavigation { get; set; } = null!;
}
