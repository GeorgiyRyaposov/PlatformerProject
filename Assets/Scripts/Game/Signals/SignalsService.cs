using System;
using Common.ServiceLocator;
using UnityEngine.Assertions;
using Zenject;

namespace Game.Signals
{
    public class SignalsService : IService
    {
        private readonly SignalBus signalBus;

        public SignalsService(SignalBus signalBus)
        {
            this.signalBus = signalBus;
            Assert.IsNotNull(signalBus, "SignalBus is null");
        }

        public void Fire<TSignal>()
        {
            signalBus.Fire<TSignal>();
        }
        public void Fire<TSignal>(TSignal signal)
        {
            signalBus.Fire(signal);
        }

        public void Subscribe<TSignal>(Action callback)
        {
            signalBus.Subscribe<TSignal>(callback);
        }
        public void Unsubscribe<TSignal>(Action callback)
        {
            signalBus.Unsubscribe<TSignal>(callback);
        }
        public void TryUnsubscribe<TSignal>(Action callback)
        {
            signalBus.TryUnsubscribe<TSignal>(callback);
        }
    }
}