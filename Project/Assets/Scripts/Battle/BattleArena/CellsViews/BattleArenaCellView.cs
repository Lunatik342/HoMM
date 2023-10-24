using System;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ornament;
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private CellColors _cellStateColors;

        private CellViewState _prevState;

        public void PaintCell(CellViewState state)
        {
            _background.color = _cellStateColors[state];
        }

        public class Factory: PlaceholderFactory<BattleArenaCellView>
        {
            
        }
    }

    public enum CellViewState
    {
        Default,
        Walkable,
        EnemyWalkable,
        WalkableAndEnemyWalkableIntersection,
        TargetableByRangedAbility,
        CurrentUnit,
        MoveTarget,
        AoeSpellTarget,
        MeleeAttackTarget
    }

    [Serializable]
    public class CellColors : SerializableDictionary<CellViewState, Color>
    {
        
    }
}