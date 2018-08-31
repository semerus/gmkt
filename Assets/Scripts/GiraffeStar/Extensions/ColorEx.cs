using UnityEngine;

namespace GiraffeStar
{
    public static class ColorEx
    {
        public static Color OverrideAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}


