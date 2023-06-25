using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    internal interface IConfigurableFixture: IFixture
    {
        new string Name { get; set; }
        new int StartChannel { get; set; }
    }
}
