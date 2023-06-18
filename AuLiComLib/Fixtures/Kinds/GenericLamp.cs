using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Kinds
{
    /// <summary>
    /// A generic lamp with a single channel to control intensity
    /// </summary>
    internal class GenericLamp : IFixture
    {
        public const string Kind = "GenericLamp";

        public GenericLamp(IFixtureDefinition definition)
        {
            Name = definition.Name;
            Value = ChannelValue.FromByte(definition.Channel, 0);
        }

        public string Name { get; }

        private ChannelValue Value { get; set; }
    }
}
