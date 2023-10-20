using Zenject;

namespace Battle.Units.Movement
{
    public interface IUnitComponentBinder
    {
        void BindComponentToContainer(DiContainer container);
    }
}