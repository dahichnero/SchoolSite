using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string SurName { get; set; } = null!;

    public int UserI { get; set; }

    public int Class { get; set; }

    public DateOnly Birth { get; set; }

    public string? Image { get; set; }

    public int Gender { get; set; }

    public virtual Class ClassNavigation { get; set; } = null!;

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();

    public virtual User UserINavigation { get; set; } = null!;
}
