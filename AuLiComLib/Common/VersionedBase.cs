using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public class VersionedBase : IVersioned
    {
        public int Version
        {
            get => _version;
            set
            {
                if (value != _version)
                {
                    _version = value;
                    OnVersionChanged(new VersionChangedEventArgs());
                }
            }
        }
        private int _version;

        protected virtual void OnVersionChanged(VersionChangedEventArgs eventArgs)
        {
            VersionChanged?.Invoke(this, eventArgs);
        }

        public event EventHandler<VersionChangedEventArgs> VersionChanged;
    }
}
