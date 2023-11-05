using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class ColorUtilities
    {
        public static Color TransparentWhite { get; } = new(1, 1, 1, 0);

        public static void SetAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}