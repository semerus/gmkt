using System;

namespace GiraffeStar
{
    public static class StringEx
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Length == 0;
        }
    }
}