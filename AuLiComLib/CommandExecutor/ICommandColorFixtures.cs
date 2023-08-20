namespace AuLiComLib.CommandExecutor
{
    public interface ICommandColorFixtures
    {
        bool TryGetColorChannelValuePropertiesByChannel(int channel, out ICommandColorChannelValueProperties colorProperties);
    }
}