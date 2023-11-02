using Zenject;

namespace Battle.Units.Generic
{
    public interface IUnitComponentBinder
    {
        void BindRelatedComponentToContainer(DiContainer container);
    }
}