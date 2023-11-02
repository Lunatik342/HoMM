using Battle.Units.Generic;
using UnityEngine;
using Zenject;

namespace Battle.Units.StaticData.Components.Movement
{
    public abstract class MovementStaticData: ScriptableObject, IUnitComponentBinder
    {
        [field: SerializeField] public int TravelDistance { get; private set; }
        public abstract void BindRelatedComponentToContainer(DiContainer container);
    }
}