using System.Collections.Generic;
using System.Threading.Tasks;
using Battle.BattleArena.CellsViews;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Movement;
using Infrastructure.SimpleStateMachine;
using RogueSharp;

namespace Battle.BattleFlow.StateMachine
{
    public class ObservingGridViewState: IState
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly IMapHolder _mapHolder;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly MoveCommandProcessor _moveCommandProcessor;
        private readonly PathfindingService _pathfindingService;
        private readonly PathDisplayService _pathDisplayService;

        private TaskCompletionSource<bool> _taskCompletionSource;
        private List<Cell> _reachableCells;
        private Unit _unit;
        
        public ObservingGridViewState(BattleCellsInputService cellsInputService, 
            IMapHolder mapHolder, 
            BattleArenaCellsDisplayService cellsDisplayService,
            MoveCommandProcessor moveCommandProcessor,
            PathfindingService pathfindingService,
            PathDisplayService pathDisplayService)
        {
            _cellsInputService = cellsInputService;
            _mapHolder = mapHolder;
            _cellsDisplayService = cellsDisplayService;
            _moveCommandProcessor = moveCommandProcessor;
            _pathfindingService = pathfindingService;
            _pathDisplayService = pathDisplayService;
        }
        
        public void Exit()
        {
            
        }

        public void Enter()
        {
            
        }
    }
}