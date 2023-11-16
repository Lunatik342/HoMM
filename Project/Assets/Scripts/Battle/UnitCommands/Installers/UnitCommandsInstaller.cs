using Battle.UnitCommands.Processors;
using Battle.UnitCommands.Providers;
using Zenject;

namespace Battle.UnitCommands.Installers
{
    public class UnitCommandsInstaller: MonoInstaller<UnitCommandsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<LocalPlayerControlledCommandProvider, LocalPlayerControlledCommandProvider.Factory>().AsSingle();
            Container.BindFactory<AIControlledCommandProvider, AIControlledCommandProvider.Factory>().AsSingle();
            
            Container.Bind<MoveCommandProcessor>().AsSingle();
            Container.Bind<MeleeAttackCommandProcessor>().AsSingle();
            
            Container.Bind<CommandsProcessorFacade>().AsSingle();
        }
    }
}