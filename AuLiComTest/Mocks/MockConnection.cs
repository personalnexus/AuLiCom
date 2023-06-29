using AuLiComLib.Protocols;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.Mocks
{
    internal class MockConnection : IConnection
    {
        public MockConnection(IReadOnlyUniverse initialUniverse)
        {
            CurrentUniverse = initialUniverse;
        }

        public void Dispose()
        {
        }

        public IReadOnlyUniverse CurrentUniverse { get; private set; }

        public void SendUniverse(IReadOnlyUniverse universe) => CurrentUniverse = universe;
    }
}
