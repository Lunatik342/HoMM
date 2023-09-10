using Battle.BattleField;
using Battle.BattleField.Cells;
using UnityEditor.VersionControl;
using Zenject;
using Task = System.Threading.Tasks.Task;

namespace Battle
{
    public class BattleStarter: IInitializable
    {
        private readonly BattleFieldFactory _battleFieldFactory;
        private readonly BattleFieldStaticDataService _staticDataService;

        public BattleStarter(BattleFieldFactory battleFieldFactory, BattleFieldStaticDataService staticDataService)
        {
            _battleFieldFactory = battleFieldFactory;
            _staticDataService = staticDataService;
        }

        public async void Initialize()
        {
            await StartBattle();
        }

        private async Task StartBattle()
        {
            var targetBattleFieldId = BattleFieldId.Blank;
            
            await _battleFieldFactory.SpawnBattleField(targetBattleFieldId);
        }
    }
}