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
        public Fixtures(params IFixture[] fixtures)
        {
            _fixturesByName = fixtures.ToDictionary(x => x.Name);
        }

        private readonly Dictionary<string, IFixture> _fixturesByName;

        public T Get<T>(string name) where T : IFixture => (T)_fixturesByName[name];
    }
}
