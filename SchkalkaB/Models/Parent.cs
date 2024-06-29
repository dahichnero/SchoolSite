using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Parent
{
    public int ParentsId { get; set; }

    public string NameParent { get; set; } = null!;

    public string LastNameParent { get; set; } = null!;

    public string SurNameParent { get; set; } = null!;

    public int StatusParent { get; set; }

    public virtual StatusParent StatusParentNavigation { get; set; } = null!;

    public virtual ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();
}
