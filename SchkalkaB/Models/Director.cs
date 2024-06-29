using System;
using System.Collections.Generic;

namespace SchkalkaB.Models;

public partial class Director
{
    public int DirectorId { get; set; }

    public string DirectorName { get; set; } = null!;

    public string DirectorLastName { get; set; } = null!;

    public string DirectorSurName { get; set; } = null!;

    public int UserI { get; set; }

    public int Status { get; set; }

    public DateOnly Birth { get; set; }

    public string? Image { get; set; }

    public int Gender { get; set; }

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual User UserINavigation { get; set; } = null!;
}
