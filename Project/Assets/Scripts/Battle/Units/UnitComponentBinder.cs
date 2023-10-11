using System;

namespace Battle.Units.Movement
{
    public interface IUnitComponentBinder
    {
        Type GetContractTypeToBind();
        object GetArgument();
    }
}