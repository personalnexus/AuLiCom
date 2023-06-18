using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    internal interface IFixture
    {
        string Name { get; }
        void SetValue(ChannelValue relativeChannelValue);
    }
}
