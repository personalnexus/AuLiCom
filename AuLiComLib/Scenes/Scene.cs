using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    internal class Scene : IScene
    {
        public Scene(string name, double order, IEnumerable<ChannelValue> values)
        {
            _name = name;
            _order = order;
            _values = new byte[513];  //TODO: move universe size to shared place
            foreach (var channelValue in values)
            {
                _values[channelValue.Channel] = channelValue.Value;
            }
        }

        private readonly string _name;
        private readonly double _order;
        private readonly byte[] _values;

        internal byte GetChannelValue(int channel) => _values[channel];

    }
}
