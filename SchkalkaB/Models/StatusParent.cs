using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class StatusParent
{
    public int StatusParentsId { get; set; }

    public string StatusParentsName { get; set; } = null!;

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
}
