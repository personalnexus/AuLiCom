﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public interface IReadOnlyUniverseProvider
    {
        IReadOnlyUniverse GetUniverseByName(string name);
    }
}
