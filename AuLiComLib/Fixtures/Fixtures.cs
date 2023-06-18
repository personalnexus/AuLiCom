using AuLiComLib.Fixtures.Kinds;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    /// <summary>
    /// Manages the fixtures which in turn have one or more channels.
    /// </summary>
    public class Fixtures
    {
        public Fixtures(IList<IFixtureDefinition> definitions, IConnection connnection)
        {
            _connnection = connnection;
            _fixturesByName = definitions
                .Select(CreateFixture)
                .ToDictionary(x => x.Name, x => x);
        }

        private readonly IConnection _connnection;
        private readonly Dictionary<string, IFixture> _fixturesByName;

        private IFixture CreateFixture(IFixtureDefinition definition)
        {
            IFixture result = definition.Kind switch
            {
                GenericLamp.Kind => new GenericLamp(definition),
                _ => throw new ArgumentException($"Invalid fixture definition kind '{definition.Kind}'")
            };
            return result;
        }
    }
}
