using AuLiComLib.Common;
using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AuLiComSim
{
    public class SimulatorConnection : IConnection, IDisposable
    {
        private StageView _stageView;
        private readonly Thread _uiThread;

        public SimulatorConnection()
        {
            CurrentUniverse = Universe.CreateEmptyReadOnly();

            // TODO: fix race condition when creating of _stageView
            _uiThread = new Thread(() =>
            {
                _stageView = new StageView();
                _stageView.ShowDialog();
            });
            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.Start();
        }

        public void Dispose()
        {
            _stageView.Dispatcher.InvokeAsync(_stageView.Hide);
        }

        // IConnection

        public IReadOnlyUniverse CurrentUniverse { get; private set; }

        public void SendUniverse(IReadOnlyUniverse universe)
        {
            CurrentUniverse = universe;
            _observers.OnNext(this);

            _stageView.Dispatcher.InvokeAsync(() =>
            {
                _stageView.Led1.FillWithRgbFrom(universe, startingAtChannel: 4);
                _stageView.Led2.FillWithRgbFrom(universe, startingAtChannel: 7);
                _stageView.Led3.FillWithRgbFrom(universe, startingAtChannel: 10);
            });
        }

        // IObservable

        private readonly Observers<IConnection> _observers = new();

        public IDisposable Subscribe(IObserver<IConnection> observer) => _observers.Subscribe(observer);

        public int Version => _observers.Version;
    }
}
