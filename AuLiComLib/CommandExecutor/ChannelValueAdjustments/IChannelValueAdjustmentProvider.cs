using AuLiComLib.CommandExecutor.ChannelValueAdjustments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public interface IChannelValueAdjustmentProvider
    {
        ChannelValueAdjustment PreviousAdjustment { get; }
    }
}
