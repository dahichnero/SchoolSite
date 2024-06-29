using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class DayOfWeek
{
    public int DayId { get; set; }

    public string DayOfWeekName { get; set; } = null!;

    public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
}
