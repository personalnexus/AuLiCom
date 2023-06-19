namespace AuLiComLib.Protocols
{
    /// <summary>
    /// Abstract interface describing a connection e.g. via DMX to get/set channel values
    /// </summary>
    public interface IConnection
    {
        void SetValue(ChannelValue channelValue);
        void SetValues(ReadOnlySpan<ChannelValue> channelValues);
        void SetValuesToZero();

        byte GetValue(int channel);
        IEnumerable<ChannelValue> GetValues();
    }
}