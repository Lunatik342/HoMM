using Battle.Units.StaticData;

namespace Utilities.UsefullClasses
{
    public class UnitsCount
    {
        public UnitId UnitId { get; }
        public int Count { get; }

        public UnitsCount(UnitId unitId, int count)
        {
            UnitId = unitId;
            Count = count;
        }
    }
}