using Battle.BattleField;
using Zenject;

namespace Battle
{
    public class BattleStarter: IInitializable
    {
        private readonly BattleFieldSpawner _battleFieldSpawner;

        public BattleStarter(BattleFieldSpawner battleFieldSpawner)
        {
            _battleFieldSpawner = battleFieldSpawner;
        }

        public void Initialize()
        {
            StartBattle();
        }

        private void StartBattle()
        {
            _battleFieldSpawner.SpawnBattleField(BattleFieldId.Blank);
        }
    }
}