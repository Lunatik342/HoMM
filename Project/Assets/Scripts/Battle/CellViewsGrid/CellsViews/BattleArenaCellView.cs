using System;
using UnityEngine;
using Zenject;

namespace Battle.CellViewsGrid.CellsViews
{
    public class BattleArenaCellView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ornament;
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private CellColors _cellStateColors;

        private CellViewState _prevState;

        public void PaintCell(CellViewState state)
        {
            if (state == CellViewState.Empty)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _background.color = _cellStateColors[state];
            }
        }

        public class Factory: PlaceholderFactory<BattleArenaCellView>
        {
            
        }
    }

    public enum CellViewState
    {
        Empty = -1,
        Default = 0,
        Walkable = 1,
        EnemyWalkable = 2,
        WalkableAndEnemyWalkableIntersection = 3,
        TargetableByRangedAbility = 4,
        CurrentUnit = 5,
        MoveTarget = 6,
        AoeSpellTarget = 7,
        MeleeAttackTarget = 8,
    }

    [Serializable]
    public class CellColors : SerializableDictionary<CellViewState, Color>
    {
        
    }
}