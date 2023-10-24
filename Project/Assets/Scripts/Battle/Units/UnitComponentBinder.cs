using Zenject;

namespace Battle.Units.Movement
{
    public interface IUnitComponentBinder
    {
        void BindRelatedComponentToContainer(DiContainer container);
    }
}