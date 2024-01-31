namespace AuLiComLib.Protocols
{
    internal readonly struct ChannelValueChange2
    {
        public ChannelValueChange2(ChannelValue currentChannelValue, byte targetValue)
        {
            _startChannelValue = currentChannelValue;
            _targetValue = targetValue;
        }

        private readonly ChannelValue _startChannelValue;
        private readonly byte _targetValue;

        public bool HasChange => _startChannelValue.Value != _targetValue;

        public ChannelValue GetNextValue(double fadePortionLeft)
        {
            byte newValue = HasChange
                ? (byte)Math.Round(_startChannelValue.Value + ((_targetValue - _startChannelValue.Value) * fadePortionLeft), 0, MidpointRounding.ToZero)
                : _targetValue;
            return _startChannelValue.WithValue(newValue);
        }
    }
}
