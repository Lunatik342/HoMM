using System.Collections.Generic;
using Battle.BattleField.Cells;
using UnityEngine;
using Zenject;

namespace Battle.BattleField.Installers
{
    [CreateAssetMenu(fileName = "BattleFieldSettingsInstaller", menuName = "Installers/BattleFieldSettingsInstaller")]
    public class BattleFieldSettingsInstaller: ScriptableObjectInstaller<BattleFieldSettingsInstaller>
    {
        [SerializeField] private List<BattleFieldStaticData> _battleFieldsStaticData;
        [SerializeField] private BattleFieldCellView _cellViewPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_battleFieldsStaticData);
            Container.BindInstance(_cellViewPrefab);
        }
    }
}