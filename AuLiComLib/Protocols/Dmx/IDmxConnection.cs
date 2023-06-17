namespace AuLiComLib.Protocols.Dmx
{
    public interface IDmxConnection
    {
        void SetValue(DmxChannelValue channelValue);
        void SetValues(ReadOnlySpan<DmxChannelValue> channelValues);
        void SetValuesToZero();
        IEnumerable<DmxChannelValue> GetValues();
    }
}