using System;
using System.Collections.Generic;

namespace Stacktim.Models;

public partial class TeamsPlayer
{
    public int PlayerId { get; set; }

    public int TeamId { get; set; }

    public int? Role { get; set; }

    public DateOnly? JoinDate { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
