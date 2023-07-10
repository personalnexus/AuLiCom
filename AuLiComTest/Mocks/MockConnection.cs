using AuLiComLib.Common;
using AuLiComLib.Protocols;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.Mocks
{
    internal class MockConnection : VersionedBase, IConnection
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

        public IDisposable Subscribe(IObserver<IConnection> observer)
        {
            throw new NotImplementedException();
        }
    }
}
