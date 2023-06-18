using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    /// <summary>
    /// Type serialized from file, hence the writable and nullable properties
    /// </summary>
    public class FixtureDefinition : IFixtureDefinition
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Mode { get; set; }
        public int Channel { get; set; }
    }
}
