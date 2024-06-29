using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
}
