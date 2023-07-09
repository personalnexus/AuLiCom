﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public interface IVersioned
    {
        int Version { get; }
        event EventHandler<VersionChangedEventArgs> VersionChanged;
    }
}
