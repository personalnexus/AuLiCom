using AuLiComLib.Protocols.Dmx;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AuLiComLib.Protocols
{
    public class ChannelValueChanges2
    {
        public ChannelValueChanges2(IConnection connection,
                                    IReadOnlyUniverse targetUniverse,
                                    TimeSpan fadeTime)
        {
            _targetUniverse = targetUniverse;
            _connection = connection;
            _fadeTime = fadeTime;
            _changes = _connection
                           .CurrentUniverse
                           .GetValues()
                           .Select(x => new ChannelValueChange2(currentChannelValue: x,
                                                                targetValue: targetUniverse.GetValue(x.Channel).Value))
                           .ToArray();
        }

        private readonly IReadOnlyUniverse _targetUniverse;
        private readonly IConnection _connection;
        private readonly TimeSpan _fadeTime;
        private readonly ChannelValueChange2[] _changes;

        public bool HasChanges => _changes.Any(x => x.HasChange);

        public Task Apply()
        {
            if (!HasChanges)
            {
                return Task.CompletedTask;
            }
            else
            {
                return Task.Run(() =>
                {
                    DateTime start = DateTime.Now;
                    DateTime end = start + _fadeTime;
                    double ticksLeft;
                    double fadeTimeTicks = _fadeTime.Ticks;
                    while ((ticksLeft = (end - DateTime.Now).Ticks) >= 0)
                    {
                        double fadePortionLeft = 1 - (ticksLeft / fadeTimeTicks);
                        _changes
                            .Select(x => x.GetNextValue(fadePortionLeft))
                            .ToReadOnlyUniverse()
                            .SendTo(_connection);
                    }
                    // Make sure we actually end up at the target in case there are rounding issues
                    _targetUniverse.SendTo(_connection);
                });
            }
        }

        private const int FadeIntervalInMilliseconds = 10; // TODO: move into configuration
    }
}
