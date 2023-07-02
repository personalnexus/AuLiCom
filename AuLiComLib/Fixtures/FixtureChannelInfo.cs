using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public readonly record struct FixtureChannelInfo(string FixtureName,
                                                     string FixtureType,
                                                     string? FixtureAlias,
                                                     string ChannelName,
                                                     int StartChannel);
}
