using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public class ChannelValueChanges
    {
        public ChannelValueChanges(IConnection connection,
                                   IReadOnlyUniverse targetUniverse,
                                   TimeSpan fadeTime) 
        {
            _connection = connection;
            _stepCount = Math.Max(1,
                                  (int)(fadeTime.TotalMilliseconds / FadeIntervalInMilliseconds));
            _changes = _connection
                        .CurrentUniverse
                        .GetValues()
                        .Select(x => new ChannelValueChange(currentChannelValue: x,
                                                            targetValue: targetUniverse.GetValue(x.Channel).Value,
                                                            stepCount: _stepCount))
                        .Where(x => x.HasChange)
                        .ToArray();
        }

        private readonly int _stepCount;
        private readonly IConnection _connection;
        private readonly ChannelValueChange[] _changes;

        public bool HasChanges => _changes.Length > 0;

        public async Task Apply()
        {
            if (HasChanges)
            {
                for (int step = 0; step < _stepCount; step++)
                {
                    _changes
                    .Select(x => x.GetNextValue(step))
                    .ToReadOnlyUniverse()
                    .SendTo(_connection);
                    await Task.Delay(FadeIntervalInMilliseconds);
                }
            }
        }

        private const int FadeIntervalInMilliseconds = 50; // TODO: move into configuration
    }
}
