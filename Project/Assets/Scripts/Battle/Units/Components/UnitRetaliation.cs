using Battle.Units.Components.Interfaces;

namespace Battle.Units.Components
{
    public class UnitRetaliation: ITurnEndEventReceiver
    {
        private int _retaliationsCount = 1;

        public bool CanRetaliate => _retaliationsCount > 0;
        
        public void OnRetaliation()
        {
            _retaliationsCount--;
        }

        void ITurnEndEventReceiver.OnTurnEnd()
        {
            _retaliationsCount = 1;
        }
    }
}