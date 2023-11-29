using System.Collections.Generic;
using Algorithms;
using Algorithms.RogueSharp;
using Battle.UnitCommands.Commands;
using Battle.Units;
using UnityEngine;
using Utilities;

namespace Battle.AI.Considerations
{
    public class MoveConsideration: IConsideration
    {
        private readonly Unit _unit;
        private readonly Cell _targetCell;
        
        private readonly List<Cell> _reachableByUnitCells;
        private readonly List<Unit> _enemyUnits;
        private readonly List<Cell>[] _enemyReachableCells;
        private readonly float _maxTravelDistanceForMap;

        public float ConsiderationResult { get; private set; }
        public bool CalculationComplete { get; private set; }

        public MoveConsideration(
            Unit unit, 
            Cell targetCell,
            List<Cell> reachableByUnitCells, 
            List<Unit> enemyUnits, 
            List<Cell>[] enemyReachableCells,
            float maxTravelDistanceForMap)
        {
            _unit = unit;
            _targetCell = targetCell;
            _reachableByUnitCells = reachableByUnitCells;
            _enemyUnits = enemyUnits;
            _enemyReachableCells = enemyReachableCells;
            _maxTravelDistanceForMap = maxTravelDistanceForMap;
        }
        
        public void Consider()
        {
            CalculationComplete = true;
            ConsiderationResult = Calculate(_reachableByUnitCells, _enemyUnits, _enemyReachableCells, _targetCell);
        }

        public ICommand GetCommand()
        {
            return new UnitMoveCommand(_unit, _targetCell.GridPosition);
        }
        
        //Unit can attack more units and can be attacked by less units = better (attackableInRangeRatio)
        //Can be attacked by less units = better (cannotBeAttackedByRatio)
        //Closer to enemy units = better (distanceCoeficient)
        //Pretty bad implementation, needs rework, but good enough for now
        private float Calculate(List<Cell> reachableCells, List<Unit> allEnemyUnits, List<Cell>[] enemyReachableCells, Cell reachableCell)
        {
            var distanceWeight = 0.001f;
            var countOfEnemiesThatCanAttack = GetEnemiesCountThatCanReachCellForAttack(allEnemyUnits, enemyReachableCells, reachableCell);
            var (countOfEnemiesThatCanBeAttacked, averageDistanceToEnemies) = GetEnemiesCountThatCanBeAttacked(reachableCells, allEnemyUnits, reachableCell);
            var distanceCoeficient = (1f - (averageDistanceToEnemies / _maxTravelDistanceForMap)) * distanceWeight;

            if (countOfEnemiesThatCanAttack == 0)
            {
                if (countOfEnemiesThatCanBeAttacked != 0)
                {
                    return allEnemyUnits.Count + 1 + distanceCoeficient;
                }
                else
                {
                    return distanceCoeficient;
                }
            }

            if (countOfEnemiesThatCanBeAttacked == 0)
            {
                var noOneToAttackPenalty = 100;
                return distanceCoeficient - noOneToAttackPenalty;
            }
            
            var attackableInRangeRatio = (countOfEnemiesThatCanBeAttacked / countOfEnemiesThatCanAttack);
            var cannotBeAttackedByRatio = (allEnemyUnits.Count - countOfEnemiesThatCanAttack) / allEnemyUnits.Count;

            /*return attackableInRangeRatio * cannotBeAttackedByRatio + distanceCoeficient;*/
            return distanceCoeficient;
        }

        private int GetEnemiesCountThatCanReachCellForAttack(List<Unit> allEnemyUnits, List<Cell>[] enemyReachableCells, Cell reachableCell)
        {
            int canBeAttackedByEnemiesCount = 0;

            for (int i = 0; i < allEnemyUnits.Count; i++)
            {
                var reachableForEnemy = enemyReachableCells[i];
                var enemyUnitCell = allEnemyUnits[i].PositionProvider.OccupiedCell;

                if (ReachableForAttackCellsFinder.CanReachCellForMeleeAttack(reachableCell, enemyUnitCell, reachableForEnemy))
                {
                    canBeAttackedByEnemiesCount++;
                }
            }
            
            return canBeAttackedByEnemiesCount;
        }

        private (int canAttackEnemiesCount, float averageDistanceToEnemies) GetEnemiesCountThatCanBeAttacked(
            List<Cell> reachableCells, 
            List<Unit> allEnemyUnits, 
            Cell reachableCell)
        {
            int canAttackEnemiesCount = 0;
            float distanceToEnemies = 0;

            foreach (var enemyUnit in allEnemyUnits)
            {
                var enemyUnitCell = enemyUnit.PositionProvider.OccupiedCell;

                if (ReachableForAttackCellsFinder.CanReachCellForMeleeAttack(enemyUnitCell, _unit.PositionProvider.OccupiedCell, reachableCells))
                {
                    canAttackEnemiesCount++;
                }
                
                distanceToEnemies += CellsUtilities.CalculateDistance(reachableCell, enemyUnitCell);
            }

            var averageDistanceToEnemies = distanceToEnemies / allEnemyUnits.Count;
            
            return (canAttackEnemiesCount, averageDistanceToEnemies);
        }
    }
}