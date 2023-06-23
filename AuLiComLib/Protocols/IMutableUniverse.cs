namespace AuLiComLib.Protocols
{
    public interface IMutableUniverse
    {
        IMutableUniverse SetValue(ChannelValue channelValue);
        IMutableUniverse SetValues(IEnumerable<ChannelValue> channelValues);

        IMutableUniverse CombineWith(IReadOnlyUniverse other, ChannelValueAggregator aggregatingChannelValuesWith);

        IReadOnlyUniverse AsReadOnly();
    }
}