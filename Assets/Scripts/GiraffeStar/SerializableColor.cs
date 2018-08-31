using UnityEngine;
using Newtonsoft.Json;

namespace GiraffeStar
{
    public struct SerializableColor
    {         
        [JsonProperty]
        public float r { get; private set; }
        [JsonProperty]
        public float g { get; private set; }
        [JsonProperty]
        public float b { get; private set; }
        [JsonProperty]
        public float a { get; private set; }

        public SerializableColor(float r, float g, float b, float a = 1f )
        {
            this.r = InRange(r);
            this.g = InRange(g);
            this.b = InRange(b);
            this.a = InRange(a);
        }

        static float InRange(float x)
        {
            return Mathf.Min(1, Mathf.Max(x, 0));
        }

        public static implicit operator Color(SerializableColor color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }

        public static implicit operator SerializableColor(Color color)
        {
            return new SerializableColor(color.r, color.g, color.b, color.a);
        }
    }
}


