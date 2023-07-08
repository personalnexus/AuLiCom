namespace AuLiComLib.Protocols
{
    public interface IReadOnlyUniverse
    {
        ChannelValue GetValue(int channel);
        IEnumerable<ChannelValue> GetValues();
        byte[] GetValuesCopy();

        void WriteValuesTo(ISerialPort port);

        bool HasSameValuesAs(IReadOnlyUniverse other);
    }
}