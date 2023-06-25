using AuLiComLib.Common;
using AuLiComLib.Fixtures.Kinds;
using AuLiComLib.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public class FixtureKindJsonConverter : TypedJsonConverter<IFixture>
    {
        public FixtureKindJsonConverter(IConnection connection): base(connection)
        {
            Register<GenericLamp>();
            Register<CameoLedBar3Ch2>();
        }
    }
}
