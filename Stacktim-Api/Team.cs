﻿using System;
using System.Collections.Generic;

namespace Stacktim;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public int CaptainId { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Player Captain { get; set; } = null!;
}
