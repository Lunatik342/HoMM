using Battle.UnitCommands.Processors;
using Battle.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.UnitCommands.Commands
{
    public class UnitMoveCommand : ICommand
    {
        public Unit Unit { get; private set; }
        public Vector2Int Position { get; private set; }
        
        public UnitMoveCommand(Unit unit, Vector2Int position)
        {
            Unit = unit;
            Position = position;
        }

        public async UniTask Process(CommandsProcessorFacade commandsProcessorFacade)
        {
            await commandsProcessorFacade.ProcessMoveCommand(this);
        }
    }
}