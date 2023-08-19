using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Colors
{
    public interface IColor
    {
        string Name { get; }

        byte Red { get; }
        byte Green { get; }
        byte Blue { get; }
    }
}
