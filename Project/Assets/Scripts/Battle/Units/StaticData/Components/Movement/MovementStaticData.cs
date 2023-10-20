using System;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.Units
{
    public abstract class MovementStaticData: ScriptableObject, IUnitComponentBinder
    {
        [field: SerializeField] public int TravelDistance { get; private set; }
        public abstract void BindComponentToContainer(DiContainer container);
    }
}