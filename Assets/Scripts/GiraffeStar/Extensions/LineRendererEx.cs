using UnityEngine;

namespace GiraffeStar
{
    public static class LineRendererEx
    {
        public static void SetFullColor(this LineRenderer line, Color color)
        {
            line.startColor = color;
            line.endColor = color;
            //var gradient = new Gradient();
            //gradient.SetKeys(
            //    new GradientColorKey[] {new GradientColorKey(color, 0f), new GradientColorKey(color, 1f)},
            //    new GradientAlphaKey[] {new GradientAlphaKey(color.a, 0f), new GradientAlphaKey(color.a, 1f)}
            //    );
            //line.colorGradient = gradient;
        }

        public static void SetFullWidth(this LineRenderer line, float width)
        {
            line.startWidth = width;
            line.endWidth = width;
        }
    }
}

