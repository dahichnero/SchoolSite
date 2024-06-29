using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class StudentParent
{
    public int StudentParentId { get; set; }

    public int Student { get; set; }

    public int Parent { get; set; }

    public virtual Parent ParentNavigation { get; set; } = null!;

    public virtual Student StudentNavigation { get; set; } = null!;
}
