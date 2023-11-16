namespace Utilities.UsefullClasses
{
    public readonly struct MinMaxValue
    {
        public readonly int Min { get; }
        public readonly int Max { get; }

        public MinMaxValue(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public float Average()
        {
            return (Min + Max) / 2f;
        }
    }
}