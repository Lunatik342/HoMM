using System.Collections.Generic;
using Battle.Units.Components.Interfaces;

namespace Battle.Units.Components
{
    public class UnitDeathHandler
    {
        private readonly UnitHealth _unitHealth;
        private readonly List<IDeathEventReceiver> _deathEventReceivers;

        public UnitDeathHandler(UnitHealth unitHealth, List<IDeathEventReceiver> deathEventReceivers)
        {
            _unitHealth = unitHealth;
            _deathEventReceivers = deathEventReceivers;
        }

        public void Initialize()
        {
            _unitHealth.UnitDied += HandleDeath;
        }

        private void HandleDeath()
        {
            foreach (var deathEventReceiver in _deathEventReceivers)
            {
                deathEventReceiver.OnDeath();
            }
        }
    }
}