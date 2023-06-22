namespace AuLiComLib.Protocols
{
    public interface IMutableUniverse: IReadOnlyUniverse
    {
        IMutableUniverse SetValue(ChannelValue channelValue);
        IMutableUniverse SetValues(IEnumerable<ChannelValue> channelValues);

        IReadOnlyUniverse AsReadOnly();
    }
}