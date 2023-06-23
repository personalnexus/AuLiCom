using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    internal class MutableUniverse: ReadOnlyUniverse, IMutableUniverse
    {
        internal MutableUniverse(): base(CreateEmptyValues())
        {
        }

        internal MutableUniverse(IReadOnlyUniverse source): base(source.GetValuesCopy())
        {
        }

        internal MutableUniverse(IEnumerable<ChannelValue> channelValues): this()
        {
            foreach (ChannelValue channelValue in channelValues)
            {
                SetValue(channelValue);
            }
        }

        public IMutableUniverse SetValue(ChannelValue channelValue)
        {
            SetValueInternal(channelValue.Channel, channelValue.Value);
            return this;
        }

        public IMutableUniverse SetValues(IEnumerable<ChannelValue> channelValues)
        {
            foreach (ChannelValue channelValue in channelValues)
            {
                SetValue(channelValue);
            }
            return this;
        }

        public IMutableUniverse CombineWith(IReadOnlyUniverse other, ChannelValueAggregator aggregatingChannelValuesWith)
        {
            for (int channel = 1; channel < ChannelCount; channel++)
            {
                SetValueInternal(channel, aggregatingChannelValuesWith(GetValue(channel), other.GetValue(channel)));
            }
            return this;
        }

        public IReadOnlyUniverse AsReadOnly() => new MutableUniverse(this); // Copy constructor so any changes to this object are not reflected in the returned read-only version
    }
}
