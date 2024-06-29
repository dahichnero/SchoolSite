using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string NameEvent { get; set; } = null!;

    public int Teacher { get; set; }

    public int Smena { get; set; }

    public int NumLesson { get; set; }

    public DateOnly Date { get; set; }

    public virtual Teacher TeacherNavigation { get; set; } = null!;
}
